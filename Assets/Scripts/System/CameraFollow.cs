using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine;

namespace System
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float floor0y;
        [SerializeField] private float floor1y;
        [SerializeField] private float floor0to1trigger;
        [SerializeField] private float floor1to0trigger;
        [SerializeField] private float scrollTime;

        private int _currentScreen;
        private PlayerMovement _player;
        private bool _scrolling;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerMovement>();
            _currentScreen = 0;
        }

        private void Update()
        {
            if (!_scrolling && _currentScreen == 0 && _player.transform.position.y > floor0to1trigger)
            {
                StartCoroutine(Scroll(floor0y, floor1y));
            }
            else if (!_scrolling && _currentScreen == 1 && _player.transform.position.y < floor1to0trigger)
            {
                StartCoroutine(Scroll(floor1y, floor0y));
            }
        }

        IEnumerator Scroll(float startY, float endY)
        {
            _scrolling = true;
            float elapsed = 0f;

            while (elapsed < scrollTime)
            {
                float t = elapsed / scrollTime;
                t = Mathf.SmoothStep(0, 1, t);
                float y = Mathf.Lerp(startY, endY, t);
                
                Vector3 newPos = transform.position;
                newPos.y = y;
                transform.position = newPos;
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            Vector3 final = transform.position;
            final.y = endY;
            transform.position = final;
            _scrolling = false;
            _currentScreen = 1 - _currentScreen;
        }
    }
}