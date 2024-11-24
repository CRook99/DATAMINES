using System;
using Entities.Resources;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class Indicator : MonoBehaviour
    {
        private DropPoint _point;
        private float _margin;
        private RectTransform _canvas;
        private Camera _cam;
        [SerializeField] private GameObject artRoot;
        [SerializeField] private TextMeshPro timerText;

        private void Awake()
        {
            _cam = Camera.main;
        }

        public void Initialize(DropPoint p)
        {
            _point = p;
        }

        private void Update()
        {
            if (!_point.HasRequest)
            {
                artRoot.SetActive(false);
                return;
            }

            var pos = _cam.WorldToViewportPoint(_point.transform.position);

            if (pos.x is >= 0 and <= 1 && pos.y is >= 0 and <= 1)
            {
                artRoot.SetActive(false);
            }
            else
            {
                artRoot.SetActive(true);
                timerText.text = Formatting.FormatTime(_point.RequestTimer);
            }
        }
    }
}