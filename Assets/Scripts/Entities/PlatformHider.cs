using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlatformHider : MonoBehaviour
{

    [SerializeField] private float setHideInterval = 3f;
    [SerializeField]  float setHideTimer = 1f; 
    private bool _hiding;
    public GameObject platform;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private BoxCollider2D boxCollider;
    private float _timer;
    private float _hideTimer;
    
    
    void Awake()
    {
        _timer = setHideInterval;
        _hideTimer = setHideTimer;
        spriteRenderer = platform.GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        if (spriteRenderer)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_hiding)
        {
            _hideTimer -= Time.deltaTime;
            if (_hideTimer <= 0)
            {
                _hiding = false;
                ShowPlatform();
                _hideTimer = setHideTimer;
            }
            return;
        }
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = setHideInterval;
            _hiding = true;
            HidePlatform();
        }
    }

    public void HidePlatform()
    {
        if (spriteRenderer)
        {
            Color transparentColor = spriteRenderer.color;
            transparentColor.a = 0.2f;
            spriteRenderer.color = transparentColor;
        }
    
        if (boxCollider != null)
        {
            Debug.Log("off");
            boxCollider.enabled = false;
        }
    }
    
    public void ShowPlatform()
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = originalColor;
        }
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
            Physics2D.SyncTransforms();
        }
    }
}
