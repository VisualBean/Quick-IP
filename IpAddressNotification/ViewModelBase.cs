using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace IpAddressNotification
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The event args cache.
        /// </summary>
        private readonly Dictionary<string, PropertyChangedEventArgs> eventArgsCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            eventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();
        }

        #region Implementation of INotifyPropertyChanged

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args;
            if (!eventArgsCache.TryGetValue(propertyName, out args))
            {
                args = new PropertyChangedEventArgs(propertyName);
                eventArgsCache.Add(propertyName, args);
            }

            OnPropertyChanged(args);
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, args);
        }

        #endregion
    }
}
