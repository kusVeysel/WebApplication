using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
	public class SatinAlmaAddVM 
	{
		public int urunID { get; set; }
		public int tedarikciID { get; set; }
		public int adet { get; set; }
		public string birimFiyat { get; set; }
		public string toplamFiyat { get; set; }
		public string kdvOran { get; set; }
		public string satinAlmaTarih { get; set; }
	}
}