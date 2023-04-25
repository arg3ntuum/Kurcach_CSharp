using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace Kursach
{
    /*
    Задачи:
    
    +1) Створити – створення нового дочірнього вікна.
    +2) Відкрити – відкриття файлів із зображеннями 
            ( формат файлів *.bmp, *.jpg, *.png, *.gif, *.tiff).
    3) Зберегти – збереження зображення в тому самому форматі.
    4) Зберегти як… - збереження зображення в одному з наступних форматів 
            *.bmp, *.jpg, *.png, *.gif, *.tiff
    5) Закрити – закриття дочірнього вікна. Перед закриттям дочірнього вікна 
            потрібно вивести запит про необхідність збереження змін у файл, якщо вони відбувались.
    6) Вихід – закриття всього додатку. Перед закриттям додатку необхідно вивести запит 
            про збереження змін в усіх відкритих файлах, в яких вони відбувалися.
    */
    public partial class FormGeneral : Form
    {
        public const string FilesFilter = 
            "Image files(*.bmp;*.jpg;*.png;*.gif;*.tiff)|*.bmp;*.jpg;*.png;*.gif;*.tiff";

        // Буфер для картинки, через який передається на форму картинка
        public Image ImageBuffer { get; private set; }
        public string ImageName { get; private set; }
        private int _nextFormNumber { get; set; }

        public FormGeneral()
        {
            InitializeComponent();

            // Присваиваем свойствам значения
            ImageBuffer = null;
            ImageName = string.Empty;
            _nextFormNumber = 1;

            // Присваиваем название для формы
            Text = "Kursach";
            // Форма является контейнером для дочерних MDI-форм
            IsMdiContainer = true;

            // Встановити назву для image файлу
            OpenFileDialogWindow.FileName = "image.png";
            // Встановлюємо фільтр для файлів
            OpenFileDialogWindow.Filter =
                FilesFilter;
            
            //ResizeRedraw = true;
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создание нового экземпляра дочерней формы
            FormChild newChild = new FormChild(this, "Редактор" + _nextFormNumber++);

            // Вывод созданной формы
            newChild.Show();

            // Подзагрузка Image
            newChild.UploadImageToBuffer();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //если результат равен Cancel, то выходим
            if (OpenFileDialogWindow.ShowDialog() == DialogResult.Cancel)
                return;

            // получаем выбранный файл
            ImageName = OpenFileDialogWindow.FileName;

            try
            {
                ImageBuffer = Image.FromFile(ImageName);
            }
            catch
            {
                MessageBox.Show("Cannot find file " + ImageName +
               "!", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            // Определение активного дочернего MDI-окна
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            if (activeChildForm == null)
            {
                DialogResult dialogResult =
                    MessageBox.Show("Вы хотите создать форму для отображения картинки?", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    CreateToolStripMenuItem_Click(null, null);
            }
            else {
                activeChildForm.UploadImageToBuffer();
                activeChildForm.Invalidate();
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormChild activeChildForm = (FormChild)this.ActiveMdiChild) { 
                if (activeChildForm != null)
                {
                    DialogResult dialogResult =
                         MessageBox.Show("Вы хотите сохранить картинку в том же файле?", "Внимание!", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {

                        activeChildForm.ImageBuffer.Save(activeChildForm.ImagePath);
                        activeChildForm.Close();
                    }
                }
                else MessageBox.Show("ActiveMdiChild == null!");
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            if (activeChildForm != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = FilesFilter;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FileName = saveFileDialog1.FileName;
                    activeChildForm.ImageBuffer.Save(FileName);
                    activeChildForm.Close();
                }
            }
            else MessageBox.Show("ActiveMdiChild == null!");
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Определение активного дочернего MDI-окна
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            // Перед тем как использовать потолок, необходимо убедиться в том, что он доступен
            if (activeChildForm != null)
                activeChildForm.Close();// Закрытие окна
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormGeneral_Load(object sender, EventArgs e)
        {

        }
    }
}




