using System;
using System.Collections.Generic;
using Entities;
using Entities.Player;
using Entities.Resources;
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
            Vector3 worldPosWithOffset = _player.transform.position + offset;
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(worldPosWithOffset);
            RectTransform rt = GetComponent<RectTransform>();
    
            Vector2 canvasSize = rt.root.GetComponent<RectTransform>().sizeDelta;
            Vector2 desiredPosition = new Vector2(
                (viewportPoint.x * canvasSize.x) - (canvasSize.x * 0.5f),
                (viewportPoint.y * canvasSize.y) - (canvasSize.y * 0.5f)
            );
    
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, desiredPosition, followSpeed * Time.deltaTime);
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