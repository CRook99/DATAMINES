using System;
using System.Collections.Generic;
using Entities;
using Entities.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        public static InventoryUI Instance { get; private set; }
        
        [SerializeField] private Vector3 offset;
        [SerializeField] private float followSpeed;
        private List<InventorySlot> _inventorySlots;
        private PlayerMovement _player;
        private Vector3 _desiredPosition;
        private Vector3 _currentPosition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _player = FindObjectOfType<PlayerMovement>();
            _player.OnSideSwitch += SideSwitch;

            _inventorySlots = new();
            foreach (Transform t in transform)
            {
                if (t == transform) continue;

                if (t.TryGetComponent(out InventorySlot slot))
                {
                    _inventorySlots.Add(slot);
                }
            }
            
            Refresh(new List<ResourceScriptableObject>());

            
        }

        private void Update()
        {
            _desiredPosition = Camera.main.WorldToScreenPoint(_player.transform.position + offset); 
            transform.position = Vector3.Lerp(transform.position, _desiredPosition, followSpeed * Time.deltaTime);
        }

        private void SideSwitch(Direction direction)
        {
            offset.x = (direction == Direction.LEFT) ? Mathf.Abs(offset.x) : -Mathf.Abs(offset.x);
        }

        public void Refresh(List<ResourceScriptableObject> resources)
        {
            int i = 0;
            
            for (; i < resources.Count; i++)
            {
                _inventorySlots[i].gameObject.SetActive(true);
                _inventorySlots[i].Refresh(resources[i]);
            }

            for (int j = i; j < _inventorySlots.Count; j++)
            {
                _inventorySlots[j].gameObject.SetActive(false);
            }
        }
    }
}