using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests
{
    public class AsyncLockExtensionsTests
    {
        [Fact]
        public void GetOrCreateWithLock_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var get_count2 = 0;
            var get_count3 = 0;
            var start1 = new Semaphore (0, 1);
            var start2 = new Semaphore (0, 1);
            var start3 = new Semaphore (0, 1);
            var end1 = new Semaphore (0, 1);
            object[] result = { null };
            object t3_result = 3;

            var t1 = new Thread (() =>
                                 {
                                     start1.WaitOne ();

                                     result[0] = key.GetOrCreateWithLock
                                         (() => null,
                                          () =>
                                          {
                                              start2.Release ();
                                              end1.WaitOne ();
                                              Thread.Sleep (200); // wait for t2 to lock

                                              return (object) 1;
                                          });
                                 });

            var t2 = new Thread (() =>
                                 {
                                     result[0] = key.GetOrCreateWithLock
                                         (() =>
                                          {
                                              if (get_count2++ == 0)
                                              {
                                                  start2.WaitOne ();
                                                  end1.Release ();
                                                  return null;
                                              }

                                              start3.Release ();
                                              Thread.Sleep (200); // wait for t1 to exit and t3 to get the lock
                                              t3_result = 4;

                                              return result[0];
                                          },
                                          () => (object) 2);
                                 });

            var t3 = new Thread (() =>
                                 {
                                     result[0] = key.GetOrCreateWithLock
                                         (() =>
                                          {
                                              if (get_count3++ == 0)
                                              {
                                                  start3.WaitOne ();
                                                  return null;
                                              }

                                              Thread.Sleep (100); // wait for t2 to exit

                                              return null;
                                          },
                                          () => t3_result);
                                 });

            // act
            t1.Start ();
            t2.Start ();
            t3.Start ();

            start1.Release ();

            if (!t3.Join (1000))
                throw new TimeoutException ();


            // assert
            result.Should ().Equal (4);
        }


        [Fact]
        public void GetOrCreateWithLock_Value_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var get_count2 = 0;
            var get_count3 = 0;
            var start1 = new Semaphore (0, 1);
            var start2 = new Semaphore (0, 1);
            var start3 = new Semaphore (0, 1);
            var end1 = new Semaphore (0, 1);
            int[] result = { 0 };
            int t3_result = 3;

            var t1 = new Thread (() =>
                                 {
                                     start1.WaitOne ();

                                     result[0] = key.GetOrCreateWithLock
                                         (() => 0,
                                          () =>
                                          {
                                              start2.Release ();
                                              end1.WaitOne ();
                                              Thread.Sleep (200); // wait for t2 to lock

                                              return 1;
                                          },
                                          0);
                                 });

            var t2 = new Thread (() =>
                                 {
                                     result[0] = key.GetOrCreateWithLock
                                         (() =>
                                          {
                                              if (get_count2++ == 0)
                                              {
                                                  start2.WaitOne ();
                                                  end1.Release ();
                                                  return 0;
                                              }

                                              start3.Release ();
                                              Thread.Sleep (200); // wait for t1 to exit and t3 to get the lock
                                              t3_result = 4;

                                              return result[0];
                                          },
                                          () => 2,
                                          0);
                                 });

            var t3 = new Thread (() =>
                                 {
                                     result[0] = key.GetOrCreateWithLock
                                         (() =>
                                          {
                                              if (get_count3++ == 0)
                                              {
                                                  start3.WaitOne ();
                                                  return 0;
                                              }

                                              Thread.Sleep (100); // wait for t2 to exit

                                              return 0;
                                          },
                                          () => t3_result,
                                          0);
                                 });

            // act
            t1.Start ();
            t2.Start ();
            t3.Start ();

            start1.Release ();

            if (!t3.Join (1000))
                throw new TimeoutException ();


            // assert
            result.Should ().Equal (4);
        }


        [Fact]
        public async Task GetOrCreateWithLockAsync_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var get_count2 = 0;
            var get_count3 = 0;
            var start1 = new SemaphoreSlim (0, 1);
            var start2 = new SemaphoreSlim (0, 1);
            var start3 = new SemaphoreSlim (0, 1);
            var end1 = new SemaphoreSlim (0, 1);
            object[] result = { null };
            object t3_result = 3;

            var timeout = new CancellationTokenSource (1000).Token;

            var t1 = Task.Run
                (async () =>
                       {
                           await start1.WaitAsync (timeout);

                           result[0] = await key.GetOrCreateWithLockAsync
                                                 (() => Task.FromResult ((object) null),
                                                  async () =>
                                                        {
                                                            start2.Release ();
                                                            await end1.WaitAsync (timeout);

                                                            await Task.Delay (200, timeout); // wait for t2 to lock

                                                            return (object) 1;
                                                        },
                                                  timeout);
                       },
                 timeout);

            var t2 = Task.Run
                (async () =>
                       {
                           result[0] = await key.GetOrCreateWithLockAsync
                                                 (async () =>
                                                        {
                                                            if (get_count2++ == 0)
                                                            {
                                                                await start2.WaitAsync (timeout);
                                                                end1.Release ();
                                                                return null;
                                                            }

                                                            start3.Release ();
                                                            await Task.Delay (200, timeout); // wait for t1 to exit and t3 to get the lock
                                                            t3_result = 4;

                                                            return result[0];
                                                        },
                                                  () => Task.FromResult ((object) 2),
                                                  timeout);
                       },
                 timeout);

            var t3 = Task.Run
                (async () =>
                       {
                           result[0] = await key.GetOrCreateWithLockAsync
                                                 (async () =>
                                                        {
                                                            if (get_count3++ == 0)
                                                            {
                                                                await start3.WaitAsync (timeout);
                                                                return null;
                                                            }

                                                            await Task.Delay (100, timeout); // wait for t2 to exit

                                                            return null;
                                                        },
                                                  () => Task.FromResult (t3_result),
                                                  timeout);
                       },
                 timeout);

            // act
            start1.Release ();

            await Task.WhenAll (t1, t2, t3);


            // assert
            result.Should ().Equal (4);
        }


        [Fact]
        public async Task GetOrCreateWithLockAsync_Value_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var get_count2 = 0;
            var get_count3 = 0;
            var start1 = new SemaphoreSlim (0, 1);
            var start2 = new SemaphoreSlim (0, 1);
            var start3 = new SemaphoreSlim (0, 1);
            var end1 = new SemaphoreSlim (0, 1);
            int[] result = { 0 };
            int t3_result = 3;

            var timeout = new CancellationTokenSource (1000).Token;

            var t1 = Task.Run
                (async () =>
                       {
                           await start1.WaitAsync (timeout);

                           result[0] = await key.GetOrCreateWithLockAsync
                                                 (() => Task.FromResult (0),
                                                  async () =>
                                                        {
                                                            start2.Release ();
                                                            await end1.WaitAsync (timeout);

                                                            await Task.Delay (200, timeout); // wait for t2 to lock

                                                            return 1;
                                                        },
                                                  0,
                                                  timeout);
                       },
                 timeout);

            var t2 = Task.Run
                (async () =>
                       {
                           result[0] = await key.GetOrCreateWithLockAsync
                                                 (async () =>
                                                        {
                                                            if (get_count2++ == 0)
                                                            {
                                                                await start2.WaitAsync (timeout);
                                                                end1.Release ();
                                                                return 0;
                                                            }

                                                            start3.Release ();
                                                            await Task.Delay (200, timeout); // wait for t1 to exit and t3 to get the lock
                                                            t3_result = 4;

                                                            return result[0];
                                                        },
                                                  () => Task.FromResult (2),
                                                  0,
                                                  timeout);
                       },
                 timeout);

            var t3 = Task.Run
                (async () =>
                       {
                           result[0] = await key.GetOrCreateWithLockAsync
                                                 (async () =>
                                                        {
                                                            if (get_count3++ == 0)
                                                            {
                                                                await start3.WaitAsync (timeout);
                                                                return 0;
                                                            }

                                                            await Task.Delay (100, timeout); // wait for t2 to exit

                                                            return 0;
                                                        },
                                                  () => Task.FromResult (t3_result),
                                                  0,
                                                  timeout);
                       },
                 timeout);

            // act
            start1.Release ();

            await Task.WhenAll (t1, t2, t3);


            // assert
            result.Should ().Equal (4);
        }


        [Fact]
        public void Lock_Action_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var start1 = new Semaphore (0, 1);
            var start2 = new Semaphore (0, 1);
            var start3 = new Semaphore (0, 1);
            object[] result = { null };
            object t3_result = 3;

            var t1 = new Thread (() =>
                                 {
                                     start1.WaitOne ();

                                     key.Lock
                                         (() =>
                                          {
                                              start2.Release ();
                                              Thread.Sleep (200); // wait for t2 to lock

                                              result[0] = 1;
                                          });
                                 });

            var t2 = new Thread (() =>
                                 {
                                     start2.WaitOne ();

                                     key.Lock
                                         (() =>
                                          {
                                              start3.Release ();
                                              Thread.Sleep (200); // wait for t1 to exit and t3 to get the lock
                                              t3_result = 4;
                                          });
                                 });

            var t3 = new Thread (() =>
                                 {
                                     start3.WaitOne ();

                                     key.Lock
                                         (() =>
                                          {
                                              Thread.Sleep (100); // wait for t2 to exit

                                              result[0] = t3_result;
                                          });
                                 });

            // act
            t1.Start ();
            t2.Start ();
            t3.Start ();

            start1.Release ();

            if (!t3.Join (1000))
                throw new TimeoutException ();


            // assert
            result.Should ().Equal (4);
        }


        [Fact]
        public void Lock_Func_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var start1 = new Semaphore (0, 1);
            var start2 = new Semaphore (0, 1);
            var start3 = new Semaphore (0, 1);
            object[] result = { null };
            object t3_result = 3;

            var t1 = new Thread (() =>
                                 {
                                     start1.WaitOne ();

                                     result[0] = key.Lock
                                         (() =>
                                          {
                                              start2.Release ();
                                              Thread.Sleep (200); // wait for t2 to lock

                                              return (object) 1;
                                          });
                                 });

            var t2 = new Thread (() =>
                                 {
                                     start2.WaitOne ();

                                     result[0] = key.Lock
                                         (() =>
                                          {
                                              start3.Release ();
                                              Thread.Sleep (200); // wait for t1 to exit and t3 to get the lock
                                              t3_result = 4;

                                              return result[0];
                                          });
                                 });

            var t3 = new Thread (() =>
                                 {
                                     start3.WaitOne ();

                                     result[0] = key.Lock
                                         (() =>
                                          {
                                              Thread.Sleep (100); // wait for t2 to exit

                                              return t3_result;
                                          });
                                 });

            // act
            t1.Start ();
            t2.Start ();
            t3.Start ();

            start1.Release ();

            if (!t3.Join (1000))
                throw new TimeoutException ();


            // assert
            result.Should ().Equal (4);
        }


        [Fact]
        public async Task LockAsync_Action_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var start1 = new SemaphoreSlim (0, 1);
            var start2 = new SemaphoreSlim (0, 1);
            var start3 = new SemaphoreSlim (0, 1);
            int[] result = { 0 };
            int t3_result = 3;

            var timeout = new CancellationTokenSource (1000).Token;

            var t1 = Task.Run
                (async () =>
                       {
                           await start1.WaitAsync (timeout);

                           await key.LockAsync
                               (async () =>
                                      {
                                          start2.Release ();
                                          await Task.Delay (200, timeout); // wait for t2 to lock

                                          result[0] = 1;
                                      },
                                timeout);
                       },
                 timeout);

            var t2 = Task.Run
                (async () =>
                       {
                           await start2.WaitAsync (timeout);

                           await key.LockAsync
                               (async () =>
                                      {
                                          start3.Release ();
                                          await Task.Delay (200, timeout); // wait for t1 to exit and t3 to get the lock
                                          t3_result = 4;
                                      },
                                timeout);
                       },
                 timeout);

            var t3 = Task.Run
                (async () =>
                       {
                           await start3.WaitAsync (timeout);

                           await key.LockAsync
                               (async () =>
                                      {
                                          await Task.Delay (100, timeout); // wait for t2 to exit

                                          result[0] = t3_result;
                                      },
                                timeout);
                       },
                 timeout);

            // act
            start1.Release ();

            await Task.WhenAll (t1, t2, t3);


            // assert
            result.Should ().Equal (4);
        }


        [Fact]
        public async Task LockAsync_Func_ThreeThreads_1ExitAfter2IsLocked_3StartsWhen2IsInCreate_3ShouldGetTheLockOnlyAfter2 ()
        {
            // arrange
            var key = new { id = Guid.NewGuid () };
            var start1 = new SemaphoreSlim (0, 1);
            var start2 = new SemaphoreSlim (0, 1);
            var start3 = new SemaphoreSlim (0, 1);
            int[] result = { 0 };
            int t3_result = 3;

            var timeout = new CancellationTokenSource (1000).Token;

            var t1 = Task.Run
                (async () =>
                       {
                           await start1.WaitAsync (timeout);

                           result[0] = await key.LockAsync
                                                 (async () =>
                                                        {
                                                            start2.Release ();
                                                            await Task.Delay (200, timeout); // wait for t2 to lock

                                                            return 1;
                                                        },
                                                  timeout);
                       },
                 timeout);

            var t2 = Task.Run
                (async () =>
                       {
                           await start2.WaitAsync (timeout);

                           result[0] = await key.LockAsync
                                                 (async () =>
                                                        {
                                                            start3.Release ();
                                                            await Task.Delay (200, timeout); // wait for t1 to exit and t3 to get the lock
                                                            t3_result = 4;

                                                            return result[0];
                                                        },
                                                  timeout);
                       },
                 timeout);

            var t3 = Task.Run
                (async () =>
                       {
                           await start3.WaitAsync (timeout);

                           result[0] = await key.LockAsync
                                                 (async () =>
                                                        {
                                                            await Task.Delay (100, timeout); // wait for t2 to exit

                                                            return t3_result;
                                                        },
                                                  timeout);
                       },
                 timeout);

            // act
            start1.Release ();

            await Task.WhenAll (t1, t2, t3);


            // assert
            result.Should ().Equal (4);
        }
    }
}