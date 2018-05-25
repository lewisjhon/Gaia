using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gaia.ViewModel.Base
{
    public class RelayCommand<T> : ICommand
    {
        #region Fields
        private readonly Action<T> mExecute = null;
        private readonly Predicate<T> mCanExecute = null;
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute)
          : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.mExecute = execute;
            this.mCanExecute = canExecute;
        }

        #endregion // Constructors

        #region events
        /// <summary>
        /// Eventhandler to re-evaluate whether this command can execute or not
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.mCanExecute != null)
                    CommandManager.RequerySuggested += value;
            }

            remove
            {
                if (this.mCanExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Determine whether this pre-requisites to execute this command are given or not.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.mCanExecute == null ? true : this.mCanExecute((T)parameter);
        }

        /// <summary>
        /// Execute the command method managed in this class.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            this.mExecute((T)parameter);
        }
        #endregion methods
    }

    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Action
        /// </summary>
        private readonly Action<object> _execute;


        /// <summary>
        /// Predicate
        /// </summary>
        private readonly Predicate<object> _canExecute;
    }
}
