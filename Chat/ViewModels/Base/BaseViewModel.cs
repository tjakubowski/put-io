using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Chat.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>
        /// Gets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T Get<T>([CallerMemberName] string name = null)
        {
            Debug.Assert(name != null, "name != null");
            object value = null;
            if (_properties.TryGetValue(name, out value))
                return value == null ? default(T) : (T)value;
            return default(T);
        }

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        protected void Set<T>(T value, [CallerMemberName] string name = null)
        {
            Debug.Assert(name != null, "name != null");
            if (Equals(value, Get<T>(name)))
                return;
            _properties[name] = value;
            OnPropertyChanged(name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
