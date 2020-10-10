using System;
using System.Collections.Generic;

namespace GameServer.Util
{
    /// <summary>
    /// This extension is used to assist in auto generating ids for jobs and rooms
    /// </summary>
    public static class SortedListExtensions
    {
        ///Add item to sortedList (numeric key) to next available key item, and return key
        public static int AddNext<T>(this SortedList<int, T> sortedList, T item)
        {
            int key = 1; // Make it 0 to start from Zero based index
            int count = sortedList.Count;

            int counter=0;
            do
            {
                if (count == 0) break;
                int nextKeyInList = sortedList.Keys[counter++];

                if (key != nextKeyInList) break;

                key = nextKeyInList +1;

                if (count == 1 || counter == count  ) break;


                if (key != sortedList.Keys[counter])
                    break;

            } while (true);

            sortedList.Add(key, item);
            return key;
        }

    }

    public static class RandomExtensions
    {
        public static float NextFloat(this Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            // choose -149 instead of -126 to also generate subnormal floats (*)
            double exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }
    }
}