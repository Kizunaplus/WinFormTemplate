using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication.Controllers;
using WindowsFormsApplication.Controllers.Commands;
using WindowsFormsApplication.Controllers.State;
using WindowsFormsApplication.Models;
using WindowsFormsApplication.Models.Enums;
using WindowsFormsApplication.Models.EventArg;
using WindowsFormsApplication.Services;
using WindowsFormsApplication.Views;
using WindowsFormsApplication.Views.CommonDialog;

namespace WindowsFormsApplication.Views.Forms
{
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

        /// <summary>
        /// 現在定義されているコントローラのタイプ一覧
        /// </summary>
        private List<Type> currentDomainControllersType;
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
                    this.View = value.Index();
                    if (this.IsMdiContainer == true)
                    {
                        Control control = this.View as Control;
                        if (control != null)
                        {
                            control.Tag = value;
                        }
                    }
                    else
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
                        var control = view as Control;
                        if (control != null)
                        {
                            // 前回の画面を破棄
                            this.MainContentPanel.Controls.Remove(control);
                            view.Dispose();
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
                    }
                }
            }
        }

        /// <summary>
        /// 現在定義されているコントローラのタイプ一覧
        /// </summary>
        public List<Type> CurrentDomainControllersType
        {
            get
            {
                if (currentDomainControllersType == null)
                {
                    currentDomainControllersType = new List<Type>();
                    Type controllerType = typeof(IController);

                    var assembly = Assembly.GetExecutingAssembly();
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (controllerType.IsAssignableFrom(type) == true)
                        {
                            currentDomainControllersType.Add(type);
                        }
                    }
                }
                return currentDomainControllersType;
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

            this.NortifyStyles = NortifyStyles.Alway;
            this.MenuStyles = MenuStyles.None;
            this.StatusStyles = StatusStyles.EventOnly;
            this.CanFullScreenMode = true;

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
                    // 通知メッセージ
                    this.notifyIcon.BalloonTipIcon = nortifyEventArgs.Icon;
                    this.notifyIcon.BalloonTipTitle = nortifyEventArgs.Title;
                    this.notifyIcon.BalloonTipText = nortifyEventArgs.Message;
                    this.notifyIcon.ShowBalloonTip(3000);
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(FileNewCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ファイル作成
                var dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new FileNewCommand().Execute(new RunState(typeof(Application)), new FileEventArgs() { FilePath = dialog.FileName });
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(FileOpenCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ファイルオープン
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new FileOpenCommand().Execute(new RunState(typeof(Application)), new FileEventArgs() { FilePath = dialog.FileName });
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(FileSaveAsCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ファイル保存
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new FileSaveAsCommand().Execute(new RunState(typeof(Application)), new FileEventArgs() { FilePath = dialog.FileName });
                }
            });
            register.Regist(eventData);


            eventData = new CommandEventData(typeof(Application), typeof(PrinterCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // 印刷設定
                var printDocument = new System.Drawing.Printing.PrintDocument();
                var dialog = new PageSetupDialog();
                dialog.Document = printDocument;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new PrinterCommand().Execute(new RunState(typeof(Application)), new FileEventArgs());
                }
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(HelpAboutCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // バージョン情報ダイアログの表示
                new AboutForm().ShowDialog();
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(ToolOptionCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // 設定ダイアログの表示
                new SettingForm().ShowDialog();
            });
            register.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(WindowCloseCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                // ダイアログを閉じる
                new Thread(delegate()
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        this.Close();
                    });
                }).Start();
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

                    this.debugMessageArea.Text = message;
                    this.debugMessageArea.Select(message.Length, 0);
                    this.debugMessageArea.ScrollToCaret();

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
                while (activeControl is ViewControl)
                {
                    if (((ViewControl)activeControl).ActiveControl == null)
                    {
                        break;
                    }
                    activeControl = ((ViewControl)activeControl).ActiveControl;
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
        /// フォームクローズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
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
        public void ChangeController(string controllerName)
        {
            lock (this.CurrentDomainControllersType)
            {
                Type controllerType = null;
                foreach (Type type in this.CurrentDomainControllersType)
                {
                    if (type.Name.EndsWith(controllerName + "Controller") == true)
                    {
                        controllerType = type;
                        break;
                    }
                }
                if (controllerType == null)
                {
                    // 見つからない
                    return;
                }

                if (cacheControllerList.ContainsKey(controllerType) == true)
                {
                    // キャッシュを発見
                    this.Controller = cacheControllerList[controllerType];
                    return;
                }

                Type controllerIfType = typeof(IController);
                if (controllerIfType.IsAssignableFrom(controllerType) == false)
                {
                    // 見つからない
                    return;
                }

                ConstructorInfo constructor = controllerType.GetConstructor(new Type[0]);
                var newController = constructor.Invoke(null) as IController;
                this.Controller = newController;
                this.cacheControllerList.Add(controllerType, newController);
            }
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

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            printDocument.Print();
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

            var commandType = Assembly.GetExecutingAssembly().GetType(typeof(ICommand).Namespace + "." + menuItem.Tag + "Command");
            if (commandType == null)
            {
                return;
            }
            var command = Activator.CreateInstance(commandType) as ICommand;
            if (command == null)
            {
                return;
            }
            command.Execute(new NonState(typeof(Application)), EventArgs.Empty);

        }
    }
}
