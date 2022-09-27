using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexGame.Utils
{
    public delegate UniTask ActionAsync(CancellationToken cancellationToken);

    public delegate UniTask ActionAsync<in T>(T value, CancellationToken cancellationToken);

    public static class ActionAsyncExtensions
    {
        public static UniTask InvokeAsync(this ActionAsync handler, CancellationToken cancellationToken = default)
        {
            Delegate[] delegates = handler?.GetInvocationList();

            if (delegates == null || delegates.Length == 0)
                return UniTask.CompletedTask;

            var tasks = new UniTask[delegates.Length];

            for (var i = 0; i < delegates.Length; i++)
                tasks[i] = (UniTask) delegates[i].DynamicInvoke(cancellationToken);

            return UniTask.WhenAll(tasks);
        }
    
        public static UniTask InvokeAsync<T>(this ActionAsync<T> handler, T value, CancellationToken cancellationToken = default)
        {
            Delegate[] delegates = handler?.GetInvocationList();

            if (delegates == null || delegates.Length == 0)
                return UniTask.CompletedTask;

            var tasks = new UniTask[delegates.Length];

            for (var i = 0; i < delegates.Length; i++)
                tasks[i] = (UniTask) delegates[i].DynamicInvoke(value, cancellationToken);

            return UniTask.WhenAll(tasks);
        }
    }
}