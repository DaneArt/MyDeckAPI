using Microsoft.AspNetCore.Http;
using MyDeckAPI.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace MyDeckAPI.Data.MediaContent
{
    public class ContentSaver
    {
        private readonly AuthUtils authOptions;

        public ContentSaver(AuthUtils authOptions)
        {
            this.authOptions = authOptions;
        }

        public virtual async Task<string> Save(IFormFile file)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataPath = Path.GetDirectoryName(currentDirectory);

            String md5Path = authOptions.GetMd5HashString(file.FileName.Split(".")[0]);

            StringBuilder pathBuilder = new StringBuilder();


            for (int i = 0; i < md5Path.Length; i++)
            {
                pathBuilder.Append(md5Path[i]);
                if (i % 2 == 0)
                {
                    pathBuilder.Append("/");
                }
            }
            Directory.CreateDirectory(dataPath + @"/UsersData/" + pathBuilder.ToString());
            

            using (var stream = File.Create(dataPath + @"/UsersData/" + pathBuilder.ToString()+  file.FileName))
            {
                await file.CopyToAsync(stream);
            }

            return pathBuilder.ToString();

        }
        public virtual async Task<string> Save(IFormFile file, string path)
        {
            Directory.CreateDirectory(path);
            var randomName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            if (extension == ".jpeg" || extension == ".jpg" || extension == ".png")
            {
                using (var stream = new FileStream(path + randomName + extension, FileMode.OpenOrCreate))
                {
                    await file.CopyToAsync(stream);
                }

                return randomName;
            }
            return null;
        }
        public async Task<MyDeckAPI.Models.File> DownloadPicture(string url, Guid fileId)
        {
            using (var client = new WebClient())
            {
                var content = client.DownloadData(url);
                var stream = new MemoryStream(content);
                
                using (stream)
                {var randomName = fileId.ToString();
                    var formFile = new FormFile(stream, 0, stream.Length, "Textfile", randomName + ".jpg");
                    var localPath = await Save(formFile);
                    return new MyDeckAPI.Models.File { File_Id = fileId, Path = localPath, Md5 = authOptions.GetMd5HashString(fileId.ToString()), Size = stream.Length, Type = "image" };
                }

            }
        }
    }
}
