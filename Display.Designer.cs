namespace Project
{
    partial class Display
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
            this.CurrentCardText = new System.Windows.Forms.Label();
            this.currentCardImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.currentCardImage)).BeginInit();
            this.SuspendLayout();
            // 
            // CurrentCardText
            // 
            this.CurrentCardText.AutoSize = true;
            this.CurrentCardText.Location = new System.Drawing.Point(12, 9);
            this.CurrentCardText.Name = "CurrentCardText";
            this.CurrentCardText.Size = new System.Drawing.Size(13, 13);
            this.CurrentCardText.TabIndex = 0;
            this.CurrentCardText.Text = "0";
            // 
            // currentCardImage
            // 
            this.currentCardImage.Location = new System.Drawing.Point(15, 26);
            this.currentCardImage.Name = "currentCardImage";
            this.currentCardImage.Size = new System.Drawing.Size(281, 343);
            this.currentCardImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.currentCardImage.TabIndex = 1;
            this.currentCardImage.TabStop = false;
            this.currentCardImage.Click += new System.EventHandler(this.currentCardImage_Click);
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 381);
            this.Controls.Add(this.currentCardImage);
            this.Controls.Add(this.CurrentCardText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Display";
            this.Text = "Display";
            this.Load += new System.EventHandler(this.Display_Load);
            ((System.ComponentModel.ISupportInitialize)(this.currentCardImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label CurrentCardText;
        private System.Windows.Forms.PictureBox currentCardImage;
    }
}