using System;

namespace NeuralNetworkLibrary.Math
{
    public class JaggedArrayCopier<T>
    {
        /// <summary>
        /// Create copy of array.
        /// </summary>
        /// <param name="source">The array to be copied</param>
        /// <returns>Copy of array</returns>
        public T[][] CreateCopy(T[][] source)
        {
            var len = source.Length;
            var dest = new T[len][];
            for (var x = 0; x < len; x++)
            {
                var inner = source[x];
                var ilen = inner.Length;
                var newer = new T[ilen];
                Array.Copy(inner, newer, ilen);
                dest[x] = newer;
            }
            return dest;
        }
    } 
}

