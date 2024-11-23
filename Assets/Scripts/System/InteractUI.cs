using Entities.Player;
using TMPro;
using UnityEngine;

namespace System
{
    public class InteractUI : MonoBehaviour
    {
        public static InteractUI Instance { get; private set; }
        
        [SerializeField] private Vector3 offset;
        
        private TextMeshProUGUI _text;
        private PlayerMovement _player;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            
            _player = FindObjectOfType<PlayerMovement>();
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(_player.transform.position + offset);
            transform.position = pos;
        }

        public void Show()
        {
            _text.enabled = true;
        }

        public void Show(string text)
        {
            SetText(text);
            Show();
        }

        public void Hide()
        {
            _text.enabled = false;
        }

        private void SetText(string text)
        {
            _text.text = text;
        }
    }
}