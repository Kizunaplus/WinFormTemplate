using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Views;

namespace WindowsFormsApplication.Views
{
    public partial class WebView : ViewControl
    {
        public WebView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Uri uri = null;

            try
            {
                uri = new Uri(this.textBox1.Text);
                this.webViewControl1.Url = uri;
            }
            catch { }
        }
    }
}
