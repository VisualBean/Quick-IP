using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IpAddressNotification
{
    internal class RelayCommand : ICommand
    {
        /// <summary>
        ///     Contains elements that make Button commands work - found online no need to comment further
        /// </summary>

        #region Members
        private readonly Func<Boolean> _canExecute;

        private readonly Action _execute;

        #region Constructors

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(Object parameter)
        {
            _execute();
        }

        #endregion

        #endregion
    }
}
