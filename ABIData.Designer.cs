
namespace APIData
{
    partial class ABIData
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labFilePath = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnFilePath = new System.Windows.Forms.Button();
            this.btnParse = new System.Windows.Forms.Button();
            this.panGen1 = new System.Windows.Forms.Panel();
            this.panGen2 = new System.Windows.Forms.Panel();
            this.panGen3 = new System.Windows.Forms.Panel();
            this.panGen4 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // labFilePath
            // 
            this.labFilePath.AutoSize = true;
            this.labFilePath.Location = new System.Drawing.Point(15, 18);
            this.labFilePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labFilePath.Name = "labFilePath";
            this.labFilePath.Size = new System.Drawing.Size(77, 20);
            this.labFilePath.TabIndex = 0;
            this.labFilePath.Text = "文件路径: ";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(104, 14);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(1318, 27);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnFilePath
            // 
            this.btnFilePath.Location = new System.Drawing.Point(1431, 14);
            this.btnFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(32, 27);
            this.btnFilePath.TabIndex = 2;
            this.btnFilePath.Text = "...";
            this.btnFilePath.UseVisualStyleBackColor = true;
            this.btnFilePath.Click += new System.EventHandler(this.btnFilePath_Click);
            // 
            // btnParse
            // 
            this.btnParse.Enabled = false;
            this.btnParse.Location = new System.Drawing.Point(15, 48);
            this.btnParse.Margin = new System.Windows.Forms.Padding(4);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(96, 27);
            this.btnParse.TabIndex = 3;
            this.btnParse.Text = "解析";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // panGen1
            // 
            this.panGen1.Location = new System.Drawing.Point(15, 82);
            this.panGen1.Margin = new System.Windows.Forms.Padding(4);
            this.panGen1.Name = "panGen1";
            this.panGen1.Size = new System.Drawing.Size(1441, 125);
            this.panGen1.TabIndex = 4;
            // 
            // panGen2
            // 
            this.panGen2.Location = new System.Drawing.Point(15, 215);
            this.panGen2.Margin = new System.Windows.Forms.Padding(4);
            this.panGen2.Name = "panGen2";
            this.panGen2.Size = new System.Drawing.Size(1441, 125);
            this.panGen2.TabIndex = 5;
            // 
            // panGen3
            // 
            this.panGen3.Location = new System.Drawing.Point(15, 348);
            this.panGen3.Margin = new System.Windows.Forms.Padding(4);
            this.panGen3.Name = "panGen3";
            this.panGen3.Size = new System.Drawing.Size(1441, 125);
            this.panGen3.TabIndex = 6;
            // 
            // panGen4
            // 
            this.panGen4.Location = new System.Drawing.Point(15, 481);
            this.panGen4.Margin = new System.Windows.Forms.Padding(4);
            this.panGen4.Name = "panGen4";
            this.panGen4.Size = new System.Drawing.Size(1441, 125);
            this.panGen4.TabIndex = 7;
            // 
            // ABIData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1472, 731);
            this.Controls.Add(this.panGen4);
            this.Controls.Add(this.panGen3);
            this.Controls.Add(this.panGen2);
            this.Controls.Add(this.panGen1);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.btnFilePath);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.labFilePath);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ABIData";
            this.Text = "ABIData";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labFilePath;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnFilePath;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.Panel panGen1;
        private System.Windows.Forms.Panel panGen2;
        private System.Windows.Forms.Panel panGen3;
        private System.Windows.Forms.Panel panGen4;
    }
}

