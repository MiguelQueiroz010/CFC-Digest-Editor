// Decompiled with JetBrains decompiler
// Type: NUC_Editor.CFCDIGUtils.Compression
// Assembly: Naruto Uzumaki Chronicles Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E857F944-3212-478A-970A-83F52E73F042
// Assembly location: E:\Users\Miguel\Downloads\Outros\Naruto_Uzumaki_Chronicles_Editor.exe

using System.Collections.Generic;
using System.Linq;


namespace CFC_Digest_Editor.CFCDIGUtils
{
  public static class Compression
  {
    public static byte[] Decompress(byte[] buffer, int decompressedSize)
    {
      uint index1 = 0;
      uint index2 = 0;
      byte num1 = 0;
      byte[] numArray1 = new byte[256];
      uint[] numArray2 = new uint[8192];
      byte[] source = new byte[decompressedSize];
      for (uint index3 = 0; (long) index3 < (long) ((IEnumerable<byte>) buffer).Count<byte>(); ++index3)
      {
        uint num2 = ((uint) buffer[(int) index3 + 1] << 8 | (uint) buffer[(int) index3]) >> (int) num1;
        ++num1;
        if (num1 == (byte) 8)
        {
          num1 = (byte) 0;
          ++index3;
        }
        uint num3 = index2;
        if ((num2 & 256U) > 0U)
        {
          source[(int) index2] = (byte) (num2 & (uint) byte.MaxValue);
          ++index2;
        }
        else
        {
          uint index4 = (uint) (((int) (num2 >> 3) & 31) + (int) index1 * 32);
          uint index5 = numArray2[(int) index4];
          byte num4 = 0;
          while ((uint) num4 < (uint) (((int) num2 & 7) + 1))
          {
            source[(int) index2] = source[(int) index5];
            ++num4;
            ++index2;
            ++index5;
          }
        }
        if ((long) index2 < (long) ((IEnumerable<byte>) source).Count<byte>())
        {
          uint index6 = (uint) numArray1[(int) index1] + index1 * 32U;
          numArray2[(int) index6] = num3;
          numArray1[(int) index1] = (byte) ((int) numArray1[(int) index1] + 1 & 31);
          index1 = (uint) source[(int) index2 - 1];
        }
        else
          break;
      }
      return source;
    }

    public static byte[] Compress(byte[] buffer)
    {
      uint index1 = 0;
      byte num1 = 0;
      ushort[] numArray1 = new ushort[256];
      uint[] numArray2 = new uint[8192];
      List<ushort> ushortList = new List<ushort>()
      {
        Capacity = ((IEnumerable<byte>) buffer).Count<byte>() / 2
      };
      List<byte> byteList = new List<byte>()
      {
        Capacity = ((IEnumerable<byte>) buffer).Count<byte>()
      };
      uint index2 = 0;
      while ((long) index2 < (long) buffer.Length)
      {
        byte num2 = 0;
        byte num3 = 0;
        byte num4 = numArray1[(int) index1] < (ushort) 32 ? (byte) ((int) numArray1[(int) index1] & 31) : (byte) 32;
        uint num5 = index2;
        for (byte index3 = 0; (int) index3 < (int) num4; ++index3)
        {
          uint index4 = (uint) index3 + index1 * 32U;
          uint num6 = numArray2[(int) index4];
          byte num7 = 0;
          uint num8 = (long) (index2 + 8U) < (long) ((IEnumerable<byte>) buffer).Count<byte>() ? 8U : (uint) ((long) ((IEnumerable<byte>) buffer).Count<byte>() - (long) index2);
          for (byte index5 = 0; (uint) index5 < num8 && (int) buffer[(int) num6 + (int) index5] == (int) buffer[(int) index2 + (int) index5]; ++index5)
            ++num7;
          if ((int) num7 > (int) num3)
          {
            num2 = index3;
            num3 = num7;
          }
        }
        ushort num9 = 0;
        ushort num10;
        if (num3 > (byte) 0)
        {
          num10 = (ushort) ((uint) (ushort) ((uint) num9 | (uint) (ushort) ((uint) num2 << 3)) | (uint) (ushort) ((uint) num3 - 1U));
          index2 += (uint) num3;
        }
        else
        {
          num10 = (ushort) (256U | (uint) buffer[(int) index2]);
          ++index2;
        }
        ushort num11 = (ushort) ((uint) num10 << (int) num1);
        ushortList.Add(num11);
        ++num1;
        if (num1 == (byte) 8)
          num1 = (byte) 0;
        uint index6 = (uint) ((ulong) ((int) numArray1[(int) index1] & 31) + (ulong) (index1 * 32U));
        numArray2[(int) index6] = num5;
        numArray1[(int) index1] = (ushort) ((uint) numArray1[(int) index1] + 1U);
        index1 = (uint) buffer[(int) index2 - 1];
      }
      for (uint index7 = 0; (long) index7 < (long) ushortList.Count; index7 += 8U)
      {
        ulong num12 = (long) (index7 + 8U) < (long) ushortList.Count ? 8UL : (ulong) ushortList.Count - (ulong) index7;
        for (uint index8 = 0; (ulong) index8 <= num12; index8 += 2U)
        {
          ushort num13 = index8 > 0U ? ushortList[(int) index8 + (int) index7 - 1] : (ushort) 0;
          ushort num14 = (ulong) index8 < num12 ? ushortList[(int) index8 + (int) index7] : (ushort) 0;
          ushort num15 = (ulong) index8 < num12 - 1UL ? ushortList[(int) index8 + (int) index7 + 1] : (ushort) 0;
          ushort num16 = (ushort) ((int) num14 | (int) num13 >> 8 | (int) num15 << 8);
          byteList.Add((byte) ((uint) num16 & (uint) byte.MaxValue));
          if ((ulong) index8 < num12)
            byteList.Add((byte) ((uint) num16 >> 8));
        }
      }
      return byteList.ToArray();
    }
  }
}
