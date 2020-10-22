using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class FilesModel
    {
        List<IFormFile> files { get; set; }

       
    }
}
