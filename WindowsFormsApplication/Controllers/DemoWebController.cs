using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using Kizuna.Plus.WinMvcForm.Framework.Views;
using Kizuna.Plus.WinMvcForm.Framework.Models;

namespace WindowsFormsApplication.Controllers
{
    /// <summary>
    /// コントローラサンプル
    /// デフォルトビューを呼び出す。
    /// </summary>
    class DemoWebController : Controller
    {
        public override IView Index()
        {
            return base.Index();
        }

        public IView Index2()
        {
            return base.Index();
        }
    }
}
