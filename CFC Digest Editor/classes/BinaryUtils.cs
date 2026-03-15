// Decompiled with JetBrains decompiler
// Type: NUC_Editor.Classes.BinaryUtils
// Assembly: Naruto Uzumaki Chronicles Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E857F944-3212-478A-970A-83F52E73F042
// Assembly location: E:\Users\Miguel\Downloads\Outros\Naruto_Uzumaki_Chronicles_Editor.exe

using System.IO;

namespace CFC_Digest_Editor.Classes
{
  public static class BinaryUtils
  {
    public static void WritePadding(this BinaryWriter writer, int padMultiple, int writeMultiple = 1)
    {
      while ((ulong) writer.BaseStream.Position % (ulong) padMultiple > 0UL)
        writer.Write(new byte[writeMultiple]);
    }

    public static int GetPaddedSize(int size, int padMultiple, int addMultiple = 1)
    {
      while (size % padMultiple != 0)
        size += addMultiple;
      return size;
    }
  }
}
