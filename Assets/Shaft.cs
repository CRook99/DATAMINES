using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _levelHeight = 1f;
    [SerializeField] private float _minLevel = -10f;
    [SerializeField] private float _maxLevel = 10f;
    [SerializeField] private float _travelSpeed = 3f;
    
    private float _currentLevel;
    private float _targetLevel;
    private Vector3 _initialPosition;
    private bool _isMoving;
    
    void Awake()
    {
        _initialPosition = transform.position;
        _currentLevel = _initialPosition.y;
        _targetLevel = _currentLevel;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (_isMoving) return;
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Check if we haven't reached the max level
            if (_targetLevel < _maxLevel)
            {
                _targetLevel += _levelHeight;
                StartCoroutine(MoveToLevel());
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Check if we haven't reached the ground floor
            if (_targetLevel > _minLevel)
            {
                _targetLevel -= _levelHeight;
                StartCoroutine(MoveToLevel());
            }
        }
    }

    private IEnumerator MoveToLevel()
    {
        _isMoving = true;
        float elapsed = 0f;
        float startY = transform.position.y;
        while (elapsed < _travelSpeed)
        {
            elapsed += Time.deltaTime;
            var yPos = Mathf.Lerp(startY, _targetLevel, elapsed / _travelSpeed);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            yield return null;
        }
        _isMoving = false;
    }
}