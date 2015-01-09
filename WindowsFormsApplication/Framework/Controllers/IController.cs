using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication.Views;
using Kizuna.Plus.WinMvcForm.Framework.Views;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers
{
    /// <summary>
    /// コントローラインターフェース
    /// </summary>
    public interface IController : IDisposable
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        void Initialize();

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>表示画面</returns>
        IView Index();
    }
}
