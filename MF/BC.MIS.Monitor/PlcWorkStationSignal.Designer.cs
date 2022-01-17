namespace MesControl
{
    partial class PlcWorkStationSignal
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
            this.groupBox_WorkStationGrp = new System.Windows.Forms.GroupBox();
            this.plcConnect_WorkStation = new MesControl.PlcConnect();
            this.groupBox_WorkStationGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_WorkStationGrp
            // 
            this.groupBox_WorkStationGrp.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_WorkStationGrp.Controls.Add(this.plcConnect_WorkStation);
            this.groupBox_WorkStationGrp.Location = new System.Drawing.Point(4, 4);
            this.groupBox_WorkStationGrp.Name = "groupBox_WorkStationGrp";
            this.groupBox_WorkStationGrp.Size = new System.Drawing.Size(125, 50);
            this.groupBox_WorkStationGrp.TabIndex = 0;
            this.groupBox_WorkStationGrp.TabStop = false;
            this.groupBox_WorkStationGrp.Text = "OP0000";
            // 
            // plcConnect_WorkStation
            // 
            this.plcConnect_WorkStation.BackColor = System.Drawing.Color.Transparent;
            this.plcConnect_WorkStation.ExternalTimerInterval = 200;
            this.plcConnect_WorkStation.IsBreakPLC = false;
            this.plcConnect_WorkStation.LabelText = "通讯状态";
            this.plcConnect_WorkStation.Location = new System.Drawing.Point(7, 21);
            this.plcConnect_WorkStation.MaximumSize = new System.Drawing.Size(110, 22);
            this.plcConnect_WorkStation.MinimumSize = new System.Drawing.Size(110, 22);
            this.plcConnect_WorkStation.Name = "plcConnect_WorkStation";
            this.plcConnect_WorkStation.PlcTestEnable = false;
            this.plcConnect_WorkStation.Size = new System.Drawing.Size(110, 22);
            this.plcConnect_WorkStation.TabIndex = 0;
            this.plcConnect_WorkStation.TestPlcBreakInterval = 0;
            // 
            // PlcWorkStationSignal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox_WorkStationGrp);
            this.Name = "PlcWorkStationSignal";
            this.Size = new System.Drawing.Size(136, 60);
            this.groupBox_WorkStationGrp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_WorkStationGrp;
        private PlcConnect plcConnect_WorkStation;
    }
}
