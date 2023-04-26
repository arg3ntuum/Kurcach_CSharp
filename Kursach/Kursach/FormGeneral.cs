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
    2. Пункт меню Інформація має виводити докладні відомості про
    зображення, що відкрите в активному дочірньому вікні, а саме – ім’я
    файлу, повний шлях до файлу, формат файлу, розміри в пікселях – висоту
    та ширину, вертикальну та горизонтальну роздільні здатності (в точках на
    сантиметр), фізичні розміри в сантиметрах, використаний формат
    пікселів, використання біта або байта прозорості, число біт на піксель.
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
        public const string FilesFilter = 
            "Image files(*.bmp;*.jpg;*.png;*.gif;*.tiff)|*.bmp;*.jpg;*.png;*.gif;*.tiff";

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
            Text = "Kursach";
            // Форма является контейнером для дочерних MDI-форм
            IsMdiContainer = true;

            // Встановити назву для image файлу
            OpenFileDialogWindow.FileName = "image.png";
            // Встановлюємо фільтр для файлів
            OpenFileDialogWindow.Filter = FilesFilter;
            
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
            ImagePath = OpenFileDialogWindow.FileName;

            try
            {
                using (var bmp = (Bitmap)Image.FromFile(ImagePath))
                {
                    ImageBuffer = new Bitmap(bmp);
                }
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
                    MessageBox.Show("Вы хотите создать форму для отображения картинки?", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButtons.YesNo);
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
            if (this.ActiveMdiChild is null)
            {
                MessageBox.Show("ActiveMdiChild == null!");
                return;
            }
            if (MessageBox.Show("Вы хотите сохранить картинку в том же файле?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    FormChild activeChildForm = (FormChild)this.ActiveMdiChild;

                    if (activeChildForm.ImageBuffer is null)
                        throw new ArgumentException("Нечего сохранять, изображение не загружено");
                    if (string.IsNullOrWhiteSpace(activeChildForm.ImagePath))
                        throw new ArgumentException("Не задан путь сохранения");
                    
                    File.Delete(activeChildForm.ImagePath);
                    activeChildForm.ImageBuffer.Save(activeChildForm.ImagePath, activeChildForm.ImageFormat);
                    activeChildForm.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name);
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
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




