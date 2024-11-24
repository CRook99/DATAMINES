using System;
using System.Collections.Generic;
using Entities.Player;
using TMPro;
using UnityEngine;
using Formatting = Utils.Formatting;

namespace Entities.Resources
{
    public class DropPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshPro timerText;
        [SerializeField] private SpriteRenderer primaryRenderer;
        [SerializeField] private SpriteRenderer secondaryRenderer;
        private ResourceScriptableObject _primaryResource;
        private ResourceScriptableObject _secondaryResource;
        private PlayerInventory _player;
        [SerializeField] private float _requestTimer;

        public bool HasRequest => _requestTimer > 0f;
    
        private void Awake()
        {
            _player = FindObjectOfType<PlayerInventory>();
            timerText.enabled = false;
        }

        private void Update()
        {
            UpdateDisplay();
            
            if (_requestTimer > 0f)
            {
                _requestTimer -= Time.deltaTime;
                if (_requestTimer < 0f)
                {
                    ExpireRequest();
                }
            }
        }

        public void NewRequest(float time, ResourceScriptableObject primary, ResourceScriptableObject secondary = null)
        {
            _primaryResource = primary;
            _secondaryResource = secondary;
            _requestTimer = time;
            timerText.enabled = true;
        }
    
        public void ReceiveResource(ResourceScriptableObject resource)
        {
            if (_primaryResource != null && _primaryResource.Equals(resource))
            {
                _primaryResource = null;
            }
            else if (_secondaryResource != null && _secondaryResource.Equals(resource))
            {
                _secondaryResource = null;
            }

            if (_primaryResource == null && _secondaryResource == null)
            {
                FulfilRequest();
            }
        }

        private void FulfilRequest()
        {
            Timer.Instance.AddTime(10f);
            _requestTimer = 0f;
            timerText.enabled = false;
        }

        private void ExpireRequest()
        {
            _primaryResource = null;
            _secondaryResource = null;
            timerText.enabled = false;
        }
    
        private void UpdateDisplay()
        {
            if (_primaryResource != null)
            {
                primaryRenderer.enabled = true;
                primaryRenderer.sprite = _primaryResource.Sprite;
            }
            else
            {
                primaryRenderer.enabled = false;
            }
            
            if (_secondaryResource != null)
            {
                secondaryRenderer.enabled = true;
                secondaryRenderer.sprite = _secondaryResource.Sprite;
            }
            else
            {
                secondaryRenderer.enabled = false;
            }

            timerText.text = Formatting.FormatTime(_requestTimer);
        }
    
        public void Interact()
        {
            List<ResourceScriptableObject> taken = _player.TakeResources(_primaryResource, _secondaryResource);
            foreach (var resource in taken)
            {
                ReceiveResource(resource);
            }
        }
    }
}
