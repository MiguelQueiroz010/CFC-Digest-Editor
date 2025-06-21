using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CFC_Digest_Editor.Classes
{
    internal class TM2
    {
        public class TIM2
        {
            public int Width;
            public int Height;
            public int Bpp;
            public byte[] TEX;
            public byte[] CLUT;
        }
        public static byte[] TIMN(byte[] TEX, byte[] CLUT, int width, int height, int bpp)
        {
            uint magic = 0x324D4954;//0x10 00 00 00 TIM Magic
            var insert = new List<byte>();
            insert.AddRange(BitConverter.GetBytes((UInt32)magic));
            insert.Add(4);
            insert.Add(0);
            insert.Add(1);
            insert.Add(0);
            insert.AddRange(BitConverter.GetBytes((UInt32)0));
            insert.AddRange(BitConverter.GetBytes((UInt32)0));

            int PictureSize = width * height;
            if(bpp==4)
                PictureSize /= 2;
            if(bpp==4)
                insert.AddRange(BitConverter.GetBytes((UInt32)PictureSize+0x70));
            else
                insert.AddRange(BitConverter.GetBytes((UInt32)PictureSize + 0x430));

            if (bpp == 4)
                insert.AddRange(BitConverter.GetBytes((UInt32)0x40));
            else
                insert.AddRange(BitConverter.GetBytes((UInt32)0x400));

            insert.AddRange(BitConverter.GetBytes((UInt32)PictureSize));
            insert.AddRange(BitConverter.GetBytes((UInt16)0x30));
            if (bpp == 4)
                insert.AddRange(BitConverter.GetBytes((UInt16)16));
            else
                insert.AddRange(BitConverter.GetBytes((UInt16)256));

            insert.Add(0);
            insert.Add(1);
            insert.Add(3);
            if (bpp == 4)
                insert.Add(4);
            else
                insert.Add(5);

            insert.AddRange(BitConverter.GetBytes((UInt16)width));
            insert.AddRange(BitConverter.GetBytes((UInt16)height));
            insert.AddRange(new byte[0x18]);
            insert.AddRange(TEX);
            insert.AddRange(CLUT);
            return insert.ToArray();

        }
        public static TIM2 GetClutandTex(byte[] tim)
        {
            var k = new List<byte[]>();

            var reader = new BinaryReader(new MemoryStream(tim));
            if (reader.ReadUInt32() == 0x324D4954)
            {
                var tim2 = new TIM2();
                reader.BaseStream.Position = 0x23;
                tim2.Bpp = reader.ReadByte();
                tim2.Width = reader.ReadUInt16();
                tim2.Height = reader.ReadUInt16();

                reader.BaseStream.Position = 0x40;
                int size = tim2.Width * tim2.Height;
                if(tim2.Bpp == 4)
                    size /= 2;
                tim2.TEX = reader.ReadBytes(size);
                tim2.CLUT = reader.ReadBytes(tim2.Bpp == 4 ? 0x40 : 0x400);
                

                return tim2;
            }
            return null;
        }
    }
}
