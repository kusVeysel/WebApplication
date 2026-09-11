using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.DB;
using WebApplication.Models;
using static WebApplication.Models.Setup;
namespace WebApplication.Controllers
{
    public class SetupController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

		#region Boyut İşlemleri

        public ActionResult SizeIndex()
        {
            List<Boyut> boyutListe = db.Boyut.ToList();
            return View(boyutListe);
        }

        public ActionResult SizeUpdate(int? ID)
        {
            if (ID == null)
            {
                return Redirect("/Setup/SizeIndex");
            }
            Boyut boyut = db.Boyut.Where(x => x.ID == ID).FirstOrDefault();
            if (boyut == null)
            { 
                return Redirect("/Setup/SizeIndex");
            }
            return View(boyut);
        }
        [HttpPost]
        public JsonResult SizeUpdate(SizeVM s)
        {
            
            ResultJSVM result = new ResultJSVM();
            Boyut boyut = db.Boyut.Where(x => x.ID == s.ID).FirstOrDefault();
            if(boyut == null)
            {
                result.Title = "Hata";
                result.Status = false;
                result.Message = "Boyut bulunamadı";
                return Json(result,JsonRequestBehavior.AllowGet);
            }

            boyut.BoyutAdi = s.SizeName;
            db.SaveChanges();
            result.Title = "Başarılı";
            result.Status = true;
            result.Message = "Boyut güncellendi";

            return Json(result,JsonRequestBehavior.AllowGet);

        }
        public ActionResult SizeAdd()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SizeAdd(SizeVM s)
        {
            ResultJSVM result = new ResultJSVM();
            Boyut boyut = new Boyut();
            try
            {
                boyut.BoyutAdi = s.SizeName;
                db.Boyut.Add(boyut);
                db.SaveChanges();
                result.Title = "Başarılı";
                result.Status = true;
                result.Message = "Boyut eklendi";

            }
            catch (Exception ex)
            {
                result.Title = "Hata";
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Kategori İşlemleri
        public ActionResult CategoryIndex()
        {
            return View(db.Kategori.ToList());
        }

        public ActionResult CategoryUpdate(int? ID)
        {
            if (ID == null)
            {
                return Redirect("/Setup/CategoryIndex");
            }
            Kategori kategori = db.Kategori.Where(x => x.ID == ID).FirstOrDefault();
            if (kategori == null)
            {
                return Redirect("/Setup/CategoryIndex");
            }
            return View(kategori);
        }
        [HttpPost]
        public JsonResult CategoryUpdate(CategoryVM c)
        {
            ResultJSVM result = new ResultJSVM();
            Kategori kategori = db.Kategori.Where(x => x.ID == c.ID).FirstOrDefault();
            if (kategori == null)
            {
                result.Title = "Hata";
                result.Status = false;
                result.Message = "Kategori bulunamadı";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            kategori.KategoriAdi = c.CategoryName;
            db.SaveChanges();
            result.Title = "Başarılı";
            result.Status = true;
            result.Message = "Kategori güncellendi";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategoryAdd()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CategoryAdd(CategoryVM c)
        {
            ResultJSVM result = new ResultJSVM();
            Kategori kategori = new Kategori();
            try
            {
                kategori.KategoriAdi = c.CategoryName;
                db.Kategori.Add(kategori);
                db.SaveChanges();
                result.Title = "Başarılı";
                result.Status = true;
                result.Message = "Kategori eklendi";
            }
            catch (Exception ex)
            {
                result.Title = "Hata";
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Materyal İşlemleri
        public ActionResult MaterialIndex()
        {
            return View(db.Materyal.ToList());
        }
        public ActionResult MaterialUpdate(int? ID)
        {
            if (ID == null)
            {
                return Redirect("/Setup/MaterialIndex");
            }
            Materyal materyal = db.Materyal.Where(x => x.ID == ID).FirstOrDefault();
            if (materyal == null)
            {
                return Redirect("/Setup/MaterialIndex");
            }
            return View(materyal);
        }
        [HttpPost]
        public JsonResult MaterialUpdate(MaterialVM m)
        {
            ResultJSVM result = new ResultJSVM();
            Materyal materyal = db.Materyal.Where(x => x.ID == m.ID).FirstOrDefault();
            if (materyal == null)
            {
                result.Title = "Hata";
                result.Status = false;
                result.Message = "Materyal bulunamadı";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            materyal.MateryalAdi = m.MaterialName;
            db.SaveChanges();
            result.Title = "Başarılı";
            result.Status = true;
            result.Message = "Materyal güncellendi";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaterialAdd()
        {
            return View();
        }
        [HttpPost]
        public JsonResult MaterialAdd(MaterialVM m)
        {
            ResultJSVM result = new ResultJSVM();
            Materyal materyal = new Materyal();
            try
            {
                materyal.MateryalAdi = m.MaterialName;
                db.Materyal.Add(materyal);
                db.SaveChanges();
                result.Title = "Başarılı";
                result.Status = true;
                result.Message = "Materyal eklendi";
            }
            catch (Exception ex)
            {
                result.Title = "Hata";
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Ödeme Tipi İşlemleri
        public ActionResult PaymentTypeIndex()
        {
            return View(db.OdemeTipi.ToList());
        }
        #endregion

    }
}