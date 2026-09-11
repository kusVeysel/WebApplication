using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.DB;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class ProductController : BaseController
	{
		// GET: Product
		public ActionResult Index()
		{
			List<ProductListVM> vmList = new List<ProductListVM>();
			List<Urun> dbResult = db.Urun.ToList();

			foreach (var item in dbResult)
			{
				ProductListVM vm = new ProductListVM()
				{
					ID = item.ID,
					UrunAdi = item.UrunAdi,
					BoyutAdi = item.Boyut.BoyutAdi.ToString(),
					MateryalAdi = item.Materyal.MateryalAdi,
					Durum = (bool)item.AktifMi,

				};
				var stUrun = db.StokTablo.Where(x => x.UrunID == item.ID).FirstOrDefault();

				if (stUrun == null)
				{
					vm.Stok = 0;
					vm.StokAlarm = true;
				}
				else
				{
					vm.Stok = (int)item.StokTablo.Sum(x => x.Stok);
					//Stok = (int)db.StokTablo.Where(x => x.UrunID == item.ID).ToList().Sum(x => x.Stok),
					vm.StokAlarm = (bool)db.StokTablo.Where(x => x.UrunID == item.ID).FirstOrDefault().StokAlarm;
				}

				vmList.Add(vm);
			}


			return View(vmList);
		}

		public ActionResult ProductInsert()
		{
			ViewBag.MaterialList = db.Materyal.Distinct().ToList();
			ViewBag.SizeList = db.Boyut.Distinct().ToList();
			return View();
		}

		public ActionResult ProductAdd(ProductInsertVM vm)
		{
			Urun urun = new Urun();

			urun.UrunAdi = vm.productName;
			urun.Aciklama = vm.description;
			urun.MateryalID = vm.materialInfo;
			urun.BoyutID = vm.sizeInfo;
			urun.AktifMi = vm.isActive;

			db.Urun.Add(urun);
			db.SaveChanges();

			StokTablo st = new StokTablo()
			{
				UrunID = urun.ID,
				Stok = 0,
				StokAlarm = false,
				AlarmSeviyesi = 5
			};

			db.StokTablo.Add(st);
			db.SaveChanges();


			return Redirect("/Product/Index");

		}

		public JsonResult ProductStaUpdate(int ID)
		{
			Urun dbResult = db.Urun.Find(ID); //Veritabanındaki Urun tablosundan parametre olarak gelen ID'ye sahip ürünü bulur
			ResultJSVM result = new ResultJSVM(); 

            if (dbResult == null)
			{
				result.Message = "Ürün Bulunamadı!";
				result.Status = false;
				result.Title = "Hata";
				return Json(result, JsonRequestBehavior.AllowGet);
			}

			if (dbResult.AktifMi == true)
			{
				dbResult.AktifMi = false;
				result.Message = "Ürün Pasif Yapıldı!";
				result.Status = false;
				result.Title = "İşlem Başarılı";
			}
			else
			{
				dbResult.AktifMi = true;
				result.Message = "Ürün Aktif Yapıldı!";
				result.Status = true;
				result.Title = "İşlem Başarılı";
			}

			db.SaveChanges();

			return Json(result, JsonRequestBehavior.AllowGet); // resultu json formatında js'ye döndürür. JsonRequestBehavior.AllowGet, get isteklerine izin verir. Post isteklerinde bu parametreye gerek yoktur

        }
		[HttpGet] // yazmak zorunda değiliz çünkü default olarak get isteklerini kabul eder. Ancak aynı isimde birden fazla parametreli method olduğundan farkını ayırmak için HttpGet attribute'u kullanılır. Bu method sadece get isteklerinde çalışır
        public ActionResult Update(int? ID)
		{
			if (ID == null)
			{
				return Redirect("/Product/Index");
			}

			Urun dbResult = db.Urun.Find(ID);

			if (dbResult == null)
			{
				return Redirect("/Product/Index");
			}
			ViewBag.SizeList = db.Boyut.ToList();
			ViewBag.MaterialList = db.Materyal.ToList();
			return View(dbResult);
		}

		[HttpPost] // Aynı isimde birden fazla parametreli method olduğundan farkını ayırmak için HttpPost attribute'u kullanılır. Bu method sadece post isteklerinde çalışır
        public JsonResult Update(ProductInsertVM vm)
		{
			Urun dbResult = db.Urun.Find(vm.ID);
			ResultJSVM result = new ResultJSVM();
			try
			{
				dbResult.UrunAdi = vm.productName;
				dbResult.AktifMi = vm.isActive;
				dbResult.MateryalID = vm.materialInfo;
				dbResult.BoyutID = vm.sizeInfo;
				dbResult.Aciklama = vm.description;

				db.SaveChanges();
				result.Title = "Başarılı";
				result.Message = dbResult.UrunAdi + " İsimli Ürün Güncellendi...";
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


		public JsonResult GetProductInfo(int ID)
		{
			StokTablo dbResult = db.StokTablo.Where(x => x.UrunID == ID).FirstOrDefault(); // StokTablo ForeginKey olduğu için find değil where ile arama yapıyoruz. UrunID'ye göre stok bilgisini getiririz. FirstOrDefault ile de tek bir sonuç döndürürüz
            ProductStockInfoJSVM vm = new ProductStockInfoJSVM();
		
			if (dbResult == null)
			{
				vm.ProductID = ID; // null olduğu için parametre olarak gelen ID'yi atıyoruz
                vm.ProductName = db.Urun.Find(ID).UrunAdi; // Urun tablosundan parametre olarak gelen ID'ye sahip ürünü bulur ve ürün adını atarız
            }
			else
			{
				vm.ID = dbResult.ID;
				vm.ProductID = (int)dbResult.UrunID;
				vm.ProductName = dbResult.Urun.UrunAdi;
				vm.Stock = (int)dbResult.Stok;
				vm.AlarmStock = (int)dbResult.AlarmSeviyesi;
			}

			return Json(vm, JsonRequestBehavior.AllowGet);
		}

		public JsonResult StockUpdate(ProductStockInfoJSVM vm)
		{
			StokTablo dbResult = db.StokTablo.Find(vm.ID);
			
			ResultJSVM result = new ResultJSVM();

			if (dbResult == null)
			{
				StokTablo st = new StokTablo()// UrunID'ye göre stok bilgisi olmadığı için yeni bir stok kaydı oluşturuyoruz
                {
					UrunID = vm.ProductID, // parametre olarak gelen ürün ID'sini atıyoruz
                    Stok = vm.Stock, // parametre olarak gelen stok bilgisini atıyoruz
                    AlarmSeviyesi = vm.AlarmStock, // parametre olarak gelen alarm seviyesi bilgisini atıyoruz

                };

				if (vm.Stock > vm.AlarmStock)
				{
					st.StokAlarm = false;
				}
				else
				{
					st.StokAlarm = true;
				}

				db.StokTablo.Add(st); // yeni stok kaydını veritabanına ekliyoruz
                db.SaveChanges();

				result.Title = "Başarılı";
                result.Message = "İşlem Başarılı";
				result.Status = true;
			}
			else
			{
				try
				{
					dbResult.Stok = vm.Stock;
					dbResult.AlarmSeviyesi = vm.AlarmStock;

					if (dbResult.Stok > dbResult.AlarmSeviyesi)
					{
						dbResult.StokAlarm = false;
					}
					else
					{
						dbResult.StokAlarm = true;
					}

					db.SaveChanges();

					result.Title = "Başarılı";
					result.Message = "İşlem Başarılı";
					result.Status = true;
				}
				catch (Exception ex)
				{
					result.Title = "Hata";
                    result.Message = ex.Message;
					result.Status = false;
				}
			}
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
/*
 Tüm sayfa değişecekse ActionResult ; sadece veri dönecekse, ufak tefek değişiklikler(mesela buton renginin değişmesi) için JsonResult kullanılır.
 */