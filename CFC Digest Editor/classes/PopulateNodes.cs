// Decompiled with JetBrains decompiler
// Type: NUC_Editor.Classes.PopulateNodes
// Assembly: Naruto Uzumaki Chronicles Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E857F944-3212-478A-970A-83F52E73F042
// Assembly location: E:\Users\Miguel\Downloads\Outros\Naruto_Uzumaki_Chronicles_Editor.exe

using System.IO;
using System.Windows.Forms;


namespace CFC_Digest_Editor.Classes
{
  public class PopulateNodes
  {
    public static void Populate(TreeNode parentNode, DirectoryInfo directory)
    {
      foreach (DirectoryInfo directory1 in directory.GetDirectories())
      {
        TreeNode parentNode1 = parentNode.Nodes[directory1.Name] ?? parentNode.Nodes.Add(directory1.Name, directory1.Name);
        parentNode1.Tag = (object) "folder";
        parentNode1.ImageIndex = 1;
        parentNode1.SelectedImageIndex = 1;
        foreach (FileInfo file in directory1.GetFiles())
        {
          TreeNode treeNode = parentNode1.Nodes[directory1.Name] ?? parentNode1.Nodes.Add(file.Name, file.Name);
          treeNode.Tag = (object) "file";
          treeNode.ImageIndex = 0;
          treeNode.SelectedImageIndex = 0;
        }
        PopulateNodes.Populate(parentNode1, directory1);
      }
    }
  }
}
