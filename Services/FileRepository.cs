using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class FileRepository : IFileRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<File> files;

        public FileRepository(MDContext context)
        {
            _context = context;
            files = _context.Set<File>(); 
        }

        public void Delete(Guid fileId)
        {
            var file = files.Find(fileId);
            System.IO.File.Delete(file.Path + file.File_Id.ToString() + file.Type == "image"? ".jpg": ".txt");
        }

        public ValueTask<File> GetFileById(Guid id)
        {
            return files.FindAsync(id);
        }

        public void  Insert(File file)
        {
             files.Add(file);

        }

        public void Update(File file)
        {
            files.Update(file);
        }

        public  void Insert(IEnumerable<File> file)
        {
             files.AddRange(file);
          
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
