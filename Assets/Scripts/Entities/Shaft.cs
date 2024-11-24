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
    [SerializeField] private float audioFadeOutDuration = 0.5f; // Duration for audio fade out
    [SerializeField] [Range(0f, 1f)] private float slowdownStartPoint = 0.6f; // When to start slowing down (0.8 = 80%)
    private AudioSource _shaftAudio;
    
    private int _currentLevel; // 0 or 1
    private float _targetLevel;
    private Vector3 _initialPosition;
    private bool _isMoving;
    private PlayerMovement _player;
    private float _initialVolume; 
    
    void Awake()
    {
        transform.position = lowerTransform.position;
        _player = FindObjectOfType<PlayerMovement>();
        _shaftAudio = GetComponent<AudioSource>();
        if (_shaftAudio != null)
        {
            _initialVolume = _shaftAudio.volume; 
        }
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

    private IEnumerator FadeOutAudio()
    {
        if (_shaftAudio != null)
        {
            float startVolume = _shaftAudio.volume;
            float elapsedTime = 0f;

            while (elapsedTime < audioFadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / audioFadeOutDuration;
                _shaftAudio.volume = Mathf.Lerp(startVolume, 0f, normalizedTime);
                yield return null;
            }

            _shaftAudio.Stop();
            _shaftAudio.volume = _initialVolume; 
            _shaftAudio.pitch = 1f; 
        }
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
            _shaftAudio.volume = _initialVolume; 
            _shaftAudio.Play();
        }
        
        float elapsed = 0f;
        
        while (elapsed < travelSpeed)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / travelSpeed;
            var yPos = Mathf.Lerp(startY, targetY, progress);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            _player.transform.position =
                new Vector3(_player.transform.position.x, movementLock.position.y, _player.transform.position.z);
            
            if (_shaftAudio != null)
            {
                float pitchProgress = progress < slowdownStartPoint ? 
                    progress : 
                    (1f - progress) * (1f / (1f - slowdownStartPoint)); // Normalized slowdown
                _shaftAudio.pitch = Mathf.Lerp(0.5f, 1.5f, pitchProgress);
            }
            
            yield return null;
        }
        
        _isMoving = false;
        _currentLevel = 1 - _currentLevel; // Flip
        
        if (_shaftAudio != null)
        {
            _shaftAudio.loop = false;
            StartCoroutine(FadeOutAudio());
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