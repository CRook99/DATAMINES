using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int inventorySize;
        
        private List<ResourceScriptableObject> _inventory;

        private void Awake()
        {
            _inventory = new();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropResource();
            }
        }

        public void AddResource(ResourceScriptableObject resource)
        {
            if (_inventory.Count == inventorySize)
            { 
                Debug.Log("Too many data packs!");
                return;
            }
            
            _inventory.Add(resource);
            InventoryUI.Instance.Refresh(_inventory);
        }

        public void RemoveResource(ResourceScriptableObject resource)
        {
            if (_inventory.Contains(resource))
            {
                _inventory.Remove(resource);
                InventoryUI.Instance.Refresh(_inventory);
                return;
            }
            
            Debug.Log($"Could not find resource with ID {resource.Id}");
        }

        public List<ResourceScriptableObject> TakeResources(ResourceScriptableObject resource1, ResourceScriptableObject resource2)
        {
            List<ResourceScriptableObject> resources = new();
            for (int i = _inventory.Count; i >= 0; i--)
            {
                if (_inventory[i].Equals(resource1) || _inventory[i].Equals(resource2))
                {
                    resources.Add(_inventory[i]);
                    RemoveResource(_inventory[i]);
                }
            }
            
            return resources;
        }

        private void DropResource()
        {
            if (_inventory.Count == 0) return;
            
            _inventory.RemoveAt(0);
            InventoryUI.Instance.Refresh(_inventory);
        }
    }
}