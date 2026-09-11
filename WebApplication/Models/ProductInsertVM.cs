using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
	public class ProductInsertVM
	{
		public int ID { get; set; }
		public string productName { get; set; }
		public int sizeInfo { get; set; }
		public int materialInfo { get; set; }
		public bool isActive { get; set; }
		public string description { get; set; }
	}
}