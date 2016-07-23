using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Services;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    class TransactionData : ITransactionData
    {
        /// <summary>
        /// ロールバック予約設定
        /// ロールバック予約をした場合、TransactionInterceptorでロールバック処理を行います。
        /// </summary>
        public bool RollbackReserve
        {
            get;
            set;
        }
    }
}
