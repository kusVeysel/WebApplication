using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.DB;

namespace WebApplication.Models
{
	public class SatinAlmaInsertVM
	{
		public List<Urun> UrunListe { get; set; }
		public List<Tedarikci> TedarikciListe { get; set; }
	}
}