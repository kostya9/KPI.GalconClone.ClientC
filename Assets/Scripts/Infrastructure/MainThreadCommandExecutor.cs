using strange.extensions.command.api;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure
{
    /// <summary>
    /// Provides ability to enqueue commands to be executed on the main thread 
    /// and wait until said command is executed (via TaskCompletionSource)
    /// </summary>
    public class MainThreadCommandExecutor
    {
        private ConcurrentQueue<(ICommand, TaskCompletionSource<bool>)> _queue = 
            new ConcurrentQueue<(ICommand, TaskCompletionSource<bool>)>();

        /// <summary>
        /// Enqueues a command to be executed on the main thread and returns Task which
        /// signals when it is executed
        /// </summary>
        public Task ExecuteOnMainThread(ICommand command)
        {
            var tcs = new TaskCompletionSource<bool>();

            _queue.Enqueue((command, tcs));

            return tcs.Task;
        }

        /// <summary>
        /// Returns whether a task was executed (basically, means whether the operation was a no-op)
        /// </summary>
        public bool ExecuteNextTask()
        {
            if (_queue.IsEmpty)
                return false;

            if (!_queue.TryDequeue(out var cmd))
            {
                return false;
            }

            try
            {
                cmd.Item1.Execute();
                cmd.Item2.SetResult(true);
            }
            catch (Exception e)
            {
                cmd.Item2.SetException(e);
            }

            return true;
        }
    }
}
