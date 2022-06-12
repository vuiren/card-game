using UnityEngine;

namespace Domain
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