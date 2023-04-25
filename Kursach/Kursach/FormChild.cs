using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursach
{
    public partial class FormChild : Form
    {
        public Image ImageBuffer { get; private set; }
        public string ImagePath { get; private set; }
        
        private FormGeneral _parent { get; set; }
        private Graphics _grfx { get; set; }
        private Rectangle _rect{ get; set; }
        
        public FormChild(FormGeneral parent, string caption)
        {
            InitializeComponent();

            // Присваиваем свойствам значения
            _parent = parent;
            _rect = ClientRectangle;
            _grfx = null;

            //подписываем события
            this.Paint += FormChild_Paint;
            this.Resize += FormChild_Resize;

            // Присваивание контейнеру родителя данной формы
            this.MdiParent = parent;

            //Задание заголовка
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
        public void UploadImageToBuffer() { 
            if (_parent.ImageBuffer == null || _parent.ImageName == string.Empty)
                return;

            ImageBuffer = _parent.ImageBuffer;
            ImagePath = _parent.ImageName;
        }

        private void FormChild_Resize(object sender, EventArgs e) =>
            Invalidate();//перерисовать

        private void FormChild_Load(object sender, EventArgs e) {  }
    }
}
