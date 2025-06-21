using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFC_Digest_Editor.Racjin.Assets
{
    public class Formats
    {
        public static string GetExtension(string path)
        {
            string extension = Path.GetExtension(path);

            using (var reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                reader.BaseStream.Position = 0x10;

                try
                {
                    if (reader.ReadUInt32() == 0x8004)
                        return ".img";
                }
                catch
                {
                    return extension;
                }
            }
            return extension;
        }
    }
}
