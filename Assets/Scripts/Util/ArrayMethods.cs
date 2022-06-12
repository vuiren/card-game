using System.Collections;
using System.Linq;

namespace Util
{
    public static class ArrayMethods
    {
        public static string TurnArrayToString<T>(T array) where T : IEnumerable
        {
            var result = "";

            foreach (var i in array)
            {
                result += i.ToString() + ',';
            }

            if (result.Length > 0)
                result = result.Remove(result.Length - 1);
            return result;
        }

        public static int[] TurnIdsStringToIntArray(string idsString)
        {
            var ids = idsString.Split(',');

            return ids.Select(int.Parse).ToArray();
        } 
    }
}