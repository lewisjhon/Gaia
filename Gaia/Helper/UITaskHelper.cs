using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gaia.Helper
{
    public sealed class UITaskHelper
    {
        private readonly TaskScheduler _uiScheduler;

        public UITaskHelper()
        {
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public TaskScheduler UIScheduler
        {
            get { return _uiScheduler; }
        }

        public Task ChangeUIAsync(Action action)
        {
            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, _uiScheduler);
        }

        public void ChangeUI(Action action)
        {
            ChangeUIAsync(action);
        }

        public void ChangeByDispatcher(Action action)
        {
            App.Current.Dispatcher.Invoke(action, System.Windows.Threading.DispatcherPriority.Background);
        }

        public Task RegisterContinuation(Task task, Action action)
        {
            return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.None, _uiScheduler);
        }

        public Task RegisterContinuation<TResult>(Task<TResult> task, Action action)
        {
            return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.None, _uiScheduler);
        }

        public Task RegisterSucceededHandler(Task task, Action action)
        {
            return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, _uiScheduler);
        }

        public Task RegisterSucceededHandler<TResult>(Task<TResult> task, Action<TResult> action)
        {
            return task.ContinueWith(t => action(t.Result), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, _uiScheduler);
        }

        public Task RegisterFaultedHandler(Task task, Action<Exception> action)
        {
            return task.ContinueWith(t => action(t.Exception), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, _uiScheduler);
        }
        public Task RegisterFaultedHandler<TResult>(Task<TResult> task, Action<Exception> action)
        {
            return task.ContinueWith(t => action(t.Exception), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, _uiScheduler);
        }
        public Task RegisterCancelledHandler(Task task, Action action)
        {
            return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, _uiScheduler);
        }
        public Task RegisterCancelledHandler<TResult>(Task<TResult> task, Action action)
        {
            return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, _uiScheduler);
        }
    }
}
