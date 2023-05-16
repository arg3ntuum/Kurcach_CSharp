namespace Kursach
{
    partial class FormChild
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChangePoint_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePoint_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ChangePoint_NumericUpDown
            // 
            this.ChangePoint_NumericUpDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChangePoint_NumericUpDown.Location = new System.Drawing.Point(0, 0);
            this.ChangePoint_NumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ChangePoint_NumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ChangePoint_NumericUpDown.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.ChangePoint_NumericUpDown.Name = "ChangePoint_NumericUpDown";
            this.ChangePoint_NumericUpDown.Size = new System.Drawing.Size(74, 20);
            this.ChangePoint_NumericUpDown.TabIndex = 0;
            this.ChangePoint_NumericUpDown.ValueChanged += new System.EventHandler(this.ChangePoint_NumericUpDown_ValueChanged);
            // 
            // FormChild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 308);
            this.Controls.Add(this.ChangePoint_NumericUpDown);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormChild";
            this.Text = "FormChild";
            this.Load += new System.EventHandler(this.FormChild_Load);
            this.Click += new System.EventHandler(this.FormChild_Click);
            ((System.ComponentModel.ISupportInitialize)(this.ChangePoint_NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.NumericUpDown ChangePoint_NumericUpDown;
    }
}