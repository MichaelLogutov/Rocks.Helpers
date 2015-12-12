using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
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
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void RetryOnException ([NotNull] this Action action,
                                             [NotNull] Func<Exception, bool> isRetriableException,
                                             int maxRetries,
                                             Action<Exception, int> logException = null)
        {
            action.RequiredNotNull ("action");
            isRetriableException.RequiredNotNull ("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    action ();
                    break;
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException (ex))
                        throw;

                    if (logException != null)
                        logException (ex, retries);
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
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static async Task RetryOnExceptionAsync ([NotNull] this Func<Task> action,
                                                        [NotNull] Func<Exception, bool> isRetriableException,
                                                        int maxRetries,
                                                        Action<Exception, int> logException = null)
        {
            action.RequiredNotNull ("action");
            isRetriableException.RequiredNotNull ("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    await action ().ConfigureAwait (false);
                    break;
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException (ex))
                        throw;

                    if (logException != null)
                        logException (ex, retries);
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
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static T RetryOnException<T> ([NotNull] this Func<T> action,
                                             [NotNull] Func<Exception, bool> isRetriableException,
                                             int maxRetries,
                                             Action<Exception, int> logException = null)
        {
            action.RequiredNotNull ("action");
            isRetriableException.RequiredNotNull ("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    return action ();
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException (ex))
                        throw;

                    if (logException != null)
                        logException (ex, retries);
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
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static async Task<T> RetryOnExceptionAsync<T> ([NotNull] this Func<Task<T>> action,
                                                              [NotNull] Func<Exception, bool> isRetriableException,
                                                              int maxRetries,
                                                              Action<Exception, int> logException = null)
        {
            action.RequiredNotNull ("action");
            isRetriableException.RequiredNotNull ("isRetriableException");

            var retries = 0;

            while (true)
            {
                try
                {
                    return await action ().ConfigureAwait (false);
                }
                catch (Exception ex)
                {
                    retries++;

                    if (retries > maxRetries || !isRetriableException (ex))
                        throw;

                    if (logException != null)
                        logException (ex, retries);
                }
            }
        }


        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<see cref="IsSqlRetriableError"/> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static T RetryOnSqlException<T> ([NotNull] this Func<T> action,
                                                int maxRetries,
                                                Action<Exception, int> logException = null)
        {
            return RetryOnException (action,
                                     ex => ex.IsSqlRetriableError (),
                                     maxRetries,
                                     logException);
        }


        /// <summary>
        ///     <para>
        ///         Retries <paramref name="action" /> maximum <paramref name="maxRetries" /> times
        ///         if catched exceptions are retriable (<see cref="IsSqlRetriableError"/> returns true for them).
        ///     </para>
        ///     <para>
        ///         Optionally logs each applicable exception before retrying the <paramref name="action" />
        ///         to specified <paramref name="logException" /> callback passing the exception and current
        ///         retry attempt count (starting with 1).
        ///     </para>
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static Task<T> RetryOnSqlExceptionAsync<T> ([NotNull] this Func<Task<T>> action,
                                                           int maxRetries,
                                                           Action<Exception, int> logException = null)
        {
            return RetryOnExceptionAsync (action,
                                          ex => ex.IsSqlRetriableError (),
                                          maxRetries,
                                          logException);
        }


        /// <summary>
        ///     Determines if specified exception is SQL timeout exception.
        /// </summary>
        public static bool IsSqlTimeoutException ([NotNull] this Exception exception)
        {
            return IsSqlTimeoutException (exception as SqlException);
        }


        /// <summary>
        ///     Determines if specified exception is SQL timeout exception.
        /// </summary>
        public static bool IsSqlTimeoutException ([CanBeNull] this SqlException exception)
        {
            return exception != null && (exception.Number == -2 || exception.Number == 11);
        }


        /// <summary>
        ///     Determines if specified exception is related to Microsoft Distributed Transaction Coordinator.
        /// </summary>
        public static bool IsMsDtcException ([NotNull] this Exception exception)
        {
            return exception.Message.Contains ("MS DTC");
        }


        /// <summary>
        ///     Determines if specified exception is <see cref="TimeoutException" /> or
        ///     have inner exception of type <see cref="TimeoutException" />.
        /// </summary>
        public static bool IsTimeoutException ([NotNull] this Exception exception)
        {
            return exception is TimeoutException || exception.InnerException is TimeoutException;
        }


        /// <summary>
        ///     Determines if specified exception is SQL deadlock exception.
        /// </summary>
        public static bool IsDeadlockException ([NotNull] this Exception exception)
        {
            var sql_exception = exception as SqlException;
            return sql_exception != null && IsDeadlockException (sql_exception);
        }


        /// <summary>
        ///     Determines if specified exception is SQL deadlock exception.
        /// </summary>
        public static bool IsDeadlockException ([NotNull] SqlException sqlException)
        {
            return sqlException.Number == 1205;
        }


        /// <summary>
        ///     Determines if specified exception is SQL unique index constraint violation exception.
        /// </summary>
        public static bool IsSqlUniqueIndexConstraintViolationException ([CanBeNull] this Exception exception)
        {
            var sql_exception = exception as SqlException;
            return sql_exception != null && IsSqlUniqueIndexConstraintViolationException (sql_exception);
        }


        /// <summary>
        ///     Determines if specified exception is SQL unique index constraint violation exception.
        /// </summary>
        public static bool IsSqlUniqueIndexConstraintViolationException ([CanBeNull] this SqlException exception)
        {
            return exception != null && (exception.Number == 2601 || exception.Number == 2627);
        }


        /// <summary>
        ///     Determines if specified exception is <see cref="TimeoutException" /> or
        ///     have inner exception of type <see cref="TimeoutException" />.
        /// </summary>
        public static bool IsTaskCancelledException ([NotNull] this Exception exception)
        {
            return exception is TaskCanceledException;
        }


        public static bool IsSqlRetriableError ([NotNull] this Exception ex)
        {
            return ex.IsTimeoutException () || ex.IsMsDtcException () || ex.IsSqlTimeoutException () || ex.IsDeadlockException ();
        }
        
    }
}