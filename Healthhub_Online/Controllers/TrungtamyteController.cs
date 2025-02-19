using Healthhub_Online.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Healthhub_Online.Controllers
{
    public class TrungtamyteController : Controller
    {
        ModelWeb db = new ModelWeb();
        [HttpGet]
        public ActionResult TraCuu()
        {
            return View();
        }


    }


}