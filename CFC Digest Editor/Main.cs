using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using CFC_Digest_Editor.Classes;
using CFC_Digest_Editor.Racjin.Assets;
using CFC_Digest_Editor.Racjin.Assets.Text;
using CFC_Digest_Editor.Racjin.CFCUtils;

namespace CFC_Digest_Editor
{

    public partial class Main : Form
    {
        public List<string> EncodingFiles = new List<string>();
        public static string SelectedEncoding;
        public static FileAllocationTable CurrentTable;
        public static bool UnpackedMode;

        public static Main maininstance;
        public string title = "CFCDigestTool";

        public TreeNode selectedNode = new TreeNode();

        public IMG TEXmages;
        public PAP pap;
        public MB0 mb0;
        public PAP_Viewer pap_editor = new PAP_Viewer();

        public Main()
        {
            InitializeComponent();
            maininstance = this;
            EncodingFiles.Add("Encoding.enc");
            SelectedEncoding = "Encoding.enc";
        }

        private void extractionToolStripMenuItem_Click(object sender, EventArgs e) => UnpackCFC();

        private void existingExtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fDialog = new OpenFileDialog { Filter = "XML File (*.XML)|*.XML;" };

            if (fDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var table = ReadXml(fDialog.FileName);

            if (table == null)
            {
                Text = title;
                return;
            }

            string inputPath = Path.Combine(table.OutputDir, table.Containers[0].Name + "_", table.Containers[0].assets[0].Name);
            if (!Directory.Exists(inputPath))
            {
                var folderDialog = new SaveFileDialog { FileName = "Open here", Filter = "Directory|directory" };

                if (MessageBox.Show("Input data not found or doesn't exist. Find it? (Directory name might use game name).", title, MessageBoxButtons.OKCancel) == DialogResult.Cancel || folderDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("You canceled!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    table.OutputDir = Path.GetDirectoryName(folderDialog.FileName);
                }
            }
            CurrentTable = TryPopulateFiles(treeView, table, true);
        }

        private void packedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fDialog = new OpenFileDialog { Filter = "Racjin CFC/XML (*.DIG, *.BIN, *.XML)|;*.DIG;*.BIN;XML;" };

            if (fDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var cfc = new CDFileContainer();
            var table = new FileAllocationTable();

            string extension = Path.GetExtension(fDialog.FileName).ToUpper();
            if (extension != ".DIG" && extension != ".BIN" && extension != ".XML")
            {
                MessageBox.Show("Unknown Format!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cfc.Name = extension != ".XML" ? fDialog.FileName : "";
            table.xmlPath = extension == ".XML" ? fDialog.FileName : "";

            string missingFile = Path.ChangeExtension(fDialog.FileName, extension == ".XML" ? ".DIG" : ".XML");
            if (!File.Exists(missingFile) || ((extension == ".XML") && !File.Exists(Path.ChangeExtension(missingFile, ".BIN"))))
            {
                string cfcFilter = "Racjin CFC (*.DIG, *.BIN)|;*.DIG;*.BIN;";
                string xmlFilter = "XML File (*.XML)|*.XML;";

                fDialog = new OpenFileDialog { Filter = extension == ".XML" ? cfcFilter : xmlFilter };
                if (fDialog.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBox.Show("You canceled!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                cfc.Name = extension != ".XML" ? cfc.Name : fDialog.FileName;
                table.xmlPath = extension == ".XML" ? table.xmlPath : fDialog.FileName;
            }

            table = ReadXml(table.xmlPath);

            if (table == null)
            {
                return;
            }

            using (var reader = new BinaryReader(File.Open(cfc.Name, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                cfc = cfc.ReadTableOfContents(reader);
                cfc.Name = Path.GetFileName(cfc.Name);
                table.Validate(table, cfc, reader, true);
                Text = title;

                if (table == null)
                {
                    MessageBox.Show("Xml Error!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            CurrentTable = TryPopulateFiles(treeView, table, false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
            => Application.Exit();

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
            => new AboutBox1(this).ShowDialog();

        private void ShowHide(Control[] controls) =>
            Array.ForEach(controls, c => c.Visible = !c.Visible);

        private void extractFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderDialog = new SaveFileDialog { FileName = "Save here", Filter = "Directory|directory" };

            if (folderDialog.ShowDialog() != DialogResult.OK)
                return;

            string output = Path.Combine(Path.GetDirectoryName(folderDialog.FileName), selectedNode.Text);
            Directory.CreateDirectory(output);

            string folderNode = string.Join(Path.DirectorySeparatorChar.ToString(), selectedNode.FullPath.Split('\\', '/').Skip(3));
            Text = Text + " - Extracting Content...";
            CloneDirectory(Path.Combine(CurrentTable.OutputDir, CurrentTable.Containers[0].Name + "_", folderNode), output);
            int num = (int)MessageBox.Show("Done.", title);
            Text = String.Concat(title, "- ", CurrentTable.GameTitle);
        }

        private void extractFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileNode = string.Join(Path.DirectorySeparatorChar.ToString(), selectedNode.FullPath.Split('\\', '/').Skip(3));
            string input = Path.Combine(CurrentTable.OutputDir, CurrentTable.Containers[0].Name + "_", fileNode);
            string extension = Path.GetExtension(fileNode);


            FileInfo fileInfo = new FileInfo(input);

            var saveDialog = new SaveFileDialog() { Filter = $"{extension} File (*{extension})|*{extension};", FileName = Path.GetFileName(selectedNode.Text) };
            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;
            File.Copy(input, saveDialog.FileName, true);
            int num = (int)MessageBox.Show("Done.", title);
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileNode = string.Join(Path.DirectorySeparatorChar.ToString(), selectedNode.FullPath.Split('\\', '/').Skip(3));
            string input = Path.Combine(CurrentTable.OutputDir, CurrentTable.Containers[0].Name + "_", fileNode);

            FileInfo fileInfo = new FileInfo(input);
            var openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            File.Copy(openDialog.FileName, input, true);
            int num = (int)MessageBox.Show("Done.", title);
        }

        private void buildFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CurrentTable.Serialize(CurrentTable))
                MessageBox.Show("Error!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                MessageBox.Show("Success!\nFiles has been packed in: " + CurrentTable.OutputDir, title);
            }
            Text = String.Concat(title, "- ", CurrentTable.GameTitle);
        }

        private void buildAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderDialog = new SaveFileDialog { FileName = "Save here", Filter = "Directory|directory" };

            if (folderDialog.ShowDialog() != DialogResult.OK)
                return;

            string outputdir = Path.Combine(Path.GetDirectoryName(folderDialog.FileName), "data", CurrentTable.GameTitle);

            if (!CurrentTable.Serialize(CurrentTable, outputdir))
                MessageBox.Show("Error!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                MessageBox.Show("Success!\nFiles has been packed in: " + outputdir, title);
            }
            Text = String.Concat(title, "- ", CurrentTable.GameTitle);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            switch (e.Node.Tag.ToString())
            {
                case "folder":
                case "section":
                case "packet":
                    this.FolderMenuStrip.Show(Cursor.Position);
                    break;
                case "file":
                    this.FileMenuStrip.Show(Cursor.Position);
                    break;
                default: break;
            }
            selectedNode = e.Node;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;

            string str = e.Node.Tag.ToString();

            var fileInfo = new FileInfo(selectedNode.Text);
            if (!(str == "file"))
                return;
        }
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
                Path.Combine($"{TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Bpp}");
                saveFileDialog.FileName = $"{TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Bpp}{Path.GetFileNameWithoutExtension(selectedNode.Text)}_{Convert.ToInt32(TEXmages.Choosed)}";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                var tim = TEXmages.GetImage(Convert.ToInt32(TEXmages.Choosed), out var mage);
                if (saveFileDialog.FilterIndex == 2)
                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, tim);
                else
                    mage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show($"Exported texture to:\n{saveFileDialog.FileName}!", title);
            }
            else if (e.Button == MouseButtons.Left)
            {
                string str = Path.Combine(CurrentTable.OutputDir, CurrentTable.Containers[0].Name + "_", string.Join(Path.DirectorySeparatorChar.ToString(), selectedNode.FullPath.Split('\\', '/').Skip(3)));

                var texDialog = new OpenFileDialog { Title = "Select a TIM2 Texture to import.", Filter = "PS2 TIM2 Texture(*.tm2)|*.tm2" };
                if (texDialog.ShowDialog() == DialogResult.OK)
                {
                    var tim2 = TM2.GetClutandTex(System.IO.File.ReadAllBytes(texDialog.FileName));
                    int bpp = (tim2.Bpp == 5 ? 8 : 4);
                    if (tim2.Width != TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Width ||
                        tim2.Height != TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Height ||
                       bpp != TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Bpp
                       && TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Index.WithError == false)
                    {
                        MessageBox.Show($"Texture size/Bpp mismatch!\nExpected: " +
                            $"{TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Width}x{TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Height} - {TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].Bpp}Bpp\n" +
                            $"Imported: {tim2.Width}x{tim2.Height} - {bpp}Bpp", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var values = TEXmages.GetPixelandColorData(System.IO.File.ReadAllBytes(texDialog.FileName), Convert.ToInt32(TEXmages.Choosed), true);
                    TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].TEX = values[0];
                    TEXmages.Images[Convert.ToInt32(TEXmages.Choosed)].CLUT = values[1];
                    TEXmages.GetImage(Convert.ToInt32(TEXmages.Choosed), out System.Drawing.Image mage);
                    IMG._main.imageViewer.Image = mage;

                    TEXmages.ReWriteIMG(str);
                    //File.WriteAllBytes(str,TEXmages.RebuildIMG(tim2.Bpp));

                    MessageBox.Show($"Imported texture from:\n{texDialog.FileName}!", title);
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

        private void DSIExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folder = new SaveFileDialog { FileName = "Select the folder where you want to extract the files.", Filter = "Directory|directory" };

            if (folder.ShowDialog() != DialogResult.OK)
                return;

            string output = Path.GetDirectoryName(folder.FileName);

            var fileDialog = new OpenFileDialog { Title = "Select the DSI file to extract.", Filter = "DSI file (*.DSI) | *.DSI" };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            string input = fileDialog.FileName;
            string outputPath = Path.Combine(input, output, Path.GetFileNameWithoutExtension(input));
            DSI.ExtractAndMerge(input, outputPath + ".m2v", outputPath + ".raw");
        }

        private void DSICompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newM2vPath;
            string newVagPath;

            // Seleciona o novo M2V
            using (var openM2v = new OpenFileDialog { Title = "Select the new M2V file to inject.", Filter = "MPEG-2 Video (*.m2v)|*.m2v" })
            {
                if (openM2v.ShowDialog() != DialogResult.OK)
                    return;
                newM2vPath = openM2v.FileName;
            }
            // Seleciona o novo VAG
            using (var openVag = new OpenFileDialog { Title = "Select the new VAG file to inject.", FileName = Path.ChangeExtension(newM2vPath, ".raw"), Filter = "VAG Audio (*.adpcm, *.raw, *.vag)|*.adpcm;*.raw;*.vag" })
            {
                if (openVag.ShowDialog() != DialogResult.OK)
                    return;
                newVagPath = openVag.FileName;
            }

            // Seleciona o destino
            var output = new SaveFileDialog { Title = "Save rebuilt DSI file as...", Filter = "DSI file (*.DSI)|*.DSI", FileName = Path.GetFileNameWithoutExtension(newM2vPath) + "_rebuilt.dsi" };
            if (output.ShowDialog() != DialogResult.OK)
                return;

            // Chama a função para reconstruir
            DSI.BuildDSIFromStreams(newM2vPath, newVagPath, output.FileName);
        }

        public void RefreshPropertyGrid()
        {
            var obj = PropertyControl.SelectedObject;
            PropertyControl.SelectedObject = null;
            PropertyControl.SelectedObject = obj;

        }

        private void otherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog() { Title = "Select other encoding file...", Filter = "All files (*.*)|*.*" })
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Pega o nome com extensão (sem caminho)
                    string fileName = Path.GetFileName(fileDialog.FileName);
                    if (EncodingFiles.Contains(fileName))
                    {
                        MessageBox.Show("This encoding file is already loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    EncodingFiles.Add(fileDialog.FileName);
                    // Cria novo item de menu
                    var ToolStripItem = new ToolStripMenuItem(fileName)
                    {
                        Checked = false,
                        CheckOnClick = true // permite marcar/desmarcar com clique

                    };
                    ToolStripItem.Click += new EventHandler(encodingsencToolStripMenuItem_Click);
                    // Adiciona ao menu (substitua pelo nome real do seu menu)
                    var last = setEncodingsFileToolStripMenuItem.DropDownItems[setEncodingsFileToolStripMenuItem.DropDownItems.Count - 1];
                    setEncodingsFileToolStripMenuItem.DropDownItems.Insert(setEncodingsFileToolStripMenuItem.DropDownItems.IndexOf(last), ToolStripItem);
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
            if (treeView.Nodes.Count > 0)
            {
                treeView.Nodes.Clear();
                treeView.Nodes.Add("data", "data", 1);
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

            Directory.Delete("path", true);

            NewToolStripMenuItem.Enabled = true;
            buildAsToolStripMenuItem.Enabled = false;
            buildToolStripMenuItem.Enabled = false;
        }

        private void DigTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Tag.ToString())
            {
                case "file":
                    selectedNode = e.Node;
                    string fileNode = string.Join(Path.DirectorySeparatorChar.ToString(), selectedNode.FullPath.Split('\\', '/').Skip(3));

                    var fileInfo = new FileInfo(fileNode);

                    string path = Path.Combine(CurrentTable.OutputDir, CurrentTable.Containers[0].Name + "_", fileNode);
                    viewLayout.Visible = true;
                    switch (fileInfo.Extension)
                    {
                        case ".rct":
                            if (Formats.GetExtension(path) == ".img")
                            {
                                CleanProps();

                                imageViewer.Visible = true;
                                // Linha 0 com altura absoluta de 50 pixels
                                viewLayout.RowStyles[0].SizeType = SizeType.Percent;
                                viewLayout.RowStyles[0].Height = 50;

                                // Linha 1 com 70% do espaço restante
                                viewLayout.RowStyles[1].SizeType = SizeType.Percent;
                                viewLayout.RowStyles[1].Height = 50;
                                TEXmages = new IMG(this).Read(File.ReadAllBytes(path), imageViewer);
                                TEXmages.GetImage(0, out var mag);

                                imageViewer.Image = mag;
                                PropertyControl.SelectedObject = TEXmages;
                            }
                            break;

                        case ".img":
                            CleanProps();

                            imageViewer.Visible = true;
                            // Linha 0 com altura absoluta de 50 pixels
                            viewLayout.RowStyles[0].SizeType = SizeType.Percent;
                            viewLayout.RowStyles[0].Height = 50;

                            // Linha 1 com 70% do espaço restante
                            viewLayout.RowStyles[1].SizeType = SizeType.Percent;
                            viewLayout.RowStyles[1].Height = 50;
                            TEXmages = new IMG(this).Read(File.ReadAllBytes(path), imageViewer);
                            TEXmages.GetImage(0, out var mage);

                            imageViewer.Image = mage;
                            PropertyControl.SelectedObject = TEXmages;
                            break;
                        case ".mb0":
                            CleanProps();
                            mb0 = new MB0(path);
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
                            pap = new PAP(path, this);
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

        private string ShowGameSelector(string formTitle, string initialName)
        {
            var textBox = new TextBox { Text = initialName, Left = 10, Top = 20, MaxLength = 45, Width = 260 };
            var okButton = new Button { Text = "OK", Left = 200, Width = 70, Top = 60, DialogResult = DialogResult.OK };
            var textForm = new Form() { AcceptButton = okButton, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, Icon = Icon, Text = formTitle, Width = 300, Height = 150, StartPosition = FormStartPosition.CenterScreen, };
            textForm.Controls.Add(textBox);
            textForm.Controls.Add(okButton);
            return textForm.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }

        private FileAllocationTable ReadXml(string xmlPath)
        {            
            if (Path.GetExtension(xmlPath).ToUpper() != ".XML")
            {
                MessageBox.Show("Unknown Format!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var table = new FileAllocationTable() { xmlPath = xmlPath };
            table.Desserialize(table);

            if (table == null)
            {
                MessageBox.Show("Xml Error!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return table;
        }

        private void UnpackCFC()
        {
            var fDialog = new OpenFileDialog { Filter = "Racjin CFC (*.DIG, *.BIN)|*.DIG;*.BIN;" };
            if (fDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string cfcPath, xmlPath, gameTitle;
            string extension = Path.GetExtension(fDialog.FileName).ToUpper();
            if (extension != ".DIG" && extension != ".BIN")
            {
                MessageBox.Show("Container must be supported!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                gameTitle = ShowGameSelector("Insert Game Name", "Game");

                var saveDialog = new SaveFileDialog() { Filter = "XML File |*.XML", FileName = gameTitle };
                if (gameTitle == null || saveDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("You canceled!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                cfcPath = fDialog.FileName;
                xmlPath = saveDialog.FileName;
            }

            using (var reader = new BinaryReader(File.Open(cfcPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                var folderDialog = new SaveFileDialog { FileName = "Save here", Title = "Save", Filter = "Directory|directory" };

                if (folderDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("You canceled!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var CFC = new CDFileContainer().ReadTableOfContents(reader);
                { CFC.Name = Path.GetFileName(cfcPath); }

                var table = new FileAllocationTable() { xmlPath = xmlPath, GameTitle = gameTitle, ContainerCount = 1, OutputDir = Path.Combine(Path.GetDirectoryName(folderDialog.FileName), "data", gameTitle) };
                table.Containers.Add(CFC);
                table.SerializeTable(table, new List<string>() { cfcPath }, true);

                if (table == null)
                {
                    Text = title;
                    MessageBox.Show("Error!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                CurrentTable = TryPopulateFiles(treeView, table, false);
            }
        }

        private FileAllocationTable TryPopulateFiles(TreeView Tree, FileAllocationTable table, bool checkFiles)
        {
            Text = title;
            string checkPath = Path.Combine(table.OutputDir, table.Containers[0].Name + "_");
            if (checkFiles && !Directory.Exists(Path.Combine(checkPath)))
            {
                MessageBox.Show($"{checkPath} not found!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var dataNode = Tree.Nodes.Add("data");
            var gameNode = dataNode.Nodes.Add(table.GameTitle);
            var containerNode = gameNode.Nodes.Add(table.Containers[0].Name);
            { dataNode.Tag = "data"; gameNode.Tag = "game"; containerNode.Tag = "container"; dataNode.ImageIndex = dataNode.SelectedImageIndex = gameNode.ImageIndex = gameNode.SelectedImageIndex = containerNode.ImageIndex = containerNode.SelectedImageIndex = 1; }

            foreach (var packet in table.Containers[0].assets)
            {
                if (checkFiles && !Directory.Exists(Path.Combine(checkPath, packet.Name)))
                {
                    MessageBox.Show($"{Path.Combine(checkPath, packet.Name)} not found!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                var packetNode = containerNode.Nodes.Add(packet.Name, packet.Name);
                { packetNode.Tag = "packet"; packetNode.ImageIndex = packetNode.SelectedImageIndex = 2; }

                foreach (var section in packet.Sections)
                {
                    if (checkFiles && !Directory.Exists(Path.Combine(checkPath, packet.Name, section.Name)))
                    {
                        MessageBox.Show($"{Path.Combine(checkPath, packet.Name, section.Name)} not found!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }

                    var sectionNode = packetNode.Nodes.Add(section.Name, section.Name);
                    { sectionNode.Tag = "section"; sectionNode.ImageIndex = sectionNode.SelectedImageIndex = 1; }

                    foreach (var file in section.Files)
                    {
                        if (checkFiles && file.Type != FileType.Null && !File.Exists(Path.Combine(checkPath, packet.Name, section.Name, file.Name)))
                        {
                            MessageBox.Show($"{Path.Combine(checkPath, packet.Name, section.Name, file.Name)} not found!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }

                        var fileNode = sectionNode.Nodes.Add(file.Name, file.Name);
                        { fileNode.Tag = "file"; fileNode.ImageIndex = fileNode.SelectedImageIndex = 0; }
                    }
                }
            }
            ShowHide(new Control[1] { MainLayout });
            dataNode.Expand();
            gameNode.Expand();
            NewToolStripMenuItem.Enabled = false;
            buildAsToolStripMenuItem.Enabled = true;
            buildToolStripMenuItem.Enabled = true;
            Text = String.Concat(title, "- ", table.GameTitle);
            return table;
        }

        private static void CloneDirectory(string root, string output)
        {
            foreach (string directory in Directory.GetDirectories(root))
            {
                string fileName = Path.GetFileName(directory);
                Directory.CreateDirectory(Path.Combine(output, fileName));
                Main.CloneDirectory(directory, Path.Combine(output, fileName));
            }
            foreach (string file in Directory.GetFiles(root))
                File.Copy(file, Path.Combine(output, Path.GetFileName(file)), true);
        }

        private void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sectionNode = selectedNode.Parent;
            var packetNode = sectionNode.Parent;
            var containerNode = packetNode.Parent;


            string fileNodePath = string.Join(Path.DirectorySeparatorChar.ToString(), selectedNode.FullPath.Split('\\', '/').Skip(3));
            string input = Path.Combine(CurrentTable.OutputDir, CurrentTable.Containers[0].Name + "_", fileNodePath);
            string path = Path.GetDirectoryName(input);

        Rename:
            string fileName = ShowGameSelector(title, selectedNode.Text);
            if (fileName == null)
            {
                MessageBox.Show("You canceled!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (fileName == selectedNode.Text)
            {
                return;
            }
            else if (File.Exists(Path.Combine(path, fileName)))
            {
                if (MessageBox.Show($"File {fileName} already exists! Insert new name?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    goto Rename;
                else
                    return;
            }
            
            foreach (var container in CurrentTable.Containers)
            {
                if (container.Name != containerNode.Text)
                    continue;

                foreach (var packet in container.assets)
                {
                    if (packet.Name != packetNode.Text)
                        continue;

                    foreach (var section in packet.Sections)
                    {
                        if (section.Name != sectionNode.Text)
                            continue;

                        foreach (var file in section.Files)
                        {
                            if (file.Name != selectedNode.Text)
                                continue;

                            file.Name = fileName;
                        }
                    }
                }
            }
            File.Move(input, Path.Combine(path, fileName));
            selectedNode.Text = fileName;

            var xmlSerializer = new XmlSerializer(typeof(FileAllocationTable));

            using (var sw = new StreamWriter(File.Open(CurrentTable.xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite), new UTF8Encoding(false)))
            {
                xmlSerializer.Serialize(sw, CurrentTable);
            }
        }
    }
}