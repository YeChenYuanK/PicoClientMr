using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LekeNet
{

    internal class ConvertEndianHelper
    {
        public static long CheckEndian(long value)
        {
            short v = 0x0102;
            byte[] bytes = BitConverter.GetBytes(v);
            if (bytes[0] == 0x02)
            {
                //是小端
                UInt64 val = (UInt64)value;
                return (long)((val & 0x00000000000000FFUL) << 56 | (val & 0x000000000000FF00UL) << 40 |
                 (val & 0x0000000000FF0000UL) << 24 | (val & 0x00000000FF000000UL) << 8 |
                 (val & 0x000000FF00000000UL) >> 8 | (val & 0x0000FF0000000000UL) >> 24 |
                 (val & 0x00FF000000000000UL) >> 40 | (val & 0xFF00000000000000UL) >> 56);
            }
            else
            {
                return value;
            }
        }

        public static double CheckEndian(double value)
        {
            short v = 0x0102;
            byte[] bytes = BitConverter.GetBytes(v);
            if (bytes[0] == 0x02)
            {
                //是小端
                UInt64 val = (UInt64)value;
                return (double)((val & 0x00000000000000FFUL) << 56 | (val & 0x000000000000FF00UL) << 40 |
                 (val & 0x0000000000FF0000UL) << 24 | (val & 0x00000000FF000000UL) << 8 |
                 (val & 0x000000FF00000000UL) >> 8 | (val & 0x0000FF0000000000UL) >> 24 |
                 (val & 0x00FF000000000000UL) >> 40 | (val & 0xFF00000000000000UL) >> 56);
            }
            else
            {
                return value;
            }
        }

        public static int CheckEndian(int value)
        {
            short v = 0x0102;
            byte[] bytes = BitConverter.GetBytes(v);
            if (bytes[0] == 0x02)
            {
                //是小端
                return (int)((value & 0x000000FF) << 24 | (value & 0x0000FF00) << 8 |
                 (value & 0x00FF0000) >> 8 | (value & 0xFF000000) >> 24);
            }
            else
            {
                return value;
            }
        }

        public static short CheckEndian(short value)
        {
            short v = 0x0102;
            byte[] bytes = BitConverter.GetBytes(v);
            if (bytes[0] == 0x02)
            {
                //是小端
                return (short)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
            }
            else
            {
                return value;
            }
        }

        public static float CheckEndian(float value)
        {
            short v = 0x0102;
            byte[] bytes = BitConverter.GetBytes(v);
            if (bytes[0] == 0x02)
            {
                //是小端
                UInt32 val = (UInt32)value;
                return (float)((val & 0x000000FFU) << 24 | (val & 0x0000FF00U) << 8 |
                 (val & 0x00FF0000U) >> 8 | (val & 0xFF000000U) >> 24);
            }
            else
            {
                return value;
            }
        }
    }
}