namespace LR2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChangeShapeBtn = new System.Windows.Forms.Button();
            this.AddPhotoBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Gallery = new LR2.HoverButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChangeShapeBtn
            // 
            this.ChangeShapeBtn.BackColor = System.Drawing.SystemColors.Control;
            this.ChangeShapeBtn.Font = new System.Drawing.Font("Arial", 12F);
            this.ChangeShapeBtn.Location = new System.Drawing.Point(0, 0);
            this.ChangeShapeBtn.Name = "ChangeShapeBtn";
            this.ChangeShapeBtn.Size = new System.Drawing.Size(147, 78);
            this.ChangeShapeBtn.TabIndex = 3;
            this.ChangeShapeBtn.Text = "Сменить форму";
            this.ChangeShapeBtn.UseVisualStyleBackColor = false;
            this.ChangeShapeBtn.Click += new System.EventHandler(this.ChangeShapeBtn_Click);
            // 
            // AddPhotoBtn
            // 
            this.AddPhotoBtn.Font = new System.Drawing.Font("Arial", 12F);
            this.AddPhotoBtn.Location = new System.Drawing.Point(0, 100);
            this.AddPhotoBtn.Name = "AddPhotoBtn";
            this.AddPhotoBtn.Size = new System.Drawing.Size(147, 78);
            this.AddPhotoBtn.TabIndex = 4;
            this.AddPhotoBtn.Text = "Добавить фото";
            this.AddPhotoBtn.UseVisualStyleBackColor = true;
            this.AddPhotoBtn.Click += new System.EventHandler(this.ButtonBrowse_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ChangeShapeBtn);
            this.panel1.Controls.Add(this.AddPhotoBtn);
            this.panel1.Location = new System.Drawing.Point(608, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 178);
            this.panel1.TabIndex = 6;
            // 
            // Gallery
            // 
            this.Gallery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Gallery.Location = new System.Drawing.Point(28, 50);
            this.Gallery.Name = "Gallery";
            this.Gallery.Size = new System.Drawing.Size(547, 365);
            this.Gallery.TabIndex = 7;
            this.Gallery.UseVisualStyleBackColor = true;
            this.Gallery.Click += new System.EventHandler(this.Gallery_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Gallery);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Галерея";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ChangeShapeBtn;
        private System.Windows.Forms.Button AddPhotoBtn;
        private System.Windows.Forms.Panel panel1;
        private HoverButton Gallery;
    }
}

