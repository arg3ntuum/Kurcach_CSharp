using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursach
{
    public partial class FormChild : Form
    {
        public Bitmap ImageBuffer { get; private set; }
        public ImageFormat ImageFormat { get; private set; }

        public string ImagePath { get; private set; }
        public bool IsChanged { get; private set; }
        public bool IsZoomed { get; private set; }

        private FormGeneral _parent { get; set; }
        private Graphics _grfx { get; set; }
        private Point _cursorCoordinates { get; set; } 
        private Color _defaultColor { get; set; }

        public FormChild(FormGeneral parent, string caption)
        {
            InitializeComponent();

            // Присваиваем публичным свойствам значения
            ImageBuffer = null;
            ImagePath = string.Empty;
            IsChanged = false;
            IsZoomed = false;
            _cursorCoordinates = new Point();

            // Присваиваем приватным свойствам значения
            _parent = parent;
            _grfx = null;
            _defaultColor = Color.White;

            // Подписываем события
            this.Paint += FormChild_Paint;
            this.Resize += FormChild_Resize;

            // Присваивание контейнеру родителя данной формы
            this.MdiParent = parent;

            // Задание заголовка
            this.Text = caption;
        }
        public void FormChild_Paint(object sender, PaintEventArgs e)
        {
            if (ImageBuffer is null)
                throw new ArgumentException(Const.Messages.ImageBuffesIsNull);

            Point distance = new Point(_cursorCoordinates.X, _cursorCoordinates.Y);
            int coefficient = 2;

            _grfx = e.Graphics;

            if(IsZoomed is false)
                _grfx.DrawImage(ImageBuffer, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
            else 
                _grfx.DrawImage(
                    ImageBuffer, 
                    -_cursorCoordinates.X,
                    -_cursorCoordinates.Y,
                    ClientRectangle.Width * coefficient,
                    ClientRectangle.Height * coefficient);

            _grfx.Dispose();
        }
        public void UploadImageToBuffer()
        {
            if (_parent.ImageBuffer is null)
                throw new ArgumentException(Const.Messages.ImageBuffesIsNull);

            if ( _parent.ImagePath == string.Empty)
                throw new ArgumentException("ImagePath is empty!");

            ImageBuffer = _parent.ImageBuffer;
            ImagePath = _parent.ImagePath;
            ImageFormat = ImageBuffer.RawFormat;
        }
        //public static string GetExtension(ImageFormat format)
        //{
        //    if (format == ImageFormat.Icon)
        //        return ".ico";
        //    if (format == ImageFormat.MemoryBmp)
        //        return ".bmp";
        //    return ImageCodecInfo.GetImageEncoders()
        //        .FirstOrDefault(x => x.FormatID == format.Guid)?
        //        .FilenameExtension
        //        .Split(';')[0]
        //        .TrimStart('*')
        //        .ToLower() ?? $".{format.ToString().ToLower()}";
        //}
        private void FormChild_Resize(object sender, EventArgs e) {
            Invalidate();//перерисовать
        }
        private void FormChild_Click(object sender, EventArgs e)
        {
            if(_parent.IsZoomWorking is false)
                return;

            if (IsZoomed is false)
                IsZoomed = true;
            else 
                IsZoomed = false;

            Invalidate();//перерисовать
        }
        private void FormChild_MouseMove(object sender, MouseEventArgs e)
        {
            _cursorCoordinates = new Point(e.X, e.Y);
        }
        private void FormChild_Load(object sender, EventArgs e) {  }

    }
}
