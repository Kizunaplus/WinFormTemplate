using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication.Controllers.State;
using WindowsFormsApplication.Models;
using WindowsFormsApplication.Models.EventArg;
using WindowsFormsApplication.Services.Log;

namespace WindowsFormsApplication.Controllers.Commands
{
    /// <summary>
    /// コマンド処理イベント登録クラス
    /// </summary>
    class CommandRegister
    {
        #region メンバー変数
        /// <summary>
        /// 現在のインスタンス
        /// </summary>
        private static CommandRegister current;

        /// <summary>
        /// イベント割付リスト
        /// </summary>
        private List<CommandEventData> evnetRegisterList;
        #endregion

        #region プロパティ
        /// <summary>
        /// 現在のインスタンス
        /// </summary>
        public static CommandRegister Current
        {
            get
            {
                if (current == null)
                {
                    current = new CommandRegister();
                }

                return current;
            }
            set
            {
                current = value;
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandRegister()
        {
            evnetRegisterList = new List<CommandEventData>();

            Initialize();
        }

        /// <summary>
        /// イベントの初期化処理
        /// </summary>
        protected void Initialize()
        {
            lock (this.evnetRegisterList)
            {
#if ENABLE_LOGGER
                // ログ出力処理(エラー)
                var eventData = new CommandEventData(typeof(Application), typeof(LogCommand), StateMode.Error, delegate(object sender, EventArgs args)
                {
                    var threadExceptionArgs = args as System.Threading.ThreadExceptionEventArgs;
                    if (threadExceptionArgs != null)
                    {
                        // 未Catch例外
                        LogFactory.Fatal(threadExceptionArgs.Exception);
                    }
                    var exceptionEventArgs = args as ExceptionEventArgs;
                    if (exceptionEventArgs != null)
                    {
                        // 通常例外
                        LogFactory.Fatal(exceptionEventArgs.Exception);
                    }

#if DEBUG
                    if (exceptionEventArgs != null)
                    {
                        // デバッグ(通知エリアに例外情報を表示)
                        NortifyMessageEventArgs eventArgs = new NortifyMessageEventArgs();
                        eventArgs.Icon = ToolTipIcon.Error;
                        eventArgs.Title = "例外";
                        eventArgs.Message = exceptionEventArgs.Exception.ToString();

                        NortifyMessageCommand command = new NortifyMessageCommand();
                        command.Execute(new NonState(typeof(Application)), eventArgs);
                    }
#endif
                });
                this.evnetRegisterList.Add(eventData);

                // ログ出力処理(デバッグ)
                eventData = new CommandEventData(typeof(Application), typeof(LogCommand), StateMode.Log, delegate(object sender, EventArgs args)
                {
                    var exceptionEventArgs = args as ExceptionEventArgs;
                    if (exceptionEventArgs != null)
                    {
                        // デバッグメッセージ
                        LogFactory.Debug(exceptionEventArgs.Exception.Message);
                    }
                    var messageEventArgs = args as LogMessageEventArgs;
                    if (messageEventArgs != null)
                    {
                        // デバッグメッセージ
                        LogFactory.Debug(messageEventArgs.Message);
                    }
                });
                this.evnetRegisterList.Add(eventData);
#endif
            }
        }
        #endregion

        #region 登録
        /// <summary>
        /// イベントの登録
        /// </summary>
        /// <param name="eventData">イベントデータ</param>
        /// <returns>true: 成功, false: 失敗</returns>
        public bool Regist(CommandEventData eventData)
        {
            lock (this.evnetRegisterList)
            {
                evnetRegisterList.Add(eventData);
            }

            return true;
        }
        #endregion

        #region 解除
        /// <summary>
        /// イベントの解除
        /// </summary>
        /// <param name="eventData">イベントデータ</param>
        /// <returns>true: 成功, false: 失敗</returns>
        public bool Unregist(CommandEventData eventData)
        {
            lock (this.evnetRegisterList)
            {
                evnetRegisterList.Remove(eventData);
            }

            return true;
        }

        /// <summary>
        /// イベントの解除
        /// 指定したイベント元のイベントを全て削除
        /// </summary>
        /// <param name="source">イベント元</param>
        /// <returns>true: 成功, false: 失敗</returns>
        public bool UnregistOfSource(object source)
        {
            lock (this.evnetRegisterList)
            {
                for (int listIndex = this.evnetRegisterList.Count - 1; 0 <= listIndex; listIndex--)
                {
                    var eventData = this.evnetRegisterList[listIndex];
                    if (eventData.Source == source)
                    {
                        this.evnetRegisterList.RemoveAt(listIndex);
                    }
                }
            }

            return true;
        }
        #endregion

        #region コマンド実行
        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="state">状態</param>
        /// <param name="args">イベントハンドラ引数</param>
        /// <returns>true: 成功, false: 失敗</returns>
        public bool Execute(ICommand command, IState state, EventArgs args)
        {
            object source = state.GetSource();
            Type sourceType = typeof(object);
            if (source != null)
            {
                // イベント発生元のデータ型を取得
                sourceType = source.GetType();
            }
            StateMode mode = state.GetMode();

#if DEBUG
            // ログに出力　(コマンド実行)
            if (command is LogCommand == false)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(new DebugState(typeof(Application)), new LogMessageEventArgs() { Message = command.GetType().Name + " Execute" });
            }
#endif

            // 対象のすべてのイベントハンドラを実行
            lock (this.evnetRegisterList)
            {
                foreach (var eventData in this.evnetRegisterList)
                {
                    if (eventData.Source != source
                        && (eventData.Source is Type == true && sourceType.IsAssignableFrom((Type)eventData.Source))
                        && source != null)
                    {
                        // イベント対象ではない（定義されているイベント発生元とは異なる）
                        // イベント発生元がnull の場合は、チェックしない
                        continue;
                    }

                    if (eventData.CommandType != command.GetType()
                        && eventData.CommandType != null)
                    {
                        // イベント対象ではない（定義されているコマンドと異なる）
                        // コマンド未指定(null) の場合は、チェックしない
                        continue;
                    }

                    if (eventData.Mode != mode && mode != StateMode.None)
                    {
                        // 対象のモードが異なる
                        // モード未指定(None) の場合は、チェックしない
                        continue;
                    }

                    var handler = eventData.Handler;
                    if (handler == null)
                    {
                        // イベントハンドラが存在しない
                        continue;
                    }

                    // イベントの呼び出し出し
                    handler(source, args);
                }
            }

            return true;
        }
        #endregion
    }
}
