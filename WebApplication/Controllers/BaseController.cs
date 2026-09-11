using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.DB;

namespace WebApplication.Controllers
{
    public class BaseController : Controller
    {
        protected VeyselEntities db = new VeyselEntities();
    }
}