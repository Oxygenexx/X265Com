using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x265Com.ScriptCS.FileSelection;

namespace x265Com.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult TestOpenFileDialog()
        {
            FileSelectionHelper.OpenFileDialog_Click();
            return View("Index");
        }
    }
}