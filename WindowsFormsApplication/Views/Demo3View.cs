using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Views.CommonDialog;
using System.Threading;
using Kizuna.Plus.WinMvcForm.Framework.Views;
using Kizuna.Plus.WinMvcForm.Framework.Utility;

namespace WindowsFormsApplication.Views
{
    public partial class Demo3View : ViewControl
    {
        public Demo3View()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 暗号化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = CryptUtility.encrypt(this.textBox1.Text);
        }

        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = CryptUtility.decrypt(this.textBox1.Text);
        }
    }
}
