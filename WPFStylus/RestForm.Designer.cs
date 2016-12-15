namespace WPFStylus
{
    partial class RestForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ready_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(39, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(620, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please rest for at least 5 seconds";
            // 
            // ready_btn
            // 
            this.ready_btn.Enabled = false;
            this.ready_btn.Location = new System.Drawing.Point(238, 125);
            this.ready_btn.Name = "ready_btn";
            this.ready_btn.Size = new System.Drawing.Size(222, 65);
            this.ready_btn.TabIndex = 1;
            this.ready_btn.Text = "I am ready";
            this.ready_btn.UseVisualStyleBackColor = true;
            this.ready_btn.Click += new System.EventHandler(this.ready_btn_Click);
            // 
            // RestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 236);
            this.ControlBox = false;
            this.Controls.Add(this.ready_btn);
            this.Controls.Add(this.label1);
            this.Name = "RestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ready_btn;
    }
}