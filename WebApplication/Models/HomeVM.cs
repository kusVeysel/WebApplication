using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.DB;

namespace WebApplication.Models
{
	public class HomeVM
	{
		public int TotalOrderCount { get; set; }
		public int ActiveTotalCustomerCount { get; set; }
		public int KritikUrunSayisi { get; set; }
		public List<Siparis> Son10Siparis { get; set; }
		public List<StokTablo> KritikUrunListe { get; set; }
	}
}