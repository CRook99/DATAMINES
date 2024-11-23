using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Player;
using UnityEngine;

public class DropPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer primaryRenderer;
    [SerializeField] private SpriteRenderer secondaryRenderer;
    private ResourceScriptableObject _primaryResource;
    private ResourceScriptableObject _secondaryResource;
    private PlayerInventory _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerInventory>();
    }

    public void NewRequest(ResourceScriptableObject primary, ResourceScriptableObject secondary = null)
    {
        _primaryResource = primary;
        UpdatePrimaryDisplay();
        _secondaryResource = secondary;
        UpdateSecondaryDisplay();
    }

    public void ReceiveResource(ResourceScriptableObject resource)
    {
        if (_primaryResource != null && _primaryResource.Equals(resource))
        {
            _primaryResource = null;
            UpdatePrimaryDisplay();
        }
        else if (_secondaryResource != null && _secondaryResource.Equals(resource))
        {
            _secondaryResource = null;
            UpdateSecondaryDisplay();
        }
    }

    private void UpdatePrimaryDisplay()
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
    }
    
    private void UpdateSecondaryDisplay()
    {
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
