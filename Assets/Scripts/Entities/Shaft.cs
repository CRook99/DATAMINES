using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Shaft : MonoBehaviour, IInteractable
{
    [SerializeField] private BoxCollider2D leftWall;
    [SerializeField] private BoxCollider2D rightWall;
    [SerializeField] private Transform lowerTransform;
    [SerializeField] private Transform upperTransform;
    [SerializeField] private Transform movementLock;
    [SerializeField] private float travelSpeed;
    private AudioSource _shaftAudio;
    
    private int _currentLevel; // 0 or 1
    private float _targetLevel;
    private Vector3 _initialPosition;
    private bool _isMoving;
    private PlayerMovement _player;
    
    void Awake()
    {
        transform.position = lowerTransform.position;
        _player = FindObjectOfType<PlayerMovement>();
        _shaftAudio = GetComponent<AudioSource>();

    }

    public void Interact()
    {
        if (_isMoving)
        {
            return;
        }
        StartCoroutine(MoveToLevel());
        _shaftAudio.Play();
    }

    private void OpenDoors()
    {
        leftWall.enabled = false;
        rightWall.enabled = false;
    }

    private void CloseDoors()
    {
        leftWall.enabled = true;
        rightWall.enabled = true;
    }

    private IEnumerator MoveToLevel()
    {
        _player.transform.position =
            new Vector3(transform.position.x, _player.transform.position.y, _player.transform.position.z);
        CloseDoors();
        var startY = _currentLevel == 1 ? upperTransform.position.y : lowerTransform.position.y;
        var targetY = _currentLevel == 0 ? upperTransform.position.y : lowerTransform.position.y;
        _isMoving = true;
        if (_shaftAudio != null)
        {
            _shaftAudio.loop = true;
            _shaftAudio.Play();
        }
        
        float elapsed = 0f;
        
        float progress = 0f;
        
        
        while (elapsed < travelSpeed)
        {
            elapsed += Time.deltaTime;
            var yPos = Mathf.Lerp(startY, targetY, elapsed / travelSpeed);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            _player.transform.position =
                new Vector3(_player.transform.position.x, movementLock.position.y, _player.transform.position.z);
            
            if (_shaftAudio != null)
            {
                _shaftAudio.pitch = Mathf.Lerp(0.5f, 1.5f, progress); // Example: start slow and speed up
            }
            
            
            yield return null;
        }
        _isMoving = false;
        _currentLevel = 1 - _currentLevel; // Flip
        
        if (_shaftAudio != null)
        {
            _shaftAudio.loop = false;
            _shaftAudio.Stop();
            _shaftAudio.pitch = 1f; // Reset pitch to normal
        }
        OpenDoors();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var text = $"E - Travel";
        InteractUI.Instance.Show(text);
    }
        
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        InteractUI.Instance.Hide();
    }
}