// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdapterViewModel.cs" company="PixoBox">
//   PixoBox owns this code motherfucker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using IpAddressNotification;

namespace IpAdressNotification
{
    /// <summary>
    ///     The network view model.
    /// </summary>
    internal class AdapterViewModel : ViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AdapterViewModel" /> class.
        /// </summary>
        public AdapterViewModel()
        {
            NetworkChange.NetworkAddressChanged +=
                NetworkChange_NetworkAddressChanged;

          UpdateAdapters();
        }

        /// <summary>
        ///     Gets or sets the network model.
        /// </summary>
        public AdapterModel CurrentNIC
        {
            get { return _currentNIC; }
            set
            {
                _currentNIC = value;
                OnPropertyChanged("CurrentNIC");
            }
        }

        /// <summary>
        ///     Gets or sets the _network model.
        /// </summary>
        private AdapterModel _currentNIC { get; set; }

        /// <summary>
        /// Gets or sets the ni cs.
        /// </summary>
        public ObservableCollection<AdapterModel> NICs
        {
            get { return _nics; }
            set
            {
                _nics = value;
                OnPropertyChanged("NICs");
            }
        }

        /// <summary>
        /// Gets or sets the _nics.
        /// </summary>
        private ObservableCollection<AdapterModel> _nics { get; set; }


        /// <summary>
        /// The get all ip v 4 interfaces.
        /// </summary>
        /// <returns>
        /// The <see cref="ObservableCollection"/>.
        /// </returns>
        private ObservableCollection<AdapterModel> GetAllIPV4Interfaces()
        {
            var adapters = new ObservableCollection<AdapterModel>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in interfaces)
            {
                var ipAddress = nic.GetIPProperties()
                    .UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork &&
                                                           !IPAddress.IsLoopback(ip.Address));
                if (ipAddress != null)
                {
                    var adapter = new AdapterModel();
                    if (ipAddress.Address.ToString() == CurrentNIC.IPAdress)
                    {
                        adapter.IsCurrent = true;
                    }

                    adapter.IPAdress = ipAddress.Address.ToString();
                    adapter.Name = nic.Name;
                    adapters.Add(adapter);
                }
            }

            return adapters;
        }

        /// <summary>
        /// The network change_ network address changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            UpdateAdapters();
        }

        private void UpdateAdapters()
        {
            CurrentNIC = GetCurrentInterface(GetCurrentUsedOutBoundIP());
            NICs = GetAllIPV4Interfaces();
        }

        /// <summary>
        /// The lowest metric end point.
        /// </summary>
        /// <param name="remoteIPEndPoint">
        /// The remote ip end point.
        /// </param>
        /// <returns>
        /// The <see cref="IPEndPoint"/>.
        /// </returns>
        public static IPEndPoint LowestMetricEndPoint(IPEndPoint remoteIPEndPoint)
        {
            var testSocket = new Socket(remoteIPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                testSocket.Connect(remoteIPEndPoint);
            }
            catch (Exception)
            {
                return new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
            }

            return (IPEndPoint)testSocket.LocalEndPoint;
        }

        /// <summary>
        ///     The get current ip.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string GetCurrentUsedOutBoundIP()
        {
            return LowestMetricEndPoint(new IPEndPoint(IPAddress.Parse("8.8.8.8"), 0)).Address.ToString();
        }

        public ICommand CloseApplicationCommand
        {
            get { return new RelayCommand(CloseApplicationExecute, CanExecute); }
        }

        public Action CloseAction { get; set; }

        /// <summary>
        /// The do refresh certficates.
        /// </summary>
        private void CloseApplicationExecute()
        {
            CloseAction();
        }

        public bool CanExecute()
        {
            return true;
        }
        /// <summary>
        /// The get current interface.
        /// </summary>
        /// <param name="ipAddress">
        /// The ip adress.
        /// </param>
        /// <returns>
        /// The <see cref="CurrentNIC"/>.
        /// </returns>
        public AdapterModel GetCurrentInterface(string ipAddress)
        {
            if (ipAddress == "0.0.0.0")
            {
                return new AdapterModel {IPAdress = "0.0.0.0", Name = "N/A", IsCurrent = false};
            }

            NetworkInterface networkInterface = null;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.ToString() == ipAddress)
                    {
                        networkInterface = nic;
                        break;
                    }
                }
            }

            return new AdapterModel {IPAdress = ipAddress, Name = networkInterface.Name, IsCurrent = true};
        }    
    }
}