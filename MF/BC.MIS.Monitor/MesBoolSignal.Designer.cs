namespace MesControl
{
    partial class MesBoolSignal
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
            this.pictureBox_Signal = new System.Windows.Forms.PictureBox();
            this.label_SignalName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Signal)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Signal
            // 
            this.pictureBox_Signal.BackgroundImage = global::Monitor.Properties.Resources.none;
            this.pictureBox_Signal.Location = new System.Drawing.Point(3, 1);
            this.pictureBox_Signal.Name = "pictureBox_Signal";
            this.pictureBox_Signal.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_Signal.TabIndex = 0;
            this.pictureBox_Signal.TabStop = false;
            // 
            // label_SignalName
            // 
            this.label_SignalName.AutoSize = true;
            this.label_SignalName.Font = new System.Drawing.Font("宋体", 12F);
            this.label_SignalName.Location = new System.Drawing.Point(32, 3);
            this.label_SignalName.MaximumSize = new System.Drawing.Size(72, 16);
            this.label_SignalName.MinimumSize = new System.Drawing.Size(72, 16);
            this.label_SignalName.Name = "label_SignalName";
            this.label_SignalName.Size = new System.Drawing.Size(72, 16);
            this.label_SignalName.TabIndex = 1;
            this.label_SignalName.Text = "心跳信号";
            // 
            // MesBoolSignal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label_SignalName);
            this.Controls.Add(this.pictureBox_Signal);
            this.MaximumSize = new System.Drawing.Size(110, 22);
            this.MinimumSize = new System.Drawing.Size(110, 22);
            this.Name = "MesBoolSignal";
            this.Size = new System.Drawing.Size(110, 22);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Signal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Signal;
        private System.Windows.Forms.Label label_SignalName;
    }
}
