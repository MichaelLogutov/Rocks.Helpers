using System;
using System.Collections.Generic;

namespace Rocks.Helpers
{
    public class KeyEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> getKey;


        public KeyEqualityComparer (Func<T, object> getKey)
        {
            this.getKey = getKey;
        }


        public bool Equals (T x, T y)
        {
            return this.getKey (x).Equals (this.getKey (y));
        }


        public int GetHashCode (T obj)
        {
            return this.getKey (obj).GetHashCode ();
        }
    }
}