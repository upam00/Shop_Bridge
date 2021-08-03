using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Bridge.Models
{
    public class Uploader
    {
        public string Id { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public string About { get; set; }

        public IFormFile ImageFile { get; set; }


    }
}
