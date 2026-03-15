namespace CFC_Digest_Editor
{
    partial class Main
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("data");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dSIExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dSICompilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEncodingsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodingsencToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.SubLayout = new System.Windows.Forms.TableLayoutPanel();
            this.DigTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.viewLayout = new System.Windows.Forms.TableLayoutPanel();
            this.imageViewer = new System.Windows.Forms.PictureBox();
            this.PropertyControl = new System.Windows.Forms.PropertyGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.MainLayout.SuspendLayout();
            this.SubLayout.SuspendLayout();
            this.viewLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageViewer)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(649, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.buildFileToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dSIExtractorToolStripMenuItem,
            this.dSICompilerToolStripMenuItem,
            this.setEncodingsFileToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // dSIExtractorToolStripMenuItem
            // 
            this.dSIExtractorToolStripMenuItem.Name = "dSIExtractorToolStripMenuItem";
            this.dSIExtractorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dSIExtractorToolStripMenuItem.Text = "DSI Extractor";
            this.dSIExtractorToolStripMenuItem.Click += new System.EventHandler(this.dSIExtractorToolStripMenuItem_Click);
            // 
            // dSICompilerToolStripMenuItem
            // 
            this.dSICompilerToolStripMenuItem.Name = "dSICompilerToolStripMenuItem";
            this.dSICompilerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dSICompilerToolStripMenuItem.Text = "DSI Compiler";
            this.dSICompilerToolStripMenuItem.Click += new System.EventHandler(this.dSICompilerToolStripMenuItem_Click);
            // 
            // setEncodingsFileToolStripMenuItem
            // 
            this.setEncodingsFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encodingsencToolStripMenuItem,
            this.otherToolStripMenuItem});
            this.setEncodingsFileToolStripMenuItem.Name = "setEncodingsFileToolStripMenuItem";
            this.setEncodingsFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setEncodingsFileToolStripMenuItem.Text = "Set Encodings File";
            // 
            // encodingsencToolStripMenuItem
            // 
            this.encodingsencToolStripMenuItem.Checked = true;
            this.encodingsencToolStripMenuItem.CheckOnClick = true;
            this.encodingsencToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.encodingsencToolStripMenuItem.Name = "encodingsencToolStripMenuItem";
            this.encodingsencToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.encodingsencToolStripMenuItem.Text = "Encoding.enc";
            this.encodingsencToolStripMenuItem.Click += new System.EventHandler(this.encodingsencToolStripMenuItem_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.otherToolStripMenuItem.Text = "Other...";
            this.otherToolStripMenuItem.Click += new System.EventHandler(this.otherToolStripMenuItem_Click);
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.24653F));
            this.MainLayout.Controls.Add(this.SubLayout, 0, 0);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 24);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 2;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.97669F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.02331F));
            this.MainLayout.Size = new System.Drawing.Size(649, 429);
            this.MainLayout.TabIndex = 1;
            this.MainLayout.Visible = false;
            // 
            // SubLayout
            // 
            this.SubLayout.ColumnCount = 2;
            this.SubLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.SubLayout.Controls.Add(this.DigTree, 0, 0);
            this.SubLayout.Controls.Add(this.viewLayout, 1, 0);
            this.SubLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubLayout.Location = new System.Drawing.Point(3, 3);
            this.SubLayout.Name = "SubLayout";
            this.SubLayout.RowCount = 1;
            this.SubLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubLayout.Size = new System.Drawing.Size(643, 380);
            this.SubLayout.TabIndex = 0;
            // 
            // DigTree
            // 
            this.DigTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DigTree.ImageIndex = 1;
            this.DigTree.ImageList = this.imageList1;
            this.DigTree.Location = new System.Drawing.Point(3, 3);
            this.DigTree.Name = "DigTree";
            treeNode2.ImageKey = "folder";
            treeNode2.Name = "Nó0";
            treeNode2.Text = "data";
            this.DigTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.DigTree.SelectedImageIndex = 0;
            this.DigTree.Size = new System.Drawing.Size(361, 374);
            this.DigTree.TabIndex = 0;
            this.DigTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DigTree_AfterSelect);
            this.DigTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.DigTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "file");
            this.imageList1.Images.SetKeyName(1, "folder");
            // 
            // viewLayout
            // 
            this.viewLayout.ColumnCount = 1;
            this.viewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.viewLayout.Controls.Add(this.imageViewer, 0, 0);
            this.viewLayout.Controls.Add(this.PropertyControl, 0, 1);
            this.viewLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewLayout.Location = new System.Drawing.Point(370, 3);
            this.viewLayout.Name = "viewLayout";
            this.viewLayout.RowCount = 2;
            this.viewLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.viewLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.viewLayout.Size = new System.Drawing.Size(270, 374);
            this.viewLayout.TabIndex = 1;
            this.viewLayout.Visible = false;
            // 
            // imageViewer
            // 
            this.imageViewer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(3, 3);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(264, 232);
            this.imageViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageViewer.TabIndex = 0;
            this.imageViewer.TabStop = false;
            this.imageViewer.Click += new System.EventHandler(this.imageViewer_Click);
            this.imageViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imageViewer_MouseClick);
            // 
            // PropertyControl
            // 
            this.PropertyControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyControl.HelpVisible = false;
            this.PropertyControl.Location = new System.Drawing.Point(3, 241);
            this.PropertyControl.Name = "PropertyControl";
            this.PropertyControl.Size = new System.Drawing.Size(264, 130);
            this.PropertyControl.TabIndex = 1;
            this.PropertyControl.ToolbarVisible = false;
            this.PropertyControl.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyControl_PropertyValueChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 26);
            this.contextMenuStrip1.Text = "Folder";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.importToolStripMenuItem.Text = "Extract Folder";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.extractFolderToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractFileToolStripMenuItem,
            this.importFileToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(132, 48);
            // 
            // extractFileToolStripMenuItem
            // 
            this.extractFileToolStripMenuItem.Name = "extractFileToolStripMenuItem";
            this.extractFileToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.extractFileToolStripMenuItem.Text = "Extract File";
            this.extractFileToolStripMenuItem.Click += new System.EventHandler(this.extractFileToolStripMenuItem_Click);
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.importFileToolStripMenuItem.Text = "Import File";
            this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CFC_Digest_Editor.Properties.Resources.PrincipalBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(649, 453);
            this.Controls.Add(this.MainLayout);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CFC Digest Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.MainLayout.ResumeLayout(false);
            this.SubLayout.ResumeLayout(false);
            this.viewLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageViewer)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        public System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.TableLayoutPanel SubLayout;
        private System.Windows.Forms.TreeView DigTree;
        public System.Windows.Forms.PictureBox imageViewer;
        public System.Windows.Forms.PropertyGrid PropertyControl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dSIExtractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dSICompilerToolStripMenuItem;
        public System.Windows.Forms.TableLayoutPanel viewLayout;
        private System.Windows.Forms.ToolStripMenuItem setEncodingsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encodingsencToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
    }
}

