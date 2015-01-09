using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication.Models;
using Kizuna.Plus.WinMvcForm.Framework.Services;

namespace WindowsFormsApplication.Services
{
    interface IDemoService : IService
    {
        DemoModel GetData();
    }
}
