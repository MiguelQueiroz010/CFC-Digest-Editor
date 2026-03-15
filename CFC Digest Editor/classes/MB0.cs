using NUC_Raw_Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using static NUC_Raw_Tools.ArquivoRAW.RAW;

namespace CFC_Digest_Editor.classes
{
    //TEXT
    public class MB0
    {
        public byte[] Data;
        public uint Position;
        public uint Size;
        public uint SeqCount;
        public List<byte[]> sequences;
        public List<int> seqpointers;

        private string fileName;

        public class ImportExportEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (provider?.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService edSvc)
                {
                    var form = new Form
                    {
                        Text = "Import / Export Text",
                        Width = 300,
                        Height = 100,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        StartPosition = FormStartPosition.CenterScreen
                    };

                    var btnImport = new Button { Text = "Import", Left = 30, Width = 100, Top = 20 };
                    var btnExport = new Button { Text = "Export", Left = 150, Width = 100, Top = 20 };

                    btnImport.Click += (sender, e) =>
                    {
                        var openFileDialog = new OpenFileDialog
                        {
                            Filter = "Plain Text Files (*.txt)|*.txt"
                        };
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string text = System.IO.File.ReadAllText(openFileDialog.FileName);
                            if (context.Instance is MB0 mb0)
                            {
                                mb0.TextView = text;
                            }
                            form.Close();
                        }
                    };

                    btnExport.Click += (sender, e) =>
                    {
                        var saveFileDialog = new SaveFileDialog
                        {
                            Filter = "Plain Text Files (*.txt)|*.txt",
                            FileName = context.Instance is MB0 mb01 ? Path.GetFileNameWithoutExtension(mb01.fileName) + ".txt" : "exported_text.txt"

                        };
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (context.Instance is MB0 mb0)
                            {
                                System.IO.File.WriteAllText(saveFileDialog.FileName, mb0.TextView);
                            }
                            form.Close();
                        }
                    };

                    form.Controls.Add(btnImport);
                    form.Controls.Add(btnExport);
                    edSvc.ShowDialog(form);
                }

                return value;
            }
        }
        public class RichTextEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown; // Abre uma janelinha
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {

                IWindowsFormsEditorService edSvc = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (edSvc != null)
                {
                    var richTextBox = new RichTextBox()
                    {
                        Dock = DockStyle.Fill,
                        Multiline = true,
                        WordWrap = false,
                        ScrollBars = RichTextBoxScrollBars.Both,
                        Font = new System.Drawing.Font("Consolas", 10),
                        Text = value?.ToString() ?? "",
                        Width = 300,
                        Height = 200
                    };

                    edSvc.DropDownControl(richTextBox);
                    return richTextBox.Text;
                }

                return value;
            }
        }

        [Editor(typeof(RichTextEditor), typeof(UITypeEditor))]
        [DisplayName("Text Editor")]
        public string TextView
        {
            get
            {
                return string.Join("\r\n\r\n", GetStrings());
            }
            set
            {
                var lines = value.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sequences = lines.Select(line => Encodings.Naruto.UzumakiChronicles2.GetBytes(line)).ToList();
                SeqCount = (uint)sequences.Count;
                Save();
            }
        }

        [Editor(typeof(ImportExportEditor), typeof(UITypeEditor))]
        [DisplayName("Import/Export Text")]
        [Description("Import/export sequences to .txt")]
        public string ImportExport => "Click here to begin...";

        public enum Int
        {
            Int16,
            Int32,
            Int64,
            UInt16,
            UInt32,
            UInt64
        };
        public static byte[] ReadSequence(byte[] file, int offset, string breaker)
        {
            var sequence = new List<byte>();
            var memory = new MemoryStream(file);
            var reader = new BinaryReader(memory);
            reader.BaseStream.Position = offset;
            uint pointer = reader.ReadUInt32();
            reader.Close();
            memory.Close();
            for (uint i = pointer; file[i].ToString("X2") + file[i + 1].ToString("X2") != "0080"; i += 2)
            {
                //MessageBox.Show(file[i].ToString("X2") + file[i + 1].ToString("X2"));
                sequence.Add(file[i]);
                sequence.Add(file[i + 1]);
            }
            return sequence.ToArray();
        }
        public static ulong ReadUInt(byte[] s, int offset, Int type)
        {
            ulong retur = 0;
            var memory = new MemoryStream(s);
            var reader = new BinaryReader(memory);
            try
            {
                reader.BaseStream.Position = offset;
                switch (type)
                {
                    case Int.UInt16:
                        retur = reader.ReadUInt16();
                        break;
                    case Int.UInt32:
                        retur = reader.ReadUInt32();
                        break;
                    case Int.UInt64:
                        retur = reader.ReadUInt64();
                        break;
                }
            }
            catch (Exception) { }
            reader.Close();
            memory.Close();
            return retur;
        }

        public List<string> GetStrings() => sequences.Select(seq => Encodings.Naruto.UzumakiChronicles2.GetString(seq)).ToList();
        public MB0(string path)
        {
            fileName = path;
            Data = System.IO.File.ReadAllBytes(fileName);
            sequences = new List<byte[]>();
            Position = 0;
            Size = (uint)Data.Length;
            SeqCount = (uint)ReadUInt(Data, (int)Position, Int.UInt32);
            int pos = 4;
            for (int i = 0; i < SeqCount; i++)
            {
                sequences.Add(ReadSequence(Data, pos + (i * 4), "8001"));
            }
        }
        public void Save()
        {
            //tamanho do bloco de pointers: Contagem *4 +4(contagem uint32)
            #region Iniciar Array de Ponteiro/Texto
            int tablesize = ((int)SeqCount * 4) + 4;
            var table = new List<byte>();
            table.AddRange(BitConverter.GetBytes(Convert.ToInt32(SeqCount)));
            #endregion
            int offset = tablesize;
            for (int ia = 0; ia < SeqCount; ia++)
            {
                byte[] off = BitConverter.GetBytes(Convert.ToInt32(offset));
                table.AddRange(off);
                offset += sequences[ia].Length + 2;
            }
            foreach (var s in sequences)
            {
                table.AddRange(s);
                table.Add(0);
                table.Add(0x80);
            }

            Size = (uint)table.Count;
            Data = new byte[Size];
            Data = table.ToArray();
            System.IO.File.WriteAllBytes(fileName, Data);
        }
        public int AllLength(List<byte[]> data)
        {
            int length = 0;
            foreach (var d in data)
            {
                length += d.Length + 2;
            }
            return length;
        }
    }
}
