using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Views.CommonDialog;
using WindowsFormsApplication.Models;
using Kizuna.Plus.WinMvcForm.Framework.Native;
using Kizuna.Plus.WinMvcForm.Framework.Services.Interceptor;

namespace Kizuna.Plus.WinMvcForm.Framework.Views.Forms
{
    /// <summary>
    /// MainForm抽象クラス
    /// </summary>
    public partial class AbstractForm : Form
    {
        #region メンバー変数
        /// <summary>
        /// 通知エリア表示スタイル
        /// </summary>
        private NortifyStyles nortifyStyles;

        /// <summary>
        /// メニューエリア表示スタイル
        /// </summary>
        private MenuStyles menuStyles;

        /// <summary>
        /// ステータスエリア表示スタイル
        /// </summary>
        private StatusStyles statusStyles;

        /// <summary>
        /// 現在フルスクリーンモード表示を行っているか取得
        /// </summary>
        private bool isCurrentFullScreenMode;

        /// <summary>
        /// コントローラのCache
        /// </summary>
        private Dictionary<Type, IController> cacheControllerList;

        /// <summary>
        /// 現在表示中のコントローラ
        /// </summary>
        private IController controller;

        /// <summary>
        /// 現在表示中のビュー
        /// </summary>
        private IView view;
        #endregion

        #region プロパティ
        /// <summary>
        /// 読み込み完了判断フラグ
        /// </summary>
        public bool IsLoaded
        {
            get;
            private set;
        }

        [Browsable(true)]
        [DefaultValue(NortifyStyles.None)]
        [Category("Other")]
        /// <summary>
        /// 通知エリア表示スタイル
        /// </summary>
        public NortifyStyles NortifyStyles
        {
            get { return nortifyStyles; }
            set
            {
                nortifyStyles = value;
                if (this.nortifyStyles == NortifyStyles.None)
                {
                    // 非表示
                    this.notifyIcon.Visible = false;
                    this.ShowInTaskbar = true;
                }
                else if (this.nortifyStyles == NortifyStyles.Alway)
                {
                    // 常に表示
                    this.notifyIcon.Visible = true;
                    this.ShowInTaskbar = true;
                }
                else if (this.nortifyStyles == NortifyStyles.MinimumWindow
                    || this.nortifyStyles == NortifyStyles.MinimumWindowAndHiddenTask)
                {
                    // 最小化時
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.notifyIcon.Visible = true;
                    }
                    else
                    {
                        this.notifyIcon.Visible = false;
                        if (this.nortifyStyles == NortifyStyles.MinimumWindowAndHiddenTask)
                        {
                            // タスク非表示
                            this.ShowInTaskbar = false;
                        }
                        else
                        {
                            this.ShowInTaskbar = true;
                        }
                    }
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(MenuStyles.Alway)]
        [Category("Other")]
        /// <summary>
        /// メニューエリア表示スタイル
        /// </summary>
        public MenuStyles MenuStyles
        {
            get { return menuStyles; }
            set
            {
                menuStyles = value;
                if (this.menuStyles == MenuStyles.None)
                {
                    // 非表示
                    this.mainMenuStrip.Visible = false;
                }
                else if (this.menuStyles == MenuStyles.Alway)
                {
                    // 常に表示
                    this.mainMenuStrip.Visible = true;
                }
                else if (this.menuStyles == MenuStyles.AltKeyDown)
                {
                    // Alt押下時変更
                    this.mainMenuStrip.Visible = false;
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(StatusStyles.EventOnly)]
        [Category("Other")]
        /// <summary>
        /// ステータスエリア表示スタイル
        /// </summary>
        public StatusStyles StatusStyles
        {
            get { return statusStyles; }
            set
            {
                statusStyles = value;
                if (this.statusStyles == StatusStyles.None)
                {
                    // 非表示
                    this.statusLabel.Visible = false;
                }
                else if (this.statusStyles == StatusStyles.EventOnly)
                {
                    // 常に表示
                    this.statusLabel.Visible = false;
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Other")]
        /// <summary>
        /// フルスクリーンモード可能かフラグ
        /// </summary>
        public bool CanFullScreenMode
        {
            get;
            set;
        }

        /// <summary>
        /// 現在表示中のコントローラ
        /// </summary>
        public IController Controller
        {
            get
            {
                if (this.IsMdiContainer == true)
                {
                    Form mdiForm = this.ActiveMdiChild;
                    if (mdiForm == null)
                    {
                        return null;
                    }
                    return mdiForm.Tag as IController;
                }
                else
                {
                    return controller;
                }
            }
            protected set
            {
                if (value != null)
                {
                    if (controller != null)
                    {
                        // インスタンスの破棄
                        //controller.Dispose();
                    }
                    // インスタンスの初期化
                    value.Initialize();

                    // 初期画面の表示
                    if (this.IsMdiContainer == false)
                    {
                        this.controller = value;
                    }
                }
            }
        }

        /// <summary>
        /// 現在表示中のビュー
        /// </summary>
        public IView View
        {
            get
            {
                if (this.IsMdiContainer == true)
                {
                    Form mdiForm = this.ActiveMdiChild;
                    if (mdiForm == null || mdiForm.Controls.Count <= 0)
                    {
                        return null;
                    }
                    return mdiForm.Controls[0] as IView;
                }
                else
                {
                    return view;
                }
            }
            protected set
            {
                if (value != null)
                {
                    if (view != null)
                    {
                        // インスタンスの破棄
                        //controller.Dispose();
                    }
                    if (this.IsMdiContainer == true)
                    {
                        var control = value as Control;
                        if (control != null)
                        {
                            // 登録した画面を追加
                            Form subForm = new Form();
                            subForm.Controls.Add(control);
                            control.Dock = DockStyle.Fill;

                            value.Initialize();
                            this.searchControl.View = null;

                            if (this.MdiChildren.Count() <= 0)
                            {
                                // なにもウィンドウがいない場合は、最大化
                                subForm.WindowState = FormWindowState.Maximized;
                            }
                            subForm.MdiParent = this;

                            subForm.Show();
                        }
                    }
                    else
                    {
                        MethodInvoker methodInvoker = delegate()
                        {
                            var control = view as Control;
                            if (control != null)
                            {
                                // 前回の画面を破棄
                                this.MainContentPanel.Controls.Remove(control);
                            }
                            control = value as Control;
                            if (control != null)
                            {
                                // 登録した画面を追加
                                this.MainContentPanel.Controls.Add(control);
                                control.Dock = DockStyle.Fill;

                                value.Initialize();
                                view = value;
                                this.searchControl.View = null;
                            }
                        };

                        if (this.MainContentPanel.InvokeRequired == true)
                        {
                            this.MainContentPanel.Invoke(methodInvoker);
                        }
                        else
                        {
                            methodInvoker();
                        }
                    }
                }
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AbstractForm()
        {
            this.IsLoaded = false;
            this.controller = null;
            this.view = null;
            this.cacheControllerList = new Dictionary<Type, IController>();

            InitializeComponent();
            RegistViewEvent();

            this.StatusStyles = Models.Enums.StatusStyles.EventOnly;
            this.MenuStyles = Models.Enums.MenuStyles.Alway;
            this.NortifyStyles = Models.Enums.NortifyStyles.Alway;
            this.CanFullScreenMode = false;

            this.notifyIcon.Text = Application.ProductName;

#if !DEBUG
            this.debugMessageArea.Visible = false;
#endif
        }

        /// <summary>
        /// ビューイベントの登録
        /// </summary>
        private void RegistViewEvent()
        {
            var register = CommandRegister.Current;

            // ステータス更新イベントの登録
            var eventData = new CommandEventData(typeof(Application), typeof(StatusMessageUpdateCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                var messageEventArgs = args as StatusMessageUpdateEventArgs;
                if (messageEventArgs != null)
                {
                    var updateMethod = (MethodInvoker)delegate()
                    {
                        var prevMessageEventArgs = this.statusLabel.Tag as StatusMessageUpdateEventArgs;
                        if (this.statusLabel.Visible == false)
                        {
                            prevMessageEventArgs = null;
                        }

                        if (prevMessageEventArgs != null && prevMessageEventArgs.Priority > messageEventArgs.Priority)
                        {
                            // 優先度が高いメッセージが登録中
                            return;
                        }
                        if (prevMessageEventArgs != null
                            && string.IsNullOrEmpty(messageEventArgs.Message) == true && prevMessageEventArgs.Id != messageEventArgs.Id)
                        {
                            // メッセージを消す場合は、同一IDが表示されている場合のみ
                            return;
                        }

                        // ステータスメッセージ
                        this.statusLabel.Text = messageEventArgs.Message;
                        this.statusLabel.Tag = messageEventArgs;

                        // 表示状態
                        this.statusLabel.Visible = !string.IsNullOrEmpty(messageEventArgs.Message);
                    };

                    UpdateControlChange(updateMethod);
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(NortifyMessageCommand), StateMode.Log, delegate(object sender, EventArgs args)
            {
                var nortifyEventArgs = args as NortifyMessageEventArgs;
                if (nortifyEventArgs != null && string.IsNullOrEmpty(nortifyEventArgs.Message) == false)
                {
                    UpdateControlChange(delegate()
                    {
                        // 通知メッセージ
                        this.notifyIcon.BalloonTipIcon = nortifyEventArgs.Icon;
                        this.notifyIcon.BalloonTipTitle = nortifyEventArgs.Title;
                        this.notifyIcon.BalloonTipText = nortifyEventArgs.Message;
                        this.notifyIcon.ShowBalloonTip(3000);
                    });
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(NortifyMessageCommand), StateMode.Error, delegate(object sender, EventArgs args)
            {
                var nortifyEventArgs = args as NortifyMessageEventArgs;
                if (nortifyEventArgs != null && string.IsNullOrEmpty(nortifyEventArgs.Message) == false)
                {
                    Control control = nortifyEventArgs.Source as Control;
                    if (control != null)
                    {
                        // 通知メッセージ
                        this.validToolTip.Show(nortifyEventArgs.Message, this, control.Location, 3000);
                    }
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(FileNewCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ファイル作成
                using (var dialog = new SaveFileDialog())
                {
                    UpdateControlChange(delegate()
                    {
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            new FileNewCommand().Execute(new RunState(typeof(Application)), new FileEventArgs() { FilePath = dialog.FileName });
                        }
                    });
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(FileOpenCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ファイルオープン
                using (var dialog = new OpenFileDialog())
                {
                    UpdateControlChange(delegate()
                    {
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            new FileOpenCommand().Execute(new RunState(typeof(Application)), new FileEventArgs() { FilePath = dialog.FileName });
                        }
                    });
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(FileSaveAsCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ファイル保存
                using (var dialog = new OpenFileDialog())
                {
                    UpdateControlChange(delegate()
                    {
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            new FileSaveAsCommand().Execute(new RunState(typeof(Application)), new FileEventArgs() { FilePath = dialog.FileName });
                        }
                    });
                }
            });
            register.Regist(eventData);


            eventData = new CommandEventData(typeof(Application), typeof(PrinterCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // 印刷設定
                var printDocument = new System.Drawing.Printing.PrintDocument();
                var dialog = new PageSetupDialog();
                dialog.Document = printDocument;
                UpdateControlChange(delegate()
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        new PrinterCommand().Execute(new RunState(typeof(Application)), new FileEventArgs());
                    }
                });
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(HelpAboutCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // バージョン情報ダイアログの表示
                using (var dialog = new AboutForm())
                {
                    UpdateControlChange(delegate()
                    {
                        dialog.ShowDialog();
                    });
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(ToolOptionCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // 設定ダイアログの表示
                using (var dialog = new SettingForm())
                {
                    UpdateControlChange(delegate()
                    {
                        dialog.ShowDialog();
                    });
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(WindowCloseCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ダイアログを閉じる
                ThreadPool.QueueUserWorkItem((WaitCallback)delegate(object state)
                {
                    UpdateControlChange((MethodInvoker)delegate()
                    {
                        this.Close();
                    });
                });
            });
            eventData = new CommandEventData(typeof(Application), typeof(EditCopyCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // コピー処理
                String contents = null;
                Control control = this.GetSourceActiveControl();
                if (control != null)
                {
                    var textBox = control as TextBoxBase;
                    if (textBox != null)
                    {
                        // TextBox
                        contents = textBox.SelectedText;
                    }
                    var dateTimePicker = control as DateTimePicker;
                    if (dateTimePicker != null)
                    {
                        // DateTimePicker
                        contents = dateTimePicker.Text;
                    }
                    var datagrid = control as DataGridView;
                    if (datagrid != null)
                    {
                        // DataGridView
                    }
                }

                if (String.IsNullOrEmpty(contents) == false)
                {
                    Clipboard.SetText(contents);
                }

            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(EditCutCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // カット処理
                String contents = null;
                Control control = this.GetSourceActiveControl();
                if (control != null)
                {
                    var textBox = control as TextBoxBase;
                    if (textBox != null)
                    {
                        // TextBox
                        int selectionStart = textBox.SelectionStart;
                        String tmpContents = textBox.Text;
                        contents = textBox.SelectedText;
                        textBox.Text = tmpContents.Substring(0, selectionStart) + tmpContents.Substring(selectionStart + textBox.SelectionLength);
                        textBox.SelectionStart = selectionStart;
                    }
                    var dateTimePicker = control as DateTimePicker;
                    if (dateTimePicker != null)
                    {
                        // DateTimePicker
                        contents = dateTimePicker.Text;
                    }
                    var datagrid = control as DataGridView;
                    if (datagrid != null)
                    {
                        // DataGridView
                        datagrid.SelectAll();
                    }
                }

                if (String.IsNullOrEmpty(contents) == false)
                {
                    Clipboard.SetText(contents);
                }

            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(EditPasteCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // 貼り付け処理
                String contents = Clipboard.GetText();
                Control control = this.GetSourceActiveControl();
                if (string.IsNullOrEmpty(contents) == false && control != null)
                {
                    var textBox = control as TextBoxBase;
                    if (textBox != null)
                    {
                        // TextBox
                        int selectionStart = textBox.SelectionStart;
                        int selectionLength = textBox.SelectionLength;
                        String tmpContents = textBox.Text;
                        if (1 < selectionLength)
                        {
                            tmpContents = tmpContents.Substring(0, selectionStart) + tmpContents.Substring(selectionStart + selectionLength);
                        }
                        textBox.Text = tmpContents.Insert(selectionStart, contents);
                        textBox.SelectionStart = selectionStart + contents.Length;
                    }
                    var dateTimePicker = control as DateTimePicker;
                    if (dateTimePicker != null)
                    {
                        // DateTimePicker
                        dateTimePicker.Text = contents;
                    }
                    var datagrid = control as DataGridView;
                    if (datagrid != null)
                    {
                        // DataGridView
                        datagrid.SelectAll();
                    }
                }

            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(EditSelectCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // 選択処理
                Control control = this.GetSourceActiveControl();
                if (control != null)
                {
                    var textBox = control as TextBoxBase;
                    if (textBox != null)
                    {
                        // TextBox
                        textBox.SelectionStart = 0;
                        textBox.SelectionLength = textBox.Text.Length;
                    }
                    var datagrid = control as DataGridView;
                    if (datagrid != null)
                    {
                        // DataGridView
                        datagrid.SelectAll();
                    }
                }

            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(EditUndoCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // アンドゥー
                Control control = this.GetSourceActiveControl();
                if (control != null)
                {
                    var textBox = control as TextBoxBase;
                    if (textBox != null)
                    {
                        // TextBox
                        if (textBox.CanUndo == true)
                        {
                            textBox.Undo();
                        }
                    }
                }

            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(EditRedoCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // アンドゥー
                Control control = this.GetSourceActiveControl();
                if (control != null)
                {
                    var textBox = control as TextBoxBase;
                    if (textBox != null)
                    {
                        // TextBox
                        //textBox.re();
                    }
                }

            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // コントローラの呼び出し。
                ActionEventArgs actionArgs = args as ActionEventArgs;
                if (actionArgs != null)
                {
                    String controller = actionArgs.Controller;
                    String actionName = actionArgs.ActionName;
                    Object[] parameters = actionArgs.Parameters;
                    if (String.IsNullOrEmpty(controller) == true)
                    {
                        // Controller
                        controller = Controller.GetType().Name;
                    }
                    if (String.IsNullOrEmpty(actionName) == true)
                    {
                        // Action
                        actionName = "Index";
                    }


                    var oldId = Guid.Empty;
                    if (this.Controller != null)
                    {
                        oldId = this.Controller.ServiceId;
                    }
                    var newController = ChangeController(controller);
                    ServicePool.Current.ReleaseService(oldId);

                    FieldInfo[] fields = newController.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                    foreach (FieldInfo field in fields)
                    {
                        // Intect属性がついているフィールドに値を設定
                        InjectAttribute.InjectService<IService>(newController, field, newController.ServiceId);
                    }

                    // Controllerのメソッドを実行

                    var invokerMethod = newController.GetType().GetMethod(actionName);
                    if (invokerMethod == null)
                    {
                        // メソッドを取得失敗
                        return;
                    }

                    ViewStateData.CurrentThread.Items.Clear();
                    ViewStateData.CurrentThread.Items["Controller"] = controller;
                    ViewStateData.CurrentThread.Items["Action"] = actionName;
                    ViewStateData.CurrentThread.Items[TransactionInterceptorAttribute.SERVICE_KEY] = new TransactionData();

                    object result = invokerMethod.Invoke(newController, parameters);

                    Thread thread = new Thread(delegate()
                    {

                        UpdateControlChange((MethodInvoker)delegate()
                        {
                            this.View = result as IView;
                            if (this.View != null)
                            {
                                this.View.InitBindData();
                            }
                            if (this.IsMdiContainer == true)
                            {
                                Control control = this.View as Control;
                                if (control != null)
                                {
                                    // MDIの場合は、Viewのタグにコントローラを設定
                                    control.Tag = newController;
                                }
                            }
                        });
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();

                }
            });
            register.Regist(eventData);
#if DEBUG
            eventData = new CommandEventData(typeof(Application), typeof(LogCommand), StateMode.Log, delegate(object sender, EventArgs args)
            {
                var exceptionEventArgs = args as ExceptionEventArgs;
                if (exceptionEventArgs != null)
                {
                    // デバッグメッセージ
                    this.debugMessageArea.Text = exceptionEventArgs.Exception.Message;
                }
                var messageEventArgs = args as LogMessageEventArgs;
                if (messageEventArgs != null)
                {
                    // デバッグメッセージ
                    string message = this.debugMessageArea.Text;
                    message += DateTime.Now.ToString("HH:mm:ss") + " " + messageEventArgs.Message + Environment.NewLine;
                    string[] lineData = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    if (lineData != null && 2000 < lineData.Length)
                    {
                        message = "";
                        for (int rowIndex = lineData.Length - 2000; rowIndex < lineData.Length; rowIndex++)
                        {
                            message += lineData[rowIndex] + Environment.NewLine;
                        }
                    }

                    UpdateControlChange(delegate()
                    {
                        this.debugMessageArea.Text = message;
                        this.debugMessageArea.Select(message.Length, 0);
                        this.debugMessageArea.ScrollToCaret();
                    });

                }
            });
            register.Regist(eventData);
#endif
        }
        #endregion

        #region イベント
        /// <summary>
        /// フォームローディングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (ConfigurationData.Current != null)
            {
                var formBounds = ConfigurationData.Current.FormBounds;
                if (formBounds != null)
                {
                    var setFormBounds = formBounds.Value;
                    if (formBounds.Value.Right < 0)
                    {
                        // 画面外 左側
                        setFormBounds.Location = new Point(0, formBounds.Value.Location.Y);
                    }
                    if (formBounds.Value.Bottom < 0)
                    {
                        // 画面外 左側
                        setFormBounds.Location = new Point(formBounds.Value.Location.X, 0);
                    }

                    this.StartPosition = FormStartPosition.Manual;
                    this.Bounds = setFormBounds;
                }
            }

            if (this.NortifyStyles == NortifyStyles.MinimumWindow
                || this.NortifyStyles == NortifyStyles.MinimumWindowAndHiddenTask)
            {
                // 最小化した場合通知エリアに表示
                this.notifyIcon.Visible = true;
            }

            var applicationIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.Icon = applicationIcon;
            this.notifyIcon.Icon = applicationIcon;

            this.Text = Application.ProductName;


            if (this.IsMdiContainer == true)
            {
                // Mdiのばあいは非表示
                this.MainContentPanel.Visible = false;
            }

            // 初期化終了
            this.IsLoaded = true;
        }

        /// <summary>
        /// キー入力イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt == true && this.MenuStyles == Models.Enums.MenuStyles.AltKeyDown)
            {
                this.mainMenuStrip.Visible = !this.mainMenuStrip.Visible;
            }
            if (((e.Control == true && e.KeyCode == Keys.F) || e.KeyCode == Keys.F3)
                && this.View != null && this.View.CanSearch == true)
            {
                // 検索可能なビューの場合は、検索コントロールを表示
                this.searchControl.View = this.View;
                if (e.Control == true && e.KeyCode == Keys.F)
                {
                    this.searchControl.Visible = !this.searchControl.Visible;
                    if (this.searchControl.Visible == true)
                    {
                        // 表示した場合は、アクティブにする
                        this.searchControl.Select();
                    }
                }
                else
                {
                    this.searchControl.Visible = true;
                }

                if (e.KeyCode == Keys.F3)
                {
                    // 次へ、前へ検索
                    bool isNext = (ModifierKeys & Keys.Control) != Keys.Control;
                    var searchCommand = new SearchCommand();
                    searchCommand.Execute(new NonState(typeof(Application)), new SearchEventArgs() { Target = this.View, IsNext = isNext });
                }
            }
            if (e.KeyCode == Keys.F11 && this.CanFullScreenMode == true)
            {
                // フルスクリーン表示
                if (isCurrentFullScreenMode == false)
                {
                    this.FormBorderStyle = FormBorderStyle.None;

                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.WindowState = FormWindowState.Normal;
                    }
                    this.WindowState = FormWindowState.Maximized;
                    isCurrentFullScreenMode = true;
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                    isCurrentFullScreenMode = false;
                }
            }
            if (this.isCurrentFullScreenMode == true && e.KeyCode == Keys.Escape)
            {
                // フルスクリーンモード解除
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                isCurrentFullScreenMode = false;
            }
            if (e.KeyCode == Keys.F1)
            {
                var activeControl = this.ActiveControl;
                while (activeControl is AbstractView)
                {
                    if (((AbstractView)activeControl).ActiveControl == null)
                    {
                        break;
                    }
                    activeControl = ((AbstractView)activeControl).ActiveControl;
                }

                // ヘルプの表示
                var helpCommand = new HelpCommand();
                helpCommand.Execute(new NonState(activeControl), EventArgs.Empty);
            }
        }

        /// <summary>
        /// フォームリサイズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.IsLoaded == true && this.WindowState == FormWindowState.Normal)
            {
                if (ConfigurationData.Current != null)
                {
                    ConfigurationData.Current.FormBounds = this.Bounds;
                }
            }

            if (this.WindowState == FormWindowState.Minimized &&
                (this.NortifyStyles == NortifyStyles.MinimumWindow
                || this.NortifyStyles == NortifyStyles.MinimumWindowAndHiddenTask))
            {
                // 最小化した場合通知エリアに表示
                if (this.NortifyStyles == NortifyStyles.MinimumWindowAndHiddenTask)
                {
                    this.ShowInTaskbar = false;
                }
                this.notifyIcon.Visible = true;
            }
            else
            {
                if (this.NortifyStyles != NortifyStyles.Alway)
                {
                    this.ShowInTaskbar = true;
                    this.notifyIcon.Visible = true;
                }
            }
        }

        /// <summary>
        /// フォーム移動イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (this.IsLoaded == true && this.WindowState == FormWindowState.Normal)
            {
                if (ConfigurationData.Current == null)
                {
                    return;
                }
                ConfigurationData.Current.FormBounds = this.Bounds;
            }
        }

        /// <summary>
        /// フォームクローズ前イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // エラーがあっても終了
            e.Cancel = false;
        }

        /// <summary>
        /// フォームクローズ後イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbstractForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.notifyIcon.Visible = false;

            if (this.IsMdiContainer == true)
            {
                // サブウィンドウを閉じる
                foreach (var subForm in this.MdiChildren)
                {
                    subForm.Close();
                }
            }

            // キャッシュのクリア
            foreach (IController controller in this.cacheControllerList.Values)
            {
                controller.Dispose();
            }
            this.cacheControllerList.Clear();

            CommandRegister.Current.UnregistOfSource(typeof(Application));
        }
        #endregion

        #region 画面変更
        /// <summary>
        /// コントローラの変更
        /// <param name="controllerName">コントローラ名</param>
        /// </summary>
        public IController ChangeController(string controllerName)
        {
            IController ret;
            lock (MvcCooperationData.Current.CurrentDomainControllersType)
            {
                Type controllerType = null;
                foreach (Type type in MvcCooperationData.Current.CurrentDomainControllersType)
                {
                    if (type.Name.EndsWith(controllerName) == true
                        || type.Name.EndsWith(controllerName + "Controller") == true)
                    {
                        controllerType = type;
                        break;
                    }
                }
                if (controllerType == null)
                {
                    // 見つからない
                    return null;
                }

                if (cacheControllerList.ContainsKey(controllerType) == true)
                {
                    // キャッシュを発見
                    ret = cacheControllerList[controllerType];
                    this.Controller = ret;
                    return ret;
                }

                Type controllerIfType = typeof(IController);
                if (controllerIfType.IsAssignableFrom(controllerType) == false)
                {
                    // 見つからない
                    return null;
                }

                ConstructorInfo constructor = controllerType.GetConstructor(new Type[0]);
                var newController = constructor.Invoke(null) as IController;
                this.Controller = newController;
                this.cacheControllerList.Add(controllerType, newController);
                ret = newController;
            }

            return ret;
        }

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

        #region イメージの取得
        /// <summary>
        /// コントロールのイメージを取得する
        /// </summary>
        /// <param name="ctrl">キャプチャするコントロール</param>
        /// <returns>取得できたイメージ</returns>
        public static Bitmap GetControlImage(Control ctrl)
        {
            Bitmap img = null;
            using (Graphics graphics = ctrl.CreateGraphics())
            {
                IntPtr graphicsDc = graphics.GetHdc();
                img = new Bitmap(ctrl.ClientRectangle.Width,
                    ctrl.ClientRectangle.Height, graphics);
                using (Graphics memg = Graphics.FromImage(img))
                {
                    IntPtr memgDc = memg.GetHdc();
                    Win32GraphicNativeMethods.BitBlt(memgDc, 0, 0, img.Width, img.Height, graphicsDc, 0, 0, Win32GraphicNativeMethods.SRCCOPY);
                    memg.ReleaseHdc(memgDc);
                }
                graphics.ReleaseHdc(graphicsDc);
            }
            return img;
        }
        #endregion

        #region 印刷
        /// <summary>
        /// 現在表示しているコンテンツを印刷します。
        /// </summary>
        private void PrintDocument()
        {
            if (this.View == null)
            {
                // 印字対象が存在しない
                return;
            }

            using (PrintDocument printDocument = new PrintDocument())
            {
                printDocument.PrintPage += PrintDocument_PrintPage;
                printDocument.Print();
            }
        }

        /// <summary>
        /// ページ印刷イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // コントロールのイメージを描画
            Control control = this.View as Control;
            Image image = GetControlImage(control);
            Rectangle imageRect = new Rectangle(new Point(), image.Size);

            e.Graphics.DrawImage(image, e.PageSettings.PrintableArea, imageRect, GraphicsUnit.Pixel);
        }
        #endregion

        #region コントロール無効化
        /// <summary>
        /// コントロールの有効/無効　非表示/表示設定を行います。
        /// </summary>
        /// <param name="visible">表示する場合はtrue, それ以外はfalse</param>
        /// <param name="enabled">有効化する場合はtrue, それ以外はfalse</param>
        /// <param name="controls">対象のコントロール配列</param>
        protected void SetVisibleEnableControl(bool visible, bool enabled, Component[] controls)
        {
            foreach (Component component in controls)
            {
                var control = component as Control;
                if (control != null)
                {
                    control.Visible = visible;
                    control.Enabled = enabled;
                    continue;
                }

                var toolStripMenuItem = component as ToolStripItem;
                if (toolStripMenuItem != null)
                {
                    toolStripMenuItem.Visible = visible;
                    toolStripMenuItem.Enabled = enabled;
                    continue;
                }

            }
        }
        #endregion

        #region 通知エリアイベント
        /// <summary>
        /// 通知エリアのダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // 標準に戻す
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }
        #endregion

        #region デバッグエリアイベント
        /// <summary>
        /// デバッグエリアのダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void debugMessageArea_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            this.debugMessageArea.Visible = false;
#endif
        }
        #endregion

        #region メニュークリックイベント
        /// <summary>
        /// メニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null || menuItem.Tag == null)
            {
                return;
            }

            Type commandType = null;
            foreach (Type type in MvcCooperationData.Current.CurrentDomainCommandType)
            {
                if (type.Name.EndsWith((String)menuItem.Tag + "Command") == true)
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
            command.Execute(new NonState(typeof(Application)), e);

        }
        #endregion

        #region 画面コントロール
        /// <summary>
        /// 実際にアクティブになっているコントロールを取得
        /// </summary>
        /// <returns></returns>
        private Control GetSourceActiveControl()
        {
            Control preControl = this.ActiveControl;
            Control control = this.ActiveControl;
            while (control != null)
            {
                preControl = control;
                if (control is IContainerControl == false)
                {
                    // コンテナではない場合は、そのコントロールが根源と判断
                    preControl = control;
                    break;
                }
                control = ((IContainerControl)control).ActiveControl;
            }

            return preControl;
        }
        #endregion
    }
}
