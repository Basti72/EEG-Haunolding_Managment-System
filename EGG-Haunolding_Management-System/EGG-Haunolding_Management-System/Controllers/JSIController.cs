using Microsoft.AspNetCore.Mvc;
using Jint;
using Jint.Native;
using Jint.Runtime.Interop;
using EGG_Haunolding_Management_System.Models;
using EGG_Haunolding_Management_System.Class;
using Jint.Native.Object;

namespace EGG_Haunolding_Management_System.Controllers
{
    public class JSIController : Controller
    {
        private readonly IDataStore m_DataStore;
        public JSIController(IDataStore dataStore)
        {
            m_DataStore = dataStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ExecuteJavaScript(JSIViewModel model)
        {
            var engine = new Engine(cfg => cfg.AllowClr());
            try
            {
                var result = engine.Execute(model.Code).GetCompletionValue().ToString();
                ViewBag.Result = result;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(nameof(Index));
        }
    }
}
