using System;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private IInteractable _currentInteractable;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && _currentInteractable != null)
            {
                _currentInteractable.Interact(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_currentInteractable != null || 
                !other.TryGetComponent(out IInteractable interactable)) return;

            _currentInteractable = interactable;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _currentInteractable = null;
        }
    }
}