using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace VeilMail.Utilities
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Convert an async Task to a Unity coroutine with callbacks.
        /// </summary>
        public static IEnumerator AsCoroutine<T>(this Task<T> task, Action<T> onSuccess = null, Action<Exception> onError = null)
        {
            while (!task.IsCompleted)
                yield return null;

            if (task.IsFaulted)
                onError?.Invoke(task.Exception?.InnerException ?? task.Exception);
            else if (task.IsCanceled)
                onError?.Invoke(new OperationCanceledException());
            else
                onSuccess?.Invoke(task.Result);
        }

        /// <summary>
        /// Convert an async Task (no return value) to a Unity coroutine with callbacks.
        /// </summary>
        public static IEnumerator AsCoroutine(this Task task, Action onSuccess = null, Action<Exception> onError = null)
        {
            while (!task.IsCompleted)
                yield return null;

            if (task.IsFaulted)
                onError?.Invoke(task.Exception?.InnerException ?? task.Exception);
            else if (task.IsCanceled)
                onError?.Invoke(new OperationCanceledException());
            else
                onSuccess?.Invoke();
        }
    }
}
