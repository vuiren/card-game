using UnityEngine;

namespace Controllers
{
    public class WaitingAnimationController : MonoBehaviour
    {
        [SerializeField] private GameObject waitingAnimation;

        public void HideWaitingAnimation()
        {
            waitingAnimation.SetActive(false);
        }
    }
}