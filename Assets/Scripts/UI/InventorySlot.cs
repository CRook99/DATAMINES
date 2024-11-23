using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        public void Refresh(ResourceScriptableObject resource)
        {
            image.sprite = resource.Sprite;
        }
    }
}