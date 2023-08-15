using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace QuickEye.ScriptableObjectVariants
{
    public static class JObjectUtility
    {
        public static void SetArraySize(this JArray array, int newSize)
        {
            if (newSize < 0) throw new ArgumentOutOfRangeException(nameof(newSize));
            JToken lastValue = array.Last;
            var currentSize = array.Count;
            if (currentSize < newSize)
            {
                while (currentSize < newSize)
                {
                    array.Add(lastValue);
                    ++currentSize;
                }
            }
            else
            {
                while (currentSize > newSize)
                {
                    array.RemoveAt(currentSize-1);
                    ++currentSize;
                }
            }
        }
    }
}