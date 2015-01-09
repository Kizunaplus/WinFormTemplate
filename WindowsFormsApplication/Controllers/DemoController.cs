using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Views;
using WindowsFormsApplication.Services;

namespace WindowsFormsApplication.Controllers
{
    /// <summary>
    /// コントローラサンプル
    /// デフォルトビューを呼び出す。
    /// </summary>
    class DemoController : Controller
    {
        /// <summary>
        /// デモサービス
        /// </summary>
        [Inject]
        IDemoService demoService;

        public override IView Index()
        {
            ViewStateData.CurrentThread.Items["Model"] = demoService.GetData();

            return base.Index();
        }

        public IView Index2()
        {
            return GetView("Demo2");
        }
    }
}
