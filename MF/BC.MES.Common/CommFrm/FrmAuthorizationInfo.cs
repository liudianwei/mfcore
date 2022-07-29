using Common.Helper;
using MES.Common.Properties;
using System;
using System.Drawing;
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
            var res = SystemFramework.AuthorizationManager.Check(MachineCode, label_Product.Text, out string msg, out var info);
            if (res)
            {
                label_starttime.Text = DateTime.ParseExact(info?.StartDateTime, "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm");
                label_starttime.ForeColor = Color.Black;
                label_endtime.Text = DateTime.ParseExact(info?.EndDateTime, "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm");
                label_endtime.ForeColor = Color.Black;
                label_company.Text = info?.Company;
                Icon = System.Drawing.Icon.FromHandle(Resources.已授权.GetHicon());
                button_upload.Visible = false;
                button_close.Location = new System.Drawing.Point(592, 440);
            }
            else
            {
                label_starttime.ForeColor = Color.Red;
                label_endtime.ForeColor = Color.Red;
                label_company.Text = "";
                switch (msg)
                {
                    case "license expired":
                        label_starttime.Text = "授权过期";
                        label_endtime.Text = "授权过期";
                        break;

                    case "unauthorized":
                        label_starttime.Text = "未获取授权文件lic";
                        label_endtime.Text = "未获取授权文件lic";
                        break;

                    default:
                        label_starttime.Text = msg;
                        label_endtime.Text = msg;
                        break;
                }

                Icon = Icon = System.Drawing.Icon.FromHandle(Resources.授权警告.GetHicon());
                button_upload.Visible = true;
                button_upload.Location = new System.Drawing.Point(499, 440);
                button_close.Location = new System.Drawing.Point(592, 440);
            }
        }
    }
}