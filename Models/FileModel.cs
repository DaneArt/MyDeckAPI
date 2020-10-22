using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    [BindProperties(SupportsGet = true)]
    public class FileModel
    {
        public String id { get; set; }
        public String type { get; set; }


    }
}
