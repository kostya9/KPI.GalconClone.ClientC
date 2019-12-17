using strange.extensions.command.api;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure
{
    public class MainThreadCommandExecutor
    {
        private ConcurrentQueue<(ICommand, TaskCompletionSource<bool>)> _queue = 
            new ConcurrentQueue<(ICommand, TaskCompletionSource<bool>)>();

        public Task ExecuteOnMainThread(ICommand command)
        {
            var tcs = new TaskCompletionSource<bool>();

            _queue.Enqueue((command, tcs));

            return tcs.Task;
        }

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
