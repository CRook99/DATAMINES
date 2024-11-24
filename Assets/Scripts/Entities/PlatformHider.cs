using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlatformHider : MonoBehaviour
{
    [SerializeField] private int set; // 0 or 1
    [SerializeField] private float interval;
    [SerializeField] private float solidTime;
    [SerializeField] private int flashes;

    [SerializeField] private float solidFlashAlpha;
    [SerializeField] private float clearFlashAlpha;
    [SerializeField] private float clearAlpha;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _clear;
    private Color _clearFlash;
    private Color _solid;
    private Color _solidFlash;
    
    private BoxCollider2D _boxCollider;
    
    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        
        _clear = _spriteRenderer.color;
        _clear.a = clearAlpha;

        _clearFlash = _spriteRenderer.color;
        _clearFlash.a = clearFlashAlpha;

        _solidFlash = _spriteRenderer.color;
        _solidFlash.a = solidFlashAlpha;

        _solid = _spriteRenderer.color;

        if (set == 0)
        {
            StartCoroutine(SolidSpan());
        }
        else
        {
            HidePlatform();
            StartCoroutine(ClearSpan());
        }
    }

    IEnumerator SolidSpan()
    {
        yield return new WaitForSeconds(solidTime);

        float flashTime = (interval - solidTime) / (flashes * 2);
        for (int i = 0; i < flashes; i++)
        {
            _spriteRenderer.color = _solidFlash;
            yield return new WaitForSeconds(flashTime);
            _spriteRenderer.color = _solid;
            yield return new WaitForSeconds(flashTime);
        }

        HidePlatform();
        StartCoroutine(ClearSpan());
    }

    IEnumerator ClearSpan()
    {
        yield return new WaitForSeconds(solidTime);

        float flashTime = (interval - solidTime) / (flashes * 2);
        for (int i = 0; i < flashes; i++)
        {
            _spriteRenderer.color = _clearFlash;
            yield return new WaitForSeconds(flashTime);
            _spriteRenderer.color = _clear;
            yield return new WaitForSeconds(flashTime);
        }

        ShowPlatform();
        StartCoroutine(SolidSpan());
    }
    
    public void HidePlatform()
    {
        if (_spriteRenderer)
        {
            _spriteRenderer.color = _clear;
        }
    
        if (_boxCollider != null)
        {
            _boxCollider.enabled = false;
        }
    }
    
    public void ShowPlatform()
    {
        if (_spriteRenderer)
        {
            _spriteRenderer.color = _solid;
        }
        if (_boxCollider != null)
        {
            _boxCollider.enabled = true;
            Physics2D.SyncTransforms();
        }
    }
}
