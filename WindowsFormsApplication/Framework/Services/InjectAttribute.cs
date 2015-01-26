using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using System.Reflection;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    /// <summary>
    /// サービスのインジェクション属性
    /// </summary>
    class InjectAttribute : Attribute
    {
        /// <summary>
        /// 対象オブジェクトにサービスをインジェクションします。
        /// </summary>
        /// <typeparam name="T">サービスの型</typeparam>
        /// <param name="target">インジェクション対象のインスタンス</param>
        /// <param name="field">対象のフィールド</param>
        public static void InjectService<T>(Object target, FieldInfo field) where T : class
        {
            T service = (T)ServicePool.Current.GetService(field.Name);
            if (service == null)
            {
                // 存在しないサービス
                var logCommand = new LogCommand();
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.InjectServiceNotFoundServiceMessage, target, field);

                return;
            }

            // サービスの設定
            field.SetValue(target, service);
        }
    }
}
