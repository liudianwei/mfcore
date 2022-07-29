namespace Common.Frm
{
    partial class FrmAuthorizationInfo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAuthorizationInfo));
            this.label1 = new System.Windows.Forms.Label();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_upload = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.button_copy = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label10 = new System.Windows.Forms.Label();
            this.label_Product = new System.Windows.Forms.Label();
            this.label_starttime = new System.Windows.Forms.Label();
            this.label_endtime = new System.Windows.Forms.Label();
            this.label_company = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(31, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "机 器 码";
            // 
            // txt_code
            // 
            this.txt_code.Location = new System.Drawing.Point(100, 320);
            this.txt_code.Multiline = true;
            this.txt_code.Name = "txt_code";
            this.txt_code.ReadOnly = true;
            this.txt_code.Size = new System.Drawing.Size(248, 58);
            this.txt_code.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txt_code, "这里应该填的是发布端的IP");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(31, 410);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "授权时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(31, 434);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "到期时间";
            // 
            // button_upload
            // 
            this.button_upload.Location = new System.Drawing.Point(499, 440);
            this.button_upload.Name = "button_upload";
            this.button_upload.Size = new System.Drawing.Size(87, 29);
            this.button_upload.TabIndex = 12;
            this.button_upload.Text = "上传授权(&A)";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.Button_upload_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(592, 440);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(87, 29);
            this.button_close.TabIndex = 13;
            this.button_close.Text = "关闭(&E)";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.Button_close_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(31, 458);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "授权主体";
            // 
            // button_copy
            // 
            this.button_copy.Font = new System.Drawing.Font("宋体", 7F);
            this.button_copy.Location = new System.Drawing.Point(354, 320);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(55, 23);
            this.button_copy.TabIndex = 19;
            this.button_copy.Text = "复制(&C)";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(31, 386);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 20;
            this.label10.Text = "产品信息";
            // 
            // label_Product
            // 
            this.label_Product.AutoSize = true;
            this.label_Product.BackColor = System.Drawing.Color.Transparent;
            this.label_Product.Location = new System.Drawing.Point(100, 386);
            this.label_Product.Name = "label_Product";
            this.label_Product.Size = new System.Drawing.Size(65, 12);
            this.label_Product.TabIndex = 23;
            this.label_Product.Text = "ames-print";
            // 
            // label_starttime
            // 
            this.label_starttime.AutoSize = true;
            this.label_starttime.BackColor = System.Drawing.Color.Transparent;
            this.label_starttime.Location = new System.Drawing.Point(100, 410);
            this.label_starttime.Name = "label_starttime";
            this.label_starttime.Size = new System.Drawing.Size(101, 12);
            this.label_starttime.TabIndex = 24;
            this.label_starttime.Text = "2022-05-23 18:00";
            // 
            // label_endtime
            // 
            this.label_endtime.AutoSize = true;
            this.label_endtime.BackColor = System.Drawing.Color.Transparent;
            this.label_endtime.Location = new System.Drawing.Point(100, 433);
            this.label_endtime.Name = "label_endtime";
            this.label_endtime.Size = new System.Drawing.Size(101, 12);
            this.label_endtime.TabIndex = 25;
            this.label_endtime.Text = "2022-05-23 18:00";
            // 
            // label_company
            // 
            this.label_company.AutoSize = true;
            this.label_company.BackColor = System.Drawing.Color.Transparent;
            this.label_company.Location = new System.Drawing.Point(100, 458);
            this.label_company.Name = "label_company";
            this.label_company.Size = new System.Drawing.Size(149, 12);
            this.label_company.TabIndex = 26;
            this.label_company.Text = "苏州宏软信息技术有限公司";
            // 
            // FrmAuthorizationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MES.Common.Properties.Resources.背景图;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(702, 485);
            this.Controls.Add(this.label_company);
            this.Controls.Add(this.label_endtime);
            this.Controls.Add(this.label_starttime);
            this.Controls.Add(this.label_Product);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_upload);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_code);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAuthorizationInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "授权信息";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Button button_upload;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label_Product;
        private System.Windows.Forms.Label label_starttime;
        private System.Windows.Forms.Label label_endtime;
        private System.Windows.Forms.Label label_company;
    }
}