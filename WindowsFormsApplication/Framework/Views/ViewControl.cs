using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Models.Validation;
using System.Threading;

namespace Kizuna.Plus.WinMvcForm.Framework.Views
{
    public class ViewControl : UserControl, IView
    {
        #region メンバー変数
        /// <summary>
        /// イベント割付データ一覧
        /// </summary>
        private List<CommandEventData> commandEventDataList;

        /// <summary>
        /// 
        /// </summary>
        private CommonControl.HelperControl helperControl;

        /// <summary>
        /// 入力チェック処理情報
        /// </summary>
        private IDictionary<Control, IList<ModelValidationAttribute>> validationControlDict;
        #endregion

        #region プロパティ
        /// <summary>
        /// ヘルプ情報表示設定
        /// </summary>
        protected List<Tuple<Control, string>> HelpGuide
        {
            get
            {
                return this.helperControl.HelpGuide;
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewControl()
        {
            // イベント割付データ一覧の取得
            commandEventDataList = GetCommandEventDataList();

            var eventData = new CommandEventData(typeof(Control), typeof(HelpCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                this.helperControl.Show(sender as Control);
            });
            commandEventDataList.Add(eventData);

            // ヘルプ表示
            this.helperControl = new Kizuna.Plus.WinMvcForm.Framework.Views.CommonControl.HelperControl();


            validationControlDict = new Dictionary<Control, IList<ModelValidationAttribute>>();

            this.SetStyle(ControlStyles.ContainerControl, true);
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            
            this.SuspendLayout();
            this.Load += new System.EventHandler(this.ViewControl_Load);
            this.ResumeLayout(false);
        }

        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            RegistViewEvent();
        }

        /// <summary>
        /// バインドデータの設定
        /// </summary>
        public virtual void InitBindData()
        {
            Type modelType = MvcCooperationData.View2Model(this.GetType());
            validationControlDict.Clear();

            if (ViewStateData.CurrentThread != null)
            {
                foreach (Object obj in ViewStateData.CurrentThread.Items.Values)
                {
                    if (obj.GetType() == modelType)
                    {
                        foreach (Control control in this.Controls)
                        {
                            var dataGridView = control as DataGridView;
                            if (dataGridView != null)
                            {
                                dataGridView.DataSource = obj;
                            }
                            else
                            {
                                control.DataBindings.Clear();
                                PropertyInfo property = modelType.GetProperty(control.Name, BindingFlags.Instance | BindingFlags.Public);
                                if (property == null)
                                {
                                    continue;
                                }

                                Binding binding = new Binding("Text", obj, control.Name);
                                control.DataBindings.Add(binding);

                                List<ModelValidationAttribute> validAttrList = new List<ModelValidationAttribute>();
                                foreach (Attribute attr in property.GetCustomAttributes(typeof(ModelValidationAttribute), true))
                                {
                                    ModelValidationAttribute ivAttr = attr as ModelValidationAttribute;
                                    if (ivAttr == null)
                                    {
                                        continue;
                                    }
                                    validAttrList.Add(ivAttr);
                                }

                                if (0 < validAttrList.Count)
                                {
                                    validationControlDict.Add(control, validAttrList);
                                }

                                control.Validating += control_Validating;
                            }
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        #region 更新
        /// <summary>
        /// コントロールの更新
        /// </summary>
        public override void Refresh()
        {
            InitBindData();
        }
        #endregion

        #region イベント発行
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // バインド処理の初期化
            InitBindData();

            if (DesignMode == false)
            {
                this.helperControl.AssignControl = this;
                this.Controls.Add(this.helperControl);
                this.helperControl.BringToFront();
            }
        }
        #endregion

        #region イベント操作
        /// <summary>
        /// 表示イベントの登録
        /// </summary>
        private void RegistViewEvent()
        {
            if (commandEventDataList == null || commandEventDataList.Count <= 0)
            {
                // 未登録
                return;
            }

            var register = CommandRegister.Current;
            foreach (CommandEventData data in commandEventDataList)
            {
                register.Regist(data);
            }
        }

        /// <summary>
        /// 表示イベントの解除
        /// </summary>
        private void UnregistViewEvent()
        {
            if (commandEventDataList == null || commandEventDataList.Count <= 0)
            {
                // 未登録
                return;
            }

            var register = CommandRegister.Current;
            foreach (CommandEventData data in commandEventDataList)
            {
                register.Unregist(data);
            }
        }

        /// <summary>
        /// イベント一覧の取得
        /// </summary>
        /// <returns></returns>
        public virtual List<CommandEventData> GetCommandEventDataList()
        {
            var list = new List<CommandEventData>();
            return list;
        }
        #endregion

        #region ロードイベント
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sendar"></param>
        /// <param name="args"></param>
        private void ViewControl_Load(object sendar, EventArgs args)
        {
            foreach (Control control in this.Controls)
            {
                Button button = control as Button;
                if (button == null)
                {
                    continue;
                }
                button.Click += button_DefaultClick;
            }
        }
        #endregion

        #region 入力チェックイベント
        /// <summary>
        /// 入力チェック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Validating(object sender, CancelEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) {
                return;
            }

            if (validationControlDict.ContainsKey(control) == true)
            {
                String message;
                if (ModelValidation.Valid(control, validationControlDict[control], out message) == false)
                {
                    e.Cancel = true;

                    NortifyMessageEventArgs eventArgs = new NortifyMessageEventArgs();
                    eventArgs.Source = control;
                    eventArgs.Message = message;

                    NortifyMessageCommand command = new NortifyMessageCommand();
                    command.Execute(new ErrorState(typeof(Application)), eventArgs);

                    control.BackColor = Color.LightPink;
                }
                else
                {
                    control.BackColor = TextBox.DefaultBackColor;
                }
            }
        }
        #endregion

        #region ボタン処理イベント
        /// <summary>
        /// ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void button_DefaultClick(object sender, EventArgs e)
        {
            var button = sender as ButtonBase;
            if (button == null || button.Tag == null)
            {
                return;
            }

            Type commandType = null;
            foreach (Type type in MvcCooperationData.Current.CurrentDomainCommandType)
            {
                if (type.Name.EndsWith((String)button.Tag + "Command") == true)
                {
                    commandType = type;
                }
            }
            if (commandType == null)
            {
                return;
            }
            var command = Activator.CreateInstance(commandType) as ICommand;
            if (command == null)
            {
                return;
            }
            var enableControlList = new List<Control>();
            foreach (Control control in this.Controls)
            {
                if (control.Enabled == false)
                {
                    continue;
                }
                control.Enabled = false;
                enableControlList.Add(control);
            }
            this.Cursor = Cursors.WaitCursor;
            new Thread(new ThreadStart(delegate()
            {
                command.Execute(new NonState(typeof(Application)), e);
                UpdateControlChange(delegate()
                {
                    foreach (Control control in enableControlList)
                    {
                        control.Enabled = true;
                    }

                    this.Cursor = Cursors.Default;
                });

            })).Start();

        }
        #endregion

        #region 画面変更
        /// <summary>
        /// マルチスレッド動作時の画面更新処理を
        /// 適切なスレッドで動作させる
        /// </summary>
        /// <param name="updateMethod"></param>
        private void UpdateControlChange(MethodInvoker updateMethod)
        {
            // 画面更新処理
            if (this.InvokeRequired == true)
            {
                this.Invoke(updateMethod);
            }
            else
            {
                updateMethod();
            }
        }
        #endregion

        #region 検索
        /// <summary>
        /// 検索可能かを取得します。
        /// </summary>
        public virtual bool CanSearch { get { return false; } }

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="searchWord">検索キーワード</param>
        /// <param name="isNext">true: 次を検索, false: 前を検索</param>
        public virtual bool Search(string searchWord, bool isNext)
        {
            return false;
        }
        #endregion

        #region 破棄処理
        /// <summary>
        /// 破棄処理
        /// </summary>
        /// <param name="isDispose"></param>
        protected override void Dispose(bool isDispose)
        {
            base.Dispose(isDispose);

            if (isDispose == true)
            {
                UnregistViewEvent();
            }
        }
        #endregion
    }
}
