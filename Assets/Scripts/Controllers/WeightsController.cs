using System;
using QFSW.QC;
using Services;
using UnityEngine;
using Util;
using Zenject;

namespace Controllers
{
    public class WeightsController : MonoBehaviour
    {
        [SerializeField] private Weights _weights;
        public Action OnRightWeightUpdate;
        private BetsController _betsController;
        private IPlayerService _playerService;

        [Inject]
        public void Construct(BetsController betsController, IPlayerService playerService)
        {
            _betsController = betsController;
            _playerService = playerService;
        }

        [Command("weights.getLeftWeight")]
        public int GetLeftPotWeight()
        {
            return _weights.leftWeight;
        }

        [Command("weights.getRightWeight")]
        public int GetRightPotWeight()
        {
            return _weights.rightWeight;
        }

        [Command("weights.setLeftWeight")]
        public void SetLeftWeight(int weightCount, bool sendData = true)
        {
            _weights.leftWeight = weightCount;
        }

        [Command("weights.setRightWeight")]
        public void SetRightWeight(int weight)
        {
            _weights.rightWeight = weight;
            OnRightWeightUpdate?.Invoke();
        }
    }
}