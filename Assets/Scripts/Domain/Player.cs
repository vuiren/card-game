using System;
using UnityEngine;

namespace Game_Code.Domain
{
    [RequireComponent(typeof(Actor))]
    public class Player : MonoBehaviour
    {
        public Actor actor;

        private void Awake()
        {
            actor = GetComponent<Actor>();
        }
    }
}