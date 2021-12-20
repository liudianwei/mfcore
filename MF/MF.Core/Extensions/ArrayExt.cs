using System;
using System.Linq;

namespace MF.Core.Extensions
{
    public static class ArrayExt
    {
        public static Span<T> GetSpan<T>(T[] arr)
        {
            var part = new Span<T>(arr);
            return part;
        }

        public static Span<T> GetSpan<T>(T[] arr, int start, int length)
        {
            var part = new Span<T>(arr, start: start, length: length);
            return part;
        }

        public static Memory<T> GetMemory<T>(T[] arr)
        {
            var part = new Memory<T>(arr);
            return part;
        }

        public static Memory<T> GetMemory<T>(T[] arr, int start, int length)
        {
            var part = new Memory<T>(arr, start: start, length: length);
            return part;
        }

        /// <summary>
        ///  data原有的数组，req 新数组 , 得到delete、insert、update的数组
        /// </summary>
        /// <param name="oldArr"></param>
        /// <param name="newArr"></param>
        /// <returns>delete、insert、update</returns>
        public static (string[], string[], string[]) GetOperationArray(string[] oldArr, string[] newArr)
        {
            var deleteArr = oldArr.Except(newArr).ToArray();
            var insertArr = newArr.Except(oldArr).ToArray();
            var updateArr = newArr.Intersect(oldArr).ToArray();
            return (deleteArr, insertArr, updateArr);
        }
    }
}