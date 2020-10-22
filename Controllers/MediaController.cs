using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyDeckAPI.Controllers
{
    [Route("mydeckapi/[controller]")]
    public class MediaController : Controller
    {

        private readonly ILogger<DeckController> logger;
        private readonly IFileRepository fileRepository;
        private readonly ContentSaver contentSaver;
        private readonly AuthUtils authUtils;

        public MediaController(ILogger<DeckController> logger, IFileRepository fileRepository, ContentSaver contentSaver, AuthUtils authUtils)
        {
            this.logger = logger;
            this.fileRepository = fileRepository;
            this.contentSaver = contentSaver;
            this.authUtils = authUtils;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Media(Guid id)
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var dataPath = Path.GetDirectoryName(currentDirectory);
                var fileMeta = await fileRepository.GetFileById(id);
                var imageFileStream = System.IO.File.OpenRead(dataPath + @"/UsersData/" + fileMeta.Path+ id.ToString() + ".jpg");
                return File(imageFileStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromForm] List<FileModel> files)
        {
            try
            { 
                for(int i = 0; i < files.Count; i++)
                {
                    var fileMeta = files[i];
                    var fileData = Request.Form.Files[i];
                    if (fileData != null)
                    {
                        String md5Path = await contentSaver.Save(fileData);
                         fileRepository.Insert(new MyDeckAPI.Models.File
                        {
                            File_Id = Guid.Parse(fileMeta.id),
                            Type = fileMeta.type,
                            Path = md5Path,
                            Md5 = authUtils.GetMd5HashString(fileMeta.id),
                            Size = fileData.Length
                        });
                    }
                 
                }

                fileRepository.Save();

                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromForm] List<FileModel> files)
        {
            try
            { 
                for(int i = 0; i < files.Count; i++)
                {
                    var fileMeta = files[i];
                    var fileData = Request.Form.Files[i];
                    if (fileData != null)
                    {
                        String md5Path = await contentSaver.Save(fileData);
                        fileRepository.Update(new Models.File
                        {
                            File_Id = Guid.Parse(fileMeta.id),
                            Type = fileMeta.type,
                            Path = md5Path,
                            Md5 = authUtils.GetMd5HashString(fileMeta.id),
                            Size = fileData.Length
                        });
                    }
                 
                }

                fileRepository.Save();

                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
