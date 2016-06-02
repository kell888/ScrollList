using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        KellScrollList.InfoControl ic;

        private void infoListControl1_ElementEvent(object sender, KellScrollList.ElementArgs e)
        {
            ic = e.Element;
            label1.Text = e.Action + ": " + (e.Element != null ? e.Element.Info : "") + (e.OldInfo != null ? "[OldInfo=" + e.OldInfo + "]" : "");
            if (e.Action == KellScrollList.Action.Click)
            {
                textBox1.Text = e.Element.Info;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ic != null)
                infoListControl1.RemoveInfo(ic);
            else
                MessageBox.Show("先指定要删除的信息！");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            infoListControl1.ClearInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ic != null)
                infoListControl1.ModifyInfo(ic, textBox1.Text);
            else
                MessageBox.Show("先指定要修改的信息！");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            infoListControl1.AddInfo(textBox1.Text, new KellScrollList.LinkObject(textBox2.Text.Trim(), checkBox1.Checked, new Action<string>(Test), this, new object[] { textBox2.Text }), panel1.BackColor);
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = panel1.BackColor;
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    panel1.BackColor = cd.Color;
                }
            }
        }

        public void Test(string msg)
        {
            MessageBox.Show(msg);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            infoListControl1.NewInDown = checkBox2.Checked;
        }
    }
}
