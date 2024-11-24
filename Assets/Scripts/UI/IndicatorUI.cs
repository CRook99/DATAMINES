using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities.Resources;
using UI;
using UnityEngine;

public class IndicatorUI : MonoBehaviour
{
    private Camera _cam;
    public RectTransform Canvas;
    public Indicator Prefab;
    
    [SerializeField] private float margin;
    private List<DropPoint> _dropPoints;
    private Dictionary<DropPoint, Indicator> _indicators;


    private void Awake()
    {
        _cam = Camera.main;
        _dropPoints = FindObjectsOfType<DropPoint>().ToList();
        _indicators = new();
        foreach (DropPoint p in _dropPoints)
        {
            Indicator ind = Instantiate(Prefab, transform);
            ind.Initialize(p);
            _indicators.Add(p, ind);
        }
    }

    private void Update()
    {
        foreach (var kvp in _indicators)
        {
            DropPoint point = kvp.Key;
            Indicator indicator = kvp.Value;
            RectTransform indicatorRT = indicator.GetComponent<RectTransform>();

            Vector3 viewportPos = _cam.WorldToViewportPoint(point.transform.position);
        
            Vector3 clampedViewportPos = new Vector3(
                Mathf.Clamp(viewportPos.x, 0.01f, 0.99f),
                Mathf.Clamp(viewportPos.y, 0.01f, 0.99f),
                viewportPos.z
            );
        
            Vector3 screenPos = _cam.ViewportToScreenPoint(clampedViewportPos);
        
            screenPos.x = Mathf.Clamp(screenPos.x, margin, Screen.width - margin);
            screenPos.y = Mathf.Clamp(screenPos.y, margin, Screen.height - margin);
        
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Canvas.GetComponent<RectTransform>(), // Use parent RectTransform instead of Canvas
                screenPos,
                _cam,
                out canvasPos
            );
        
            indicatorRT.localPosition = canvasPos;
        }
    }
}
