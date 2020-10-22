using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Interfaces
{
    public interface IFileRepository
    {
        void Insert(File file);
        void Update(File file);
        void Save();
        void Insert(IEnumerable<File> file);
        void Delete(Guid fileId);

        ValueTask<File> GetFileById(Guid id);
    }
}
