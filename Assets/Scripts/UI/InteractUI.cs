using Entities.Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InteractUI : MonoBehaviour
    {
        public static InteractUI Instance { get; private set; }
        
        [SerializeField] private Vector3 offset;
        [SerializeField] private Canvas canvas;
        
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
            _text.text = "";
        }

        // private void Update()
        // {
        //     Vector3 pos = Camera.main.WorldToScreenPoint(_player.transform.position + offset);
        //     
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //         canvas.GetComponent<RectTransform>(), // The canvas's RectTransform
        //         pos,                            // The screen space position
        //         Camera.main,                          // The camera rendering the canvas
        //         out Vector2 localPos                  // The resulting local position
        //     );
        //     
        //     transform.position = localPos;
        // }
        
        private void Update()
        {
            Vector3 worldPosWithOffset = _player.transform.position + offset;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosWithOffset);
            
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(worldPosWithOffset);
            RectTransform rt = GetComponent<RectTransform>();
    
            Vector2 canvasSize = rt.root.GetComponent<RectTransform>().sizeDelta;
            Vector2 finalPosition = new Vector2(
                (viewportPoint.x * canvasSize.x) - (canvasSize.x * 0.5f),
                (viewportPoint.y * canvasSize.y) - (canvasSize.y * 0.5f)
            );
    
            rt.anchoredPosition = finalPosition;
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