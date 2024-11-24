using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class Shaft : MonoBehaviour, IInteractable
{
    [SerializeField] private BoxCollider2D leftWall;
    [SerializeField] private BoxCollider2D rightWall;
    [SerializeField] private Transform lowerTransform;
    [SerializeField] private Transform upperTransform;
    [SerializeField] private float travelSpeed;
    
    private int _currentLevel; // 0 or 1
    private float _targetLevel;
    private Vector3 _initialPosition;
    private bool _isMoving;
    private PlayerMovement _player;
    
    void Awake()
    {
        transform.position = lowerTransform.position;
        _player = FindObjectOfType<PlayerMovement>();
    }

    public void Interact()
    {
        StartCoroutine(MoveToLevel());
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
        float elapsed = 0f;
        while (elapsed < travelSpeed)
        {
            elapsed += Time.deltaTime;
            var yPos = Mathf.Lerp(startY, targetY, elapsed / travelSpeed);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            yield return null;
        }
        _isMoving = false;
        _currentLevel = 1 - _currentLevel; // Flip
        OpenDoors();
    }
}