using System;
using System.Text;
namespace ShopBridgeTests
{
    public static class Helper
    {
        public static decimal RandomDecimal(long integerPartMaxNo, long decimalPartMaxNo)
        {
            var integerPartAsLong = RandomLong(1, integerPartMaxNo);
            var decimalPartAsLong = RandomLong(0, decimalPartMaxNo);

            var len = decimalPartAsLong.ToString().Length;

            var divideWith = Math.Pow(10, len);
            var decimalPart = (decimal)(decimalPartAsLong / divideWith);

            return integerPartAsLong + decimalPart;
        }

        public static string RandomString(int length)
        {
            var letterString = new StringBuilder();
            var random = new Random(Guid.NewGuid().GetHashCode());

            for (var counter = 0; counter < length; counter++)
            {
                var c = Convert.ToChar(random.Next(33, 126));
                letterString.Append(c);
            }
            return letterString.ToString();
        }

        public static string RandomLettersString(int length)
        {
            var letterString = new StringBuilder();
            var random = new Random(Guid.NewGuid().GetHashCode());

            for (var counter = 0; counter < length; counter++)
            {
                var c = Convert.ToChar(random.Next(97, 122));
                letterString.Append(c);
            }
            return letterString.ToString();
        }

        private static long RandomLong(long min, long max)
        {
            return min + (long)RandomULong(0, (ulong)Math.Abs(max - min));
        }

        private static ulong RandomULong(ulong min, ulong max)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var high = random.Next((int)(min >> 32), (int)(max >> 32));
            var minLow = Math.Min((int)min, (int)max);
            var maxLow = Math.Max((int)min, (int)max);
            var low = (uint)random.Next(minLow, maxLow);
            var result = (ulong)high;
            result <<= 32;
            result |= low;
            return result;
        }
    }
}
