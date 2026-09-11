using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
	public class ProductStockInfoJSVM
	{
		public int ID { get; set; }
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public int Stock { get; set; }
		public int AlarmStock { get; set; }
	}
}