using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyDeckAPI.Interfaces;

namespace MyDeckAPI.Services.Usecases
{
    public class DeleteDeckUseCase : IUseCase<int, DeleteDeckParams>
    {
        private readonly IFileRepository fileRepository;
        private readonly IDeckRepository deckRepository;

        public DeleteDeckUseCase(IFileRepository fileRepository, IDeckRepository deckRepository)
        {
            this.fileRepository = fileRepository;
            this.deckRepository = deckRepository;
        }

        public async Task<int> Invoke(DeleteDeckParams param)
        {
            try
            {
                var deck = deckRepository.FindById(param.deckId);
                fileRepository.Delete(deck.Deck.Icon);
                foreach (FullCardModel c in deck.Deck.Cards)
                {
                    fileRepository.Delete(c.Answer.Id);
                    fileRepository.Delete(c.Question.Id);
                };
                deckRepository.Delete(param.deckId);
                deckRepository.Save();
                return 0;
            }catch(Exception e)
            {
                return 1;
            }
        }
    }

    public class DeleteDeckParams
    {
        public DeleteDeckParams(Guid deckId)
        {
            this.deckId = deckId;
        }


        public Guid deckId { get; set; }
    }
}
