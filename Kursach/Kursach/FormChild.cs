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
        public Image ImageBuffer { get; private set; }
        public ImageFormat ImageFormat { get; private set; }
        
        public string ImagePath { get; private set; }

        private FormGeneral _parent { get; set; }
        private Graphics _grfx { get; set; }
        private Rectangle _rect{ get; set; }

        private Color _defaultColor { get; set; }

        public FormChild(FormGeneral parent, string caption)
        {
            InitializeComponent();

            // Присваиваем публичным свойствам значения
            ImageBuffer = null;
            ImagePath = string.Empty;

            // Присваиваем приватным свойствам значения
            _parent = parent;
            _grfx = null;
            _rect = ClientRectangle;
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
            if (ImageBuffer == null)
                return;

            _grfx = e.Graphics;

            _grfx.DrawImage(ImageBuffer, 0, 0, _rect.Width, _rect.Height);

            _grfx.Dispose();
        }
        public void UploadImageToBuffer()
        {
            if (_parent.ImageBuffer == null || _parent.ImagePath == string.Empty)
                return;

            ImageBuffer = _parent.ImageBuffer;
            ImagePath = _parent.ImagePath;
            ImageFormat = GetImageFormat();
        }
        public ImageFormat GetImageFormat() {
            string extension = Path.GetExtension(ImagePath).ToLower();

            switch (extension)
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                case ".png":
                    return ImageFormat.Png;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".jpg":
                    return ImageFormat.Jpeg;
                default:
                    return null;
            }
        }
        private void FormChild_Resize(object sender, EventArgs e) =>
            Invalidate();//перерисовать

        private void FormChild_Load(object sender, EventArgs e) {  }
    }
}
