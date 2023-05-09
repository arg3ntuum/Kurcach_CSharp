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
        private int coefficient { get; set; }

        public FormChild(FormGeneral parent, string caption)
        {
            InitializeComponent();

            // Присваиваем публичным свойствам значения
            ImageBuffer = null;
            ImagePath = string.Empty;
            IsChanged = false;
            IsZoomed = false;

            //По дефолту выключаем ChangePoint_NumericUpDown
            ChangePoint_NumericUpDown.Enabled = false;

            // Присваиваем приватным свойствам значения
            _parent = parent;
            coefficient = 2;

            // Подписываем события
            this.Paint += FormChild_Paint;
            this.Resize += FormChild_Resize;

            // Присваивание контейнеру родителя данной формы
            this.MdiParent = parent;

            // Задание заголовка
            this.Text = caption;
        }
        private void FormChild_Load(object sender, EventArgs e) {  }
        public void FormChild_Paint(object sender, PaintEventArgs e)
        {
            if (ImageBuffer is null) { 
                MessageBox.Show(Const.Messages.ImageBuffesIsNull);
                return;
            }

            Graphics graphics = e.Graphics;
            Bitmap userImage = null;
            
            if (IsChanged is true)
                userImage = GetChangedImage();
            else 
                userImage = ImageBuffer;

            if (IsZoomed is false)
                graphics.DrawImage(userImage, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
            else
                graphics.DrawImage(userImage, -PointToClient(Cursor.Position).X, -PointToClient(Cursor.Position).Y,
                    ClientRectangle.Width * coefficient, ClientRectangle.Height * coefficient);

            graphics.Dispose();

            if (IsChanged is true)
            {
                // Получаем ответ от пользователя
                DialogResult dialogResult =
                    MessageBox.Show(Const.Messages.DownloadImageToBuffer, Const.Messages.Attention, MessageBoxButtons.YesNo);

                if(dialogResult == DialogResult.Yes)
                    ImageBuffer = userImage;
            }
        }
        public Bitmap GetChangedImage()
        {
            Bitmap bitmap = ImageBuffer;
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++) {
                    Color color = Color.FromArgb(
                            GetNumPixel(bitmap.GetPixel(x, y).R),
                            GetNumPixel(bitmap.GetPixel(x, y).G),
                            GetNumPixel(bitmap.GetPixel(x, y).B)
                            );
                    
                    bitmap.SetPixel(x, y, color);
                }
            return bitmap;
        }
        private byte GetNumPixel(byte num)
        {
            byte newNum = 0;
            try
            {
                newNum = Convert.ToByte(num + ChangePoint_NumericUpDown.Value);
            }
            catch (OverflowException)
            {
                newNum = 254;
            }
            return newNum;
        }
        public void UploadImageToBuffer()
        {
            if (_parent.ImageBuffer is null) { 
                return;
            }

            if (_parent.ImagePath == string.Empty) { 
                MessageBox.Show(Const.Messages.IsNullOrWhiteSpace);
                return;
            }

            ImageBuffer = _parent.ImageBuffer;
            ImagePath = _parent.ImagePath;
            ImageFormat = ImageBuffer.RawFormat;
        }
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
        private void ChangePoint_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (IsChanged is false)
                IsChanged = true;
            Invalidate();//перерисовать
        }
    }
}
