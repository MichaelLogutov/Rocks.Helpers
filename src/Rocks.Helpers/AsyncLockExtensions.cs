using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Rocks.Helpers
{
    public static class AsyncLockExtensions
    {
        private static readonly ConcurrentDictionary<object, LockToken> Locks =
            new ConcurrentDictionary<object, LockToken> ();

        /// <summary>
        ///     Executes <paramref name="get" /> method and returns it's value if it not null.
        ///     Otherwise acquire <paramref name="key" /> based lock, checks <paramref name="get" />
        ///     method again and if it's still null - executes <paramref name="create" /> and
        ///     returns it's value.
        /// </summary>
        public static T GetOrCreateWithLock<T> (this object key, Func<T> get, Func<T> create)
            where T : class
        {
            var result = get ();

            if (result == null)
            {
                var @lock = Locks.GetOrAdd (key, x => new LockToken ());

                @lock.Acquire ();

                try
                {
                    result = get ();
                    if (result == null)
                        result = create ();
                }
                finally
                {
                    ReleaseLock (key, @lock);
                }
            }

            return result;
        }


        /// <summary>
        ///     Executes <paramref name="get" /> method and returns it's value if it not <paramref name="nullValue" />.
        ///     Otherwise acquire <paramref name="key" /> based lock, checks <paramref name="get" />
        ///     method again and if it's still <paramref name="nullValue" /> - executes <paramref name="create" /> and
        ///     returns it's value.
        /// </summary>
        public static T GetOrCreateWithLock<T> (this object key, Func<T> get, Func<T> create, T nullValue)
            where T : struct
        {
            var result = get ();

            if (Equals (result, nullValue))
            {
                var @lock = Locks.GetOrAdd (key, x => new LockToken ());

                @lock.Acquire ();

                try
                {
                    result = get ();
                    if (Equals (result, nullValue))
                        result = create ();
                }
                finally
                {
                    ReleaseLock (key, @lock);
                }
            }

            return result;
        }


        /// <summary>
        ///     Executes <paramref name="get" /> method and returns it's value if it not null.
        ///     Otherwise acquire <paramref name="key" /> based lock, checks <paramref name="get" />
        ///     method again and if it's still null - executes <paramref name="create" /> and
        ///     returns it's value.
        /// </summary>
        public static async Task<T> GetOrCreateWithLockAsync<T> (this object key,
                                                                 Func<Task<T>> get,
                                                                 Func<Task<T>> create,
                                                                 CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            var result = await get ().ConfigureAwait (false);

            if (result == null)
            {
                var @lock = Locks.GetOrAdd (key, x => new LockToken ());

                await @lock.AcquireAsync (cancellationToken).ConfigureAwait (false);

                try
                {
                    result = await get ().ConfigureAwait (false);
                    if (result == null)
                        result = await create ().ConfigureAwait (false);
                }
                finally
                {
                    ReleaseLock (key, @lock);
                }
            }

            return result;
        }


        /// <summary>
        ///     Executes <paramref name="get" /> method and returns it's value if it not <paramref name="nullValue" />.
        ///     Otherwise acquire <paramref name="key" /> based lock, checks <paramref name="get" />
        ///     method again and if it's still <paramref name="nullValue" /> - executes <paramref name="create" /> and
        ///     returns it's value.
        /// </summary>
        public static async Task<T> GetOrCreateWithLockAsync<T> (this object key,
                                                                 Func<Task<T>> get,
                                                                 Func<Task<T>> create,
                                                                 T nullValue,
                                                                 CancellationToken cancellationToken = default(CancellationToken))
            where T : struct
        {
            var result = await get ().ConfigureAwait (false);

            if (Equals (result, nullValue))
            {
                var @lock = Locks.GetOrAdd (key, x => new LockToken ());

                await @lock.AcquireAsync (cancellationToken).ConfigureAwait (false);

                try
                {
                    result = await get ().ConfigureAwait (false);
                    if (Equals (result, nullValue))
                        result = await create ().ConfigureAwait (false);
                }
                finally
                {
                    ReleaseLock (key, @lock);
                }
            }

            return result;
        }


        /// <summary>
        ///     Acquire <paramref name="key" /> based lock, and executes <paramref name="action" />.
        /// </summary>
        public static void Lock (this object key, Action action)
        {
            var @lock = Locks.GetOrAdd (key, x => new LockToken ());

            @lock.Acquire ();

            try
            {
                action ();
            }
            finally
            {
                ReleaseLock (key, @lock);
            }
        }


        /// <summary>
        ///     Acquire <paramref name="key" /> based lock, and executes <paramref name="action" />.
        /// </summary>
        public static async Task LockAsync (this object key,
                                            Func<Task> action,
                                            CancellationToken cancellationToken = default(CancellationToken))
        {
            var @lock = Locks.GetOrAdd (key, x => new LockToken ());

            await @lock.AcquireAsync (cancellationToken).ConfigureAwait (false);

            try
            {
                await action ().ConfigureAwait (false);
            }
            finally
            {
                ReleaseLock (key, @lock);
            }
        }


        /// <summary>
        ///     Acquire <paramref name="key" /> based lock, executes <paramref name="action" />
        ///     and returns it's value.
        /// </summary>
        public static T Lock<T> (this object key, Func<T> action)
        {
            var @lock = Locks.GetOrAdd (key, x => new LockToken ());

            @lock.Acquire ();

            try
            {
                var result = action ();
                return result;
            }
            finally
            {
                ReleaseLock (key, @lock);
            }
        }


        /// <summary>
        ///     Acquire <paramref name="key" /> based lock, executes <paramref name="action" />
        ///     and returns it's value.
        /// </summary>
        public static async Task<T> LockAsync<T> (this object key,
                                                  Func<Task<T>> action,
                                                  CancellationToken cancellationToken = default(CancellationToken))
        {
            var @lock = Locks.GetOrAdd (key, x => new LockToken ());

            await @lock.AcquireAsync (cancellationToken).ConfigureAwait (false);

            try
            {
                var result = await action ().ConfigureAwait (false);
                return result;
            }
            finally
            {
                ReleaseLock (key, @lock);
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        private static void ReleaseLock (object key, LockToken @lock)
        {
            @lock.Release ();

            if (@lock.IsNotUsed)
            {
                Locks.TryRemove(key, out var tmp);
            }
        }

        private class LockToken
        {
            private readonly SemaphoreSlim semaphore;

            private int awaiting;

            public LockToken ()
            {
                this.semaphore = new SemaphoreSlim (1, 1);
            }

            public bool IsNotUsed
            {
                get { return this.awaiting == 0; }
            }

            public void Acquire ()
            {
                Interlocked.Increment (ref this.awaiting);
                this.semaphore.Wait ();
            }


            public Task AcquireAsync (CancellationToken cancellationToken = default(CancellationToken))
            {
                Interlocked.Increment (ref this.awaiting);
                return this.semaphore.WaitAsync (cancellationToken);
            }


            public void Release ()
            {
                this.semaphore.Release ();
                Interlocked.Decrement (ref this.awaiting);
            }
        }
    }
}