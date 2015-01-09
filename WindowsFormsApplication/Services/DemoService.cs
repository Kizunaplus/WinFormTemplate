using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication.Models;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Framework.Services.Interceptor;

namespace WindowsFormsApplication.Services
{
    [ServiceAttribute("demoService")]
    class DemoService : IDemoService
    {
        [TransactionInterceptor]
        [ExceptionInterceptor]
        [JournalInterceptor]
        public DemoModel GetData()
        {
            DemoModel data = new DemoModel();
            data.InfoMessage = "Demo Message";

            return data;
        }
    }
}
