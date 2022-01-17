namespace MesControl
{
    partial class PlcConnect
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox_OpName = new System.Windows.Forms.PictureBox();
            this.label_OpName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_OpName)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_OpName
            // 
            this.pictureBox_OpName.BackgroundImage = global::Monitor.Properties.Resources.BreakPLC;
            this.pictureBox_OpName.Location = new System.Drawing.Point(3, 1);
            this.pictureBox_OpName.MaximumSize = new System.Drawing.Size(20, 20);
            this.pictureBox_OpName.MinimumSize = new System.Drawing.Size(20, 20);
            this.pictureBox_OpName.Name = "pictureBox_OpName";
            this.pictureBox_OpName.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_OpName.TabIndex = 0;
            this.pictureBox_OpName.TabStop = false;
            // 
            // label_OpName
            // 
            this.label_OpName.AutoSize = true;
            this.label_OpName.Font = new System.Drawing.Font("宋体", 12F);
            this.label_OpName.Location = new System.Drawing.Point(32, 3);
            this.label_OpName.MaximumSize = new System.Drawing.Size(72, 16);
            this.label_OpName.MinimumSize = new System.Drawing.Size(72, 16);
            this.label_OpName.Name = "label_OpName";
            this.label_OpName.Size = new System.Drawing.Size(72, 16);
            this.label_OpName.TabIndex = 1;
            this.label_OpName.Text = "通讯状态";
            // 
            // PlcConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label_OpName);
            this.Controls.Add(this.pictureBox_OpName);
            this.MaximumSize = new System.Drawing.Size(110, 22);
            this.MinimumSize = new System.Drawing.Size(110, 22);
            this.Name = "PlcConnect";
            this.Size = new System.Drawing.Size(110, 22);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_OpName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_OpName;
        private System.Windows.Forms.Label label_OpName;
    }
}
