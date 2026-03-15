using NUC_Raw_Tools.ArquivoRAW;
using Rainbow.ImgLib.Encoding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CFC_Digest_Editor.classes.PAP;
using static IOextent;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CFC_Digest_Editor.classes
{
    //TEXTURE ARRAY
    public class IMG
    {
        public System.Windows.Forms.Button btnImport = new System.Windows.Forms.Button(), btnExport = new System.Windows.Forms.Button();

        [DllImport("ea_swizzle.dll")]
        private static extern void swizzle4(byte[] input, byte[] output, int width, int height);

        [DllImport("ea_swizzle.dll")]
        private static extern void unswizzle4(byte[] input, byte[] output, int width, int height);

        public class IMG_Entry
        {
            //0 a 0x10
            public int Id;
            public int Unk1;
            public int Unk2;
            public int Unk3;

            //0x10 a 0x20
            public int Unk4;
            public int Unk5;
            public int Unk6;
            public int Unk7;

            //0x20 a 0x30
            public int PxBlockSize;
            public ushort Unk8, Unk9, Unk10, Unk11;
            public int Unk12;

            //0x30 a 0x40
            public int Unk13;
            public int Unk14;
            public int Unk15;
            public int Unk16;

            //0x40 a 0x50
            public int Unk17;
            public int Unk18;
            public int Unk19;
            public int Unk20;

            [Category("VU_Block")]
            public int Identifier { get { return Id; } set { Id = value; } }

            [Category("VU_Block")]
            public int Unk1_ { get { return Unk1; } set { Unk1 = value; } }

            [Category("VU_Block")]
            public int Unk2_ { get { return Unk2; } set { Unk2 = value; } }

            [Category("VU_Block")]
            public int Unk3_ { get { return Unk3; } set { Unk3 = value; } }

            [Category("VU_Block")]
            public int Unk4_ { get { return Unk4; } set { Unk4 = value; } }

            [Category("VU_Block")]
            public int Unk5_ { get { return Unk5; } set { Unk5 = value; } }

            [Category("VU_Block")]
            public int Unk6_ { get { return Unk6; } set { Unk6 = value; } }

            [Category("VU_Block")]
            public int Unk7_ { get { return Unk7; } set { Unk7 = value; } }

            [Category("VU_Block")]
            public int PxBlockSize_ { get { return PxBlockSize; } set { PxBlockSize = value; } }

            [Category("VU_Block")]
            public ushort Unk8_ { get { return Unk8; } set { Unk8 = value; } }

            [Category("VU_Block")]
            public ushort Unk9_ { get { return Unk9; } set { Unk9 = value; } }

            [Category("VU_Block")]
            public ushort Unk10_ { get { return Unk10; } set { Unk10 = value; } }

            [Category("VU_Block")]
            public ushort Unk11_ { get { return Unk11; } set { Unk11 = value; } }

            [Category("VU_Block")]
            public int Unk12_ { get { return Unk12; } set { Unk12 = value; } }

            [Category("VU_Block")]
            public int Unk13_ { get { return Unk13; } set { Unk13 = value; } }

            [Category("VU_Block")]
            public int Unk14_ { get { return Unk14; } set { Unk14 = value; } }

            [Category("VU_Block")]
            public int Unk15_ { get { return Unk15; } set { Unk15 = value; } }

            [Category("VU_Block")]
            public int Unk16_ { get { return Unk16; } set { Unk16 = value; } }

            [Category("VU_Block")]
            public int Unk17_ { get { return Unk17; } set { Unk17 = value; } }

            [Category("VU_Block")]
            public int Unk18_ { get { return Unk18; } set { Unk18 = value; } }

            [Category("VU_Block")]
            public int Unk19_ { get { return Unk19; } set { Unk19 = value; } }

            [Category("VU_Block")]
            public int Unk20_ { get { return Unk20; } set { Unk20 = value; } }

            public static IMG_Entry Read(byte[] input) => new IMG_Entry()
            {
                Id = (int)input.ReadUInt(0, 32),
                Unk1 = (int)input.ReadUInt(4, 32),
                Unk2 = (int)input.ReadUInt(8, 32),
                Unk3 = (int)input.ReadUInt(12, 32),
                Unk4 = (int)input.ReadUInt(16, 32),
                Unk5 = (int)input.ReadUInt(20, 32),
                Unk6 = (int)input.ReadUInt(24, 32),
                Unk7 = (int)input.ReadUInt(28, 32),
                PxBlockSize = (int)input.ReadUInt(32, 32),
                Unk8 = (ushort)input.ReadUInt(36, 16),
                Unk9 = (ushort)input.ReadUInt(38, 16),
                Unk10 = (ushort)input.ReadUInt(40, 16),
                Unk11 = (ushort)input.ReadUInt(42, 16),
                Unk12 = (int)input.ReadUInt(44, 32),
                Unk13 = (int)input.ReadUInt(48, 32),
                Unk14 = (int)input.ReadUInt(52, 32),
                Unk15 = (int)input.ReadUInt(56, 32),
                Unk16 = (int)input.ReadUInt(60, 32),
                Unk17 = (int)input.ReadUInt(64, 32),
                Unk18 = (int)input.ReadUInt(68, 32),
                Unk19 = (int)input.ReadUInt(72, 32),
                Unk20 = (int)input.ReadUInt(76, 32)
            };

            public byte[] Rebuild()
            {
                var list = new List<byte>();
                list.AddRange(BitConverter.GetBytes((UInt32)Id));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk1));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk2));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk3));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk4));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk5));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk6));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk7));
                list.AddRange(BitConverter.GetBytes((UInt32)PxBlockSize));
                list.AddRange(BitConverter.GetBytes((UInt16)Unk8));
                list.AddRange(BitConverter.GetBytes((UInt16)Unk9));
                list.AddRange(BitConverter.GetBytes((UInt16)Unk10));
                list.AddRange(BitConverter.GetBytes((UInt16)Unk11));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk12));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk13));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk14));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk15));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk16));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk17));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk18));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk19));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk20));
                return list.ToArray();
            }
        }
        public class IndexBlock
        {
            public ushort TEX_Index;
            public ushort CLUT_Index;
            public bool UnkFlag1, UnkFlag2;
            public int Unk;

            public ushort Width;
            public ushort Height;

            public int TexOffset, ClutOffset;

            public bool WithError = false;

            public static IndexBlock Read(byte[] input) => new IndexBlock()
            {
                TEX_Index = (ushort)input.ReadUInt(0, 16),
                CLUT_Index = (ushort)input.ReadUInt(8, 16),
                UnkFlag1 = Convert.ToBoolean(input[0xA]),
                UnkFlag2 = Convert.ToBoolean(input[0xB]),
                Unk = (int)input.ReadUInt(0xC, 32),
                Width = (ushort)(input.ReadUInt(0x10, 16)),
                Height = (ushort)(input.ReadUInt(0x12, 16))
            };

            public byte[] Rebuild()
            {
                var list = new List<byte>();
                list.AddRange(BitConverter.GetBytes((UInt16)TEX_Index));
                list.AddRange(Enumerable.Range(0, 6).Select(x => (byte)0xFF));
                list.AddRange(BitConverter.GetBytes((UInt16)CLUT_Index));
                list.Add(Convert.ToByte(UnkFlag1));
                list.Add(Convert.ToByte(UnkFlag2));
                list.AddRange(BitConverter.GetBytes((UInt32)Unk));
                list.AddRange(BitConverter.GetBytes((UInt16)(Width)));
                list.AddRange(BitConverter.GetBytes((UInt16)(Height)));
                return list.ToArray();
            }
        }         
        public class Image
        {
            public byte[] TEX;
            public byte[] CLUT;

            public int Width,Wunswizz;
            public int Height, Hunswizz;
            public int Bpp;

            public int Width_A, Height_A, Width_B;
            public int CLT_Width_A, CLT_Height_A, CLT_Width_B;

            public IMG_Entry Entry;
            public IndexBlock Index;

        }

        public int Count;
        private int Data_Count;

        public List<Image> Images;
        private List<IMG_Entry> Entries;
        private List<IndexBlock> Indexes;

        private Block[] Blocks;
        public List<string> _itens = new List<string>();

        public class Folders : StringConverter
        {
            public static List<string> data
            {
                get
                {
                    return Enumerable.Range(0, _main.TEXmages.Images.Count).Select(
                        x => x.ToString()
                        ).ToList();
                }
                set { }
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true; // Habilita a lista suspensa
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true; // Restringe a escolha às opções da lista
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(data);
            }
        }
        public string Choosed = "0";

        public class Import : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal; // Define que o editor será modal (abre uma janela ao clicar)
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (context.Instance is IMG img)
                {
                    OpenFileDialog opn = new OpenFileDialog();
                    opn.Title = "Select a TIM2 Texture to import.";
                    opn.Filter = "PS2 TIM2 Texture(*.tm2)|*.tm2";
                    if (opn.ShowDialog() == DialogResult.OK)
                    {
                        var tim2 = TM2.GetClutandTex(System.IO.File.ReadAllBytes(opn.FileName));
                        int bpp = (tim2.Bpp == 5 ? 8 : 4);
                        if (tim2.Width != img.Images[Convert.ToInt32(img.Choosed)].Width 
                            || tim2.Height != img.Images[Convert.ToInt32(img.Choosed)].Height ||
                             bpp != img.Images[Convert.ToInt32(img.Choosed)].Bpp)
                        {
                            if (img.Images[Convert.ToInt32(img.Choosed)].Index.WithError == false)
                            {
                                MessageBox.Show($"Texture size/Bpp mismatch!\nExpected: " +
                                                        $"{img.Images[Convert.ToInt32(img.Choosed)].Width}x{img.Images[Convert.ToInt32(img.Choosed)].Height} - {img.Images[Convert.ToInt32(img.Choosed)].Bpp}Bpp\n" +
                                                        $"Imported: {tim2.Width}x{tim2.Height} - {bpp}Bpp", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return value;
                            }
                        }
                        var values = img.GetPixelandColorData(System.IO.File.ReadAllBytes(opn.FileName), Convert.ToInt32(img.Choosed), true);
                        img.Images[Convert.ToInt32(img.Choosed)].TEX = values[0];
                        img.Images[Convert.ToInt32(img.Choosed)].CLUT = values[1];
                        img.GetImage(Convert.ToInt32(img.Choosed), out System.Drawing.Image mage);
                        IMG._main.imageViewer.Image = mage;
                        string str = _main.path + "\\" + _main.nodepath;

                        img.ReWriteIMG(str);

                        MessageBox.Show($"Imported texture from:\n{opn.FileName}!", "Action");
                    }
                    return value; // Retorna o valor original (ou alterado, se necessário)
                }
                else
                    return value;
            }

        }
        public class ExportAll : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal; // Define que o editor será modal (abre uma janela ao clicar)
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (context.Instance is IMG img)
                {
                    var save = new FolderBrowserDialog();
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        DialogResult result = MessageBox.Show("Deseja salvar as imagens em formato TM2?", "Escolher Formato", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        bool salvarComoTm2 = result == DialogResult.Yes;

                        string dir = save.SelectedPath + @"/" + Path.GetFileNameWithoutExtension(IMG._main.nodename);
                        Directory.CreateDirectory(dir);
                        string extensao = salvarComoTm2 ? "tm2" : "png";
                        
                        // Exportando as texturas
                        for (int i = 0; i < img.Images.Count; i++)
                        {
                            int bpp = img.Images[i].Bpp;
                            var tim = img.GetImage(i, out var mage);
                            string caminho = Path.Combine(dir, $"{bpp}{Path.GetFileNameWithoutExtension(IMG._main.nodename)}_{i}.{extensao}");
                            if(salvarComoTm2)
                                File.WriteAllBytes(caminho, tim);
                            else
                                mage.Save(caminho, System.Drawing.Imaging.ImageFormat.Png);
                        }

                        MessageBox.Show($"Exported textures to:\n{save.SelectedPath + @"/" + Path.GetFileNameWithoutExtension(IMG._main.nodename)}!", "Action");
                    }
                    return value; // Retorna o valor original (ou alterado, se necessário)
                }
                else
                    return value;
            }
        }

        public static Main _main;
        // Construtor que recebe uma PictureBox para exibição das texturas
        public IMG(Main main)
        {
            _main = main;
        }

        [Category("Image Array")]
        [DisplayName("Image")]
        [TypeConverter(typeof(Folders))]
        public string Choose
        {
            get
            {
                return Choosed;
            }
            set
            {
                Choosed = value;
                GetImage(Convert.ToInt32(Choosed), out System.Drawing.Image mage);
                _main.imageViewer.Image = mage;
                OnPropertyChanged(nameof(Choosed));
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Bpp));

            }
        }

        [Category("Image Array")]
        [DisplayName("Width")]
        public int Width
        {
            get
            {
                return Images[Convert.ToInt32(Choosed)].Width;
            }
        }

        [Category("Image Array")]
        [DisplayName("Height")]
        public int Height
        {
            get
            {
                return Images[Convert.ToInt32(Choosed)].Height;
            }
        }
        [Category("Image Array")]
        [DisplayName("Bits Por Pixel")]
        public int Bpp
        {
            get
            {
                return Images[Convert.ToInt32(Choosed)].Bpp;
            }
        }

        private string _meuValor = "Import Texture...";
        [Editor(typeof(Import), typeof(UITypeEditor))]
        public string ImportOption
        {
            get { return _meuValor; }
            set { _meuValor = value; }
        }
        private string _exp = "Export All Textures";
        [Editor(typeof(ExportAll), typeof(UITypeEditor))]
        public string ExportOption
        {
            get { return _exp; }
            set { _exp = value; }
        }

        [Category("Image Array")]
        [DisplayName("With Errors?")]
        public bool Error
        {
            get
            {
                return Images[Convert.ToInt32(Choosed)].Index.WithError;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private struct Block
        {
            public int BlockOffset;

            public int Width;
            public int Height;
            public int Height2;

            public int Bpp;

            public static Block Read(byte[] input)
            {
                var block = new Block();
                block.BlockOffset = (int)input.ReadUInt(4, 32);
                block.Width = (int)input.ReadUInt(8, 16);
                block.Height = (int)input.ReadUInt(0xA, 16);
                block.Height2 = (int)input.ReadUInt(0xC, 16);
                block.Bpp = block.Width == 8 ? 2 : 1;
                return block;
            }
        }

        public IMG Read(byte[] input, PictureBox pictureBox)
        {
            var img = new IMG(_main);

            img.Count = (int)input.ReadUInt(0, 32);
            img.Data_Count = (int)input.ReadUInt(4, 32);
            
            img.Entries = Enumerable.Range(0, img.Count).Select(a =>
            IMG_Entry.Read(input.ReadBytes(0x10 + (a * 0x50), 0x50)))
                .ToList();
           
            img.Indexes = Enumerable.Range(0, img.Count).Select(c =>
            IndexBlock.Read(input.ReadBytes(0x10 + (img.Count * 0x50) + (c * 0x14), 0x14)))
                .ToList();

            img.Blocks = Enumerable.Range(0, img.Data_Count).Select(d =>
            Block.Read(input.ReadBytes(0x10 + (img.Count * 0x50) + (img.Count * 0x14) + (d * 0x10), 0x10)))
                .ToArray();

            img.Images = new List<Image>();
            int x = 0, i = 0;
            foreach (var entry in img.Indexes)
            {
                // Calculando os offsets e tamanhos antes
                int texOffset = img.Blocks[entry.TEX_Index].BlockOffset
                    + 0x10 + (img.Count * 0x50) + (img.Count * 0x14) + (0x10 * entry.TEX_Index);

                int texSize = (entry.Width * entry.Height) / img.Blocks[entry.CLUT_Index].Bpp;

                int clutOffset = img.Blocks[entry.CLUT_Index].BlockOffset
                    + 0x10 + (img.Count * 0x50) + (img.Count * 0x14) + (0x10 + (0x10 * (entry.CLUT_Index-1)));

                entry.TexOffset = texOffset;
                entry.ClutOffset = clutOffset;

                int width = entry.Width;
                int height = entry.Height;
                int wuznws = entry.Width;
                int huznws = entry.Height;
                if (texSize < (clutOffset - texOffset))
                {
                    entry.WithError = true;

                    // Corrigindo o tamanho da textura
                    texSize = (clutOffset - texOffset);
                    width = img.Blocks[entry.TEX_Index].Width + img.Blocks[entry.TEX_Index].Height2;
                    height = img.Blocks[entry.TEX_Index].Height * 4;
                    wuznws = width;
                    huznws = img.Blocks[entry.TEX_Index].Width + img.Blocks[entry.TEX_Index].Height;
                }

                
                int clutSize = img.Blocks[entry.CLUT_Index].Bpp == 2 ? 0x40 : 0x400;

                // Criando a imagem
                var newImage = new Image()
                {
                    TEX = input.ReadBytes(texOffset, texSize),
                    CLUT = input.ReadBytes(clutOffset, clutSize),

                    Width = width,
                    Height = height,
                    Bpp = img.Blocks[entry.CLUT_Index].Bpp == 2 ? 4 : 8,
                    
                    Wunswizz = wuznws,
                    Hunswizz = huznws,

                    Width_A = img.Blocks[entry.TEX_Index].Width,
                    Height_A = img.Blocks[entry.TEX_Index].Height,
                    Width_B = img.Blocks[entry.TEX_Index].Height2,

                    CLT_Width_A = img.Blocks[entry.CLUT_Index].Width,
                    CLT_Height_A = img.Blocks[entry.CLUT_Index].Height,
                    CLT_Width_B = img.Blocks[entry.CLUT_Index].Height2,

                    Entry = img.Entries[i],
                    Index = entry
                };

                // Adicionando à lista
                img.Images.Add(newImage);


                i++;
                x+=2;
            }

            _itens = Enumerable.Range(0, img.Images.Count).Select(a => $"{a}").ToList();

            return img;
        }

        public void ReWriteIMG(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            int i = 0;
            foreach (var index in Indexes)
            {
                    Array.Copy(
                    Images[i].TEX, // origem
                    0,                           // início na origem
                    bytes,                       // destino
                    index.TexOffset,            // início no destino
                    Images[i].TEX.Length // quantidade de bytes
                );
                Array.Copy(
                    Images[i].CLUT, // origem
                    0,                           // início na origem
                    bytes,                       // destino
                    index.ClutOffset,            // início no destino
                    Images[i].CLUT.Length // quantidade de bytes
                );
                i++;
            }
            File.WriteAllBytes(path, bytes);
        }
        public byte[] RebuildIMG(int bpp)
        {
            var list = new List<byte>();

            var array = new List<byte>();

            list.AddRange(BitConverter.GetBytes((UInt32)Count));
            list.AddRange(BitConverter.GetBytes((UInt32)Data_Count));
            list.AddRange(BitConverter.GetBytes((UInt64)0));

            int Offset = (Data_Count * 0x10) + 0x24;

            int k = 0;
            for (int i =0; i< Indexes.Count; i++)
            {
                //Calculando os offsets e tamanhos antes
                int texSize = Images[i].TEX.Length;
                int clutSize = Images[i].Bpp == 4 ? 0x40 : 0x400;



                Indexes[i].TEX_Index = (ushort)k;
                Indexes[i].CLUT_Index = (ushort)(k + 1);

                Blocks[k].BlockOffset = Offset;
                if (k > Images.Count)
                {
                    Blocks[k].Width = (ushort)Images[i].Width_A;
                    Blocks[k].Height = (ushort)Images[i].Height_A;
                    Blocks[k].Height2 = (ushort)Images[i].Width_B;
                }
                Offset += texSize - 0x10;

                Blocks[k + 1].BlockOffset = Offset;
                if (k + 1 > Images.Count)
                {
                    Blocks[k + 1].Width = (ushort)Images[i].CLT_Width_A;
                    Blocks[k + 1].Height = (ushort)Images[i].CLT_Height_A;
                    Blocks[k + 1].Height2 = (ushort)Images[i].CLT_Width_B;
                }
                Offset += clutSize - 0x10;


                k += 2;
            }

            foreach (var entry in Entries)
            {
                list.AddRange(entry.Rebuild());
            }
            foreach (var index in Indexes)
            {
                list.AddRange(index.Rebuild());
            }

            foreach (var block in Blocks)
            {
                list.AddRange(BitConverter.GetBytes((UInt32)0));
                list.AddRange(BitConverter.GetBytes((UInt32)block.BlockOffset));
                list.AddRange(BitConverter.GetBytes((UInt16)block.Width));
                list.AddRange(BitConverter.GetBytes((UInt16)block.Height));
                list.AddRange(BitConverter.GetBytes((UInt16)block.Height2));
                list.AddRange(BitConverter.GetBytes((UInt16)0));
            }
            while(list.Count < Blocks[0].BlockOffset + 0x10 +(0x50 * Count) + (0x14 * Count))
            {
                list.Add(0);
            }

            list.AddRange(array.ToArray());

            return list.ToArray();
        }

        byte[] unswizzledPixelData;
        public byte[] GetImage(int Index, out System.Drawing.Image mage)
        {
            System.Drawing.Image image = null;
            List<Color> pal = new List<Color>();

            int pos = 0;
            byte r, g, b, a;
            int pixelindex = 0;
            int paletteidx = 0;
            int width = Images[Index].Width;
            int height = Images[Index].Height;
            int wUnsw = width;
            int hUnsw = height;

            if (Images[Index].Index.WithError==true)
            {
                width = Images[Index].Width_A + Images[Index].Width_B;
                height = Images[Index].Height_A*4;
                wUnsw = width;
                hUnsw = Images[Index].Width_A + Images[Index].Height_A;
            }

            while (pos < Images[Index].CLUT.Length)
            {
                r = Images[Index].CLUT[pos];
                g = Images[Index].CLUT[pos + 1];
                b = Images[Index].CLUT[pos + 2];
                a = Images[Index].CLUT[pos + 3];
                if (a <= 128)
                    a = (byte)((a * 255) / 128);
                pal.Add(Color.FromArgb(a, r, g, b));
                pos += 4;
            }
            Color[] cores = unswizzlePalette(pal.ToArray());

            if (cores.Length <= 256 && cores.Length > 64)
            {
                unswizzledPixelData = UnSwizzle8(wUnsw, hUnsw, Images[Index].TEX);
                ImageDecoderIndexed imageDecoder = new ImageDecoderIndexed(unswizzledPixelData, width, height, 
                    IndexCodec.FromNumberOfColors(cores.Length, Rainbow.ImgLib.Common.ByteOrder.LittleEndian), cores);
                image = imageDecoder.DecodeImage();
                mage = image;
                return TM2.TIMN(unswizzledPixelData, ColorsToByteArray(pal.ToArray(), 8), width, height, 8);
            }
            if (cores.Length <= 16)
            {
                byte[] unswizz = ConvertPS2EA4bit(Images[Index].TEX, wUnsw, hUnsw, 4, false);
                ImageDecoderIndexed imageDecoder = new ImageDecoderIndexed(
                    unswizz,
                    width, height,
                    IndexCodec.FromNumberOfColors(16, Rainbow.ImgLib.Common.ByteOrder.LittleEndian), cores);
                image = imageDecoder.DecodeImage();
                mage = image;
                return TM2.TIMN(unswizz, ColorsToByteArray(pal.ToArray(), 4), width, height, 4);
            }
            mage = image;
            return null;
        }
        public List<byte[]> GetPixelandColorData(byte[] input, int index, bool swizzle)
        {
            var list = new List<byte[]>();
            var tim = TM2.GetClutandTex(input);

            byte[] swizzled = swizzle ? (tim.Bpp == 4 ? ConvertPS2EA4bit(tim.TEX, Images[index].Wunswizz, Images[index].Hunswizz, 4, true) :
                Swizzle8(tim.Width, tim.Height, tim.TEX)) : tim.TEX;
            list.Add(swizzled);
            int pos = 0;
            byte r, g, b, a;
            var pal = new List<Color>();

            while (pos < tim.CLUT.Length)
            {
                r = tim.CLUT[pos];
                g = tim.CLUT[pos + 1];
                b = tim.CLUT[pos + 2];
                a = tim.CLUT[pos + 3];

                pal.Add(Color.FromArgb(a, r, g, b));
                pos += 4;
            }
            Color[] cores = pal.ToArray();

            list.Add(ColorsToByteArray(cores, tim.Bpp == 4 ? 4 : 8));
            return list;
        }
        #region Swizzlers/Unswizzlers

       

        public static byte[] ConvertPS2EA4bit(byte[] imageData, int imgWidth, int imgHeight, int bpp, bool swizzleFlag)
        {
            if (bpp != 4)
                throw new Exception($"Not supported bpp={bpp} for EA swizzle!");

            byte[] convertedData = new byte[imageData.Length];

            if (swizzleFlag)
                swizzle4(imageData, convertedData, imgWidth, imgHeight);
            else
                unswizzle4(imageData, convertedData, imgWidth, imgHeight);

            return convertedData;
        }
        public static byte[] UnSwizzle(byte[] pixel, int width, int height, int bpp)
        {
            byte[] pSwizTexels = new byte[width * height / 2];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    // unswizzle
                    int pageX = x & (~0x7f);
                    int pageY = y & (~0x7f);

                    int pages_horz = (width + 127) / 128;
                    int pages_vert = (height + 127) / 128;

                    int page_number = (pageY / 128) * pages_horz + (pageX / 128);

                    int page32Y = (page_number / pages_vert) * 32;
                    int page32X = (page_number % pages_vert) * 64;

                    int page_location = page32Y * height * 2 + page32X * 4;

                    int locX = x & 0x7f;
                    int locY = y & 0x7f;

                    int block_location = ((locX & (~0x1f)) >> 1) * height + (locY & (~0xf)) * 2;
                    int swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;

                    int column_location = posY * height * 2 + ((x + swap_selector) & 0x7) * 4;

                    int byte_num = (x >> 3) & 3;     // 0,1,2,3
                    int bits_set = (y >> 1) & 1;     // 0,1 (lower/upper 4 bits)
                    int pos = page_location + block_location + column_location + byte_num;

                    // get the pen
                    if ((bits_set & 1) != 0)
                    {
                        int uPen = (pixel[pos] >> 4) & 0xf;
                        byte pix = pSwizTexels[index >> 1];
                        if ((index & 1) != 0)
                        {
                            pSwizTexels[index >> 1] = (byte)(((uPen << 4) & 0xf0) | (pix & 0xf));
                        }
                        else
                        {
                            pSwizTexels[index >> 1] = (byte)((pix & 0xf0) | (uPen & 0xf));
                        }
                    }
                    else
                    {
                        int uPen = pixel[pos] & 0xf;
                        byte pix = pSwizTexels[index >> 1];
                        if ((index & 1) != 0)
                        {
                            pSwizTexels[index >> 1] = (byte)(((uPen << 4) & 0xf0) | (pix & 0xf));
                        }
                        else
                        {
                            pSwizTexels[index >> 1] = (byte)((pix & 0xf0) | (uPen & 0xf));
                        }
                    }
                }
            }
            return pSwizTexels;

        }

        #region Code from: https://github.com/TGEnigma/DDS3-Model-Studio/blob/d96e6b3e7821f0d527105641be6e300521291e6f/Source/DDS3ModelLibrary/PS2/GS/GSPixelFormatHelper.cs
        public static byte[] UnSwizzle8(int width, int height, byte[] paletteIndices)
        {
            var newPaletteIndices = new byte[paletteIndices.Length];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int blockLocation = (y & (~0xF)) * width + (x & (~0xF)) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int positionY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = positionY * width * 2 + ((x + swapSelector) & 0x7) * 4;
                    int byteNumber = ((y >> 1) & 1) + ((x >> 2) & 2); // 0,1,2,3
                    newPaletteIndices[y * width + x] = paletteIndices[blockLocation + columnLocation + byteNumber];
                }
            }

            return newPaletteIndices;
        }
        public static byte[] Swizzle8(int width, int height, byte[] paletteIndices)
        {
            var newPaletteIndices = new byte[paletteIndices.Length];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte uPen = paletteIndices[(y * width + x)];

                    int blockLocation = (y & (~0xF)) * width + (x & (~0xF)) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int positionY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = positionY * width * 2 + ((x + swapSelector) & 0x7) * 4;
                    int byteNumber = ((y >> 1) & 1) + ((x >> 2) & 2); // 0,1,2,3

                    newPaletteIndices[blockLocation + columnLocation + byteNumber] = uPen;
                }
            }

            return newPaletteIndices;
        }
        #endregion
        public static Color[] unswizzlePalette(Color[] palette)
        {
            if (palette.Length == 256)
            {
                Color[] unswizzled = new Color[palette.Length];

                int j = 0;
                for (int i = 0; i < 256; i += 32, j += 32)
                {
                    copy(unswizzled, i, palette, j, 8);
                    copy(unswizzled, i + 16, palette, j + 8, 8);
                    copy(unswizzled, i + 8, palette, j + 16, 8);
                    copy(unswizzled, i + 24, palette, j + 24, 8);
                }
                return unswizzled;
            }
            else
            {
                return palette;
            }
        }
        public static Color[] SwizzlePalette(Color[] palette)
        {
            if (palette.Length == 16)
            {
                Color[] swizzled = new Color[palette.Length];

                swizzled[0] = palette[0];
                swizzled[1] = palette[8];
                swizzled[2] = palette[4];
                swizzled[3] = palette[12];
                swizzled[4] = palette[2];
                swizzled[5] = palette[10];
                swizzled[6] = palette[6];
                swizzled[7] = palette[14];
                swizzled[8] = palette[1];
                swizzled[9] = palette[9];
                swizzled[10] = palette[5];
                swizzled[11] = palette[13];
                swizzled[12] = palette[3];
                swizzled[13] = palette[11];
                swizzled[14] = palette[7];
                swizzled[15] = palette[15];

                return swizzled;
            }
            else
            {
                return palette;
            }
        }
        public static Color[] swizzlePalette(Color[] palette)
        {
            if (palette.Length == 256)
            {
                Color[] unswizzled = new Color[palette.Length];

                int j = 0;
                for (int i = 0; i < 256; i += 32, j += 32)
                {
                    copySW(palette, i, unswizzled, j, 8);
                    copySW(palette, i + 16, unswizzled, j + 8, 8);
                    copySW(palette, i + 8, unswizzled, j + 16, 8);
                    copySW(palette, i + 24, unswizzled, j + 24, 8);
                }
                return unswizzled;
            }
            else
            {
                return palette;
            }
        }
        private static void copy(Color[] unswizzled, int i, Color[] swizzled, int j, int num)
        {
            for (int x = 0; x < num; ++x)
            {
                unswizzled[i + x] = swizzled[j + x];
            }
        }
        private static void copySW(Color[] unswizzled, int i, Color[] swizzled, int j, int num)
        {
            for (int x = 0; x < num; ++x)
            {
                swizzled[j + x] = unswizzled[i + x];
            }
        }
        #endregion
        public static byte[] ColorsToByteArray(Color[] clut, int bpp)
        {
            int size = 256 * 4;
            if (bpp == 4)
                size = 16 * 4;
            byte[] coresbyte = new byte[size];//1024 bytes = 256 cores
            for (int i = 0; i < coresbyte.Length; i += 4)
            {
                if ((i / 4) < clut.Length)
                {
                    coresbyte[i] = clut[i / 4].R;
                    coresbyte[i + 1] = clut[i / 4].G;
                    coresbyte[i + 2] = clut[i / 4].B;
                    coresbyte[i + 3] = clut[i / 4].A;
                }

            }
            return coresbyte;
        }
    }
}
