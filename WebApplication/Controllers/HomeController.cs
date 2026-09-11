using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.DB;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            HomeVM vm = new HomeVM();

            vm.TotalOrderCount = db.Siparis.Count();
            vm.ActiveTotalCustomerCount = db.Musteri.Count(x => x.AktifMi == true);
            vm.KritikUrunSayisi = db.StokTablo.Count(x => x.StokAlarm == true);
            vm.Son10Siparis = db.Siparis.OrderByDescending(x => x.ID).Take(10).ToList();
            vm.KritikUrunListe = db.StokTablo.Where(x => x.StokAlarm == true).ToList();

            return View(vm);
        }

        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Giris(sorgu s)
        {
            ResultJSVM result = new ResultJSVM();

            List<Admin> dbResult = db.Admin.Where(x => x.UserName == s.kullanici && x.Password == s.sifre).ToList();
            
            if (dbResult.Count > 0) {
                Admin son= dbResult.Where(x => x.AktifMi == true).FirstOrDefault();
                if (son != null)
                {
                    result.Status = true;
                    result.Title = "Başarılı";
                    result.Message = "İşlem başarılı";
                }
                else
                {
                    result.Status =false;
                    result.Title = "Başarısız";
                    result.Message = "Kullanıcı Aktif Değil Yönetici İle İletişime Geçin";
                }

            }
            else
            {
                result.Status = false;
                result.Title = "Başarısız";
                result.Message = "Şifre ya da kullanıcı adı hatalı";
            }

            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}