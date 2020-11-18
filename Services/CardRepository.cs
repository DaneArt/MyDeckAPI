using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using MyDeckAPI.Security;

namespace MyDeckAPI.Services
{
    public class CardRepository : ICardRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<Card> table;
        private readonly AuthUtils authUtils;

        public CardRepository(MDContext context, AuthUtils authUtils)
        {
            _context = context;
            this.authUtils = authUtils;
            table = _context.Set<Card>();
        }
        public void Delete(IEnumerable<Card> cards)
        {
            table.RemoveRange(cards);
          
        }

        public Task<List<Card>> FindAll()
        {
            return table.ToListAsync();
        }

        public ValueTask<Card> FindById(object Id)
        {
            return table.FindAsync(Id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task InsertAsync(IEnumerable<Card> obj)
        {
            try
            {
                var list = new List<Card>();
                var exceptionList = new List<Card>();
                foreach (Card card in obj)
                {
                    if (card.IsValid())
                    {
                        if (await _context.Files.FindAsync(card.Answer) == null)
                        {
                            var ansFileId = card.Answer;
                            var ansFileMd5Hash = authUtils.GetMd5HashString(ansFileId.ToString());
                            var ansFilePath = authUtils.GetFilePathFromMD5(ansFileMd5Hash);
                             _context.Files.Add(new File() { File_Id = ansFileId ,Md5 = ansFileMd5Hash,Path = ansFilePath,Type = ""});
                        }
                        if (await _context.Files.FindAsync(card.Question) == null)
                        {
                            var queFileId = card.Question;
                            var queFileMd5Hash = authUtils.GetMd5HashString(queFileId.ToString());
                            var queFilePath = authUtils.GetFilePathFromMD5(queFileMd5Hash);
                            _context.Files.Add(new File() { File_Id = queFileId ,Md5 = queFileMd5Hash,Path = queFilePath,Type = ""});
                        }
                        list.Add(card);
                        _context.Cards.Add(card);
                    }
                    else
                    {
                        exceptionList.Add(card);
                    }
                }
                
                if (exceptionList.Count > 0) { throw new NonValidatedModelException<Card>(exceptionList); }
                
            }
            catch { throw; }
        }

        public async Task UpdateAsync(IEnumerable<Card> obj)
        {
            try
            {
                var list = new List<Card>();
                var exceptionList = new List<Card>();
                foreach (Card card in obj)
                {
                    if (card.IsValid())
                    {
                        if (await _context.Files.FindAsync(card.Answer) == null)
                        {
                            var ansFileId = card.Answer;
                            var ansFileMd5Hash = authUtils.GetMd5HashString(ansFileId.ToString());
                            var ansFilePath = authUtils.GetFilePathFromMD5(ansFileMd5Hash);
                             _context.Files.Add(new File() { File_Id = ansFileId ,Md5 = ansFileMd5Hash,Path = ansFilePath,Type = ""});
                        }
                        if (await _context.Files.FindAsync(card.Question) == null)
                        {
                            var queFileId = card.Question;
                            var queFileMd5Hash = authUtils.GetMd5HashString(queFileId.ToString());
                            var queFilePath = authUtils.GetFilePathFromMD5(queFileMd5Hash);
                             _context.Files.Add(new File() { File_Id = queFileId ,Md5 = queFileMd5Hash,Path = queFilePath,Type = ""});
                        }

                        list.Add(card);
                    }
                    else
                    {
                        exceptionList.Add(card);
                    }
                }

                
                if (exceptionList.Count > 0)
                {
                    throw new NonValidatedModelException<Card>(exceptionList);
                }

                _context.Cards.UpdateRange(list);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

    }
}
