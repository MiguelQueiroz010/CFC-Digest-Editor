using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CFC_Digest_Editor.Classes;
using CFC_Digest_Editor.CFCDIGUtils;
using System.Reflection;
using NUC_Raw_Tools.ArquivoRAW;
using CFC_Digest_Editor.classes;

namespace CFC_Digest_Editor
{

    public partial class Main : Form
    {
        public string DigPath, FatPath;
        public List<string> EncodingFiles = new List<string>();
        public static string SelectedEncoding;

        public static Main maininstance;
        public static bool error;
        public string path = "C:\\tmp";
        private string title = "CFC Digest Editor";
        public string datafolder = "C:\\tmp\\data";
        public string nodepath;
        public string nodename;

        public IMG TEXmages;
        public PAP pap;
        public MB0 mb0;
        public PAP_Viewer pap_editor = new PAP_Viewer();

        public Main()
        {
            InitializeComponent();
            maininstance = this;
            DigTree.Nodes[0].Tag = (object)"nothing";
            EncodingFiles.Add("Encoding.enc");
            SelectedEncoding = "Encoding.enc";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
            => Application.Exit();

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
            => new AboutBox1(this).ShowDialog();

        private void ShowHide(Control[] controls) =>
            Array.ForEach(controls, c => c.Visible = !c.Visible);
        private static void CloneDirectory(string root, string dest)
        {
            foreach (string directory in Directory.GetDirectories(root))
            {
                string fileName = Path.GetFileName(directory);
                if (!Directory.Exists(Path.Combine(dest, fileName)))
                    Directory.CreateDirectory(Path.Combine(dest, fileName));
                Main.CloneDirectory(directory, Path.Combine(dest, fileName));
            }
            foreach (string file in Directory.GetFiles(root))
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)), true);
        }

        private void extractFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str1 = "Save here";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = str1;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            string str2 = Path.GetDirectoryName(saveFileDialog.FileName) + "\\" + this.nodename;
            if (!Directory.Exists(str2))
                Directory.CreateDirectory(str2);
            Main.maininstance.Text = this.title + " - Extracting Content...";
            Main.CloneDirectory(this.path + "/" + this.nodepath, str2);
            int num = (int)MessageBox.Show("Done.", "Naruto Uzumaki Chronicles Editor");
            Main.maininstance.Text = this.title;
        }

        private void extractFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = this.path + "\\" + this.nodepath;
            FileInfo fileInfo = new FileInfo(str);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = this.nodename;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            string fileName = saveFileDialog.FileName;
            File.Copy(str, fileName, true);
            int num = (int)MessageBox.Show("Done.", "Naruto Uzumaki Chronicles Editor");
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = this.path + "\\" + this.nodepath;
            FileInfo fileInfo = new FileInfo(str);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            File.Copy(openFileDialog.FileName, str, true);
            int num = (int)MessageBox.Show("Done.", "Naruto Uzumaki Chronicles Editor");
        }

        private void buildFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (BinaryReader binaryReader = new BinaryReader((Stream)File.Open(FatPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                binaryReader.BaseStream.Position = 32L;
                DIG.PakCount = Records.GetPakCount(binaryReader);
                List<Records> RecordsList = new List<Records>();
                Records.CurrentTbl = 32;
                for (int index = 0; index < DIG.PakCount; ++index)
                {
                    Records records = new Records(binaryReader);
                    RecordsList.Add(records);
                    Records.CurrentTbl += 24;
                }
                using (BinaryWriter dig = new BinaryWriter((Stream)File.Open(DigPath, FileMode.Create, FileAccess.Write, FileShare.Read)))
                {
                    DIG.Pack(binaryReader, dig, RecordsList, this.datafolder);
                    if (Main.error)
                        return;
                    int num = (int)MessageBox.Show("Success! All files have been packed within " + Path.GetFileName(DigPath) + ".", "Naruto Uzumaki Chronicles Editor");
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (BinaryReader binaryReader = new BinaryReader((Stream)File.Open(FatPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                binaryReader.BaseStream.Position = 32L;
                DIG.PakCount = Records.GetPakCount(binaryReader);
                List<Records> RecordsList = new List<Records>();
                Records.CurrentTbl = 32;
                for (int index = 0; index < DIG.PakCount; ++index)
                {
                    Records records = new Records(binaryReader);
                    RecordsList.Add(records);
                    Records.CurrentTbl += 24;
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CFC.DIG file (*.DIG) | *.DIG";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                string fileName = saveFileDialog.FileName;
                using (BinaryWriter dig = new BinaryWriter((Stream)File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read)))
                {
                    DIG.Pack(binaryReader, dig, RecordsList, this.datafolder);
                    if (Main.error)
                        return;
                    int num = (int)MessageBox.Show("Success! All files have been packed within " + Path.GetFileName(fileName) + ".", "Naruto Uzumaki Chronicles Editor");
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            switch (e.Node.Tag.ToString())
            {
                case "folder":
                    this.contextMenuStrip1.Show(Cursor.Position);
                    this.nodename = e.Node.Text;
                    this.nodepath = e.Node.FullPath;
                    break;
                case "file":
                    this.contextMenuStrip2.Show(Cursor.Position);
                    this.nodename = e.Node.Text;
                    this.nodepath = e.Node.FullPath;
                    break;
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string str = e.Node.Tag.ToString();
            FileInfo fileInfo = new FileInfo(e.Node.FullPath);
            if (!(str == "file"))
                return;
            this.nodename = e.Node.Text;
            this.nodepath = e.Node.FullPath;
            //switch (fileInfo.Extension)
            //{
            //    case ".img":
            //        int num = (int)MessageBox.Show(fileInfo.Extension);
            //        break;
            //}
        }
        private async void ReadDig()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "CFC.DIG file (*.DIG) | *.DIG";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;


            DigPath = openFileDialog1.FileName;
            DIG.PakCount = 0;


            using (BinaryReader binaryReader1 = new BinaryReader((Stream)File.Open(DigPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                binaryReader1.BaseStream.Position = 0x10;

                List<Package> packageList = new List<Package>();
                while (true)
                {
                    Package package = new Package(binaryReader1);
                    if (package.Offset != 0)
                    {
                        packageList.Add(package);
                        ++DIG.PakCount;
                    }
                    else
                        break;
                }
                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                openFileDialog2.Filter = "CFC.FAT file (*.FAT) | *.FAT";

                if (openFileDialog2.ShowDialog() != DialogResult.OK)
                {
                    if (MessageBox.Show("The app needs a CFC.FAT in order to organize the files, do you want to create it? (Select no if you already have it)", "Naruto Uzumaki Chronicles Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "FAT File |*.FAT";
                        if (saveFileDialog.ShowDialog() != DialogResult.OK && saveFileDialog.ShowDialog() != DialogResult.OK)
                            return;
                        using (BinaryWriter binaryWriter = new BinaryWriter((Stream)File.Open(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read)))
                        {
                            FAT.BuildFAT(binaryReader1, binaryWriter, packageList);
                            if (Main.error)
                            {
                                binaryWriter.Close();
                                File.Delete(saveFileDialog.FileName);
                                return;
                            }
                            binaryWriter.BaseStream.Seek(0L, SeekOrigin.End);
                            binaryWriter.WritePadding(16);
                            int position1 = (int)binaryWriter.BaseStream.Position;
                            binaryWriter.Write(new byte[16]);
                            int position2 = (int)binaryWriter.BaseStream.Position;
                            binaryWriter.Write(new byte[16]);
                            binaryWriter.BaseStream.Position = 0;
                            binaryWriter.Write(position1);
                            binaryWriter.Write(position2);
                            int num = (int)MessageBox.Show("Success! All packages have been recorded within " + Path.GetFileName(saveFileDialog.FileName) + ".", "Naruto Uzumaki Chronicles Editor");
                            FatPath = saveFileDialog.FileName;
                        }
                    }
                    else
                    {
                        if (openFileDialog2.ShowDialog() != DialogResult.OK)
                            return;
                        FatPath = openFileDialog2.FileName;
                    }
                }
                else
                    FatPath = openFileDialog2.FileName;
                if (Directory.Exists(this.path))
                    Directory.Delete(this.path, true);
                using (BinaryReader binaryReader2 = new BinaryReader((Stream)File.Open(FatPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    List<Records> RecordsList = new List<Records>();
                    Records.CurrentTbl = 32;
                    for (int index = 0; index < DIG.PakCount; ++index)
                    {
                        Records records = new Records(binaryReader2);
                        RecordsList.Add(records);
                        Records.CurrentTbl += 24;
                    }
#if DEBUG
                    DIG.Unpack(binaryReader1, binaryReader2, this.path, packageList, RecordsList);
#endif
#if !DEBUG
                    await Task.Run(() =>
                    {
                        DIG.Unpack(binaryReader1, binaryReader2, this.path, packageList, RecordsList);
                    });
#endif
                }
                ShowHide(new Control[1] { MainLayout });

                DirectoryInfo directory = new DirectoryInfo(this.path + "\\data");
                DigTree.Nodes[0].Tag = (object)"folder";
                PopulateNodes.Populate(this.DigTree.Nodes[0], directory);
                DigTree.Nodes[0].Expand();
                openToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }

        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e) => ReadDig();
        private void imageViewer_Click(object sender, EventArgs e)
        {

        }
        private void imageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Select where to save your TIM2 texture.";
                saveFileDialog.Filter = "PortableNetworkGrpahics(*.png)|*.png|PS2 TIM2 Texture(*.tm2)|*.tm2";
                saveFileDialog.FileName = $"{TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Bpp}{Path.GetFileNameWithoutExtension(this.nodename)}_{Convert.ToInt32(TEXmages.Choosed)}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                var tim = TEXmages.GetImage(Convert.ToInt32(TEXmages.Choosed), out var mage);
                if (saveFileDialog.FilterIndex == 2)
                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, tim);
                else
                    mage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show($"Exported texture to:\n{saveFileDialog.FileName}!", "Action");
            }
            else if (e.Button == MouseButtons.Left)
            {
                string str = this.path + "\\" + this.nodepath;

                OpenFileDialog opn = new OpenFileDialog();
                opn.Title = "Select a TIM2 Texture to import.";
                opn.Filter = "PS2 TIM2 Texture(*.tm2)|*.tm2";
                if (opn.ShowDialog() == DialogResult.OK)
                {
                    var tim2 = TM2.GetClutandTex(System.IO.File.ReadAllBytes(opn.FileName));
                    int bpp = (tim2.Bpp == 5 ? 8 : 4);
                    if (tim2.Width != TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Width ||
                        tim2.Height != TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Height ||
                       bpp != TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Bpp
                       && TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Index.WithError == false)
                    {
                        MessageBox.Show($"Texture size/Bpp mismatch!\nExpected: " +
                            $"{TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Width}x{TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Height} - {TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Bpp}Bpp\n" +
                            $"Imported: {tim2.Width}x{tim2.Height} - {bpp}Bpp", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var values = TEXmages.GetPixelandColorData(System.IO.File.ReadAllBytes(opn.FileName), Convert.ToInt32(TEXmages.Choosed), true);
                    TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].TEX = values[0];
                    TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].CLUT = values[1];
                    TEXmages.GetImage(Convert.ToInt32(TEXmages.Choosed), out System.Drawing.Image mage);
                    IMG._main.imageViewer.Image = mage;

                    TEXmages.ReWriteIMG(str);
                    //File.WriteAllBytes(str,TEXmages.RebuildIMG(tim2.Bpp));

                    MessageBox.Show($"Imported texture from:\n{opn.FileName}!", "Action");
                }
            }
        }
        private void PropertyControl_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyControl.Refresh();
        }
        private void CleanProps()
        {
            if (pap_editor.Visible)
            {
                pap_editor.Close();
            }

            if (pap != null)
            {
                viewLayout.RowStyles.RemoveAt(viewLayout.RowStyles.Count - 1);
                viewLayout.Controls.Remove(pap.button);
            }

            pap = null;
            mb0 = null;
            TEXmages = null;

            PropertyControl.SelectedObject = null;
            imageViewer.Image = null;
            imageViewer.Visible = false;
            // Linha 0 com altura absoluta de 50 pixels
            viewLayout.RowStyles[0].SizeType = SizeType.Absolute;
            viewLayout.RowStyles[0].Height = 0;
            // Linha 1 com 70% do espaço restante
            viewLayout.RowStyles[1].SizeType = SizeType.Percent;
            viewLayout.RowStyles[1].Height = 100;
        }

        private void dSIExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folder = new FolderBrowserDialog();
            folder.Description = "Select the folder where you want to extract the files.";
            if (folder.ShowDialog() != DialogResult.OK)
                return;

            string output = folder.SelectedPath+@"\";

            var open = new OpenFileDialog();
            open.Title = "Select the DSI file to extract.";
            open.Filter = "DSI file (*.DSI) | *.DSI";
            if (open.ShowDialog() != DialogResult.OK)
                return;

            string input = open.FileName;

            DSI.ExtractAndMerge(input, $"{output}{Path.GetFileNameWithoutExtension(input)}.m2v",
                $"{output}{Path.GetFileNameWithoutExtension(input)}.raw");
        }

        private void dSICompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Seleciona o novo M2V
            var openM2v = new OpenFileDialog();
            openM2v.Title = "Select the new M2V file to inject.";
            openM2v.Filter = "MPEG-2 Video (*.m2v)|*.m2v";
            if (openM2v.ShowDialog() != DialogResult.OK)
                return;

            string newM2vPath = openM2v.FileName;

            // Seleciona o novo VAG
            var openVag = new OpenFileDialog();
            openVag.Title = "Select the new VAG file to inject.";
            openVag.Filter = "VAG Audio (*.adpcm, *.raw, *.vag)|*.adpcm;*.raw;*.vag";
            if (openVag.ShowDialog() != DialogResult.OK)
                return;

            string newVagPath = openVag.FileName;

            // Seleciona o destino
            var save = new SaveFileDialog();
            save.Title = "Save the rebuilt DSI file as...";
            save.Filter = "DSI file (*.DSI)|*.DSI";
            save.FileName = Path.GetFileNameWithoutExtension(newM2vPath) + "_rebuilt.dsi";
            if (save.ShowDialog() != DialogResult.OK)
                return;

            string outputDsiPath = save.FileName;

            // Chama a função para reconstruir
            DSI.BuildDSIFromStreams(newM2vPath, newVagPath, outputDsiPath);
        }
        public void RefreshPropertyGrid()
        {
            var obj = PropertyControl.SelectedObject;
            PropertyControl.SelectedObject = null;
            PropertyControl.SelectedObject = obj;
            
        }

        private void otherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select other encoding file...";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Pega o nome com extensão (sem caminho)
                    string fileName = Path.GetFileName(openFileDialog.FileName);
                    if(EncodingFiles.Contains(fileName))
                    {
                        MessageBox.Show("This encoding file is already loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    EncodingFiles.Add(openFileDialog.FileName);
                    // Cria novo item de menu
                    ToolStripMenuItem newItem = new ToolStripMenuItem(fileName)
                    {
                        Checked = false,
                        CheckOnClick = true // permite marcar/desmarcar com clique
                        
                    };
                    newItem.Click += new EventHandler(encodingsencToolStripMenuItem_Click);
                    // Adiciona ao menu (substitua pelo nome real do seu menu)
                    var last = setEncodingsFileToolStripMenuItem.DropDownItems[setEncodingsFileToolStripMenuItem.DropDownItems.Count - 1];
                    setEncodingsFileToolStripMenuItem.DropDownItems.Insert(setEncodingsFileToolStripMenuItem.DropDownItems.IndexOf(last), newItem);
                }
            }
        }

        private void encodingsencToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in setEncodingsFileToolStripMenuItem.DropDownItems)
            {
                if (item != clickedItem && item.Text != "Other...")
                {
                    item.Checked = false;
                }
            }

            // Atualiza a variável SelectedEncoding com base no item selecionado
            if (clickedItem.Checked)
            {
                SelectedEncoding = EncodingFiles.FirstOrDefault(x => Path.GetFileName(x) == clickedItem.Text);
            }
            else
            {
                SelectedEncoding = null;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(DigTree.Nodes.Count> 0)
            {
                DigTree.Nodes.Clear();
                DigTree.Nodes.Add("data", "data",1);
                ShowHide(new Control[1] { MainLayout });
            }
            if (pap_editor.Visible)
            {
                pap_editor.Close();
            }

            if (pap != null)
            {
                viewLayout.RowStyles.RemoveAt(viewLayout.RowStyles.Count - 1);
                viewLayout.Controls.Remove(pap.button);
            }

            pap = null;
            mb0 = null;
            TEXmages = null;

            PropertyControl.SelectedObject = null;
            imageViewer.Image = null;
            imageViewer.Visible = false;
            // Linha 0 com altura absoluta de 50 pixels
            viewLayout.RowStyles[0].SizeType = SizeType.Absolute;
            viewLayout.RowStyles[0].Height = 0;
            // Linha 1 com 70% do espaço restante
            viewLayout.RowStyles[1].SizeType = SizeType.Percent;
            viewLayout.RowStyles[1].Height = 0;

            Directory.Delete(path, true);

            openToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
        }

        private void DigTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Tag.ToString())
            {
                case "file":
                    this.nodename = e.Node.Text;
                    this.nodepath = e.Node.FullPath;
                    FileInfo fileInfo = new FileInfo(e.Node.FullPath);
                    string str = this.path + "\\" + this.nodepath;
                    viewLayout.Visible = true;
                    switch (fileInfo.Extension)
                    {
                        case ".img":
                            CleanProps();

                            imageViewer.Visible = true;
                            // Linha 0 com altura absoluta de 50 pixels
                            viewLayout.RowStyles[0].SizeType = SizeType.Percent;
                            viewLayout.RowStyles[0].Height = 50;

                            // Linha 1 com 70% do espaço restante
                            viewLayout.RowStyles[1].SizeType = SizeType.Percent;
                            viewLayout.RowStyles[1].Height = 50;
                            TEXmages = new IMG(this).Read(File.ReadAllBytes(str), imageViewer);
                            TEXmages.GetImage(0, out var mage);

                            imageViewer.Image = mage;
                            PropertyControl.SelectedObject = TEXmages;
                            break;
                        case ".mb0":
                            CleanProps();
                            mb0 = new MB0(str);
                            PropertyControl.SelectedObject = mb0;
                            imageViewer.Visible = false;

                            // Linha 0 com altura absoluta de 50 pixels
                            viewLayout.RowStyles[0].SizeType = SizeType.Absolute;
                            viewLayout.RowStyles[0].Height = 0;

                            // Linha 1 com 70% do espaço restante
                            viewLayout.RowStyles[1].SizeType = SizeType.Percent;
                            viewLayout.RowStyles[1].Height = 100;
                            break;
                        case ".pap":
                            CleanProps();
                            pap = new PAP(str, this);
                            PropertyControl.SelectedObject = pap;
                            break;
                        default:
                            CleanProps();
                            break;
                    }
                    break;
                default:
                    viewLayout.Visible = false;
                    break;
            }
        }
    }
    
}
