// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkModel.cs" company="PixoBox">
//   PixoBox owns this code motherfucker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace IpAdressNotification
{
    /// <summary>
    /// The network model.
    /// </summary>
    internal class AdapterModel
    {

        public AdapterModel()
        {
            
        }
        /// <summary>
        /// Gets or sets a value indicating whether is current.
        /// </summary>
        public bool IsCurrent { 
            get; //{ return _isCurrent; }
            set; //{ _isCurrent = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            set;
            //get { return _name; }
            //set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        public string IPAdress
        {
            get;
            set;
            //get { return _ipAdress; }
            //set { _ipAdress = value; }
        }

        ///// <summary>
        ///// Gets or sets the _name.
        ///// </summary>
        //private string _name { get; set; }

        ///// <summary>
        ///// Gets or sets the _ip adress.
        ///// </summary>
        //private string _ipAdress { get; set; }


        ///// <summary>
        ///// Gets or sets a value indicating whether _is online.
        ///// </summary>
        //private bool _isCurrent { get; set; }

    }
}