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

namespace WindowsFormsApplication.Views
{
    public partial class DemoView : ViewControl
    {
        public Guid Id
        {
            get;
            set;
        }

        public DemoView()
        {
            InitializeComponent();

            Id = Guid.NewGuid();

            this.HelpGuide.Add(new Tuple<Control, string>(this.InfoMessage, "メッセージを入力"));
            this.HelpGuide.Add(new Tuple<Control, string>(this.button4, "通知を押す。\nメッセージが通知エリアに表示されます。"));
            this.HelpGuide.Add(new Tuple<Control, string>(this.checkBox2, "ステータスを押す。\nメッセージがステータスバーに表示されます。"));
            this.HelpGuide.Add(new Tuple<Control, string>(this.button5, "進捗を押す"));
            this.HelpGuide.Add(new Tuple<Control, string>(this.button2, "バージョンを押す"));
            this.HelpGuide.Add(new Tuple<Control, string>(this.button3, "設定を押す"));
        }

        #region 検索
        /// <summary>
        /// 検索可能かを取得します。
        /// </summary>
        public override bool CanSearch { get { return true; } }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            StatusMessageUpdateEventArgs eventArgs = new StatusMessageUpdateEventArgs();
            eventArgs.Id = Guid.NewGuid();
            eventArgs.Message = "テスト";
            eventArgs.Priority = 2;

            button_DefaultClick(sender, eventArgs);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            StatusMessageUpdateEventArgs eventArgs = new StatusMessageUpdateEventArgs();
            eventArgs.Id = this.Id;
            eventArgs.Message = "テスト";
            eventArgs.Priority = 1;

            button_DefaultClick(sender, eventArgs);

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            StatusMessageUpdateEventArgs eventArgs = new StatusMessageUpdateEventArgs();
            eventArgs.Id = this.Id;
            eventArgs.Message = "";
            eventArgs.Priority = 1;

            button_DefaultClick(sender, eventArgs);
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            NortifyMessageEventArgs eventArgs = new NortifyMessageEventArgs();
            eventArgs.Icon = ToolTipIcon.Info;
            eventArgs.Title = "通知";
            eventArgs.Message = "通知しました";

            button_DefaultClick(sender, eventArgs);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NortifyMessageEventArgs eventArgs = new NortifyMessageEventArgs();
            eventArgs.Icon = ToolTipIcon.Info;
            eventArgs.Title = "通知";
            eventArgs.Message = InfoMessage.Text;

            button_DefaultClick(sender, eventArgs);

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            StatusMessageUpdateEventArgs eventArgs = new StatusMessageUpdateEventArgs();
            eventArgs.Id = this.Id;
            if (checkBox2.CheckState == CheckState.Checked)
            {
                eventArgs.Message = InfoMessage.Text;
            }
            else
            {
                eventArgs.Message = string.Empty;
            }
            eventArgs.Priority = 1;

            button_DefaultClick(sender, eventArgs);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var form = new ProgressForm();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += form.ProgressChangedEventHandler;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;

            form.ProgressChancelEvent += form_ProgressChancelEvent;
            backgroundWorker1.RunWorkerAsync();
            form.ShowDialog();

        }

        void form_ProgressChancelEvent(object sender, CancelEventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);
                backgroundWorker1.ReportProgress(i);

                if (backgroundWorker1.CancellationPending == true)
                {
                    break;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ActionEventArgs eventArgs = new ActionEventArgs();
            eventArgs.ActionName = "Index2";

            ActionCommand command = new ActionCommand();
            command.Execute(new NonState(typeof(Application)), eventArgs);

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            ActionEventArgs eventArgs = new ActionEventArgs();
            eventArgs.Controller = "Web";

            ActionCommand command = new ActionCommand();
            command.Execute(new NonState(typeof(Application)), eventArgs);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            throw new NotSupportedException();
        }
    }
}
