using UI;
using System;
using Entities.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities
{
    public class ResourcePoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private ResourceScriptableObject resource;
        private PlayerInventory _playerInventory;

        private void Awake()
        {
            _playerInventory = FindObjectOfType<PlayerInventory>();
        }

        public void Interact(PlayerInteraction _)
        {
            _playerInventory.AddResource(resource);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerMovement _)) return;

            var text = $"E - Pick up {resource.Name}";
            InteractUI.Instance.Show(text);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerMovement _)) return;
            InteractUI.Instance.Hide();
        }
    }
}