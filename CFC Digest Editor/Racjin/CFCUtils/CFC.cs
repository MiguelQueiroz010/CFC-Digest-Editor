using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CFC_Digest_Editor.Classes;
using static CFC_Digest_Editor.Racjin.Assets.PAP;

namespace CFC_Digest_Editor.Racjin.CFCUtils
{    
    public class FileAllocationTable
    {
        [XmlIgnore]
        public string xmlPath { get; set; }
        public string Tool = Main.maininstance.title;
        public string BuildDate = DateTime.Today.ToString("dd/MM/yyyy");
        public string GameTitle;
        public string OutputDir;
        public int ContainerCount;
        public List<CDFileContainer> Containers = new List<CDFileContainer>();


        public FileAllocationTable Validate(FileAllocationTable table, CDFileContainer container, BinaryReader reader, bool unpack)
        {
            if (table.Containers[0].assets.Count != container.assets.Count)
            {
                return null;
            }

            int p = 0;
            try
            {
                foreach (var packet in container.assets)
                {
                    Main.maininstance.Text = $"{Main.maininstance.title} - Validating [{p + 1}/{container.assets.Count}]...";
                    var packetInfo = table.Containers[0].assets[p++];

                    reader.BaseStream.Position = packet.Offset;
                    using (var asset = new BinaryReader(new MemoryStream(!packet.IsCompressed ? reader.ReadBytes(packet.Size) : Compression.Decompress(reader.ReadBytes(packet.pSize), packet.Size))))
                    {
                        packet.GetVersion(packet, asset);
                        packet.ReadContent(packet, asset);

                        if (packetInfo.Version != packet.Version || packetInfo.SectionCount != packet.SectionCount || packetInfo.IsCompressed != packet.IsCompressed || packetInfo.Index != packet.Index)
                        {
                            return null;
                        }

                        int s = 0;
                        foreach (var section in packet.Sections)
                        {
                            var sectionInfo = packetInfo.Sections[s++];

                            if (sectionInfo.Index != section.Index || sectionInfo.ID != section.ID || sectionInfo.ContainerID != section.ContainerID || sectionInfo.FileCount != section.FileCount || sectionInfo.SpecialFlag != section.SpecialFlag || sectionInfo.Padding != section.Padding)
                            {
                                return null;
                            }
                        }

                        if (unpack && !packet.Unpack(packet, asset, Path.Combine(table.OutputDir, container.Name + "_"), packetInfo.Name))
                        {
                            return null;
                        }
                        packet.Name = packetInfo.Name;
                    }
                }
            }
            catch
            {
                return null;
            }
            table.Containers[0] = container;
            return table;
        }

        public FileAllocationTable Desserialize(FileAllocationTable table)
        {
            try
            {
                using (var xml = XmlReader.Create(xmlPath))
                {
                    xml.Read();                    
                    xml.Skip();
                    xml.Read();

                    if (xml.Name != "FileAllocationTable")
                        return null;

                    xml.Read();
                    xml.Read();

                    if ((table.Tool = xml.ReadElementContentAsString()) != "CFCDigestTool")
                        return null;

                    xml.Read();
                    xml.ReadElementContentAsString();
                    xml.Read();                    

                    table.GameTitle = xml.ReadElementContentAsString();
                    xml.Read();
                    table.OutputDir = xml.ReadElementContentAsString();
                    xml.Read();
                    table.ContainerCount = xml.ReadElementContentAsInt();
                    xml.Read();
                    xml.Read();
                    xml.Read();

                    if (xml.Name != "CDFileContainer")
                        return null;

                    var container = new CDFileContainer() { assets = new List<Packet>() };

                    xml.Read();
                    xml.Read();

                    if (!Enum.TryParse(xml.ReadElementContentAsString(), out container.Gen))
                        return null;
                    
                    xml.Read();
                    container.Name = xml.ReadElementContentAsString();                    

                    xml.Read();
                    xml.Read();
                    xml.Read();
                    
                    if (xml.Name != "Packet")
                        return null;

                    int Index = 0;
                    while (xml.Name == "Packet")
                    {
                        Main.maininstance.Text = $"{Main.maininstance.title} - Reading [{Index + 1}/{"?"}]...";

                        xml.Read();
                        xml.Read();

                        var packet = new Packet() { Name = xml.ReadElementContentAsString(), Sections = new List<Section>() };
                        xml.Read();

                        if (!Enum.TryParse(xml.ReadElementContentAsString(), out packet.Version))
                            return null;

                        xml.Read();
                        packet.SectionCount = (short)xml.ReadElementContentAsInt();
                        xml.Read();
                        packet.IsCompressed = xml.ReadElementContentAsBoolean();
                        xml.Read();
                        packet.Index = xml.ReadElementContentAsInt();

                        xml.Read();
                        xml.Read();
                        
                        for (int s = 0; s < packet.SectionCount; s++)
                        {
                            xml.Read();
                            xml.Read();
                            xml.Read();
                            
                            var section = new Section() { Name = xml.ReadElementContentAsString(), Files = new List<RFile>() };
                            xml.Read();
                            section.Index = xml.ReadElementContentAsInt();
                            xml.Read();
                            section.ID = xml.ReadElementContentAsInt();
                            xml.Read();
                            section.ContainerID = xml.ReadElementContentAsInt();
                            xml.Read();
                            section.FileCount = xml.ReadElementContentAsInt();
                            xml.Read();
                            section.SpecialFlag = xml.ReadElementContentAsInt();
                            xml.Read();
                            section.Padding = xml.ReadElementContentAsInt();

                            xml.Read();
                            xml.Read();

                            xml.Read();

                            while (xml.Name == "RFile")
                            {
                                xml.Read();
                                xml.Read();

                                var file = new RFile();

                                if (!Enum.TryParse(xml.ReadElementContentAsString(), out file.Type))
                                    return null;

                                xml.Read();
                                file.Name = xml.ReadElementContentAsString();
                                xml.Read();
                                file.Index = xml.ReadElementContentAsInt();
                                xml.Read();
                                file.AdditionalSize = xml.ReadElementContentAsInt();
                                xml.Read();
                                xml.Read();
                                xml.Read();
                                section.Files.Add(file);
                            }
                            xml.Read();
                            xml.Read();
                            xml.Read();
                            packet.Sections.Add(section);
                        }
                        
                        xml.Read();
                        xml.Read();
                        xml.Read();                        

                        xml.Read();
                        xml.Read();
                        container.assets.Add(packet);
                    }

                    xml.Read();
                    xml.Read();

                    if (xml.Name != "CDFileContainer")
                        return null;

                    table.Containers.Add(container);
                }
            }
            catch (Exception e)
            { 
                MessageBox.Show(e.Message, "Xml Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return table;
        }

        public bool Serialize(FileAllocationTable table, string output = "")
        {
            string outputdir = output == "" ? table.OutputDir : output;
            try
            {
                using (var cfcStream = new MemoryStream())
                using (var cfc = new BinaryWriter(cfcStream))
                {
                    cfc.Write(new byte[table.Containers[0].assets.Max(p => p.Index) * (table.Containers[0].Gen != CFCGEN.CDDATAOLD ? 0x10 : 0x0C)]);
                    cfc.WritePadding(0x800, 0x10);

                    int Index = 1;
                    foreach (var packetInfo in table.Containers[0].assets)
                    {
                        Main.maininstance.Text = $"{Main.maininstance.title} - Packing [{Index++}/{table.Containers[0].assets.Count}]...";

                        packetInfo.Offset = (int)(cfcStream.Length);

                        using (var packetStream = new MemoryStream())
                        using (var packet = new BinaryWriter(packetStream))
                        {
                            packet.Write(new byte[packetInfo.SectionCount * (packetInfo.Version == PacketVersion.Default ? 0x10 : 0x18)]);
                            packet.WritePadding(0x10, 0x04);

                            foreach (var sectionInfo in packetInfo.Sections)
                            {
                                sectionInfo.Offset = (int)packetStream.Length;

                                using (var sectionStream = new MemoryStream())
                                using (var section = new BinaryWriter(sectionStream))
                                {
                                    if (sectionInfo.FileCount > 0)
                                    {
                                        section.Write(new byte[4 + (sectionInfo.FileCount * 0x08)]);
                                        section.WritePadding(0x10, 0x04);
                                    }

                                    foreach (var fileInfo in sectionInfo.Files)
                                    {
                                        if (fileInfo.Type == FileType.Null)
                                            continue;

                                        fileInfo.Offset = (int)sectionStream.Length;
                                        var file = File.ReadAllBytes(Path.Combine(table.OutputDir, table.Containers[0].Name + "_", packetInfo.Name, sectionInfo.Name, fileInfo.Name));
                                        fileInfo.Size = file.Length - fileInfo.AdditionalSize;
                                        sectionInfo.Size = fileInfo.Size;

                                        section.Write(file);
                                        section.WritePadding(0x10, 0x04);
                                    }
                                    section.Write(new byte[sectionInfo.Padding]);

                                    if (sectionInfo.FileCount > 0)
                                    {
                                        sectionInfo.Size = (int)sectionStream.Length;
                                        section.BaseStream.Position = 0;
                                        section.Write(sectionInfo.FileCount);

                                        foreach (var fileInfo in sectionInfo.Files)
                                        {
                                            section.Write(fileInfo.Offset);
                                            section.Write(fileInfo.Size);
                                        }
                                    }
                                    packet.Write(sectionStream.ToArray());
                                }
                            }

                            foreach (var sectionInfo in packetInfo.Sections)
                            {
                                packet.BaseStream.Position = sectionInfo.Index * (packetInfo.Version == PacketVersion.Default ? 0x10 : 0x18);

                                packet.Write((sectionInfo.ContainerID << 12) | (sectionInfo.ID & 0xFFF));
                                packet.Write(sectionInfo.Size);
                                packet.Write(sectionInfo.Offset);
                                packet.Write(0);

                                if (packetInfo.Version == PacketVersion.Default)
                                    continue;

                                packet.Write(sectionInfo.FileCount);
                                packet.Write(sectionInfo.SpecialFlag);
                            }

                            var packetBuffer = !packetInfo.IsCompressed ? packetStream.ToArray() : Compression.Compress(packetStream.ToArray(), table.Containers[0].Gen);
                            packetInfo.Size = (int)packetStream.Length;
                            packetInfo.pSize = packetBuffer.Length;
                            cfc.Write(packetBuffer);
                        }
                        cfc.WritePadding(0x800, 0x10);
                    }

                    foreach (var packetInfo in table.Containers[0].assets)
                    {
                        cfc.BaseStream.Position = packetInfo.Index * (table.Containers[0].Gen > CFCGEN.CDDATA ? 0x10 : 0x0C);
                        packetInfo.WriteMetadata(cfc, packetInfo, table.Containers[0].Gen);
                    }

                    Directory.CreateDirectory(outputdir);

                    File.WriteAllBytes(Path.Combine(outputdir, table.Containers[0].Name), cfcStream.ToArray());
                }
                var xmlSerializer = new XmlSerializer(typeof(FileAllocationTable));

                using (var sw = new StreamWriter(File.Open(Path.Combine(outputdir, table.GameTitle + ".XML"), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite), new UTF8Encoding(false)))
                {
                    xmlSerializer.Serialize(sw, table);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public FileAllocationTable SerializeTable(FileAllocationTable table, List<string> containerPaths, bool unpack)
        {
            string[] packetNames = null;

            if (MessageBox.Show("Import file name list?", Main.maininstance.title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                var openDialog = new OpenFileDialog() { FileName = "filelist.txt ", Filter = ".txt File|(*.txt);*.txt" };

                if (openDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("You canceled!", Main.maininstance.title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    packetNames = File.ReadLines(openDialog.FileName)
                      .Select(name => Path.ChangeExtension(name, null))
                      .ToArray();
                }
            }

            foreach (var container in table.Containers)
            {
                int Index = 0;
                
                using (var reader = new BinaryReader(File.Open(containerPaths[0], FileMode.Open, FileAccess.Read, FileShare.Read)))
                {                    
                    foreach (var packetInfo in container.assets)
                    {
                        Main.maininstance.Text = $"{Main.maininstance.title} - Unpacking [{Index + 1}/{container.assets.Count}]...";
                        reader.BaseStream.Position = packetInfo.Offset;

                        using (var packet = new BinaryReader(new MemoryStream(!packetInfo.IsCompressed ? reader.ReadBytes(packetInfo.Size) : Compression.Decompress(reader.ReadBytes(packetInfo.pSize), packetInfo.Size))))
                        {
                            packetInfo.GetVersion(packetInfo, packet);
                            packetInfo.ReadContent(packetInfo, packet);

                            if (unpack && !packetInfo.Unpack(packetInfo, packet, Path.Combine(table.OutputDir, container.Name + "_"), packetNames[Index++]))
                            {
                                return null;
                            }
                        }
                    }
                }
            }

            var xmlSerializer = new XmlSerializer(typeof(FileAllocationTable));
            using (var sw = new StreamWriter(File.Open(table.xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite), new UTF8Encoding(false)))
            {
                xmlSerializer.Serialize(sw, table);
            }
            return table;
        }        
    }

    public class CDFileContainer
    {
        public string Name { get; set; }
        public CFCGEN Gen;
        public List<Packet> assets { get; set; }

        public CDFileContainer ReadTableOfContents(BinaryReader reader)
        {
            Gen = GetCFCGEN(reader);
            assets = Packet.GetPackets(reader, Gen).OrderBy(p => p.Offset).ToList();
            return this;
        }
        public static string ValidateXML(string xmlPath)
        {
            try
            {
                using (var xml = XmlReader.Create(xmlPath))
                {
                    string cfcPath;
                    bool valid = BinaryUtils.ValidPath((cfcPath = xml.Name));
                    
                    xml.Read();
                    xml.Skip();
                    xml.Read();
                    xml.Read();
                    xml.Read();

                    MessageBox.Show(xml.ReadElementContentAsString()).ToString();

                    string a = Path.Combine(new string[2] { xml.Name, xml.ReadName() }.Reverse().ToArray());
                    MessageBox.Show(a).ToString();


                    //if (xml.Read() != xml.IsStartElement() || xml.Read() != xml.IsStartElement() || !File.Exists(cfcPath = Path.Combine(Path.GetDirectoryName(xmlPath), xml.Name)) || xml.Read() != xml.IsStartElement() || xml.ReadElementContentAsString() != "CFCDigestTool")
                    if (xml.Read() != xml.IsStartElement() || xml.Read() != xml.IsStartElement() || !BinaryUtils.ValidPath(Path.Combine(Path.GetDirectoryName(xmlPath), xml.Name)) || !File.Exists(cfcPath = Path.Combine(Path.GetDirectoryName(xmlPath), xml.Name)) || xml.Read() != xml.IsStartElement() || xml.ReadElementContentAsString() != "CFCDigestTool" || xml.Read() != xml.IsStartElement())
                    {
                        MessageBox.Show("Xml Error!", Main.maininstance.title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return string.Empty;
                    }
                    else
                    {
                        MessageBox.Show(xml.Name);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Xml Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            return xmlPath;
        }                    
          
        public CFCGEN GetCFCGEN(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;

            uint flag1, flag2, flag3;

            while (true)
            {
                if (reader.ReadInt32() == 0) continue;

                flag1 = reader.ReadUInt32();
                flag2 = reader.ReadUInt32();
                flag3 = reader.ReadUInt32();
                bool useCompression = (flag2 >> 16) == 0x01;

                if (flag1 >= ushort.MaxValue || useCompression || (!useCompression && flag1 == flag3))
                {
                    return CFCGEN.CFC;
                }
                else if ((flag2 & 0xFFFF) >= 0xFF)
                    return CFCGEN.CDDATAOLD;
                else if (flag3 == 0)
                    return CFCGEN.CDDATA;
            }
        }
    }

    public class Packet
    {
        public string Name;
        public PacketVersion Version;        
        [XmlIgnore]
        public int Offset { get; set; }
        [XmlIgnore]
        public int pSize { get; set; }
        public short SectionCount { get; set; }
        public bool IsCompressed { get; set; }
        [XmlIgnore]
        public int Size { get; set; }
        public int Index { get; set; }
        public List<Section> Sections { get; set; }


        public bool Unpack(Packet packet, BinaryReader reader, string OutputDir, string path = null)
        {
            packet.Name = path ?? packet.Name;

            string dir = Path.Combine(OutputDir, packet.Name);

            try
            {
                foreach (var section in packet.Sections)
                {
                    reader.BaseStream.Position = section.Offset;

                    Directory.CreateDirectory(Path.Combine(dir, section.Name));

                    foreach (var file in section.Files)
                    {
                        if (file.Type == FileType.Null)
                            continue;

                        reader.BaseStream.Position = section.Offset + file.Offset;
                        File.WriteAllBytes(Path.Combine(dir, section.Name, section.Files[file.Index].Name), reader.ReadBytes(file.Size + file.AdditionalSize));
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static List<Packet> GetPackets(BinaryReader reader, CFCGEN Gen)
        {
            var Packets = new List<Packet>();

            int minOffset = 0x7FFFFFFF;
            int index = 0;

            try
            {
                reader.BaseStream.Position = 0;
                while (reader.BaseStream.Position < minOffset)
                {
                    var packet = new Packet().ReadMetadata(reader, Gen);
                    { packet.Index = index; packet.Name = $"Packet{index++:D2}"; }

                    if (packet.Offset == 0)
                        continue;

                    minOffset = minOffset < packet.Offset ? minOffset : packet.Offset;
                    Packets.Add(packet);
                }
                return Packets;
            }
            catch
            {
                return null;
            }
        }

        private Packet ReadMetadata(BinaryReader reader, CFCGEN Gen)
        {
            int flag0 = reader.ReadInt32();
            int flag1 = reader.ReadInt32();
            short flag2 = reader.ReadInt16();
            short flag3 = reader.ReadInt16();
            int flag4 = Gen > CFCGEN.CDDATAOLD ? reader.ReadInt32() : 0;

            Offset = flag0 * 0x800;
            Size = Gen >= CFCGEN.CFC ? flag4 : flag1 * 0x800;
            SectionCount = flag2;
            pSize = Gen == CFCGEN.CFC ? flag1 : (flag4 * 0x800);
            IsCompressed = Gen >= CFCGEN.CFC ? flag3 == 1 : pSize > 0;
            return this;
        }

        public void WriteMetadata(BinaryWriter writer, Packet packet, CFCGEN Gen)
        {
            writer.Write(packet.Offset / 0x800);
            writer.Write(Gen >= CFCGEN.CFC ? packet.pSize : packet.Size / 0x800);
            writer.Write((short)(Gen >= CFCGEN.CFC ? packet.SectionCount : (int)packet.SectionCount));

            if (Gen == CFCGEN.CFC)
            {
                writer.Write(Convert.ToInt16(packet.IsCompressed));
            }
            else if (Gen == CFCGEN.CDDATAOLD)
            {
                return;
            }
            writer.Write(Gen >= CFCGEN.CFC ? packet.Size : (packet.IsCompressed ? packet.pSize : 0));
        }


        public Packet GetVersion(Packet packet, BinaryReader reader)
        {
            if (packet.Size > 0)
            {
                reader.BaseStream.Position = 0x08;
                packet.Version = reader.ReadUInt32() == packet.SectionCount * 0x10 ? PacketVersion.Default : PacketVersion.Newer;
            }
            else
                packet.Version = 0;
            return packet;
        }

        public Packet ReadContent(Packet packet, BinaryReader reader)
        {
            packet.GetSections(packet, reader);

            foreach (var section in packet.Sections)
            {
                section.GetFiles(section, reader);
            }
            return packet;
        }

        public Packet GetSections(Packet packet, BinaryReader reader)
        {
            reader.BaseStream.Position = 0;

            packet.Sections = new List<Section>();
            for (int s = 0; s < packet.SectionCount; s++)
            {
                var section = new Section().ReadMetadata(packet.Version, reader);
                { section.Index = s; section.Name = $"Section{s:D2}"; };
                packet.Sections.Add(section);
            }
            return packet;
        }        
    }

    public class Section
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int ID { get; set; }
        public int ContainerID { get; set; }
        [XmlIgnore]
        public int Size { get; set; }
        [XmlIgnore]
        public int Offset { get; set; }
        [XmlIgnore]
        public int reserved = 0; // always 0?
        public int FileCount { get; set; } // used for bleach sc2 header
        public int SpecialFlag { get; set; } // used for bleach sc2 header
        public int Padding { get; set; }
        public List<RFile> Files { get; set; }

        public Section ReadMetadata(PacketVersion packetVersion, BinaryReader reader)
        {
            int flag = reader.ReadInt32();
            ID = flag & 0xFFF;
            ContainerID = (byte)((flag >> 12) & 0xF);
            Size = reader.ReadInt32();
            Offset = reader.ReadInt32();
            reader.ReadInt32(); // reserved

            if (packetVersion == PacketVersion.Default)
                return this;

            FileCount = reader.ReadInt32();
            SpecialFlag = reader.ReadInt32();
            return this;
        }
        
        public Section GetFiles(Section section, BinaryReader reader)
        {
            section.Files = new List<RFile>();

            section.FileCount = GetFileCount(section, reader);
            if (section.FileCount == 0)
            {
                var file = new RFile().ReadMetadata(section, reader);
                file.Name = $"File{file.Index:D2}.rct";
                section.Files.Add(file);
            }
            else
            {
                int sectionSize = BinaryUtils.GetPaddedSize(4 + (8 * FileCount), 0x10, 1);

                reader.BaseStream.Position = section.Offset + 4;
                for (int f = 0; f < FileCount; f++)
                {
                    var file = new RFile().ReadMetadata(section, reader);
                    { file.Index = f; file.Name = $"File{file.Index:D2}.rct"; };

                    if (file.Offset == 0)
                    {
                        file.Type = FileType.Null;
                        file.Name = $"PlaceHolder{file.Index:D2}";
                    }
                    else if (file.Offset > sectionSize)
                    {
                        section.Files[f - 1].Type = FileType.Linkable;
                        section.Files[f - 1].AdditionalSize = file.Size + (section.Files[f - 1].Size % 0x10);
                        sectionSize += BinaryUtils.GetPaddedSize(file.Size, 0x10, 4);
                    }
                    sectionSize += BinaryUtils.GetPaddedSize(file.Size, 0x10, 4);
                    section.Files.Add(file);
                }
                section.Padding = section.Size - sectionSize;
            }
            return section;
        }

        private static int GetFileCount(Section section, BinaryReader reader)
        {
            int flag = section.FileCount;

            reader.BaseStream.Position = section.Offset;
            if (flag == 0 && section.Size >= 4)
            {
                flag = reader.ReadInt32();

                if (section.Size % 0x10 > 0 || flag <= 0 || flag > 24 || flag == 0x15 || section.Size == 0)
                    flag = 0;
                else
                {
                    int headerSize = BinaryUtils.GetPaddedSize(4 + (8 * flag), 0x10, 1);
                    for (int f = 0; f < flag; f++)
                    {
                        int Offset = reader.ReadInt32();
                        int Size = reader.ReadInt32();

                        if (Offset == 0)
                            continue;

                        if (Offset < 0 || Size < 0 || Offset > section.Size || Size > section.Size || Offset < headerSize || (f == 0) && Offset > headerSize)
                            flag = 0;
                    }
                }
            }
            return flag;
        }
    }

    public class RFile
    {
        public string Name { get; set; }
        public int Index { get; set; }
        [XmlIgnore]
        public int Offset { get; set; }
        [XmlIgnore]
        public int Size { get; set; }
        public FileType Type;
        public int AdditionalSize { get; set; }

        public RFile ReadMetadata(Section section, BinaryReader reader)
        {
            if (section.FileCount == 0)
            {
                Size = section.Size;
            }
            else if (section.Size > 0)
            {
                Offset = reader.ReadInt32();
                Size = reader.ReadInt32();
            }
            return this;
        }
    }
}
