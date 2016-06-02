using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KellScrollList
{
    [DefaultEvent("ElementEvent")]
    public partial class InfoListControl : UserControl
    {
        private static readonly object syncObj = new object();
        public InfoListControl()
        {
            InitializeComponent();
        }

        bool newInDown;
        [Description("新元素出现方向")]
        [DefaultValue(false)]
        public bool NewInDown
        {
            get
            {
                return newInDown;
            }
            set
            {
                newInDown = value;
            }
        }
        [Description("元素事件")]
        public event EventHandler<ElementArgs> ElementEvent;

        private void OnElementEvent(ElementArgs e)
        {
            if (ElementEvent != null)
                ElementEvent(this, e);
        }

        public void AddInfo(string info, LinkObject link, int maxCount = 5)
        {
            lock (syncObj)
            {
                int count = panel1.Controls.Count;
                InfoControl ic = new InfoControl();
                ic.Info = info;
                ic.SetLink(link.Address, link.IsInternal, link.Delegate, link.Target, link.Args);
                if (NewInDown)
                {
                    ic.Dock = DockStyle.Bottom;
                }
                else
                {
                    ic.Dock = DockStyle.Top;
                }
                ic.Click += new EventHandler(ic_Click);
                if (count == maxCount)
                    panel1.Controls.RemoveAt(0);
                panel1.Controls.Add(ic);
                panel1.ScrollControlIntoView(ic);
                OnElementEvent(new ElementArgs(ic, Action.Add));
            }
        }

        public void AddInfo(string info, LinkObject link, Color statusColor, int maxCount = 5)
        {
            lock (syncObj)
            {
                int count = panel1.Controls.Count;
                InfoControl ic = new InfoControl();
                ic.Info = info;
                ic.SetLink(link.Address, link.IsInternal, link.Delegate, link.Target, link.Args);
                ic.StatusColor = statusColor;
                if (NewInDown)
                {
                    ic.Dock = DockStyle.Bottom;
                }
                else
                {
                    ic.Dock = DockStyle.Top;
                }
                ic.Click += new EventHandler(ic_Click);
                if (count == maxCount)
                    panel1.Controls.RemoveAt(0);
                panel1.Controls.Add(ic);
                panel1.ScrollControlIntoView(ic);
                OnElementEvent(new ElementArgs(ic, Action.Add));
            }
        }

        void ic_Click(object sender, EventArgs e)
        {
            InfoControl ic = sender as InfoControl;
            OnElementEvent(new ElementArgs(ic, Action.Click));
        }

        public void ModifyInfo(InfoControl ic, string info)
        {
            string oldInfo = ic.Info;
            ic.Info = info;
            OnElementEvent(new ElementArgs(ic, Action.Modify, oldInfo));
        }

        public void ModifyInfo(InfoControl ic, string info, Color statusColor)
        {
            string oldInfo = ic.Info;
            ic.Info = info;
            ic.StatusColor = statusColor;
            OnElementEvent(new ElementArgs(ic, Action.Modify, oldInfo));
        }

        public void RemoveInfo(int index)
        {
            InfoControl ic = panel1.Controls[index] as InfoControl;
            panel1.Controls.RemoveAt(index);
            OnElementEvent(new ElementArgs(ic, Action.Remove));
        }

        public void RemoveInfo(InfoControl ic)
        {
            panel1.Controls.Remove(ic);
            OnElementEvent(new ElementArgs(ic, Action.Remove));
        }

        public void ClearInfo()
        {
            panel1.Controls.Clear();
            OnElementEvent(new ElementArgs(null, Action.Clear));
        }
    }

    [DefaultValue(0)]
    public enum Action : int
    {
        None = 0,
        Add = 1,
        Remove = -1,
        Modify = 2,
        Clear = 3,
        Click = 4
    }

    public class ElementArgs : EventArgs
    {
        InfoControl element;

        public InfoControl Element
        {
            get { return element; }
        }
        Action action;

        public Action Action
        {
            get { return action; }
        }
        string oldInfo;

        public string OldInfo
        {
            get { return oldInfo; }
        }

        public ElementArgs(InfoControl element, Action action = KellScrollList.Action.None, string oldInfo = null)
        {
            this.element = element;
            this.action = action;
            this.oldInfo = oldInfo;
        }
    }

    [Serializable]
    public class LinkObject
    {
        bool isInternal;

        public bool IsInternal
        {
            get { return isInternal; }
            set { isInternal = value; }
        }
        string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        Delegate delgt;

        public Delegate Delegate
        {
            get { return delgt; }
            set { delgt = value; }
        }
        object target;

        public object Target
        {
            get { return target; }
            set { target = value; }
        }

        object[] args;

        public object[] Args
        {
            get { return args; }
            set { args = value; }
        }

        public LinkObject(string address, bool isInternal = false, Delegate delgt = null, object target = null, object[] args = null)
        {
            this.address = address;
            this.isInternal = isInternal;
            this.delgt = delgt;
            this.target = target;
            this.args = args;
        }

        public override string ToString()
        {
            return address;
        }
    }
}