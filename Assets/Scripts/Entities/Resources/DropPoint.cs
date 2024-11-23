using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Player;
using Managers;
using UnityEngine;

namespace Entities.Resources
{
    public class DropPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer primaryRenderer;
        [SerializeField] private SpriteRenderer secondaryRenderer;
        private ResourceScriptableObject _primaryResource;
        private ResourceScriptableObject _secondaryResource;
        private PlayerInventory _player;
        [SerializeField] private float _requestTimer;
    
        private void Awake()
        {
            _player = FindObjectOfType<PlayerInventory>();
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

        public void NewRequest(ResourceScriptableObject primary, ResourceScriptableObject secondary = null)
        {
            _primaryResource = primary;
            _secondaryResource = secondary;

            _requestTimer = IntensityManager.Instance.RequestTimeLimit;
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
        }

        private void ExpireRequest()
        {
            _primaryResource = null;
            _secondaryResource = null;
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
