using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using NUC_Raw_Tools.ArquivoRAW;

namespace CFC_Digest_Editor.classes
{
    //2D RECTANGLES TO CUT FROM TEX

    public class PAP
    {
        private string FileName;
        private IMG RefImage;
        public Button button = new Button();

        //Calling order: Block 2 > Block 3 > Block 1 > Block 4

        //Block 1
        public class CropControl
        {
            public short Crop_Index;
            public short Crop_Count;
            public short StartPos_X, StartPos_Y;
            public short Unk1, Unk2;

            [Category("Crop Control")]
            [DisplayName("Crop Index")]
            [Description("Crop Index")]
            public short Crop_Index_CropControl { get => Crop_Index; set => Crop_Index = value; }

            [Category("Crop Control")]
            [DisplayName("Crop Count")]
            [Description("Crop Count")]
            public short Crop_Count_CropControl { get => Crop_Count; set => Crop_Count = value; }

            [Category("Crop Control")]
            [DisplayName("StartPos X")]
            [Description("StartPos X")]
            public short StartPos_X_CropControl { get => StartPos_X; set => StartPos_X = value; }

            [Category("Crop Control")]
            [DisplayName("StartPos Y")]
            [Description("StartPos Y")]
            public short StartPos_Y_CropControl { get => StartPos_Y; set => StartPos_Y = value; }

            [Category("Crop Control")]
            [DisplayName("Unk1")]
            [Description("Unk1")]
            public short Unk1_CropControl { get => Unk1; set => Unk1 = value; }

            [Category("Crop Control")]
            [DisplayName("Unk2")]
            [Description("Unk2")]
            public short Unk2_CropControl { get => Unk2; set => Unk2 = value; }

            public static CropControl Read(byte[] data) =>
                new CropControl()
                {
                    Crop_Index = BitConverter.ToInt16(data, 0x00),
                    Crop_Count = BitConverter.ToInt16(data, 0x02),
                    StartPos_X = BitConverter.ToInt16(data, 0x04),
                    StartPos_Y = BitConverter.ToInt16(data, 0x06),
                    Unk1 = BitConverter.ToInt16(data, 0x08),
                    Unk2 = BitConverter.ToInt16(data, 0x0A)
                };

            public byte[] Rebuild()
            {
                var list = new List<byte>();
                list.AddRange(BitConverter.GetBytes(Crop_Index));
                list.AddRange(BitConverter.GetBytes(Crop_Count));
                list.AddRange(BitConverter.GetBytes(StartPos_X));
                list.AddRange(BitConverter.GetBytes(StartPos_Y));
                list.AddRange(BitConverter.GetBytes(Unk1));
                list.AddRange(BitConverter.GetBytes(Unk2));
                return list.ToArray();
            }
        }

        //Block 2
        public class Index
        {
            public short Canva_Index;
            public int Canva_Count;
            public short Unk;
            public int Unk2;

            [Category("Index")]
            [DisplayName("Canva Index")]
            [Description("Canva Index")]
            public short Canva_Index_Index { get => Canva_Index; set => Canva_Index = value; }

            [Category("Index")]
            [DisplayName("Canva Count")]
            [Description("Canva Count")]
            public int Canva_Count_Index { get => Canva_Count; set => Canva_Count = value; }

            [Category("Index")]
            [DisplayName("Unk")]
            [Description("Unk")]
            public short Unk_Index { get => Unk; set => Unk = value; }

            [Category("Index")]
            [DisplayName("Unk2")]
            [Description("Unk2")]
            public int Unk2_Index { get => Unk2; set => Unk2 = value; }

            public static Index Read(byte[] data) =>
                new Index()
                {
                    Canva_Index = BitConverter.ToInt16(data, 0x00),
                    Canva_Count = BitConverter.ToInt32(data, 0x02),
                    Unk = BitConverter.ToInt16(data, 0x06),
                    Unk2 = BitConverter.ToInt32(data, 0x08)
                };
            public byte[] Rebuild()
            {
                var list = new List<byte>();
                list.AddRange(BitConverter.GetBytes(Canva_Index));
                list.AddRange(BitConverter.GetBytes(Canva_Count));
                list.AddRange(BitConverter.GetBytes(Unk));
                list.AddRange(BitConverter.GetBytes(Unk2));
                return list.ToArray();
            }
        }

        //Block 3
        public class Canva
        {
            public short Crop_Control_Index;
            public int Crop_Control_CountUnk;
            public int Unk;

            public Single Scale_Width;
            public Single Scale_Height;

            public long Unk2;

            //Index RE-Referenced area
            public short Index_Ref;
            public short Index_Ref_Count;
            public byte Tex_Alpha;

            [Category("Canva")]
            [DisplayName("Crop Control Index")]
            [Description("Crop Control Index")]
            public short Crop_Control_Index_Canva { get => Crop_Control_Index; set => Crop_Control_Index = value; }

            [Category("Canva")]
            [DisplayName("Crop Control Count Unk")]
            [Description("Crop Control Count Unk")]
            public int Crop_Control_CountUnk_Canva { get => Crop_Control_CountUnk; set => Crop_Control_CountUnk = value; }

            [Category("Canva")]
            [DisplayName("Unk")]
            [Description("Unk")]
            public int Unk_Canva { get => Unk; set => Unk = value; }

            [Category("Canva")]
            [DisplayName("Scale Width")]
            [Description("Scale Width")]
            public Single Scale_Width_Canva { get => Scale_Width; set => Scale_Width = value; }

            [Category("Canva")]
            [DisplayName("Scale Height")]
            [Description("Scale Height")]
            public Single Scale_Height_Canva { get => Scale_Height; set => Scale_Height = value; }

            [Category("Canva")]
            [DisplayName("Unk2")]
            [Description("Unk2")]
            public long Unk2_Canva { get => Unk2; set => Unk2 = value; }

            [Category("Canva")]
            [DisplayName("Index Ref")]
            [Description("Index Ref")]
            public short Index_Ref_Canva { get => Index_Ref; set => Index_Ref = value; }

            [Category("Canva")]
            [DisplayName("Index Ref Count")]
            [Description("Index Ref Count")]
            public short Index_Ref_Count_Canva { get => Index_Ref_Count; set => Index_Ref_Count = value; }

            [Category("Canva")]
            [DisplayName("Tex Alpha")]
            [Description("Texture Alpha")]
            public byte Tex_Alpha_Canva { get => Tex_Alpha; set => Tex_Alpha = value; }

            //Ends with 0x00

            public static Canva Read(byte[] data) =>
                new Canva()
                {
                    Crop_Control_Index = BitConverter.ToInt16(data, 0x00),
                    Crop_Control_CountUnk = BitConverter.ToInt32(data, 0x02),
                    Unk = BitConverter.ToInt32(data, 0x06),
                    Scale_Width = BitConverter.ToSingle(data, 0x0A),
                    Scale_Height = BitConverter.ToSingle(data, 0x0E),
                    Unk2 = BitConverter.ToInt64(data, 0x12),
                    Index_Ref = BitConverter.ToInt16(data, 0x1A),
                    Index_Ref_Count = BitConverter.ToInt16(data, 0x1C),
                    Tex_Alpha = data[0x1E]
                };
            public byte[] Rebuild()
            {
                var list = new List<byte>();
                list.AddRange(BitConverter.GetBytes(Crop_Control_Index));
                list.AddRange(BitConverter.GetBytes(Crop_Control_CountUnk));
                list.AddRange(BitConverter.GetBytes(Unk));
                list.AddRange(BitConverter.GetBytes(Scale_Width));
                list.AddRange(BitConverter.GetBytes(Scale_Height));
                list.AddRange(BitConverter.GetBytes(Unk2));
                list.AddRange(BitConverter.GetBytes(Index_Ref));
                list.AddRange(BitConverter.GetBytes(Index_Ref_Count));
                list.Add(Tex_Alpha);
                list.Add(0x00); //Ends with 0x00
                return list.ToArray();
            }
        }

        //Block 4
        public class Crop
        {
            public short TexID, TexFlag;

            public short Tex_X, Tex_Y;
            public short X, Y; //CROP
            public short Tex_Width, Tex_Height;
            public short Resize_Width, Resize_Height; //CROP

            public byte Reserved;

            public byte Tex_Alpha;
            public int Unknow;
            public short Pixel_Rotation;

            public TexEffect Effect;

            public byte Angle_Rotation;
            public byte Unknow2; //0x1E

            [Category("Crop")]
            [DisplayName("TexID")]
            [Description("Texture ID")]
            public short Texture_ID { get => TexID; set => TexID = value; }

            [Category("Crop")]
            [DisplayName("TexFlag")]
            [Description("Texture Flag")]
            public short Texture_Flag { get => TexFlag; set => TexFlag = value; }

            [Category("Crop")]
            [DisplayName("Tex X")]
            [Description("Texture X")]
            public short Texture_X { get => Tex_X; set => Tex_X = value; }

            [Category("Crop")]
            [DisplayName("Tex Y")]
            [Description("Texture Y")]
            public short Texture_Y { get => Tex_Y; set => Tex_Y = value; }

            [Category("Crop")]
            [DisplayName("X")]
            [Description("Crop X")]
            public short Crop_X { get => X; set => X = value; }

            [Category("Crop")]
            [DisplayName("Y")]
            [Description("Crop Y")]
            public short Crop_Y { get => Y; set => Y = value; }

            [Category("Crop")]
            [DisplayName("Tex Width")]
            [Description("Texture Width")]
            public short Texture_Width { get => Tex_Width; set => Tex_Width = value; }

            [Category("Crop")]
            [DisplayName("Tex Height")]
            [Description("Texture Height")]
            public short Texture_Height { get => Tex_Height; set => Tex_Height = value; }

            [Category("Crop")]
            [DisplayName("Resize Width")]
            [Description("Resize Width")]
            public short Resize_Width_Crop { get => Resize_Width; set => Resize_Width = value; }

            [Category("Crop")]
            [DisplayName("Resize Height")]
            [Description("Resize Height")]
            public short Resize_Height_Crop { get => Resize_Height; set => Resize_Height = value; }

            [Category("Crop")]
            [DisplayName("Reserved")]
            [Description("Reserved")]
            public byte Reserved_Crop { get => Reserved; set => Reserved = value; }

            [Category("Crop")]
            [DisplayName("Tex Alpha")]
            [Description("Texture Alpha")]
            public byte Texture_Alpha { get => Tex_Alpha; set => Tex_Alpha = value; }

            [Category("Crop")]
            [DisplayName("Unknow")]
            [Description("Unknow")]
            public int Unknow_Crop { get => Unknow; set => Unknow = value; }

            [Category("Crop")]
            [DisplayName("Pixel Rotation")]
            [Description("Pixel Rotation")]
            public short Pixel_Rotation_Crop { get => Pixel_Rotation; set => Pixel_Rotation = value; }

            [Category("Crop")]
            [DisplayName("Effect")]
            [Description("Effect")]
            public TexEffect Effect_Crop { get => Effect; set => Effect = value; }

            [Category("Crop")]
            [DisplayName("Angle Rotation")]
            [Description("Angle Rotation")]
            public byte Angle_Rotation_Crop { get => Angle_Rotation; set => Angle_Rotation = value; }


            //ENDS WITH 0xFF

            public enum TexEffect: byte
            {
                Nope = 0,
                BlackAndWhite = 1,
                Invert = 2
            };

            public static Crop Read(byte[] data) =>
                new Crop()
                {
                    TexID = BitConverter.ToInt16(data, 0x00),
                    TexFlag = BitConverter.ToInt16(data, 0x02),
                    Tex_X = BitConverter.ToInt16(data, 0x04),
                    Tex_Y = BitConverter.ToInt16(data, 0x06),
                    X = BitConverter.ToInt16(data, 0x08),
                    Y = BitConverter.ToInt16(data, 0x0A),
                    Tex_Width = BitConverter.ToInt16(data, 0x0C),
                    Tex_Height = BitConverter.ToInt16(data, 0x0E),
                    Resize_Width = BitConverter.ToInt16(data, 0x10),
                    Resize_Height = BitConverter.ToInt16(data, 0x12),
                    Reserved = data[0x14],
                    Tex_Alpha = data[0x15],
                    Unknow = BitConverter.ToInt32(data, 0x16),
                    Pixel_Rotation = BitConverter.ToInt16(data, 0x1A),
                    Effect = (TexEffect)data[0x1C],
                    Angle_Rotation = data[0x1D],
                    Unknow2 = data[0x1E]

                };

            public byte[] Rebuild()
            {
                var list = new List<byte>();
                list.AddRange(BitConverter.GetBytes(TexID));
                list.AddRange(BitConverter.GetBytes(TexFlag));
                list.AddRange(BitConverter.GetBytes(Tex_X));
                list.AddRange(BitConverter.GetBytes(Tex_Y));
                list.AddRange(BitConverter.GetBytes(X));
                list.AddRange(BitConverter.GetBytes(Y));
                list.AddRange(BitConverter.GetBytes(Tex_Width));
                list.AddRange(BitConverter.GetBytes(Tex_Height));
                list.AddRange(BitConverter.GetBytes(Resize_Width));
                list.AddRange(BitConverter.GetBytes(Resize_Height));
                list.Add(Reserved);
                list.Add(Tex_Alpha);
                list.AddRange(BitConverter.GetBytes(Unknow));
                list.AddRange(BitConverter.GetBytes(Pixel_Rotation));
                list.Add((byte)Effect);
                list.Add(Angle_Rotation);
                list.Add(Unknow2);
                list.Add(0xFF);
                return list.ToArray();
            }
        }

        public int ID;
        public short CropControlCount,
            IndexCount,
            CanvaCount,
            CropCount;

        public Index[] Indexes;
        public CropControl[] CropControls;
        public Canva[] Canvases;
        public Crop[] Crops;

        public int CropControlOffset,
            IndexOffset,
            CanvaOffset,
            CropOffset;

        public int Unk1, Unk2, Unk3;


        public class AllCanvas : StringConverter
        {
            public static List<string> data
            {
                get
                {
                    return Enumerable.Range(0, _main.pap.IndexCount).Select(
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
        public class CanvaSelector : StringConverter
        {
            public static List<string> Data
            {
                get
                {
                    return Enumerable.Range(0, _main.pap.select_Canva.Length)
                        .Select(x => x.ToString())
                        .ToList();
                }
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(Data);
            }
        }
        public class CropSelector : StringConverter
        {
            public static List<string> Data
            {
                get
                {
                    return Enumerable.Range(0, _main.pap.select_Crop.Length)
                        .Select(x => x.ToString())
                        .ToList();
                }
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(Data);
            }
        }
        public class ShortEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService editorService =
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService != null)
                {
                    NumericUpDown nud = new NumericUpDown
                    {
                        Minimum = short.MinValue,
                        Maximum = short.MaxValue,
                        Value = Convert.ToDecimal(value),
                        DecimalPlaces = 0,
                        BorderStyle = BorderStyle.None
                    };

                    editorService.DropDownControl(nud);
                    return Convert.ToInt16(nud.Value);
                }

                return value;
            }
        }

        // Campos internos
        public string Choosed = "0";
        public string ChosenCanva = "0";
        public int SelectedCropIndex = 0;

        public Canva[] select_Canva;
        public Crop[] select_Crop;

        private Crop _previewCrop = new Crop();

        [Category("Seleção")]
        [DisplayName("1. Selecionar Folder")]
        [TypeConverter(typeof(AllCanvas))]
        public string Choose
        {
            get => Choosed;
            set
            {
                Choosed = value;
                select_Canva = Canvases
                    .Skip(Indexes[Convert.ToInt32(Choosed)].Canva_Index)
                    .Take(Indexes[Convert.ToInt32(Choosed)].Canva_Count)
                    .ToArray();

                // Resetar estado
                ChosenCanva = "0";
                SelectedCropIndex = 0;
                select_Crop = Array.Empty<Crop>();
                _previewCrop = new Crop();
            }
        }

        [Category("Seleção")]
        [DisplayName("2. Selecionar Canva")]
        [TypeConverter(typeof(CanvaSelector))]
        public string ChooseCanva
        {
            get => ChosenCanva;
            set
            {
                ChosenCanva = value;
                var currentCanva = select_Canva[Convert.ToInt32(value)];
                select_Crop = Crops
                    .Skip(currentCanva.Crop_Control_Index)
                    .Take(currentCanva.Crop_Control_CountUnk)
                    .ToArray();

                // Resetar estado
                SelectedCropIndex = 0;
                _previewCrop = new Crop();
            }
        }

        [Category("Seleção")]
        [DisplayName("3. Selecionar Crop")]
        [TypeConverter(typeof(CropSelector))]
        public string ChooseCrop
        {
            get => SelectedCropIndex.ToString();
            set
            {

                SelectedCropIndex = Convert.ToInt32(value);
                if (select_Crop != null && SelectedCropIndex < select_Crop.Length)
                {
                    var selected = select_Crop[SelectedCropIndex];
                    _previewCrop = new Crop
                    {
                        TexID = selected.TexID,
                        TexFlag = selected.TexFlag,
                        Tex_X = selected.Tex_X,
                        Tex_Y = selected.Tex_Y,
                        X = selected.X,
                        Y = selected.Y,
                        Tex_Width = selected.Tex_Width,
                        Tex_Height = selected.Tex_Height,
                        Resize_Width = selected.Resize_Width,
                        Resize_Height = selected.Resize_Height,
                        Reserved = selected.Reserved,
                        Tex_Alpha = selected.Tex_Alpha,
                        Unknow = selected.Unknow,
                        Pixel_Rotation = selected.Pixel_Rotation,
                        Effect = selected.Effect,
                        Angle_Rotation = selected.Angle_Rotation,
                        Unknow2 = selected.Unknow2
                    };
                    if(!_main.pap_editor.Visible)
                        _main.pap_editor.Show();

                    RenderCropCanvas(selected);
                }
            }
        }
        private enum ResizeDirection { None, Right, Bottom, BottomRight }
        private ResizeDirection resizeDirection = ResizeDirection.None;

        private ResizeDirection GetResizeDirection(Point mouse, Rectangle rect)
        {
            bool nearRight = Math.Abs(mouse.X - (rect.Right)) <= resizeMargin;
            bool nearBottom = Math.Abs(mouse.Y - (rect.Bottom)) <= resizeMargin;

            if (nearRight && nearBottom) return ResizeDirection.BottomRight;
            if (nearRight) return ResizeDirection.Right;
            if (nearBottom) return ResizeDirection.Bottom;

            return ResizeDirection.None;
        }

        private bool isDragging = false;
        private bool isResizing = false;
        private Point dragOffset;
        private const int resizeMargin = 6;
        private int? _lastTexID = null;
        private Bitmap _lastBaseImage = null;
        public Rectangle cropTangle = default;

        public void InitializeViewerEvents()
        {
            var viewer = _main.pap_editor.BaseViewer;

            viewer.MouseDown -= BaseViewer_MouseDown;
            viewer.MouseMove -= BaseViewer_MouseMove;
            viewer.MouseUp -= BaseViewer_MouseUp;
            viewer.Paint -= BaseViewer_Paint;

            viewer.MouseDown += BaseViewer_MouseDown;
            viewer.MouseMove += BaseViewer_MouseMove;
            viewer.MouseUp += BaseViewer_MouseUp;
            viewer.Paint += BaseViewer_Paint;
            viewer.Cursor = Cursors.Cross;

            EnableDoubleBuffering(viewer);
        }

        private void EnableDoubleBuffering(Control c)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, c, new object[] { true });
        }

        private bool IsOnEdge(Point mouse, Rectangle rect) =>
            Math.Abs(mouse.X - (rect.X + rect.Width)) < resizeMargin &&
            Math.Abs(mouse.Y - (rect.Y + rect.Height)) < resizeMargin;

        private void BaseViewer_Paint(object sender, PaintEventArgs e)
        {
            if (cropTangle != default)
                using (Pen pen = new Pen(Color.Red, 2))
                    e.Graphics.DrawRectangle(pen, cropTangle);
        }

        private void BaseViewer_MouseDown(object sender, MouseEventArgs e)
        {
            resizeDirection = GetResizeDirection(e.Location, cropTangle);
            if (resizeDirection != ResizeDirection.None)
            {
                isResizing = true;
            }
            else if (cropTangle.Contains(e.Location))
            {
                isDragging = true;
                dragOffset = new Point(e.X - cropTangle.X, e.Y - cropTangle.Y);
            }
        }

        private void BaseViewer_MouseMove(object sender, MouseEventArgs e)
        {
            var viewer = _main.pap_editor.BaseViewer;

            if (isDragging)
            {
                cropTangle.X = e.X - dragOffset.X;
                cropTangle.Y = e.Y - dragOffset.Y;
                viewer.Invalidate();
            }
            else if (isResizing)
            {
                switch (resizeDirection)
                {
                    case ResizeDirection.Right:
                        cropTangle.Width = Math.Max(1, e.X - cropTangle.X);
                        break;
                    case ResizeDirection.Bottom:
                        cropTangle.Height = Math.Max(1, e.Y - cropTangle.Y);
                        break;
                    case ResizeDirection.BottomRight:
                        cropTangle.Width = Math.Max(1, e.X - cropTangle.X);
                        cropTangle.Height = Math.Max(1, e.Y - cropTangle.Y);
                        break;
                }
                viewer.Invalidate();
            }
            else
            {
                resizeDirection = GetResizeDirection(e.Location, cropTangle);
                switch (resizeDirection)
                {
                    case ResizeDirection.Right:
                        viewer.Cursor = Cursors.SizeWE;
                        break;
                    case ResizeDirection.Bottom:
                        viewer.Cursor = Cursors.SizeNS;
                        break;
                    case ResizeDirection.BottomRight:
                        viewer.Cursor = Cursors.SizeNWSE;
                        break;
                    default:
                        viewer.Cursor = cropTangle.Contains(e.Location) ? Cursors.SizeAll : Cursors.Cross;
                        break;
                }
            }
        }

        private void BaseViewer_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isResizing = false;
            resizeDirection = ResizeDirection.None;

            if (_previewCrop != null)
            {
                _previewCrop.X = (short)cropTangle.X;
                _previewCrop.Y = (short)cropTangle.Y;
                _previewCrop.Tex_Width = (short)cropTangle.Width;
                _previewCrop.Tex_Height = (short)cropTangle.Height;

                UpdateCropCanvas(_previewCrop);
                _main.PropertyControl.Refresh();
            }
        }

        private void UpdateCropCanvas(Crop selected)
        {
            if (_lastBaseImage == null) return;

            Rectangle rect = new Rectangle(selected.X, selected.Y, selected.Tex_Width, selected.Tex_Height);
            Bitmap clone = _lastBaseImage.Clone(rect, _lastBaseImage.PixelFormat);
            Bitmap canvas = new Bitmap(512, 448); // PS2 Game res

            int centerX = canvas.Width / 2;
            int centerY = canvas.Height / 2;

            int pixelX = centerX + selected.Tex_X;
            int pixelY = centerY - selected.Tex_Y;

            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(clone, pixelX, pixelY);
            }

            switch(_previewCrop.Effect)
            {
                case Crop.TexEffect.Nope:

                    break;
                case Crop.TexEffect.BlackAndWhite:
                    canvas = ToGrayscale(canvas);
                    break;
                case Crop.TexEffect.Invert:
                    canvas = InvertColors(canvas);
                    break;
            }

            _main.pap_editor.CropBox.Image?.Dispose();
            _main.pap_editor.CropBox.Image = canvas;
            _main.pap_editor.CropBox.Refresh(); // Força atualização imediata
        }

        public void RenderCropCanvas(Crop selected)
        {
            if (selected == null)
                return;

            _previewCrop = selected;

            InitializeViewerEvents();

            if (_lastTexID != selected.TexID || _lastBaseImage == null)
            {
                RefImage.GetImage(selected.TexID, out var mage);
                _lastBaseImage?.Dispose();
                _lastBaseImage = new Bitmap(mage);
                _lastTexID = selected.TexID;

                _main.pap_editor.BaseViewer.Image = _lastBaseImage;
                _main.pap_editor.BaseViewer.Width = _lastBaseImage.Width;
                _main.pap_editor.BaseViewer.Height = _lastBaseImage.Height;
            }

            cropTangle = new Rectangle(selected.X, selected.Y, selected.Tex_Width, selected.Tex_Height);
            _main.pap_editor.BaseViewer.Invalidate();

            UpdateCropCanvas(selected);
        }


        // --- Crop Preview Properties ---

        [Category("Crop Preview")]
        [DisplayName("A - Texture ID")]

        public short Preview_Texture_ID
        {
            get => _previewCrop.TexID;
            set { _previewCrop.TexID = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("B - Texture Flag")]
        public short Preview_Texture_Flag
        {
            get => _previewCrop.TexFlag;
            set { _previewCrop.TexFlag = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("C - Texture X")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Texture_X
        {
            get => _previewCrop.Tex_X;
            set { _previewCrop.Tex_X = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("D - Texture Y")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Texture_Y
        {
            get => _previewCrop.Tex_Y;
            set { _previewCrop.Tex_Y = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("E - Crop X")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Crop_X
        {
            get => _previewCrop.X;
            set { _previewCrop.X = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("F - Crop Y")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Crop_Y
        {
            get => _previewCrop.Y;
            set { _previewCrop.Y = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("G - Texture Width")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Texture_Width
        {
            get => _previewCrop.Tex_Width;
            set { _previewCrop.Tex_Width = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("H - Texture Height")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Texture_Height
        {
            get => _previewCrop.Tex_Height;
            set { _previewCrop.Tex_Height = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("I - Resize Width")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Resize_Width
        {
            get => _previewCrop.Resize_Width;
            set { _previewCrop.Resize_Width = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("J - Resize Height")]
        [Editor(typeof(ShortEditor), typeof(UITypeEditor))]
        public short Preview_Resize_Height
        {
            get => _previewCrop.Resize_Height;
            set { _previewCrop.Resize_Height = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("K - Reserved")]
        public byte Preview_Reserved
        {
            get => _previewCrop.Reserved;
            set { _previewCrop.Reserved = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("L - Texture Alpha")]
        public byte Preview_Texture_Alpha
        {
            get => _previewCrop.Tex_Alpha;
            set { _previewCrop.Tex_Alpha = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("M - Unknow")]
        public int Preview_Unknow
        {
            get => _previewCrop.Unknow;
            set { _previewCrop.Unknow = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("N - Pixel Rotation")]
        public short Preview_Pixel_Rotation
        {
            get => _previewCrop.Pixel_Rotation;
            set { _previewCrop.Pixel_Rotation = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("O - Effect")]
        public Crop.TexEffect Preview_Effect
        {
            get => _previewCrop.Effect;
            set { _previewCrop.Effect = value; UpdateOriginalCrop(); }
        }

        [Category("Crop Preview")]
        [DisplayName("P - Angle Rotation")]
        public byte Preview_Angle_Rotation
        {
            get => _previewCrop.Angle_Rotation;
            set { _previewCrop.Angle_Rotation = value; UpdateOriginalCrop(); }
        }

        // --- Método para refletir alterações no array original ---
        private void UpdateOriginalCrop()
        {
            var selected = _previewCrop;
            if (select_Crop != null && SelectedCropIndex < select_Crop.Length)
            {
                int indexInOriginal = Array.IndexOf(Crops, select_Crop[SelectedCropIndex]);
                if (indexInOriginal >= 0)
                {
                    Crops[indexInOriginal] = _previewCrop;
                    select_Crop[SelectedCropIndex] = _previewCrop;
                }
                RenderCropCanvas(selected);
            }
        }

        public static Main _main;
        public PAP(string Filename, Main main)
        {
            RefImage = new IMG(main).Read(File.ReadAllBytes(Filename.Substring(0, Filename.Length - 3) + "img"),
                main.pap_editor.BaseViewer);

            _main = main;
            // Suponha que você queira adicionar uma nova linha com altura automática
            _main.viewLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _main.viewLayout.RowCount += 1;
            _main.pap_editor = new PAP_Viewer();
            // Criar um controle qualquer, por exemplo, um Label

            button.Text = "Apply";
            button.Click += (s, e) =>
            {
                // Aqui você pode adicionar a lógica para o que acontece quando o botão é clicado
                byte[] rebuiltData = Rebuild();
                File.WriteAllBytes(Filename, rebuiltData);
                MessageBox.Show("File/changes saved successfully!","Action");
            };
            button.AutoSize = true;

            // Adiciona o controle na nova linha, na coluna 0
            _main.viewLayout.Controls.Add(button, 0, _main.viewLayout.RowCount - 1);


            FileName = Filename;
            byte[] data = File.ReadAllBytes(Filename);

            ID = BitConverter.ToInt32(data, 0x00);
            CropControlCount = BitConverter.ToInt16(data, 0x04);
            IndexCount = BitConverter.ToInt16(data, 0x06);
            CanvaCount = BitConverter.ToInt16(data, 0x08);
            CropCount = BitConverter.ToInt16(data, 0x0A);

            CropControlOffset = BitConverter.ToInt32(data, 0x14);
            IndexOffset = BitConverter.ToInt32(data, 0x18);
            CanvaOffset = BitConverter.ToInt32(data, 0x1C);
            CropOffset = BitConverter.ToInt32(data, 0x20);

            Unk1 = BitConverter.ToInt32(data, 0x24);
            Unk2 = BitConverter.ToInt32(data, 0x28);
            Unk3 = BitConverter.ToInt32(data, 0x2C);

            data = data.Skip(4).ToArray();


            CropControls = Enumerable.Range(0, CropControlCount)
                .Select(x => CropControl.Read(data.ReadBytes(CropControlOffset + (x * 0x0C), 0x0C))).ToArray();

            Indexes = Enumerable.Range(0, IndexCount)
                .Select(x => Index.Read(data.ReadBytes(IndexOffset + (x * 0x0C), 0x0C))).ToArray();

            Canvases = Enumerable.Range(0, CanvaCount)
                .Select(x => Canva.Read(data.ReadBytes(CanvaOffset + (x * 0x20), 0x20))).ToArray();

            Crops = Enumerable.Range(0, CropCount)
                .Select(x => Crop.Read(data.ReadBytes(CropOffset + (x*0x20), 0x20))).ToArray();


        }
        public Bitmap ToGrayscale(Bitmap original)
        {
            Bitmap gray = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    int luminance = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    Color grayColor = Color.FromArgb(pixel.A, luminance, luminance, luminance);
                    gray.SetPixel(x, y, grayColor);
                }
            }

            return gray;
        }
        public Bitmap InvertColors(Bitmap original)
        {
            Bitmap inverted = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(
                        pixel.A,
                        255 - pixel.R,
                        255 - pixel.G,
                        255 - pixel.B
                    );
                    inverted.SetPixel(x, y, invertedColor);
                }
            }

            return inverted;
        }
        public byte[] Rebuild()
        {
            var list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(BitConverter.GetBytes(CropControlCount));
            list.AddRange(BitConverter.GetBytes(IndexCount));
            list.AddRange(BitConverter.GetBytes(CanvaCount));
            list.AddRange(BitConverter.GetBytes(CropCount));

            list.AddRange(new byte[8]); //0x0 UINT64

            list.AddRange(BitConverter.GetBytes(CropControlOffset));
            list.AddRange(BitConverter.GetBytes(IndexOffset));
            list.AddRange(BitConverter.GetBytes(CanvaOffset));
            list.AddRange(BitConverter.GetBytes(CropOffset));
            list.AddRange(BitConverter.GetBytes(Unk1));
            list.AddRange(BitConverter.GetBytes(Unk2));
            list.AddRange(BitConverter.GetBytes(Unk3));

            list.AddRange(CropControls.SelectMany(x => x.Rebuild()));
            list.AddRange(Indexes.SelectMany(x => x.Rebuild()));
            list.AddRange(Canvases.SelectMany(x => x.Rebuild()));
            list.AddRange(Crops.SelectMany(x => x.Rebuild()));
            return list.ToArray();
        }

    }

}
