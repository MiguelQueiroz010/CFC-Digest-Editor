// Decompiled with JetBrains decompiler
// Type: NUC_Editor.Classes.StringTerminator
// Assembly: Naruto Uzumaki Chronicles Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E857F944-3212-478A-970A-83F52E73F042
// Assembly location: E:\Users\Miguel\Downloads\Outros\Naruto_Uzumaki_Chronicles_Editor.exe

using System.IO;
using System.Text;


namespace CFC_Digest_Editor.Classes
{
  public static class StringTerminator
  {
    public static string ReadCFCFilename(BinaryReader reader)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (byte index = reader.ReadByte(); index > (byte) 0; index = reader.ReadByte())
        stringBuilder.Append((char) index);
      return stringBuilder.ToString();
    }
  }
}
