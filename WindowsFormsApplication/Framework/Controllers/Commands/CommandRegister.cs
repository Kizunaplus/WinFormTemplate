using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Logger;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands
{
    /// <summary>
    /// コマンド処理イベント登録クラス
    /// 
    /// このクラスがコマンドの実行状態を管理します。
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

            // 定番のイベントリスナーを登録
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
                    var exceptionEventArgs = args as ExceptionEventArgs;
                    if (exceptionEventArgs != null)
                    {
                        // 通常例外
                        LogFactory.Fatal(exceptionEventArgs.Message, exceptionEventArgs.Exception);
                    }

#if DEBUG
                    if (exceptionEventArgs != null)
                    {
                        // デバッグ(通知エリアに例外情報を表示)
                        NortifyMessageEventArgs eventArgs = new NortifyMessageEventArgs();
                        eventArgs.Icon = ToolTipIcon.Error;
                        eventArgs.Title = FrameworkDebugMessage.ExceptionBalloonTitle;
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
                // イベントリスナーリストに登録
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
                // イベントリスナーから削除
                evnetRegisterList.Remove(eventData);
                eventData.Dispose();
                eventData = null;
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
                        // イベントリスナーから削除
                        this.evnetRegisterList.RemoveAt(listIndex);
                        eventData.Dispose();
                        eventData = null;
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
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.CommandStartMessage, command.GetType().FullName, args);
            }
#endif

            // 対象のすべてのイベントハンドラを実行
            lock (this.evnetRegisterList)
            {
                var eventList = new List<CommandEventData>(this.evnetRegisterList);
                foreach (var eventData in eventList)
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

                    // イベントの呼び出し
                    handler(source, args);
                }
            }

            return true;
        }
        #endregion
    }
}
