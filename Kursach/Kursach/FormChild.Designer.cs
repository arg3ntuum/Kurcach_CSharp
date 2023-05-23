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
            this.Merged_CheckBox = new System.Windows.Forms.CheckBox();
            this.AddImage_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePoint_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ChangePoint_NumericUpDown
            // 
            this.ChangePoint_NumericUpDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChangePoint_NumericUpDown.Location = new System.Drawing.Point(0, 0);
            this.ChangePoint_NumericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.ChangePoint_NumericUpDown.Size = new System.Drawing.Size(99, 22);
            this.ChangePoint_NumericUpDown.TabIndex = 0;
            this.ChangePoint_NumericUpDown.ValueChanged += new System.EventHandler(this.ChangePoint_NumericUpDown_ValueChanged);
            // 
            // Merged_CheckBox
            // 
            this.Merged_CheckBox.AutoSize = true;
            this.Merged_CheckBox.Location = new System.Drawing.Point(204, 4);
            this.Merged_CheckBox.Name = "Merged_CheckBox";
            this.Merged_CheckBox.Size = new System.Drawing.Size(132, 20);
            this.Merged_CheckBox.TabIndex = 1;
            this.Merged_CheckBox.Text = "ChooseToMerge";
            this.Merged_CheckBox.UseVisualStyleBackColor = true;
            this.Merged_CheckBox.CheckedChanged += new System.EventHandler(this.Merged_CheckBox_CheckedChanged);
            // 
            // AddImage_Button
            // 
            this.AddImage_Button.Location = new System.Drawing.Point(105, 0);
            this.AddImage_Button.Name = "AddImage_Button";
            this.AddImage_Button.Size = new System.Drawing.Size(93, 27);
            this.AddImage_Button.TabIndex = 2;
            this.AddImage_Button.Text = "AddImage";
            this.AddImage_Button.UseVisualStyleBackColor = true;
            this.AddImage_Button.Click += new System.EventHandler(this.AddImage_Button_Click);
            // 
            // FormChild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 379);
            this.Controls.Add(this.AddImage_Button);
            this.Controls.Add(this.Merged_CheckBox);
            this.Controls.Add(this.ChangePoint_NumericUpDown);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormChild";
            this.Text = "FormChild";
            this.Load += new System.EventHandler(this.FormChild_Load);
            this.Click += new System.EventHandler(this.FormChild_Click);
            ((System.ComponentModel.ISupportInitialize)(this.ChangePoint_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown ChangePoint_NumericUpDown;
        private System.Windows.Forms.Button AddImage_Button;
        public System.Windows.Forms.CheckBox Merged_CheckBox;
    }
}