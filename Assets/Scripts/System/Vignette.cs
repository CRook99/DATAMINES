using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vignette : MonoBehaviour
{
    [SerializeField] private float threshold = 0.25f;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        var c = _image.color;
        c.a = 0f;
        _image.color = c;
    }

    private void Update()
    {
        var c = _image.color;
        c.a = Mathf.Lerp(1f, 0f, Timer.Instance.Percent / threshold);
        _image.color = c;
    }
}
