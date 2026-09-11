using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.DB;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        public ActionResult Index()
        {
            List<Musteri> mList = db.Musteri.ToList();
            List<CustomerDTO> cList = new List<CustomerDTO>();

            foreach (var musteri in mList)
            {
                CustomerDTO dto = new CustomerDTO()
                {
                    ID = musteri.ID,
                    CustomerName = musteri.MusteriFirma,
                    Email = musteri.Email,
                    Phone = musteri.Telefon,
                    IsActive = (bool)musteri.AktifMi
                };

                cList.Add(dto);
            }


            return View(cList);
        }

        public JsonResult CustomerStaUpdate(int ID)
        {
            Musteri dbMusteri = db.Musteri.Find(ID);
            ResultJSVM result = new ResultJSVM();
            if (dbMusteri == null)
            {
                result.Title = "Hata";
                result.Message = "Müşteri bulunamadı";
                result.Status = false;
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            if (dbMusteri.AktifMi == true)
            {
                dbMusteri.AktifMi = false;
                result.Title = "Başarılı";
                result.Message = "Müşteri pasif hale getirildi";
                result.Status = true;
            }
            else
            {
                dbMusteri.AktifMi = true;
                result.Title = "Başarılı";
                result.Message = "Müşteri aktif hale getirildi";
                result.Status = true;
            }
            db.SaveChanges();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CustomerUpdate(int? ID)
        {
            if (ID == null)
            {
                return Redirect("/Customer/Index");
            }

            Musteri dbResult = db.Musteri.Find(ID);

            if (dbResult == null)
            {
                return Redirect("/Customer/Index");
            }

            return View(dbResult);
        }

        [HttpPost]
        public JsonResult CustomerUpdate(CustomerDTO m)
        {
            Musteri dbResult = db.Musteri.Find(m.ID);
            ResultJSVM result = new ResultJSVM();

            if (dbResult == null)
            {
                result.Title = "Hata";
                result.Message = "Müşteri bulunamadı";
                result.Status = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            dbResult.MusteriFirma = m.CustomerName;
            dbResult.Email = m.Email;
            dbResult.Telefon = m.Phone;
            dbResult.Adres = m.adres;
            dbResult.YetkiliAdSoyad = m.yetkili;
       
            db.SaveChanges();

            result.Title = "Başarılı";
            result.Message = "Müşteri bilgileri güncellendi";
            result.Status = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerAdd()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CustomerAdd(CustomerInsert m)
        {
            ResultJSVM result = new ResultJSVM();

            try
            {
                Musteri yeniMusteri = new Musteri()
                {
                    MusteriFirma = m.CustomerName,
                    Email = m.Email,
                    Telefon = m.Phone,
                    Adres = m.adres,
                    YetkiliAdSoyad = m.yetkili
                };

                db.Musteri.Add(yeniMusteri);
                db.SaveChanges();

                result.Title = "Başarılı";
                result.Message = "Müşteri eklendi";
                result.Status = true;

            }
            catch (Exception ex)
            {
                result.Title = "Hata";
                result.Message = ex.Message;
                result.Status = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}