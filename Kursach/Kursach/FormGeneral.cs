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
    /*
    Задачи:
    
    +1) Створити – створення нового дочірнього вікна.
    +2) Відкрити – відкриття файлів із зображеннями 
            ( формат файлів *.bmp, *.jpg, *.png, *.gif, *.tiff).
    +3) Зберегти – збереження зображення в тому самому форматі.
    +4) Зберегти як… - збереження зображення в одному з наступних форматів 
            *.bmp, *.jpg, *.png, *.gif, *.tiff
    +5) Закрити – закриття дочірнього вікна. Перед закриттям дочірнього вікна 
            потрібно вивести запит про необхідність збереження змін у файл, якщо вони відбувались.
    +6) Вихід – закриття всього додатку. Перед закриттям додатку необхідно вивести запит 
            про збереження змін в усіх відкритих файлах, в яких вони відбувалися.
    +-2. Пункт меню Інформація має виводити докладні відомості про
    зображення, що відкрите в активному дочірньому вікні, а саме – 
    ім’я файлу, 
    повний шлях до файлу, 
    формат файлу, 
    розміри в пікселях – висоту та ширину, 
    ?вертикальну та горизонтальну роздільні здатності (в точках на сантиметр), 
    ?фізичні розміри в сантиметрах, 
    використаний формат пікселів,
    ?використання біта або байта прозорості,(просто по самому крайнему узнаю, что с ним да как)
    число біт на піксель.
    3. Пункт меню Завдання містить 2 підпункти по кількості
        індивідуальних завдань, при виборі яких запускається обробка. 
    Варіант 10. Додайте в програму інструмент лупа для збільшення і
        зменшення зображення. Лупа збільшує зображення при натисканні лівою
        кнопкою миші, лупа зменшує зображення при натисканні правою кнопкою
        миші. При виборі цього інструменту через меню або панель інструментів
        повинен змінюватися курсор миші при знаходженні над клієнтської областю
        вікна. Кнопка на панелі інструментів і пункт меню повинні бути позначені
        при виборі відповідного інструменту.
    Варіант 20. Підсумовування двох зображень або константи і
        зображення. Функція imadd (X, Y, Z) підсумовує кожен елемент масиву X з
        відповідним елементом масиву Y і повертає суму відповідних елементів в
        результуючий масив Z. X і Y представляють собою масиви чисел із
        плаваючою комою однакового розміру і однакового формату представлення
        даних. Результуючий масив Z має той же розмір і формат представлення
        даних, що і Y, коли Y скаляр формату double. В іншому випадку розмірність і
        формат представлення даних результуючого масиву Z збігається з масивом
        X. Коли X і Y є масиви цілих чисел і елементи результуючого масиву
        перевищують допустимий діапазон, то вони скорочуються або
        округлюються. Продемонструйте в програмі застосування цієї функції.


    */
    public partial class FormGeneral : Form
    {
        // Буфер для картинки, через який передається на форму картинка
        public Bitmap ImageBuffer { get; private set; }
        public string ImagePath { get; private set; }

        private int _nextFormNumber { get; set; }

        public FormGeneral()
        {
            InitializeComponent();

            // Присваиваем свойствам значения
            ImageBuffer = null;
            ImagePath = string.Empty;
            _nextFormNumber = 1;

            // Присваиваем название для формы
            Text = Const.ProgramName;
            // Форма является контейнером для дочерних MDI-форм
            IsMdiContainer = true;

            // Встановити назву для image файлу
            OpenFileDialogWindow.FileName = Const.ImageFileName;
            // Встановлюємо фільтр для файлів
            OpenFileDialogWindow.Filter = Const.FilesFilter;
            
            //ResizeRedraw = true;
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
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //если результат равен Cancel, то выходим
            if (OpenFileDialogWindow.ShowDialog() == DialogResult.Cancel)
                return;

            // получаем выбранный файл
            ImagePath = OpenFileDialogWindow.FileName;

            try
            {
                //пытаемся открыть файл
                using (var bmp = (Bitmap)Image.FromFile(ImagePath))
                    ImageBuffer = new Bitmap(bmp);
            }
            catch
            {
                MessageBox.Show("Cannot find file " + ImagePath +
               "!", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            // Определение активного дочернего MDI-окна
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            if (activeChildForm == null)
            {
                DialogResult dialogResult =
                    MessageBox.Show("Вы хотите создать форму для отображения картинки?", Const.Messages.Attention, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    CreateToolStripMenuItem_Click(null, null);
            }
            else {
                activeChildForm.UploadImageToBuffer();
                activeChildForm.Invalidate();
            }
            ImageBuffer = null;
            ImagePath = string.Empty;
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма
            if (this.ActiveMdiChild is null)
            {
                MessageBox.Show(Const.Messages.IsNull("ActiveMdiChild"));
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
            if (this.ActiveMdiChild is null)
            {
                MessageBox.Show(Const.Messages.IsNull("ActiveMdiChild"));
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
            if (this.ActiveMdiChild is null)
            {
                MessageBox.Show(Const.Messages.IsNull("ActiveMdiChild"));
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
            if (this.ActiveMdiChild is null)
                Application.Exit();
            
            foreach (FormChild item in MdiChildren)
            {
                item.Select();
                GetAnswerToSaveAndSave(item);
            }

            //Закрываем приложение
            Application.Exit();
        }
        private void GetAnswerToSaveAndSave(FormChild item) {
            // Если файл был изменен, сохраняем значения
            if (item.IsChanged)
            {
                // Проверяем, нажал ли пользователь ОК, и тогда сохраняем
                DialogResult dialogResult =
                    MessageBox.Show(Const.Messages.ImageInThisFile, Const.Messages.Attention, MessageBoxButtons.OKCancel);

                // Проверяем, нажал ли пользователь ОК, и тогда сохраняем
                if (dialogResult == DialogResult.OK)
                    Save(item, item.ImagePath);
            }
            else
                MessageBox.Show(Const.Messages.FileNotWasChanged); 
        }
        private void Save(FormChild activeChildForm, string path)
        {
            try {
                if (activeChildForm.ImageBuffer is null)
                    throw new ArgumentException("Нечего сохранять, изображение не загружено");
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentException("Не задан путь сохранения");

                File.Delete(path);

                activeChildForm.ImageBuffer.Save(path, activeChildForm.ImageFormat);
                activeChildForm.Close();
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }
        //2 Point
        private void InformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли активная форма
            if (this.ActiveMdiChild is null) {
                MessageBox.Show("Активной формы не существует!");
                return;
            }

            // Получаем активную форму 
            FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

            if(activeChildForm.ImageBuffer is null) {
                MessageBox.Show("Картинки на форме не существует!");
                return;
            }

            double inch = 2.54;

            string text =
                $"\nFile name: {Path.GetFileName(activeChildForm.ImagePath)};" +
                $"\nFile path: {activeChildForm.ImagePath};" +
                $"\nFile format: {activeChildForm.ImageFormat};" +
                $"\nВысота изображения: {activeChildForm.ImageBuffer.Height}px;" +
                $"\nШирина изображения: {activeChildForm.ImageBuffer.Width}px;" +
                $"\nГоризонтальна роздільна здатність: {activeChildForm.ImageBuffer.HorizontalResolution / inch};" +
                $"\nВертикальну роздільна здатність: {activeChildForm.ImageBuffer.VerticalResolution / inch};" +
                $"\nВисота в см: {activeChildForm.ImageBuffer.HorizontalResolution}cm;" +
                $"\nШирина в см: {activeChildForm.ImageBuffer.Width / inch}cm;" +
                $"\nВикористаний формат пікселів: {activeChildForm.ImageBuffer.PixelFormat};" +
                $"\nВикористання біта або байта прозорості: {Image.IsAlphaPixelFormat(activeChildForm.ImageBuffer.PixelFormat)};" +
                $"\nЧисло біт на піксель: {Image.GetPixelFormatSize(activeChildForm.ImageBuffer.PixelFormat)}.";
            
            MessageBox.Show(text);
        }
        private void FormGeneral_Load(object sender, EventArgs e){}
    }
}




