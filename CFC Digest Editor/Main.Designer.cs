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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingExtractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DSIExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DSICompilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEncodingsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodingsencToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.SubLayout = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.viewLayout = new System.Windows.Forms.TableLayoutPanel();
            this.imageViewer = new System.Windows.Forms.PictureBox();
            this.PropertyControl = new System.Windows.Forms.PropertyGrid();
            this.FolderMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.MainLayout.SuspendLayout();
            this.SubLayout.SuspendLayout();
            this.viewLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageViewer)).BeginInit();
            this.FolderMenuStrip.SuspendLayout();
            this.FileMenuStrip.SuspendLayout();
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
            this.NewToolStripMenuItem,
            this.openToolStripMenuItem,
            this.buildToolStripMenuItem,
            this.buildAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // NewToolStripMenuItem
            // 
            this.NewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractionToolStripMenuItem});
            this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
            this.NewToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.NewToolStripMenuItem.Text = "New";
            // 
            // extractionToolStripMenuItem
            // 
            this.extractionToolStripMenuItem.Name = "extractionToolStripMenuItem";
            this.extractionToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.extractionToolStripMenuItem.Text = "Extraction...";
            this.extractionToolStripMenuItem.Click += new System.EventHandler(this.extractionToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.existingExtractionToolStripMenuItem,
            this.packedDataToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // existingExtractionToolStripMenuItem
            // 
            this.existingExtractionToolStripMenuItem.Name = "existingExtractionToolStripMenuItem";
            this.existingExtractionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.existingExtractionToolStripMenuItem.Text = "Existing Extraction...";
            this.existingExtractionToolStripMenuItem.Click += new System.EventHandler(this.existingExtractionToolStripMenuItem_Click);
            // 
            // packedDataToolStripMenuItem
            // 
            this.packedDataToolStripMenuItem.Name = "packedDataToolStripMenuItem";
            this.packedDataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.packedDataToolStripMenuItem.Text = "Packed Data...";
            this.packedDataToolStripMenuItem.Click += new System.EventHandler(this.packedDataToolStripMenuItem_Click);
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.Enabled = false;
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.buildToolStripMenuItem.Text = "Build";
            this.buildToolStripMenuItem.Click += new System.EventHandler(this.buildFileToolStripMenuItem_Click);
            // 
            // buildAsToolStripMenuItem
            // 
            this.buildAsToolStripMenuItem.Enabled = false;
            this.buildAsToolStripMenuItem.Name = "buildAsToolStripMenuItem";
            this.buildAsToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.buildAsToolStripMenuItem.Text = "Build as...";
            this.buildAsToolStripMenuItem.Click += new System.EventHandler(this.buildAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
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
            this.DSIExtractorToolStripMenuItem,
            this.DSICompilerToolStripMenuItem,
            this.setEncodingsFileToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // DSIExtractorToolStripMenuItem
            // 
            this.DSIExtractorToolStripMenuItem.Name = "DSIExtractorToolStripMenuItem";
            this.DSIExtractorToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.DSIExtractorToolStripMenuItem.Text = "DSI Extractor";
            this.DSIExtractorToolStripMenuItem.Click += new System.EventHandler(this.DSIExtractorToolStripMenuItem_Click);
            // 
            // DSICompilerToolStripMenuItem
            // 
            this.DSICompilerToolStripMenuItem.Name = "DSICompilerToolStripMenuItem";
            this.DSICompilerToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.DSICompilerToolStripMenuItem.Text = "DSI Compiler";
            this.DSICompilerToolStripMenuItem.Click += new System.EventHandler(this.DSICompilerToolStripMenuItem_Click);
            // 
            // setEncodingsFileToolStripMenuItem
            // 
            this.setEncodingsFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encodingsencToolStripMenuItem,
            this.otherToolStripMenuItem});
            this.setEncodingsFileToolStripMenuItem.Name = "setEncodingsFileToolStripMenuItem";
            this.setEncodingsFileToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
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
            this.SubLayout.Controls.Add(this.treeView, 0, 0);
            this.SubLayout.Controls.Add(this.viewLayout, 1, 0);
            this.SubLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubLayout.Location = new System.Drawing.Point(3, 3);
            this.SubLayout.Name = "SubLayout";
            this.SubLayout.RowCount = 1;
            this.SubLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubLayout.Size = new System.Drawing.Size(643, 380);
            this.SubLayout.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 1;
            this.treeView.ImageList = this.imageList1;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 1;
            this.treeView.Size = new System.Drawing.Size(361, 374);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DigTree_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "file");
            this.imageList1.Images.SetKeyName(1, "folder");
            this.imageList1.Images.SetKeyName(2, "packet");
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
            // FolderMenuStrip
            // 
            this.FolderMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem});
            this.FolderMenuStrip.Name = "contextMenuStrip1";
            this.FolderMenuStrip.Size = new System.Drawing.Size(181, 48);
            this.FolderMenuStrip.Text = "Folder";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importToolStripMenuItem.Text = "Export";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.extractFolderToolStripMenuItem_Click);
            // 
            // FileMenuStrip
            // 
            this.FileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFileToolStripMenuItem,
            this.extractFileToolStripMenuItem,
            this.renameFileToolStripMenuItem});
            this.FileMenuStrip.Name = "contextMenuStrip2";
            this.FileMenuStrip.Size = new System.Drawing.Size(118, 70);
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.importFileToolStripMenuItem.Text = "Import";
            this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
            // 
            // extractFileToolStripMenuItem
            // 
            this.extractFileToolStripMenuItem.Name = "extractFileToolStripMenuItem";
            this.extractFileToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.extractFileToolStripMenuItem.Text = "Export";
            this.extractFileToolStripMenuItem.Click += new System.EventHandler(this.extractFileToolStripMenuItem_Click);
            // 
            // renameFileToolStripMenuItem
            // 
            this.renameFileToolStripMenuItem.Name = "renameFileToolStripMenuItem";
            this.renameFileToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameFileToolStripMenuItem.Text = "Rename";
            this.renameFileToolStripMenuItem.Click += new System.EventHandler(this.renameFileToolStripMenuItem_Click);
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
            this.FolderMenuStrip.ResumeLayout(false);
            this.FileMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        public System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.TableLayoutPanel SubLayout;
        private System.Windows.Forms.TreeView treeView;
        public System.Windows.Forms.PictureBox imageViewer;
        public System.Windows.Forms.PropertyGrid PropertyControl;
        private System.Windows.Forms.ContextMenuStrip FolderMenuStrip;
        private System.Windows.Forms.ContextMenuStrip FileMenuStrip;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DSIExtractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DSICompilerToolStripMenuItem;
        public System.Windows.Forms.TableLayoutPanel viewLayout;
        private System.Windows.Forms.ToolStripMenuItem setEncodingsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encodingsencToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingExtractionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packedDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameFileToolStripMenuItem;
    }
}

