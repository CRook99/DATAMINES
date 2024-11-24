using System;
using System.Collections.Generic;
using Entities.Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Formatting = Utils.Formatting;

namespace Entities.Resources
{
    public class DropPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshPro timerText;
        [SerializeField] private SpriteRenderer timerBackRenderer;
        [SerializeField] private SpriteRenderer primaryRenderer;
        [SerializeField] private SpriteRenderer secondaryRenderer;
        [SerializeField] public float RequestTimer;

        private ResourceScriptableObject _primaryResource;
        private ResourceScriptableObject _secondaryResource;
        private PlayerInventory _player;
        private AudioSource _depositError;
        private AudioSource _depositSuccess;
        
        public bool HasRequest => RequestTimer > 0f;
    
        private void Awake()
        {
            _player = FindObjectOfType<PlayerInventory>();
            timerText.enabled = false;
            timerBackRenderer.enabled = false;
            _depositError = GetComponents<AudioSource>()[0]; 
            _depositSuccess =  GetComponents<AudioSource>()[1]; 
        }

        private void Update()
        {
            UpdateDisplay();
            
            if (RequestTimer > 0f)
            {
                RequestTimer -= Time.deltaTime;
                if (RequestTimer < 10f)
                {
                    timerText.outlineColor = Color.red;
                    timerText.color = Color.red;
                }
                if (RequestTimer < 0f)
                {
                    ExpireRequest();
                }
            }
        }

        public void NewRequest(float time, ResourceScriptableObject primary, ResourceScriptableObject secondary = null)
        {
            _primaryResource = primary;
            _secondaryResource = secondary;
            RequestTimer = time;
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
            Timer.Instance.AddTime(12f);
            RequestTimer = 0f;
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

            timerText.text = Formatting.FormatTime(RequestTimer);
            timerBackRenderer.enabled = HasRequest;
        }
    
        public void Interact()
        {
            List<ResourceScriptableObject> taken = _player.TakeResources(_primaryResource, _secondaryResource);
            if (taken.Count == 0)
            {
                _depositError.Play();
            }
            else
            {
                _depositSuccess.Play();
            }
            foreach (var resource in taken)
            {
                ReceiveResource(resource);
            }

            if (!HasRequest || _player.InventoryCount == 0) return;
            InteractUI.Instance.Show("E - Deposit data");
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || !HasRequest || _player.InventoryCount == 0) return;
            
            InteractUI.Instance.Show("E - Deposit data");
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            InteractUI.Instance.Hide();
        }

        public Sprite GetSprite(int num)
        {
            if (num == 0)
            {
                return _primaryResource != null ? _primaryResource.Sprite : null;
            }
            else
            {
                return _secondaryResource != null ? _secondaryResource.Sprite : null;
            }
        }
    }
}
