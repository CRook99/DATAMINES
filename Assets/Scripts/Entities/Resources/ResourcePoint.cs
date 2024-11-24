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
        private AudioSource AudioSource;

        private void Awake()
        {
            _playerInventory = FindObjectOfType<PlayerInventory>();
            spriteRenderer.sprite = resource.DispenserSprite;
            AudioSource = GetComponent<AudioSource>(); // Ensure the AudioSource is fetched
        }

        public void Interact()
        {
            _playerInventory.AddResource(resource);
            AudioSource.Play();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var text = $"E - Pick up {resource.Name}";
            InteractUI.Instance.Show(text);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            InteractUI.Instance.Hide();
        }
    }
}