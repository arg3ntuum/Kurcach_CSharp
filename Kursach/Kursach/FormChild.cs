using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Kursach
{
    public partial class FormChild : Form
    {
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

        private void FormChild_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void FormChild_Paint(object sender, PaintEventArgs e)
        {
            if (_parent.ImageBuffer == null)
                return;

            _grfx = e.Graphics;

            DrawImageToForm();
        }
        public void DrawImageToForm() {
            Rectangle rect = ClientRectangle;

            _grfx.DrawImage(_parent.ImageBuffer, 0, 0, rect.Width, rect.Height);

            _grfx.Dispose();
        }
        private void FormChild_Load(object sender, EventArgs e)
        {
        }
    }
}
