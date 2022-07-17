using System;
using UnityEngine;

namespace Util
{
    public class Weights : MonoBehaviour
    {
        [SerializeField] private Transform scaleRod;
        [SerializeField] private float speed, targetRodZRot;
        [SerializeField] private float currentRodZRot, maxZRot = -10;
        public int rightWeight, leftWeight;

        private void Start()
        {
            targetRodZRot = scaleRod.rotation.z;
        }

        private void Update()
        {
            var step = Mathf.Abs(rightWeight == 0 ? 0 : maxZRot / rightWeight);
            targetRodZRot = leftWeight == 0 ? maxZRot : maxZRot + leftWeight * step;
            currentRodZRot = Mathf.MoveTowards(currentRodZRot, targetRodZRot, speed * Time.deltaTime);
            scaleRod.rotation = Quaternion.Euler(0, 0, currentRodZRot);
        }
    }
}