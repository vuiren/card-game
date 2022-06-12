using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.DTO;
using Extensions;
using Firebase.Database;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Util;

namespace Infrastructure
{
    public class FirebaseCenterDeckService: ICenterDeckService
    {
        private readonly Configuration _configuration;
        private readonly DatabaseReference _centerDeckReference;
        private int _trumpCardId;
        private int[] _cardsInCenter;
        
        public FirebaseCenterDeckService(Configuration configuration)
        {
            _configuration = configuration;
            _centerDeckReference = FirebaseDatabase.DefaultInstance.RootReference.Child(_configuration.gameId)
                .Child("centerDeck");
            _centerDeckReference.ValueChanged += HandleValueChange;    
        }

        private void HandleValueChange(object sender, ValueChangedEventArgs e)
        {
            if(e.Snapshot?.Value == null) return;

            var value = JsonUtility.FromJson<CenterDeckData>(e.Snapshot.GetRawJsonValue());
            _trumpCardId = value.trumpCardId;

            var cards = value.cardsInGame.Split(',');
            cards = cards.Where(x => x.Length > 0).ToArray();

            _cardsInCenter = cards.Select(int.Parse).ToArray();
        }

        public void SetTrumpCard(int trumpCardId)
        {
            _centerDeckReference.GetValueAsync().ContinueWith(x =>
            {
                var centerDeckData = JsonUtility.FromJson<CenterDeckData>(x.Result.GetRawJsonValue());
                centerDeckData.trumpCardId = trumpCardId;

                _trumpCardId = trumpCardId;
                _centerDeckReference.SetRawJsonValueAsync(JsonUtility.ToJson(centerDeckData));
            });
        }

        public CardSheet GetTrumpCard()
        {
            return _configuration.cardsInGame.FirstOrDefault(x => x.cardId == _trumpCardId);
        }

        public CardSheet[] GetCards(int cardsCount)
        {
            var cards = _cardsInCenter.Take(cardsCount);
            var result = _configuration.cardsInGame.Where(x => cards.Contains(x.cardId));

            _cardsInCenter = _cardsInCenter.Skip(cardsCount).Take(_cardsInCenter.Length - cardsCount - 1).ToArray();
            SetCards(_configuration.cardsInGame.Where(x=> _cardsInCenter.Contains(x.cardId)));
            
            return result.ToArray();
        }

        public void SetCards(IEnumerable<CardSheet> cardSheets)
        {
            var enumerable = cardSheets as CardSheet[] ?? cardSheets.ToArray();
            _cardsInCenter = enumerable.Select(x => x.cardId).ToArray();
            _cardsInCenter.Shuffle();
            var cardsInGame = ArrayMethods.TurnArrayToString(_cardsInCenter);

            var centerDeckData = new CenterDeckData()
            {
                trumpCardId = _trumpCardId,
                cardsInGame = cardsInGame,
            };

            _centerDeckReference.SetRawJsonValueAsync(JsonUtility.ToJson(centerDeckData));
        }

        public CardSheet[] GetAllCardsInCenter()
        {
            return _configuration.cardsInGame;
        }
    }
}