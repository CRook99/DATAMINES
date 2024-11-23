using System;
using Entities.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities
{
    public class ResourcePoint : MonoBehaviour
    {
        [SerializeField] private ResourceScriptableObject resource;

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