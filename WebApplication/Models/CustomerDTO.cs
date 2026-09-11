using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.DB;

namespace WebApplication.Models
{
	public class CustomerDTO
	{
		public int ID { get; set; }
		public string CustomerName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
        public string adres { get; set; }
        public string yetkili { get; set; }
        public bool IsActive { get; set; }
		public List<Siparis> OrderListByID { get; set; }
	}
}