// Decompiled with JetBrains decompiler
// Type: NUC_Editor.CFCDIGUtils.Package
// Assembly: Naruto Uzumaki Chronicles Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E857F944-3212-478A-970A-83F52E73F042
// Assembly location: E:\Users\Miguel\Downloads\Outros\Naruto_Uzumaki_Chronicles_Editor.exe

using System.IO;

namespace CFC_Digest_Editor.CFCDIGUtils
{
  public class Package
  {
    private int offset;

    public int Offset
    {
      get => this.offset;
      set => this.offset = value * 2048;
    }

    public int cSize { get; set; }

    public short SecCount { get; set; }

    public bool IsCompressed { get; set; }

    public int Size { get; set; }

    public Package(BinaryReader reader)
    {
      this.Offset = reader.ReadInt32();
      this.cSize = reader.ReadInt32();
      this.SecCount = reader.ReadInt16();
      this.IsCompressed = reader.ReadInt16() == (short) 1;
      this.Size = reader.ReadInt32();
    }
  }
}
