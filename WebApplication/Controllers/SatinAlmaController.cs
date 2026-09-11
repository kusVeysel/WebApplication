using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.DB;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class SatinAlmaController : BaseController
	{
		// GET: SatinAlma
		public ActionResult Index()
		{
			List<SatinAlma> dbSA = db.SatinAlma.ToList();
			List<SatinAlmaUpdateVM> vmList = new List<SatinAlmaUpdateVM>();

			foreach (var item in dbSA)
			{
				SatinAlmaUpdateVM vm = new SatinAlmaUpdateVM();
				vm.urunID = item.ID;
				vm.tedarikciID = item.TedarikciID;
				vm.adet = (int)item.Adet;
				vm.birimFiyat = item.BirimFiyat.ToString();
				vm.toplamFiyat = item.ToplamFiyat.ToString();
				vm.kdvOran = item.KDVOranı.ToString();
				vm.satinAlmaTarih = item.SatınAlmaTarihi.ToString();
				vmList.Add(vm);
			}
			ViewBag.UrunAd = db.Urun.Distinct().ToList();
			ViewBag.teda = db.Tedarikci.Distinct().ToList();
			return View(vmList);
		}

		public ActionResult Insert()
		{
			SatinAlmaInsertVM vm = new SatinAlmaInsertVM();

			vm.UrunListe = db.Urun.ToList();
			vm.TedarikciListe = db.Tedarikci.ToList();

			return View(vm);
		}

		public JsonResult Add(SatinAlmaAddVM vm)
		{
			SatinAlma dbSA = new SatinAlma();
			ResultJSVM result = new ResultJSVM();

			try
			{
				dbSA.UrunID = vm.urunID;
				dbSA.TedarikciID = vm.tedarikciID;
				dbSA.KDVOranı = Convert.ToInt32(vm.kdvOran);
				dbSA.Adet = vm.adet;
				dbSA.BirimFiyat = Convert.ToDouble(vm.birimFiyat);
				dbSA.ToplamFiyat = Convert.ToDouble(vm.toplamFiyat);
				dbSA.SatınAlmaTarihi = Convert.ToDateTime(vm.satinAlmaTarih);

				db.SatinAlma.Add(dbSA);
				db.SaveChanges();

				StokTablo updateStock = db.StokTablo.Where(x => x.UrunID == vm.urunID).FirstOrDefault();
				if (updateStock == null)
				{
					StokTablo st = new StokTablo()
					{
						UrunID = dbSA.UrunID,
						Stok = vm.adet,
						StokAlarm = false,
						AlarmSeviyesi = 5
					};
					db.StokTablo.Add(st);
					db.SaveChanges();
				}
				else
				{
					updateStock.Stok += vm.adet;
					db.SaveChanges();
				}
				result.Message = "Ürün Ekleme İşlemi Başarılı";
				result.Status = true;
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
				result.Status = false;
			}


			return Json(result, JsonRequestBehavior.AllowGet);
		}
		public ActionResult Update(int? ID)
		{
			if (ID == null)
			{
				return Redirect("/SatinAlma/Index");
			}

			SatinAlma s = db.SatinAlma.Find(ID);

			if (s == null)
			{
				return Redirect("/SatinAlma/Index");
			}

			return View(s);
		}
		[HttpPost]
		public JsonResult Update(SatinAlmaUpdateVM vm)
		{
			SatinAlma dbSA = db.SatinAlma.Find(vm.urunID);
			ResultJSVM result = new ResultJSVM();
			try
			{
				dbSA.KDVOranı = Convert.ToInt32(vm.kdvOran);
				dbSA.Adet = vm.adet;
				dbSA.BirimFiyat = Convert.ToDouble(vm.birimFiyat);
				dbSA.SatınAlmaTarihi = Convert.ToDateTime(vm.satinAlmaTarih);
				db.SaveChanges();
				result.Message = "Ürün Güncelleme İşlemi Başarılı";
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