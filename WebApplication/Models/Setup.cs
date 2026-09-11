using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Setup
    {
        public class SizeVM
        {
            public int ID { get; set; }
            public double SizeName { get; set; }
        }

        public class CategoryVM
        {
            public int ID { get; set; }
            public string CategoryName { get; set; }
        }

        public class MaterialVM
        {
            public int ID { get; set; }
            public string MaterialName { get; set; }
        }

    }
}