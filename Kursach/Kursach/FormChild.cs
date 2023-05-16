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
        // Буфер картинки
        public Bitmap ImageBuffer { get; private set; }
        // Путь к картинке
        public string ImagePath { get; private set; }
        // Статус измененной картинки
        public bool IsChanged { get; private set; }
        // Статус приближения картинки
        public bool IsZoomed { get; private set; }
        // Коеффициент приближения
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
            // Проверяем буфер на имение картинки
            if (ImageBuffer is null) { 
                MessageBox.Show(Const.Messages.ImageBuffesIsNull);
                return;
            }

            // Переменная графики
            Graphics graphics = e.Graphics;
            // Переменная сохраняющая картинку
            Bitmap userImage = null;
            
            // Если картинка была изменена
            if (IsChanged is true)
                // Получаем измененную картинку
                userImage = GetChangedImage();
            else // Получаем не измененную картинку
                userImage = ImageBuffer;

            // Отслеживаем статус картинки
            if (IsZoomed is false)
                // Прорисовываем картинку 
                graphics.DrawImage(userImage, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
            else
                // Прорисовывем приближенную картинку
                graphics.DrawImage(userImage, -PointToClient(Cursor.Position).X, -PointToClient(Cursor.Position).Y,
                    ClientRectangle.Width * coefficient, ClientRectangle.Height * coefficient);

            // Освобождаем ресурсы графики
            graphics.Dispose();

            // Если изменения присутствуют
            if (IsChanged is true)
            {
                // Получаем ответ от пользователя
                DialogResult dialogResult =
                    MessageBox.Show(Const.Messages.DownloadImageToBuffer, Const.Messages.Attention, MessageBoxButtons.YesNo);

                // Подгружаем новую картинку в буфер
                if(dialogResult == DialogResult.Yes)
                    ImageBuffer = userImage;
            }
        }
        public Bitmap GetChangedImage()
        {
            // Создаем временную картинку для изменений
            Bitmap bitmap = ImageBuffer;
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++) {
                    // Создаем новый цвет для пикселя
                    Color color = Color.FromArgb(
                            GetNumPixel(bitmap.GetPixel(x, y).R),
                            GetNumPixel(bitmap.GetPixel(x, y).G),
                            GetNumPixel(bitmap.GetPixel(x, y).B)
                            );
                    // Устанавливаем новый пиксель
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
            // Проверяем буфер картинки
            if (((FormGeneral)this.MdiParent).ImageBuffer is null) { 
                return;
            }

            // Проверяем путь картинки
            if (((FormGeneral)this.MdiParent).ImagePath == string.Empty) { 
                MessageBox.Show(Const.Messages.IsNullOrWhiteSpace);
                return;
            }

            // Присваиваем в поля данные
            ImageBuffer = ((FormGeneral)this.MdiParent).ImageBuffer;
            ImagePath = ((FormGeneral)this.MdiParent).ImagePath;
        }
        private void FormChild_Resize(object sender, EventArgs e) {
            Invalidate();//перерисовать
        }
        private void FormChild_Click(object sender, EventArgs e)
        {
            // Если приближение не активировано выходим
            if(((FormGeneral)this.MdiParent).IsZoomWorking is false)
                return;

            // Если приближение в этой форме не было произведено, отмечаем
            if (IsZoomed is false)
                IsZoomed = true;
            else 
            // Если приближение в этой форме было произведено, выключаем
                IsZoomed = false;

            Invalidate();//перерисовать
        }
        private void ChangePoint_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            // Отмечаем изменение картинки
            if (IsChanged is false)
                IsChanged = true;
            Invalidate();//перерисовать
        }
    }
}
