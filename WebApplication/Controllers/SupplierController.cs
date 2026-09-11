using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.DB;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class SupplierController : BaseController
	{
		// GET: Supplier
		public ActionResult Index()
		{
			List<Tedarikci> tedList = db.Tedarikci.ToList();
			List<SupplierDTO> supList = new List<SupplierDTO>();

			foreach (var item in tedList)
			{
				SupplierDTO dto = new SupplierDTO()
				{
					ID = item.ID,
					Phone = item.Telefon,
					Email = item.Email,
					Name = item.TedarikciAdi,
					IsActive = (bool)item.AktifMi

				};
				supList.Add(dto);
			}

			return View(supList);
		}

		public JsonResult SupplierStaUpdate(int ID)
		{
			Tedarikci dbResult = db.Tedarikci.Find(ID);
			ResultJSVM result = new ResultJSVM();

            if (dbResult == null)
			{
				result.Message = "Tedarikçi bulunamadı";
				result.Status = false;
                result.Title = "Hata";
                return Json(result, JsonRequestBehavior.AllowGet);
			}

			if (dbResult.AktifMi == true)
			{
				result.Message = "Tedarikçi pasif hale getirildi";
				result.Status = true;
                result.Title = "Başarılı";
                dbResult.AktifMi = false;
			}
			else
			{
				result.Message = "Tedarikçi aktif hale getirildi";
				result.Status = true;
				result.Title = "Başarılı";
                dbResult.AktifMi = true;
			}
			db.SaveChanges();
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult SupplierUpdate(int? ID)
		{
			if (ID == null) {
                return Redirect("/Supplier/Index");
            }

			Tedarikci dbResult = db.Tedarikci.Find(ID);
			if (dbResult == null) {
                return Redirect("/Supplier/Index");
            }

            return View(dbResult);
        }
		[HttpPost]
		public JsonResult SupplierUpdate(SupplierDTO sDTO) { 
		ResultJSVM result = new ResultJSVM();
        Tedarikci td = db.Tedarikci.Find(sDTO.ID);
			try
			{
				td.TedarikciAdi = sDTO.Name;
                td.Email = sDTO.Email;
                td.Telefon = sDTO.Phone;
                db.SaveChanges();
                result.Message = "Tedarikçi başarıyla güncellendi";
				result.Status = true;
				result.Title= "Başarılı";

            }
			catch (Exception ex)
			{
				result.Message = ex.Message;
				result.Status = false;
				result.Title= "Hata";
            }

            return Json(result,JsonRequestBehavior.AllowGet);
        }
		public ActionResult SupplierInsert()
		{
			return View();
		}

		[HttpPost]
        public JsonResult SupplierInsert(SupplierDTO sDTO)
		{
			ResultJSVM result = new ResultJSVM();
            try
            {
                Tedarikci td = new Tedarikci()
                {
                    TedarikciAdi = sDTO.Name,
                    Email = sDTO.Email,
                    Telefon = sDTO.Phone,
                    AktifMi = true
                };
                db.Tedarikci.Add(td);
                db.SaveChanges();
                result.Message = "Tedarikçi başarıyla eklendi";
                result.Status = true;
                result.Title = "Başarılı";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = false;
                result.Title = "Hata";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}