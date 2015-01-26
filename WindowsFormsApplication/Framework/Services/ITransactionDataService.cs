using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    interface ITransactionDataService : IService
    {
        /// <summary>
        /// ロールバック予約設定
        /// ロールバック予約をした場合、TransactionInterceptorでロールバック処理を行います。
        /// </summary>
        bool RollbackReserve
        {
            get;
            set;
        }
    }
}
