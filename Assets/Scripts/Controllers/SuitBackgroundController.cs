using System.Collections.Generic;
using System.Linq;
using Domain.DTO;
using Factories;
using Scriptable_Objects;
using Services;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class SuitBackgroundController : MonoBehaviour
    {
        [SerializeField] private Transform backgroundTransform;
        private Configuration _configuration;
        private ISessionService _sessionService;
        private GameObject _backgroundInstance;
        
        [Inject]
        public void Construct(Configuration configuration, ISessionService sessionService)
        {
            _configuration = configuration;
            _sessionService = sessionService;
            sessionService.OnCardAddedToSession(RedrawBackground);
        }

        private void RedrawBackground(List<SessionPlayer> obj)
        {
            var cards = _sessionService.GetCardsInSession();
            if (cards.Count == 1)
            {
                var cardSheet = _configuration.cardsInGame.FirstOrDefault(x => x.cardId == cards[0].playedCardId);
                if (cardSheet == null)
                {
                    Debug.LogError("Card sheet not found");
                    return;
                }

                var background = _configuration.BackgroundSuits.FirstOrDefault(x => x.cardSuit == cardSheet.cardSuit);

                if (_backgroundInstance)
                {
                    Destroy(_backgroundInstance.gameObject);
                }

                _backgroundInstance = Instantiate(background.backgroundPrefab, backgroundTransform);
            }
        }
    }
}