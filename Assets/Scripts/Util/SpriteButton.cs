using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class SpriteButton : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color hoverColor, normalColor;
        public UnityEvent onPressed; 
        
        private void OnMouseDown()
        {
            onPressed?.Invoke();
        }

        private void OnMouseEnter()
        {
            _spriteRenderer.color = hoverColor;
        }

        private void OnMouseExit()
        {
            _spriteRenderer.color = normalColor;
        }

        private void OnEnable()
        {
            _spriteRenderer.color = normalColor;
        }
    }
}