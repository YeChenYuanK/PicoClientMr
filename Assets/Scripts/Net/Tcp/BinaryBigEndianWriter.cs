using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LekeNet
{

    class BinaryBigEndianWriter : BinaryWriter
    {
        public BinaryBigEndianWriter(Stream stream)
            : base(stream)
        {

        }

        //覆盖BinaryWriter的写入方法
        public void WriteDouble(double value)
        {
            UInt64 val = (UInt64)value;
            base.Write((double)((val & 0x00000000000000FFUL) << 56 | (val & 0x000000000000FF00UL) << 40 |
             (val & 0x0000000000FF0000UL) << 24 | (val & 0x00000000FF000000UL) << 8 |
             (val & 0x000000FF00000000UL) >> 8 | (val & 0x0000FF0000000000UL) >> 24 |
             (val & 0x00FF000000000000UL) >> 40 | (val & 0xFF00000000000000UL) >> 56));
        }

        override public void Write(float value)
        {
            UInt32 val = (UInt32)value;
            base.Write((float)((val & 0x000000FFU) << 24 | (val & 0x0000FF00U) << 8 |
             (val & 0x00FF0000U) >> 8 | (val & 0xFF000000U) >> 24));
        }

        override public void Write(int value)
        {
            base.Write((int)((value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
             (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24));
        }

        override public void Write(short value)
        {
            base.Write((short)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8));
        }

        override public void Write(long value)
        {
            UInt64 val = (UInt64)value;
            base.Write((double)((val & 0x00000000000000FFUL) << 56 | (val & 0x000000000000FF00UL) << 40 |
             (val & 0x0000000000FF0000UL) << 24 | (val & 0x00000000FF000000UL) << 8 |
             (val & 0x000000FF00000000UL) >> 8 | (val & 0x0000FF0000000000UL) >> 24 |
             (val & 0x00FF000000000000UL) >> 40 | (val & 0xFF00000000000000UL) >> 56));
        }
    }
}
