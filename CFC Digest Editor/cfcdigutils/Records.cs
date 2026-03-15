// Decompiled with JetBrains decompiler
// Type: NUC_Editor.CFCDIGUtils.Records
// Assembly: Naruto Uzumaki Chronicles Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E857F944-3212-478A-970A-83F52E73F042
// Assembly location: E:\Users\Miguel\Downloads\Outros\Naruto_Uzumaki_Chronicles_Editor.exe

using System.IO;

namespace CFC_Digest_Editor.CFCDIGUtils
{
  public class Records
  {
    public static int CurrentTbl;

    public int Offset { get; set; }

    public int SecCount { get; set; }

    public Records(BinaryReader reader)
    {
      reader.BaseStream.Position = (long) Records.CurrentTbl;
      this.Offset = reader.ReadInt32();
      this.SecCount = reader.ReadInt32();
    }

    public static int GetPakCount(BinaryReader reader)
    {
      int pakCount = 0;
      int num = reader.ReadInt32();
      reader.BaseStream.Position -= 4L;
      while (reader.BaseStream.Position < (long) num && reader.ReadInt32() != 0)
      {
        reader.BaseStream.Position += 20L;
        ++pakCount;
      }
      return pakCount;
    }
  }
}
