using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace Kursach
{
    public partial class FormGeneral : Form
    {
        // Буфер для картинки
        public Bitmap ImageBuffer { get; private set; }
        // Путь к картинке
        public string ImagePath { get; private set; }
        // Активен ли зум
        public bool IsZoomWorking { get; private set; }
        // Изменитель картинки
        public bool IsNumericChangerEnable { get; private set; }
        // Количество редакторов
        private int _nextFormNumber { get; set; }

        public FormGeneral()
        {
            InitializeComponent();

            // Присваиваем свойствам значения
            ImageBuffer = null;
            ImagePath = string.Empty;
            _nextFormNumber = 1;
            IsZoomWorking = false;

            // Присваиваем название для формы
            Text = Const.ProgramName;
            // Форма является контейнером для дочерних MDI-форм
            IsMdiContainer = true;

            // Встановити назву для image файлу
            OpenFileDialogWindow.FileName = Const.ImageFileName;
            // Встановлюємо фільтр для файлів
            OpenFileDialogWindow.Filter = Const.FilesFilter;
        }
        //1 Point
        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создание нового экземпляра дочерней формы
            FormChild newChild = new FormChild(this, "Редактор" + _nextFormNumber++);

            // Вывод созданной формы
            newChild.Show();

            // Подзагрузка Image
            newChild.UploadImageToBuffer();

            // Включаем changer картинки
            if(IsNumericChangerEnable is true)
                newChild.ChangePoint_NumericUpDown.Enabled = true;
            else 
                newChild.ChangePoint_NumericUpDown.Enabled = false;
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Если результат равен Cancel, то выходим
            if (OpenFileDialogWindow.ShowDialog() == DialogResult.Cancel)
                return;

            // Получаем выбранный файл
            ImagePath = OpenFileDialogWindow.FileName;

            try
            {
                // Пытаемся открыть файл
                using (var bmp = (Bitmap)Image.FromFile(ImagePath))
                    ImageBuffer = new Bitmap(bmp);
            }
            catch
            {
                // Если файл не открылся
                MessageBox.Show("Cannot find file " + ImagePath + "!", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            // Если нет активной формы, спросим про создание
            if (ActiveMdiChild is null)
            {
                DialogResult dialogResult =
                    MessageBox.Show(Const.Messages.CreateFormToViewImage, Const.Messages.Attention, MessageBoxButtons.YesNo);
                
                // Если пользователь ответил да, то создаем формочку
                if (dialogResult == DialogResult.Yes)
                    CreateToolStripMenuItem_Click(null, null);
            }
            else {
                // Определение активного дочернего MDI-окна
                FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

                // Подгружаем картинку в буфер
                activeChildForm.UploadImageToBuffer();
                // Перерисовываем графику
                activeChildForm.Invalidate();
            }
            // Очищаем буфер
            ImageBuffer = null;
            // Очищаем путь к картинке
            ImagePath = string.Empty;
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма
            if (ActiveMdiChild is null)
            {
                MessageBox.Show(Const.Messages.ActiveFormIsNull);
                return;
            }

            // Получаем активную форму 
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            // Получаем ответ от пользователя
            DialogResult dialogResult =
                MessageBox.Show(Const.Messages.ImageInThisFile, Const.Messages.Attention, MessageBoxButtons.YesNo);

            // Если ответ да, то сохраняем
            if (dialogResult == DialogResult.Yes)
                Save(activeChildForm, activeChildForm.ImagePath);
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма
            if (ActiveMdiChild is null)
            {
                MessageBox.Show(Const.Messages.ActiveFormIsNull);
                return;
            }

            // Создаем объект SaveFileDialog
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            // Проводим настройку
            saveFileDialog1.Filter = Const.FilesFilter;

            // Определение активного дочернего MDI-окна
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            // Проверяем, нажал ли пользователь ОК, и тогда сохраняем
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                Save(activeChildForm, saveFileDialog1.FileName);
        }
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма
            if (ActiveMdiChild is null)
            {
                MessageBox.Show(Const.Messages.ActiveFormIsNull);
                return;
            }

            // Определение активного дочернего MDI-окна
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            // Сохраняем значение, если пользователь того хочет
            GetAnswerToSaveAndSave(activeChildForm);

            // Закрытие окна
            activeChildForm.Close();
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма и закрываем приложение, если нет их
            if (ActiveMdiChild is null)
                Application.Exit();
            
            // Перебираем массив дочерних форм
            foreach (FormChild item in MdiChildren)
            {
                // Выбираем каждую форму
                item.Select();
                // Сохраняем картинку
                GetAnswerToSaveAndSave(item);
            }

            //Закрываем приложение
            Application.Exit();
        }
        private void GetAnswerToSaveAndSave(FormChild formChild) {
            // Если файл был изменен, сохраняем значения
            if (formChild.IsChanged is true)
            {
                // Проверяем, нажал ли пользователь ОК, и тогда сохраняем
                DialogResult dialogResult =
                    MessageBox.Show(Const.Messages.ImageInThisFile, Const.Messages.Attention, MessageBoxButtons.OKCancel);

                // Проверяем, нажал ли пользователь ОК, и тогда сохраняем
                if (dialogResult == DialogResult.OK)
                    Save(formChild, formChild.ImagePath);
            }
            else
                MessageBox.Show(Const.Messages.FileNotWasChanged); 
        }
        private void Save(FormChild activeChildForm, string path)
        {
            // Проверяем пуст ли буфер картинки
            if (activeChildForm.ImageBuffer is null)
            {
                MessageBox.Show(Const.Messages.ImageBuffesIsNull);
                return;
            }

            // Проверяем пуст ли путь к картинке
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show(Const.Messages.IsNullOrWhiteSpace);
                return;
            }

            // Если в блоке будет ошибка, прокинуть ее через MessageBox
            try
            {
                // Удаляем файл
                File.Delete(path);

                // Сохраняем файл
                activeChildForm.ImageBuffer.Save(path, activeChildForm.ImageBuffer.RawFormat);

                //Закрываем форму
                activeChildForm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }
        //2 Point
        private void InformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма
            if (ActiveMdiChild is null) {
                MessageBox.Show(Const.Messages.ActiveFormIsNull);
                return;
            }

            // Получаем активную форму 
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            //Проверяем, существует ли картинка на форме
            if (activeChildForm.ImageBuffer is null) { 
                MessageBox.Show(Const.Messages.ImageBuffesIsNull);
                return;
            }

            // Цифра для формулы
            double inch = 2.54;
            
            // Переменные dpi
            float dpiX, dpiY, dpiBase = 96;
            using (var g = new Control().CreateGraphics())
            {
                dpiX = g.DpiX;
                dpiY = g.DpiY;
            }

            // Создадим переменную текста Информации
            string text =
                $"\nFile name: {Path.GetFileName(activeChildForm.ImagePath)};" +
                $"\nFile path: {activeChildForm.ImagePath};" +
                $"\nFile format: {activeChildForm.ImageBuffer.RawFormat};" +
                $"\nВисота зображення: {activeChildForm.ImageBuffer.Height}px;" +
                $"\nШирина зображення: {activeChildForm.ImageBuffer.Width}px;" +
                $"\nГоризонтальна роздільна здатність: {activeChildForm.ImageBuffer.HorizontalResolution / inch};" +
                $"\nВертикальну роздільна здатність: {activeChildForm.ImageBuffer.VerticalResolution / inch};" +
                $"\nВисота в см: {activeChildForm.ImageBuffer.Height * dpiX / dpiBase}cm;" +
                $"\nШирина в см: {activeChildForm.ImageBuffer.Width * dpiY / dpiBase}cm;" +
                $"\nВикористаний формат пікселів: {activeChildForm.ImageBuffer.PixelFormat};" +
                $"\nВикористання біта або байта прозорості: {Image.IsAlphaPixelFormat(activeChildForm.ImageBuffer.PixelFormat)};" +
                $"\nЧисло біт на піксель: {Image.GetPixelFormatSize(activeChildForm.ImageBuffer.PixelFormat)}.";
            
            MessageBox.Show(text);
        }
        //3 Point
        private void Task1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Состояние зума переключается
            IsZoomWorking = !IsZoomWorking;

            // Активируем лупу, на новом состоянии
            if (IsZoomWorking is true)
            {
                MessageBox.Show("Режим лупа активовано!");
                // Меняем курсор
                this.Cursor = Cursors.Hand;
            }
            // Выключаем лупу, на новом состоянии
            else { 
                MessageBox.Show("Режим лупа деактивовано!");
                // Меняем курсор
                this.Cursor = Cursors.Default;
            }
        }
        private void Task2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма
            if (ActiveMdiChild is null)
            {
                MessageBox.Show(Const.Messages.ActiveFormIsNull);
                return;
            }

            // Состояние Changer переключается
            IsNumericChangerEnable = !IsNumericChangerEnable;

            // Если Changer активирован 
            if (IsNumericChangerEnable is true)
            {
                MessageBox.Show("Changer змін активовано! Змінюйте кольори!");
                
                // Включаем его на всех формах
                foreach (FormChild item in MdiChildren)
                    item.ChangePoint_NumericUpDown.Enabled = true;
            }
            // Если Changer деактивирован
            else
            {
                MessageBox.Show("Changer змін деактивовано!");

                // Выключаем его на всех формах
                foreach (FormChild item in MdiChildren)
                    item.ChangePoint_NumericUpDown.Enabled = false;
            }
        }
        private void FormGeneral_Load(object sender, EventArgs e){}
    }
}