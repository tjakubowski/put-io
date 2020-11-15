using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ServerLibrary.Models;
using ServerLibrary.Server.Messages;

namespace ServerLibrary.Server
{
    public class TcpServerAsync : TcpServer
    {
        public TcpServerAsync(IPAddress ipAddress, int port) : base(ipAddress, port)
        {
        }

        public override void Start()
        {
            StartListening();

            while (true)
                AcceptClient();
        }

        protected override void AcceptClient()
        {
            TcpClient tcpClient = _listener.AcceptTcpClient();
            TcpServerSession session = new TcpServerSession(tcpClient);

            Console.WriteLine($"Client connected");

            Task.Run(() =>
            {
                HandleClientSession(session);
                CloseClientSession(session);
            });
        }

        protected override void HandleClientSession(TcpServerSession session)
        {
            while (session.Client.Connected && session.User == null)
            {
                try
                {
                    var readBytes = session.ReadBytes();
                    var request = MessageSerializer.Deserialize(new TcpMessage(readBytes));

                    if (request is AuthenticationForm authenticationForm)
                    {
                        switch (authenticationForm.AuthenticationType)
                        {
                            case AuthenticationType.Login:
                                Login(session, authenticationForm);
                                break;
                            case AuthenticationType.Register:
                                Register(session, authenticationForm);
                                break;
                        }
                    }

                    if (session.User == null)
                        continue;

                    if (request is ChangePasswordForm changePasswordForm)
                    {
                        ChangePassword(session, changePasswordForm);
                    }
                }
                catch (ArgumentNullException)
                {
                    return;
                }
            }
        }

        void CloseClientSession(TcpServerSession session)
        {
            session.Client.Close();

            if(session.User != null)
                Console.WriteLine($"[Logout] User {session.User.Username} logged out.");

            Console.WriteLine($"[Close Session] Client disconnected");
        }

        private void Login(TcpServerSession session, AuthenticationForm form)
        {
            using (var context = new DatabaseContext())
            {
                var hash = User.CreatePassword(form.Password);
                session.User = context.Users.SingleOrDefault(u => u.Username == form.Username && u.Password == hash);

                Console.WriteLine($"[Login] User {session.User.Username} logged in");
            }
        }

        private void ChangePassword(TcpServerSession session, ChangePasswordForm form)
        {
            using (var context = new DatabaseContext())
            {
                session.User.Password = User.CreatePassword(form.Password);
                context.Users.Attach(session.User);
                context.Entry(session.User).State = EntityState.Modified;
                context.SaveChanges();

                Console.WriteLine($"[ChangePassword] User {session.User.Username} changed password");
            }
        }

        private void Register(TcpServerSession session, AuthenticationForm form)
        {
            using (var context = new DatabaseContext())
            {
                var user = new User()
                {
                    Username = form.Username,
                    Password = User.CreatePassword(form.Password)
                };

                context.Users.Add(user);
                context.SaveChanges();

                session.User = user;

                Console.WriteLine($"[Register] User {user.Username} registered");
            }
        }

    }
}
