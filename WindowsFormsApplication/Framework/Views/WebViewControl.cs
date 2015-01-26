using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using WindowsFormsApplication.Framework.Message;
using Kizuna.Plus.WinMvcForm.Framework.Views.WebJs;
using IronPython.Hosting;

namespace Kizuna.Plus.WinMvcForm.Framework.Views
{
    /// <summary>
    /// 表示クラス　WebBrowserクラス
    /// </summary>
    public class WebViewControl : WebBrowser, IView
    {
        #region メンバー変数
        /// <summary>
        /// イベント割付データ一覧
        /// </summary>
        private List<CommandEventData> commandEventDataList;

        /// <summary>
        /// 表示するページのパス
        /// </summary>
        private string pagePath;
        #endregion

        #region プロパティ
        /// <summary>
        /// 表示するページのパスを取得します。
        /// </summary>
        [Browsable(true)]
        public string PagePath
        {
            get
            {
                return pagePath;
            }
            set
            {
                string tmpPagePath = pagePath;
                pagePath = value;
                if (tmpPagePath != value 
                    && string.IsNullOrEmpty(value) == false)
                {
                    OnPagePathChanged(EventArgs.Empty);
                }
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
        public WebViewControl()
        {
            // イベント割付データ一覧の取得
            commandEventDataList = GetCommandEventDataList();

            this.ObjectForScripting = new WebViewJsAccess();
            this.WebBrowserShortcutsEnabled = false;
            this.AllowWebBrowserDrop = false;
            this.IsWebBrowserContextMenuEnabled = false;
        }

        /// <summary>
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
            if (ViewStateData.CurrentThread.Items.ContainsKey("Page") == false)
            {
                string controll = ViewStateData.CurrentThread.Items["Controller"] as String;
                string action = ViewStateData.CurrentThread.Items["Action"] as String;

                // 対象のファイルが存在するかチェック
                string[] exts = new string[] { "html", "htm", "xhtml", "py" };
                foreach (String ext in exts)
                {
                    string path = String.Format("{0}\\Views\\Web\\{1}\\{2}.{3}", Path.GetDirectoryName(Application.ExecutablePath), controll, action, ext);
                    if (File.Exists(path) == true)
                    {
                        this.PagePath = path;
                        break;
                    }
                }

                // 対象のファイルが存在するかチェック
                if (string.IsNullOrEmpty(this.PagePath) == true)
                {
                    foreach (String ext in exts)
                    {
                        string path = String.Format("{0}\\Views\\Web\\share\\{2}.{3}", Path.GetDirectoryName(Application.ExecutablePath), controll, action, ext);
                        if (File.Exists(path) == true)
                        {
                            this.PagePath = path;
                            break;
                        }
                    }
                }
            }
            else
            {
                this.PagePath = ViewStateData.CurrentThread.Items["Page"] as String;
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
        public virtual List<CommandEventData> GetCommandEventDataList() { return null; }
        #endregion

        #region イベント処理
        /// <summary>
        /// ステータステキストの変更イベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStatusTextChanged(EventArgs e)
        {
            StatusMessageUpdateEventArgs eventArgs = new StatusMessageUpdateEventArgs();
            eventArgs.Message = this.StatusText;

            new StatusMessageUpdateCommand().Execute(new NonState(typeof(Application)), eventArgs);

            base.OnStatusTextChanged(e);
        }

        /// <summary>
        /// 表示するページのパスが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e"></param>
        private void OnPagePathChanged(EventArgs e) {
            if (String.IsNullOrEmpty(this.PagePath) == true)
            {
                return;
            }

            if (this.PagePath.StartsWith("http://") == true)
            {
                // URL
                Uri uri = null;
                try
                {
                    uri = new Uri(this.PagePath);
                    this.Url = uri;
                }
                catch (Exception ex)
                {
                    // ログに出力
                    var logCommand = new LogCommand();
                    logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, this.PagePath);
                }
            }
            else if (this.PagePath.EndsWith(".py") == true)
            {
                // バインドデータを探す。
                Type modelType = ModelType;
                if (modelType == null)
                {
                    modelType = MvcCooperationData.View2Model(this.GetType());
                }
                IModel model = null;
                if (ViewStateData.CurrentThread != null)
                {
                    foreach (Object obj in ViewStateData.CurrentThread.Items.Values)
                    {
                        if (obj.GetType() == modelType)
                        {
                            model = obj as IModel;
                            break;
                        }
                    }
                }

                // python ファイル
                if (File.Exists(this.PagePath) == true)
                {
                    var pythonEngine = Python.CreateEngine();
                    var scope = pythonEngine.CreateScope();
                    scope.SetVariable("argument", model);

                    string parentPath = Path.GetDirectoryName(this.PagePath);
                    scope.SetVariable("ParentPath", parentPath.Replace("\\", "/").Replace(":/", "://"));
                    string webRootPath = Path.GetDirectoryName(parentPath);
                    scope.SetVariable("WebRootPath", webRootPath.Replace("\\", "/").Replace(":/", "://"));

                    pythonEngine.ExecuteFile(this.PagePath, scope);
                    var documentText = scope.GetVariable<string>("result");

                    this.DocumentText = documentText;
                }
            }
            else
            {
                // その他のファイル
                if (File.Exists(this.PagePath) == true)
                {
                    try
                    {
                        string parentPath = Path.GetDirectoryName(this.PagePath);
                        string webRootPath = Path.GetDirectoryName(parentPath);

                        string documentText = File.ReadAllText(this.PagePath);
                        documentText = documentText.Replace("<head>", "<head>" + Environment.NewLine + "<base href=\"file:///" + webRootPath.Replace("\\", "/").Replace(":/", "://") + "/\" />");
                        this.DocumentText = documentText;

                        //this.Navigate(this.PagePath);
                    }
                    catch (Exception ex)
                    {
                        // ログに出力
                        var logCommand = new LogCommand();
                        logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, this.PagePath);
                    }
                }
            }
        }

        /// <summary>
        /// コントロールのインスタンスを作成
        /// </summary>
        /// <returns>コントロールのインスタンス</returns>
        protected override Control.ControlCollection CreateControlsInstance()
        {
            var newInstance = base.CreateControlsInstance();
            OnLoad(EventArgs.Empty);

            return newInstance;
        }

        //
        // 概要:
        //     コントロールが初めて表示される前に発生します。
        public event EventHandler Load;

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoad(EventArgs e)
        {
            if (Load != null)
            {
                Load(this, e);
            }

            //InitBindData();
        }
        #endregion

        #region 検索
        /// <summary>
        /// 検索可能かを取得します。
        /// </summary>
        public virtual bool CanSearch { get { return true; } }

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

            UnregistViewEvent();
        }
        #endregion

        #region Javascript
        #region スクリプト
        /// <summary>
        /// 現在読み込んでいる HTML に JavaScript を追加します。
        /// </summary>
        /// <param name="scriptSrc">追加する Javascript のソースコード</param>
        protected void AddScript(string scriptSrc)
        {
            if (this.Document == null || this.Document.Body == null)
            {
                return;
            }

            System.Windows.Forms.HtmlElement el = this.Document.CreateElement("script");
            el.InnerHtml = scriptSrc;
            this.Document.Body.InsertAdjacentElement(System.Windows.Forms.HtmlElementInsertionOrientation.BeforeEnd, el);
        }

        /// <summary>
        /// 関数を実行します。
        /// </summary>
        /// <param name="funcName">関数名</param>
        /// <returns>関数の戻り値</returns>
        protected object Invoke(string funcName)
        {
            return Invoke(funcName, null, null);
        }

        /// <summary>
        /// 関数を実行します。
        /// </summary>
        /// <param name="funcName">関数名</param>
        /// <param name="funcArg"><paramref name="funcName"/> の引数。object[0] が第1引数。</param>
        /// <returns>関数の戻り値</returns>
        protected object Invoke(string funcName, object[] funcArg)
        {
            return Invoke(funcName, funcArg, null);
        }

        /// <summary>
        /// 関数を実行します。
        /// </summary>
        /// <param name="funcName">関数名</param>
        /// <param name="funcArg"><paramref name="funcName"/> の引数。object[0] が第1引数。</param>
        /// <param name="varKeys">戻り値が連想配列の場合は key の配列</param>
        /// <returns>関数の戻り値</returns>
        protected object Invoke(string funcName, object[] funcArg, string[] varKeys)
        {
            if (System.String.IsNullOrEmpty(funcName))
            {
                return null;
            }

            object scriptResult = null;

            if (funcArg == null)
            {
                // 引数なし
                if (funcName.Contains("."))
                {
                    // クラス内関数
                    scriptResult = this.Document.InvokeScript("eval", new object[] { funcName + "();" });
                }
                else
                {
                    // 通常の関数 or 匿名関数
                    scriptResult = this.Document.InvokeScript(funcName);
                }
            }
            else
            {
                // 引数あり
                bool inArr = false;

                foreach (object arg in funcArg)
                {
                    if (arg.GetType().IsArray)
                    {
                        inArr = true;
                        break;
                    }
                }

                if (!funcName.Contains(".") && !inArr)
                {
                    // クラス内関数ではない && 引数に配列なし
                    scriptResult = this.Document.InvokeScript(funcName, funcArg);
                }
                else
                {
                    // それ以外
                    string arg = ConvertArg(funcArg);
                    arg = arg.Substring(1, arg.Length - 2);
                    scriptResult = this.Document.InvokeScript("eval", new object[] { funcName + "(" + arg + ");" });
                }
            }

            return ConvertObject(scriptResult, varKeys);
        }
        #endregion

        #region 変数
        /// <summary>
        /// 変数を取得します。
        /// </summary>
        /// <param name="varName">変数名</param>
        /// <returns>変数の値</returns>
        protected object GetVariable(string varName)
        {
            return GetVariable(varName, null);
        }

        /// <summary>
        /// 変数を取得します。
        /// </summary>
        /// <param name="varName">変数名</param>
        /// <param name="varKeys">戻り値が連想配列の場合は key の配列</param>
        /// <returns>変数の値</returns>
        protected object GetVariable(string varName, string[] varKeys)
        {
            object scriptResult = this.Document.InvokeScript("eval", new object[] { varName });
            return ConvertObject(scriptResult, varKeys);
        }

        /// <summary>
        /// 変数を設定します。
        /// </summary>
        /// <param name="varName">変数名</param>
        /// <param name="varValue">変数の値</param>
        protected void SetVariable(string varName, object varValue)
        {
            this.Document.InvokeScript("eval", new object[] { varName + "=" + ConvertArg(varValue) + ";" });
        }
        #endregion

        #region Javascript C# IF
        /// <summary>
        /// JavaScript に値を返します。JavaScript から C# の関数を実行して、戻り値が配列だった場合に使用します。
        /// </summary>
        /// <param name="varValue">変数</param>
        /// <returns>変換された変数</returns>
        protected object Return(object varValue)
        {
            return (varValue == null || !varValue.GetType().IsArray) ? varValue : this.Document.InvokeScript("eval", new object[] { ConvertArg(varValue) + ";" });
        }

        /// <summary>
        /// 引数を適切な文字列に変換します。
        /// </summary>
        /// <param name="arg">引数</param>
        /// <returns>文字列に変換された引数</returns>        
        protected static string ConvertArg(object arg)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (arg == null)
            {
                sb.Append("undefined");
            }
            else if (arg is System.Collections.Generic.Dictionary<string, object>)
            {
                // 連想配列
                int count = 0;
                System.Collections.Generic.Dictionary<string, object> hashTable = arg as System.Collections.Generic.Dictionary<string, object>;

                sb.Append("{");

                foreach (string key in hashTable.Keys)
                {
                    if (count != 0)
                    {
                        sb.Append(",");
                    }

                    sb.AppendFormat("{0}:{1}", key, ConvertArg(hashTable[key]));

                    count++;
                }

                sb.Append("}");
            }
            else if (arg.GetType().IsArray)
            {
                // 配列
                int count = 0;
                sb.Append("[");

                foreach (object val in (arg as System.Array))
                {
                    if (count != 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(ConvertArg(val));
                    count++;
                }
                sb.Append("]");
            }
            else
            {
                // string や int
                if (arg is System.String)
                {
                    sb.AppendFormat("'{0}'", arg.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "\\'"));
                }
                else
                {
                    sb.Append(arg.ToString());
                }
            }

            return sb.ToString();
        }
        #endregion

        #region 型チェック、変換
        /// <summary>
        /// JavaScript の Object を変換します。
        /// </summary>
        /// <param name="jsObject">JavaScript の object</param>
        /// <param name="jsObjectKeys">連想配列の場合は key の配列</param>
        /// <returns>変換された object</returns>
        protected static object ConvertObject(object jsObject, string[] jsObjectKeys)
        {
            object result = null;

            if (IsComObject(jsObject))
            {
                // ComObjectだった場合
                if (IsJsArray(jsObject))
                {
                    // 配列だった場合
                    object[] objArray = new object[(int)GetProperty(jsObject, "length", null)];

                    for (int i = 0; i < objArray.Length; i++)
                    {
                        objArray[i] = GetProperty(jsObject, i.ToString(), null);
                        objArray[i] = ConvertObject(objArray[i], jsObjectKeys);
                    }

                    result = objArray;
                }
                else if (jsObjectKeys != null)
                {
                    // 連想配列だった場合
                    System.Collections.Generic.Dictionary<string, object> objDic = new System.Collections.Generic.Dictionary<string, object>();

                    for (int i = 0; i < jsObjectKeys.Length; i++)
                    {
                        objDic[jsObjectKeys[i]] = GetProperty(jsObject, jsObjectKeys[i], null);
                        objDic[jsObjectKeys[i]] = ConvertObject(objDic[jsObjectKeys[i]], jsObjectKeys);
                    }

                    result = objDic;
                }
                else
                {
                    // こ…これ…これは………… ComObject だあああああ┗(^o^)┛ｗｗｗｗｗ┏(^o^)┓ﾄﾞｺﾄﾞｺﾄﾞｺﾄﾞｺｗｗｗｗｗ
                    result = jsObject;
                }
            }
            else
            {
                // string や int などだった場合
                result = jsObject;
            }

            return result;
        }

        /// <summary>
        /// ComObject かどうか評価します。
        /// </summary>
        /// <param name="jsObject">評価する object</param>
        /// <returns>ComObject の場合は true</returns>
        private static bool IsComObject(object jsObject)
        {
            return jsObject.GetType().IsCOMObject;
        }

        /// <summary>
        /// JavaScript の配列かどうか評価します。
        /// </summary>
        /// <param name="jsObject">評価する object</param>
        /// <returns>配列の場合は true</returns>
        private static bool IsJsArray(object jsObject)
        {
            int length = 0;

            if (jsObject != null && IsComObject(jsObject))
            {
                // これはひどい
                try
                {
                    length = (int)GetProperty(jsObject, "length", null);
                }
                catch
                {
                    length = 0;
                }
            }

            return (length > 0);
        }

        /// <summary>
        /// 指定したプロパティを取得します。
        /// </summary>
        /// <param name="jsObject">プロパティを取得する object</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="args">呼び出すメンバに渡される引数を格納する配列</param>
        /// <returns>取得したプロパティの値</returns>
        private static object GetProperty(object jsObject, string propertyName, object[] args)
        {
            return jsObject.GetType().InvokeMember(propertyName, System.Reflection.BindingFlags.GetProperty, null, jsObject, args);
        }
        #endregion
        #endregion
    }
}
