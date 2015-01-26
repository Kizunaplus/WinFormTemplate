using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Services;

namespace WindowsFormsApplication.Framework.Services
{
    [ServiceAttribute("transaction")]
    class TransactionDataService : ITransactionDataService
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
