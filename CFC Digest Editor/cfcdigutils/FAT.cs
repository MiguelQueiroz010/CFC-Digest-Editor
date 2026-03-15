// Decompiled with JetBrains decompiler
// Type: NUC_Editor.CFCDIGUtils.FAT
// Assembly: Naruto Uzumaki Chronicles Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E857F944-3212-478A-970A-83F52E73F042
// Assembly location: E:\Users\Miguel\Downloads\Outros\Naruto_Uzumaki_Chronicles_Editor.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CFC_Digest_Editor.Classes;

namespace CFC_Digest_Editor.CFCDIGUtils
{
  public static class FAT
  {
    public static void BuildFAT(BinaryReader dig, BinaryWriter fat, List<Package> Packagelist)
    {
      string text = Main.maininstance.Text;
      int padMultiple = 32 + DIG.PakCount * 24;
      fat.WritePadding(padMultiple, 4);
      int num1 = 0;
      List<int> intList1 = new List<int>();
      int num2 = 0;
      int num3 = 0;
      List<bool> boolList = new List<bool>();
      int num4 = 0;
      foreach (Package package in Packagelist)
      {
        Main.maininstance.Text = string.Format("{0} - Reading Packages ({1}/{2})...", (object) text, (object) (num4 + 1), (object) DIG.PakCount);
        dig.BaseStream.Position = (long) package.Offset;
        byte[] numArray = package.IsCompressed ? Compression.Decompress(dig.ReadBytes(package.cSize), package.Size) : dig.ReadBytes(package.Size);
        fat.BaseStream.Position = (long) (32 + 24 * num4);
        fat.Write(padMultiple);
        fat.Write((int) package.SecCount);
        fat.Write(0L);
        fat.Write(0L);
        for (int index1 = 0; index1 < (int) package.SecCount; ++index1)
        {
          int int32_1 = BitConverter.ToInt32(numArray, 4 + 16 * index1);
          int int32_2 = BitConverter.ToInt32(numArray, 8 + 16 * index1);
          int int32_3 = int32_1 != 0 ? BitConverter.ToInt32(numArray, int32_2) : 0;
          if (int32_3 == 0 || int32_3 == 21 || int32_3 > 24)
          {
            intList1.Add(0);
            boolList.Add(false);
            ++num2;
          }
          else
          {
            int paddedSize = BinaryUtils.GetPaddedSize(4 + int32_3 * 8, 16);
            bool flag = false;
            for (int index2 = 0; index2 < int32_3; ++index2)
            {
              int int32_4 = BitConverter.ToInt32(numArray, int32_2 + 4 + 8 * index2);
              if (int32_4 != 0 & int32_4 < 4 + int32_3 * 8)
              {
                flag = false;
                break;
              }
              if (int32_4 == paddedSize)
              {
                flag = DIG.PakCount <= 600 || !(index2 == int32_3 - 1 & int32_3 > 1);
                break;
              }
            }
            if (flag)
            {
              for (int index3 = 0; index3 < int32_3; ++index3)
              {
                if (BitConverter.ToInt32(numArray, int32_2 + 4 + 8 * index3) == 0)
                {
                  boolList.Add(true);
                  ++num3;
                }
                else
                {
                  boolList.Add(false);
                  ++num2;
                  ++num3;
                }
              }
              intList1.Add(int32_3);
            }
            else
            {
              intList1.Add(0);
              boolList.Add(false);
              ++num2;
            }
          }
        }
        num1 += (int) package.SecCount;
        padMultiple += (int) package.SecCount * 12;
        ++num4;
      }
      Main.maininstance.Text = text + " - Import Filelist:";
      List<string> stringList = new List<string>();
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "CFC.FAT filelist (*.txt) | *.txt";
      if (MessageBox.Show(string.Format("{0} detected files, do you have a filelist to import? (If you select no, the files will be named as numerical order!)", (object) num2), "Naruto Uzumaki Chronicles Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        if (openFileDialog.ShowDialog() != DialogResult.OK)
        {
          Main.error = true;
          Main.maininstance.Text = text;
          return;
        }
        stringList = ((IEnumerable<string>) File.ReadAllLines(openFileDialog.FileName)).ToList<string>();
      }
      else
      {
        bool flag = false;
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "(*.txt) | *.txt";
        if (MessageBox.Show("Do you want to save the filelist?", "Naruto Uzumaki Chronicles Editor", MessageBoxButtons.YesNo) == DialogResult.Yes && saveFileDialog.ShowDialog() == DialogResult.OK)
          flag = true;
        if (flag)
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) File.Open(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read)))
          {
            for (int index = 1; index <= num2; ++index)
            {
              streamWriter.WriteLine("files\\" + index.ToString("00000") + ".unk");
              stringList.Add("files\\" + index.ToString("00000") + ".unk");
            }
          }
        }
        else
        {
          for (int index = 1; index <= num2; ++index)
            stringList.Add("files\\" + index.ToString("00000") + ".unk");
        }
      }
      if (stringList.Count != num2)
      {
        int num5 = (int) MessageBox.Show(string.Format("Missing or overload of file directories! ({0} of {1})", (object) stringList.Count, (object) num2), "Naruto Uzumaki Chronicles Editor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Main.error = true;
        Main.maininstance.Text = text;
      }
      else
      {
        Main.maininstance.Text = text;
        int index4 = 0;
        int index5 = 0;
        int index6 = 0;
        List<int> intList2 = new List<int>();
        intList2.Add(padMultiple + num3 * 8);
        fat.BaseStream.Seek(0L, SeekOrigin.End);
        fat.WritePadding(intList2[0]);
        int num6 = 32 + DIG.PakCount * 24;
        foreach (Package package in Packagelist)
        {
          for (int index7 = 0; index7 < (int) package.SecCount; ++index7)
          {
            fat.BaseStream.Position = (long) (num6 + index4 * 12);
            int num7 = intList1[index4];
            if (num7 == 0)
            {
              fat.Write(intList2[index5]);
              fat.BaseStream.Position = (long) intList2[index5];
              fat.Write(stringList[index5]);
              fat.Write((byte) 0);
              intList2.Add((int) fat.BaseStream.Position);
              fat.BaseStream.Position = (long) intList2[index5];
              fat.Write((byte) 0);
              ++index5;
              ++index6;
            }
            else
            {
              fat.Write(padMultiple);
              fat.Write(num7);
              for (int index8 = 0; index8 < num7; ++index8)
              {
                fat.BaseStream.Position = (long) padMultiple;
                if (boolList[index6])
                {
                  fat.Write(0L);
                  padMultiple += 8;
                  ++index6;
                }
                else
                {
                  fat.Write((long) intList2[index5]);
                  fat.BaseStream.Position = (long) intList2[index5];
                  fat.Write(stringList[index5]);
                  fat.Write((byte) 0);
                  intList2.Add((int) fat.BaseStream.Position);
                  fat.BaseStream.Position = (long) intList2[index5];
                  fat.Write((byte) 0);
                  ++index5;
                  ++index6;
                  padMultiple += 8;
                }
              }
            }
            ++index4;
          }
        }
      }
    }
  }
}
