using CFC_Digest_Editor.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CFC_Digest_Editor.CFCDIGUtils
{
    public static class DIG
    {
        public static int PakCount;

        public static void Unpack(BinaryReader dig, BinaryReader fat, string outputPath, List<Package> PakArchiveList, List<Records> RecordsList)
        {
            string originalText = Main.maininstance.Text;
            int extractedFiles = 0;
            List<string> paddingContents = new List<string>();
            int packageIndex = 0;

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);

            foreach (Records record in RecordsList)
            {
                Package package = PakArchiveList[packageIndex];
                Main.maininstance.Text = $"{originalText} - Reading files from Packages ({packageIndex + 1}/{PakCount})...";
                packageIndex++;

                dig.BaseStream.Position = package.Offset;
                byte[] packageData = package.IsCompressed
                    ? Compression.Decompress(dig.ReadBytes(package.cSize), package.Size)
                    : dig.ReadBytes(package.Size);

                if (package.SecCount != record.SecCount)
                {
                    MessageBox.Show("CFC.DIG and FAT not matching!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Main.error = true;
                    Main.maininstance.Text = originalText;
                    return;
                }

                ExtractFilesFromPackage(fat, record, package, packageData, outputPath, ref extractedFiles, paddingContents);
            }

            if (paddingContents.Count > 0)
                File.WriteAllLines(Path.Combine(outputPath, "paddEntries.txt"), paddingContents);

            Main.maininstance.Text = originalText;
        }

        private static void ExtractFilesFromPackage(BinaryReader fat, Records record, Package package, byte[] packageData, string outputPath, ref int extractedFiles, List<string> paddingContents)
        {
            for (int sectionIndex = 0; sectionIndex < package.SecCount; sectionIndex++)
            {
                fat.BaseStream.Position = record.Offset + 12 * sectionIndex;
                int entryOffset = fat.ReadInt32();
                int fileCount = fat.ReadInt32();
                int paddedSize = fileCount == 0 ? 0 : BinaryUtils.GetPaddedSize(4 + 8 * fileCount, 16);

                if (fileCount == 0)
                {
                    ExtractSingleFile(fat, packageData, entryOffset, sectionIndex, outputPath, ref extractedFiles);
                }
                else
                {
                    ExtractMultipleFiles(fat, packageData, entryOffset, fileCount, sectionIndex, paddedSize, outputPath, ref extractedFiles, paddingContents);
                }
            }
        }

        private static void ExtractSingleFile(BinaryReader fat, byte[] packageData, int entryOffset, int sectionIndex, string outputPath, ref int extractedFiles)
        {
            fat.BaseStream.Position = entryOffset + 1;
            int fileSize = BitConverter.ToInt32(packageData, 4 + 16 * sectionIndex);
            int fileOffset = BitConverter.ToInt32(packageData, 8 + 16 * sectionIndex);
            byte[] fileData = new byte[fileSize];
            Array.Copy(packageData, fileOffset, fileData, 0, fileSize);

            string filename = StringTerminator.ReadCFCFilename(fat);
            string filePath = Path.Combine(outputPath, "data", filename);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllBytes(filePath, fileData);

            extractedFiles++;
        }

        private static void ExtractMultipleFiles(BinaryReader fat, byte[] packageData, int entryOffset, int fileCount, int sectionIndex, int paddedSize, string outputPath, ref int extractedFiles, List<string> paddingContents)
        {
            //Theus code was inverted those two causing files wrong sizes
            int totalSize = BitConverter.ToInt32(packageData, 4 + 16 * sectionIndex);
            int baseOffset = BitConverter.ToInt32(packageData, 8 + 16 * sectionIndex);

            if (BitConverter.ToInt32(packageData, baseOffset) != fileCount)
            {
                MessageBox.Show("CFC.DIG and FAT not matching!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Main.error = true;
                return;
            }

            for (int fileIndex = 0; fileIndex < fileCount; fileIndex++)
            {
                fat.BaseStream.Position = entryOffset + 8 * fileIndex;
                int nameOffset = fat.ReadInt32();

                if (nameOffset != 0)
                {
                    fat.BaseStream.Position = nameOffset + 1;
                    string filename = StringTerminator.ReadCFCFilename(fat);
                    string filePath = Path.Combine(outputPath, "data", filename);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    int fileSize = BitConverter.ToInt32(packageData, baseOffset + 8 + 8 * fileIndex);

                    int fileDataOffset = BitConverter.ToInt32(packageData, baseOffset + 4 + 8 * fileIndex);

                    if (fileSize > 0)
                    {
                        byte[] fileData = new byte[fileSize];
                        Array.Copy(packageData, baseOffset + fileDataOffset, fileData, 0, fileSize);
                        File.WriteAllBytes(filePath, fileData);
                        extractedFiles++;
                        paddedSize += BinaryUtils.GetPaddedSize(fileSize, 16);
                    }
                    else
                    {
                        File.Create(filePath).Close();
                        extractedFiles++;
                    }
                }
            }
        }

        public static void Pack(
      BinaryReader fat,
      BinaryWriter dig,
      List<Records> RecordsList,
      string datafolder)
        {
            string text = Main.maininstance.Text;
            int curfile = 0;
            List<string> stringList = new List<string>();
            int size1 = 0;
            int paddedSize1 = BinaryUtils.GetPaddedSize(DIG.PakCount * 16 + 16, 2048, 16);
            int num1 = 0;
            dig.Write(new byte[paddedSize1]);
            int num2 = 0;
            string str1 = datafolder + "\\paddEntries.txt";
            string path1 = Path.GetDirectoryName(datafolder) + "\\paddEntries.txt";
            if (File.Exists(path1))
                stringList = ((IEnumerable<string>)File.ReadAllLines(path1)).ToList<string>();
            foreach (Records records in RecordsList)
            {
                Main.maininstance.Text = string.Format("{0} - Writing Packages ({1}/{2})...", (object)text, (object)(num2 + 1), (object)DIG.PakCount);
                ++num2;
                int num3 = 0;
                int num4 = 16 * records.SecCount;
                int num5 = 0;
                for (int index1 = 0; index1 < records.SecCount; ++index1)
                {
                    fat.BaseStream.Position = (long)(records.Offset + 12 * index1);
                    int num6 = fat.ReadInt32();
                    int num7 = fat.ReadInt32();
                    int paddedSize2 = BinaryUtils.GetPaddedSize(4 + 8 * num7, 16);
                    int num8 = 0;
                    int num9 = 0;
                    dig.BaseStream.Position = (long)(paddedSize1 + num1 + num4 + num5);
                    if (num7 != 0)
                        dig.Write(num7);
                    int num10 = 0;
                    int length1;
                    string path2;
                    byte[] buffer;
                    while (true)
                    {
                        length1 = 0;
                        int index2 = stringList.FindIndex((Predicate<string>)(str => str.Contains(string.Format("index:[{0}]", (object)(curfile + 1)))));
                        if (index2 >= 0)
                            length1 = int.Parse(Regex.Match(stringList[index2 + 1], "\\d+").Value);
                        int num11 = 0;
                        if (num7 <= 0 || num10 != num7)
                        {
                            if (num7 == 0)
                            {
                                if (num6 != 0)
                                    num11 = num6 + 1;
                            }
                            else
                            {
                                fat.BaseStream.Position = (long)(num6 + 8 * num10);
                                num11 = fat.ReadInt32();
                                if (num11 != 0)
                                    ++num11;
                            }
                            if (num11 != 0)
                            {
                                fat.BaseStream.Position = (long)num11;
                                string str2 = StringTerminator.ReadCFCFilename(fat);
                                path2 = datafolder + "\\" + str2;
                                if (Directory.Exists(Path.GetDirectoryName(path2)) && File.Exists(path2))
                                {
                                    buffer = File.ReadAllBytes(path2);
                                    curfile++;
                                    dig.BaseStream.Position = (long)(paddedSize1 + num1 + num4 + num5);
                                    if (num7 != 0)
                                    {
                                        dig.BaseStream.Position += 4L;
                                        dig.BaseStream.Position += (long)(8 * num10);
                                        dig.Write(paddedSize2 + num8);
                                        int length2 = buffer.Length;
                                        dig.Write(length2);
                                        dig.BaseStream.Position = (long)(paddedSize1 + num1 + num4 + num5 + paddedSize2 + num8);
                                        dig.Write(buffer);
                                        dig.WritePadding(16);
                                        dig.Write(new byte[length1]);
                                        size1 = length2 + length1;
                                        if (size1 != 0 || length1 > 0)
                                        {
                                            num9 += BinaryUtils.GetPaddedSize(size1, 16);
                                            num8 += BinaryUtils.GetPaddedSize(size1, 16);
                                        }
                                    }
                                    else
                                        goto label_20;
                                }
                                else
                                    break;
                            }
                            ++num10;
                        }
                        else
                            goto label_26;
                    }
                    int num12 = (int)MessageBox.Show("File or directory " + path2 + " not found!", "Naruto Uzumaki Chronicles Editor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Main.error = true;
                    Main.maininstance.Text = text;
                    return;
                label_20:
                    dig.Write(buffer);
                    dig.WritePadding(16);
                    dig.Write(new byte[length1]);
                    int num13 = size1 + length1;
                    size1 = buffer.Length;
                    num9 += BinaryUtils.GetPaddedSize(size1, 16);
                label_26:
                    dig.BaseStream.Position = (long)(paddedSize1 + num1 + 16 * index1);
                    dig.Write(index1);
                    if (num7 == 0)
                    {
                        dig.Write(size1);
                        dig.Write(num4 + num5);
                        num5 += num9;
                        num3 += num9;
                    }
                    else
                    {
                        dig.Write(num9 + paddedSize2);
                        dig.Write(num4 + num5);
                        num5 += num9 + paddedSize2;
                        num3 += num9 + paddedSize2;
                    }
                }
                int size2 = num3 + num4;
                dig.BaseStream.Position = (long)(16 * num2);
                dig.Write((paddedSize1 + num1) / 2048);
                dig.Write(size2);
                dig.Write((short)records.SecCount);
                dig.Write((short)0);
                dig.Write(size2);
                num1 += BinaryUtils.GetPaddedSize(size2, 2048, 16);
            }
            dig.BaseStream.Seek(0L, SeekOrigin.End);
            dig.WritePadding(2048);
            Main.maininstance.Text = text;
        }
    }
}
