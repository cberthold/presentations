using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaxApp.WPF.Common.Command
{
    public class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool _canExecute = true;

        protected virtual void SetCanExecute(bool canExecute)
        {
            if(_canExecute != canExecute)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool GetCanExecute()
        {
            return _canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return GetCanExecute();
        }

        public virtual void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
