using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;

namespace MyDeckAPI.Services.Usecases
{
    public class GetDecksForTrainUseCase : IUseCase<List<Deck>, GetDecksForTrainParams>
    {
        private readonly IDeckRepository _deckRepository;

        public GetDecksForTrainUseCase(IDeckRepository deckRepository)
        {
            this._deckRepository = deckRepository;
        }

        public async Task<List<Deck>> Invoke(GetDecksForTrainParams param)
        {
            List<Deck> decks = new List<Deck>(_deckRepository.FindDecksForQuickTrain(param.UserId));
            SortDecksForTrain(decks);
            return decks;
        }

        //Takes only first 40% of sorted cards in decks 
        private void SortDecksForTrain(List<Deck> sourceList)
        {
            foreach (Deck deck in sourceList)
            {
                deck.Cards = SortCardsForTrain(new List<Card>(deck.Cards), deck.XP);
            }
        }
        
        //Sort cards ascending by criteria `cardXP/deckXP`
        private List<Card> SortCardsForTrain(List<Card> sourceList, int deckXP)
        {
            sourceList.Sort((card1, card2) =>
            {
                var card1SortCriteria = card1.Statistics.ToList()[0].XP / deckXP;
                var card2SortCriteria = card2.Statistics.ToList()[0].XP / deckXP;
                if (card1SortCriteria < card2SortCriteria)
                {
                    return -1;
                }
                else if (card1SortCriteria == card2SortCriteria)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
            return sourceList;
        }
    }

    public class GetDecksForTrainParams
    {
        public readonly Guid UserId;

        public GetDecksForTrainParams(Guid userId)
        {
            UserId = userId;
        }
    }
}