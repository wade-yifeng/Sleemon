namespace Sleemon.Common
{
    using System;

    public static class ConvertHelper
    {
        public static int ConvertToInt(object obj)
        {
            if (obj == null) return 0;

            if (obj is int)
            {
                return (int)Convert.ChangeType(obj, TypeCode.Int32);
            }
            else
            {
                var value = 0;

                int.TryParse(obj.ToString(), out value);

                return value;
            }
        }

        public static bool ConvertToBool(object obj)
        {
            if (obj == null) return false;

            if (obj is bool)
            {
                return (bool)Convert.ChangeType(obj, TypeCode.Boolean);
            }
            else
            {
                var value = false;

                bool.TryParse(obj.ToString(), out value);

                return value;
            }
        }

        public static byte ConvertToByte(object obj)
        {
            if (obj == null) return 0;

            if (obj is byte)
            {
                return (byte) Convert.ChangeType(obj, TypeCode.Byte);
            }
            else
            {
                byte value = 0;

                byte.TryParse(obj.ToString(), out value);

                return value;
            }
        }
    }
}
