using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KellScrollList
{
    public partial class InfoControl : UserControl
    {
        public InfoControl()
        {
            InitializeComponent();
        }

        bool isInternal;
        string address;
        Delegate delgt;
        object target;
        object[] args;

        public Color StatusColor
        {
            get
            {
                return panel2.BackColor;
            }
            set
            {
                panel2.BackColor = value;
            }
        }

        public string Info
        {
            get
            {
                return label1.Text;
            }
            set
            {
                if (value != null)
                    label1.Text = value;
                else
                    label1.Text = "";
                toolTip1.SetToolTip(label1, label1.Text);
            }
        }

        public void SetLink(string address, bool isInternal = false, Delegate delgt = null, object target = null, object[] args = null)
        {
            this.address = address;
            this.isInternal = isInternal;
            this.delgt = delgt;
            this.target = target;
            this.args = args;
        }

        private void InfoControl_Resize(object sender, EventArgs e)
        {
            this.Height = 21;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(address))
            {
                if (isInternal)
                {
                    try
                    {
                        delgt.Method.Invoke(target, args);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("打开内部窗体出错：" + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        Process.Start(address);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("打开外部程序出错：" + ex.Message);
                    }
                }
            }
            this.OnClick(e);
        }

        public override string ToString()
        {
            return Info;
        }
    }
}
