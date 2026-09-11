using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
	public class ProductListVM
	{
		public int ID { get; set; }
		public string UrunAdi { get; set; }
		public string MateryalAdi { get; set; }
		public string BoyutAdi { get; set; }
		public int Stok { get; set; }
		public bool StokAlarm { get; set; }
		public bool Durum { get; set; }
	}
}