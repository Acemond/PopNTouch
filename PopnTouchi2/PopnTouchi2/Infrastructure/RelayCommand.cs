using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PopnTouchi2.Infrastructure
{
    /// <summary>
    /// TODO
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Parameter.
        /// TODO
        /// </summary>
        Action execute;

        /// <summary>
        /// Parameter.
        /// TODO
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            execute();
        }
    }
}