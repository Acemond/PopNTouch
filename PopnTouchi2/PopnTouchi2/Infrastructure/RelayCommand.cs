using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PopnTouchi2.Infrastructure
{
    /// <summary>
    /// Create a specific command which will be called in the Xaml
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Parameter.
        /// The action to execute
        /// </summary>
        Action execute;

        /// <summary>
        /// Parameter.
        /// TODO
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Execute setter
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