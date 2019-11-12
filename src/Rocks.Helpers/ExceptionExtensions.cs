using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<paramref name="isRetriableException" /> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RetryOnException([NotNull] this Action action,
                                            [NotNull] Func<Exception, bool> isRetriableException,
                                            int maxRetries,
                                            Action<Exception, int> logException = null)
        {
            action.RequiredNotNull("action");
            isRetriableException.RequiredNotNull("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException(ex))
                        throw;

                    logException?.Invoke(ex, retries);
                }
            }
        }


        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<paramref name="isRetriableException" /> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task RetryOnExceptionAsync([NotNull] this Func<Task> action,
                                                       [NotNull] Func<Exception, bool> isRetriableException,
                                                       int maxRetries,
                                                       Func<Exception, int, Task> logException = null)
        {
            action.RequiredNotNull("action");
            isRetriableException.RequiredNotNull("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    await action().ConfigureAwait(false);
                    break;
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException(ex))
                        throw;

                    if (logException != null)
                        await logException(ex, retries).ConfigureAwait(false);
                }
            }
        }


        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<paramref name="isRetriableException" /> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown when cancellation requested</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task RetryOnExceptionAsync([NotNull] this Func<CancellationToken, Task> action,
                                                       [NotNull] Func<Exception, bool> isRetriableException,
                                                       int maxRetries,
                                                       CancellationToken token,
                                                       Func<Exception, int, Task> logException = null)
        {

            action.RequiredNotNull("action");
            isRetriableException.RequiredNotNull("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await action(token).ConfigureAwait(false);
                    break;
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException(ex))
                        throw;

                    if (logException != null)
                        await logException(ex, retries).ConfigureAwait(false);
                }
            }
        }


        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<paramref name="isRetriableException" /> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RetryOnException<T>([NotNull] this Func<T> action,
                                            [NotNull] Func<Exception, bool> isRetriableException,
                                            int maxRetries,
                                            Action<Exception, int> logException = null)
        {
            action.RequiredNotNull("action");
            isRetriableException.RequiredNotNull("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException(ex))
                        throw;

                    logException?.Invoke(ex, retries);
                }
            }
        }


        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<paramref name="isRetriableException" /> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> RetryOnExceptionAsync<T>([NotNull] this Func<Task<T>> action,
                                                             [NotNull] Func<Exception, bool> isRetriableException,
                                                             int maxRetries,
                                                             Func<Exception, int, Task> logException = null)
        {
            action.RequiredNotNull("action");
            isRetriableException.RequiredNotNull("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    return await action().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException(ex))
                        throw;

                    if (logException != null)
                        await logException(ex, retries).ConfigureAwait(false);
                }
            }
        }


        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<paramref name="isRetriableException" /> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown when cancellation requested</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> RetryOnExceptionAsync<T>([NotNull] this Func<CancellationToken, Task<T>> action,
                                                             [NotNull] Func<Exception, bool> isRetriableException,
                                                             int maxRetries,
                                                             CancellationToken token,
                                                             Func<Exception, int, Task> logException = null)
        {
            action.RequiredNotNull("action");
            isRetriableException.RequiredNotNull("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    return await action(token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException(ex))
                        throw;

                    if (logException != null)
                        await logException(ex, retries).ConfigureAwait(false);
                }
            }
        }
    }
}