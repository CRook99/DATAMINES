using UI;
using System;
using Entities.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Resources
{
    public class ResourcePoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private ResourceScriptableObject resource;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private PlayerInventory _playerInventory;

        private void Awake()
        {
            _playerInventory = FindObjectOfType<PlayerInventory>();
            spriteRenderer.sprite = resource.DispenserSprite;
        }

        public void Interact()
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