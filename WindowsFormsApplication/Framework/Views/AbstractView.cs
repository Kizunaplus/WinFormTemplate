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
    /// <summary>
    /// 表示クラス　WinFromのコントロールクラス
    /// </summary>
    abstract class AbstractView : UserControl, IView
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

        /// <summary>
        /// バインドモデル
        /// </summary>
        private AbstractModel bindModel;
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

        /// <summary>
        /// バインドするモデルのタイプ
        /// 未指定の場合は、ビュー名に対応するモデルタイプを使用する。
        /// </summary>
        public Type ModelType
        {
            get;
            protected set;
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AbstractView()
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
            Type modelType = ModelType;
            if (modelType == null)
            {
                modelType = MvcCooperationData.View2Model(this.GetType());
            }
            validationControlDict.Clear();

            if (ViewStateData.CurrentThread != null)
            {
                foreach (Object obj in ViewStateData.CurrentThread.Items.Values)
                {
                    if (obj.GetType() == modelType)
                    {
                        bindModel = obj as AbstractModel;
                        foreach (Control control in this.Controls)
                        {
                            var dataGridView = control as DataGridView;
                            if (dataGridView != null)
                            {
                                var bs = new BindingSource();
                                dataGridView.DataSource = bs;
                                bs.DataSource = obj;
                                bs.DataMember = dataGridView.Name;

                                dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                                dataGridView.RowValidating += new DataGridViewCellCancelEventHandler(dataGridView_RowValidating);
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
            this.Update();
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
                    // 入力エラー
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
                    // 入力正常
                    control.BackColor = SystemColors.Window;
                }
            }
        }

        /// <summary>
        /// 入力チェック処理（DataGridView）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView == null)
            {
                return;
            }

            if (dataGridView.IsCurrentCellDirty == false)
            {
                // 変更していない場合は、何もしない
                return;
            }

            PropertyInfo viewBindProperty = bindModel.GetType().GetProperty(dataGridView.Name, BindingFlags.Instance | BindingFlags.Public);
            Type viewBindPropertyType = viewBindProperty.PropertyType;
            if (viewBindPropertyType.IsGenericType == true 
                && 0 < viewBindPropertyType.GetGenericArguments().Length)
            {
                viewBindPropertyType = viewBindPropertyType.GetGenericArguments()[0];
            }

            String propertyName = dataGridView.Columns[e.ColumnIndex].DataPropertyName;
            PropertyInfo property = viewBindPropertyType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
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

            String message;
            if (ModelValidation.Valid(e.FormattedValue, dataGridView.Columns[e.ColumnIndex].HeaderText, validAttrList, out message) == false)
            {
                // 入力エラー
                e.Cancel = true;
                dataGridView.Rows[e.RowIndex].ErrorText = message;
            } else {
                dataGridView.Rows[e.RowIndex].ErrorText = null;
            }

        }

        void dataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView == null)
            {
                return;
            }

            if (dataGridView.IsCurrentRowDirty == false)
            {
                // 変更していない場合は、何もしない
                return;
            }


            PropertyInfo viewBindProperty = bindModel.GetType().GetProperty(dataGridView.Name, BindingFlags.Instance | BindingFlags.Public);
            Type viewBindPropertyType = viewBindProperty.PropertyType;
            if (viewBindPropertyType.IsGenericType == true
                && 0 < viewBindPropertyType.GetGenericArguments().Length)
            {
                viewBindPropertyType = viewBindPropertyType.GetGenericArguments()[0];
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                String propertyName = column.DataPropertyName;
                if (string.IsNullOrEmpty(propertyName) == true)
                {
                    continue;
                }
                PropertyInfo property = viewBindPropertyType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
                if (property == null)
                {
                    continue;
                }
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

                String message;
                if (ModelValidation.Valid(dataGridView[column.Index, e.RowIndex].Value, column.HeaderText, validAttrList, out message) == false)
                {
                    // 入力エラー
                    e.Cancel = true;
                    dataGridView.Rows[e.RowIndex].ErrorText = message;
                    break;
                }
                else
                {
                    dataGridView.Rows[e.RowIndex].ErrorText = null;
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

            ThreadPool.QueueUserWorkItem((WaitCallback) delegate(object state)
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

            });

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
