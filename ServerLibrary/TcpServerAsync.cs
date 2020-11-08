using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Int32;

namespace ServerLibrary
{
    public class TcpServerAsync : TcpServer
    {
        private delegate void HandleDataTransmissionDelegate(TcpServerConnection connection);

        public TcpServerAsync(IPAddress ipAddress, int port) : base(ipAddress, port)
        {
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = _listener.AcceptTcpClient();
                TcpServerConnection connection = new TcpServerConnection(tcpClient);

                Console.WriteLine($"Client connected");

                HandleDataTransmissionDelegate handleDataTransmissionDelegate = HandleDataTransmission;

                handleDataTransmissionDelegate.BeginInvoke(connection, CloseClientConnection, connection);
            }

        }



        protected override void HandleDataTransmission(TcpServerConnection connection)
        {
            int choice;

            while (true)
            {

                connection.SendLine("0. Exit");
                connection.SendLine("1. Login");
                connection.SendLine("2. Register");
                connection.Send("> ");

                try
                {
                    choice = Parse(connection.Read());

                    if (choice == 0)
                        return;

                    if (choice == 1)
                    {
                        Login(connection);
                        connection.SendLine($"Hello {connection.User.Username}");
                        Console.WriteLine($"{connection.User.Username} logged in.");
                        break;
                    }
                    if (choice == 2)
                    {
                        Register(connection);
                        connection.SendLine($"You have been registered. Hello {connection.User.Username}");
                        Console.WriteLine($"{connection.User.Username} registered.");
                        break;
                    }
                }
                catch (Exception e)
                {
                    connection.SendLine(e.Message);
                }
            }

            while (true)
            {
                connection.SendLine("0. Exit");
                connection.SendLine("1. Change password");
                connection.SendLine("2. Text to uppercase");
                connection.Send("> ");

                choice = Parse(connection.Read());

                if (choice == 0)
                    return;
                try
                {
                    if (choice == 1)
                    {
                        ChangePassword(connection);
                        connection.SendLine("Success! Password changed");
                        Console.WriteLine($"{connection.User.Username} changed password.");
                    }
                    else if (choice == 2)
                    {
                        connection.Send("> ");
                        var text = connection.Read();
                        connection.SendLine(text.ToUpper());
                    }
                }
                catch (Exception e)
                {
                    connection.SendLine(e.Message);
                }
            }
        }

        void CloseClientConnection(IAsyncResult result)
        {
            TcpServerConnection connection = (TcpServerConnection) result.AsyncState;
            connection.Client.Close();

            if(connection.User != null)
                Console.WriteLine($"{connection.User.Username} logged out.");

            Console.WriteLine($"Client disconnected");
        }

        private void Login(TcpServerConnection connection)
        {
            connection.SendLine("User authentication");

            connection.Send("Username: ");
            var username = connection.Read();

            connection.Send("Password: ");
            var password = connection.Read();

            var user = SqliteDatabase.GetUser(username);

            if (user != null && user.Password == UserModel.CreatePassword(password))
            {
                connection.User = user;
            }
        }

        private void ChangePassword(TcpServerConnection connection)
        {
            connection.SendLine("Password change");

            connection.Send("New password: ");
            var password = connection.Read();

            connection.Send("Repeat password: ");

            if (password == connection.Read())
            {
                connection.User.Password = UserModel.CreatePassword(password);
                SqliteDatabase.UpdateUser(connection.User);
            }
        }

        private void Register(TcpServerConnection connection)
        {
            connection.SendLine("Registration");

            connection.Send("Username: ");
            var username = connection.Read();

            connection.Send("Password: ");
            var password = connection.Read();

            var user = new UserModel(username, UserModel.CreatePassword(password));

            SqliteDatabase.SaveUser(user);
            connection.User = user;
        }

    }
}
