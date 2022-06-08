using Common.Helper;
using MES.Common.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Common.Frm
{
    public partial class FrmAuthorizationInfo : Form
    {
        public FrmAuthorizationInfo(string MachineCode, string Product)
        {
            InitializeComponent();
            label_Product.Text = Product;
            txt_code.Text = MachineCode;
            ReadAuthorization(MachineCode);
        }

        private void Button_upload_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "license";
            openFileDialog.Title = "选择授权文件";
            openFileDialog.Filter = "授权文件|*.lic";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string filepath = openFileDialog.FileName.Trim();
            File.Copy(filepath, $"{Application.StartupPath}\\license.lic", true);
            ReadAuthorization(txt_code.Text);
        }

        private void Button_close_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label_Product.Text + "," + txt_code.Text);
        }

        /// <summary>
        /// 读取授权
        /// </summary>
        /// <param name="MachineCode"></param>
        public void ReadAuthorization(string MachineCode)
        {
            var res = SystemFramework.AuthorizationManager.Check(MachineCode, label_Product.Text);
            if (res.Item1)
            {
                label_starttime.Text = DateTime.ParseExact(res.Item3.StartDateTime, "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm");
                label_endtime.Text = DateTime.ParseExact(res.Item3.EndDateTime, "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm");
                label_company.Text = res.Item3.Company;
                Icon = System.Drawing.Icon.FromHandle(Resources.已授权.GetHicon());
                button_upload.Visible = false;
                button_close.Location = new System.Drawing.Point(98, 266);
            }
            else
            {
                label_starttime.Text = "未授权";
                label_endtime.Text = "未授权";
                label_company.Text = "未授权";
                Icon = Icon = System.Drawing.Icon.FromHandle(Resources.授权警告.GetHicon());
                button_upload.Visible = true;
                button_upload.Location = new System.Drawing.Point(29, 266);
                button_close.Location = new System.Drawing.Point(170, 266);
            }
        }
    }
}