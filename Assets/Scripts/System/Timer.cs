using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField] private float _maxTime = 100f;
    [SerializeField] private float _decreaseMultiplier = 5f;
    private Image _bar;
    private float _remainingTime;
    
    public delegate void DepleteEvent();

    void Awake()
    {
        _bar = GetComponent<Image>();
        _remainingTime = _maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Add speed in which time decreases 
        _remainingTime -= _decreaseMultiplier * Time.deltaTime;
        // _bar.fillAmount = (_remainingTime / 100f);
        
        if (_remainingTime <= 0)
        {
            OnDepletion?.Invoke();
        }
    }

    public void AddTime(float time)
    {
        if (_remainingTime + time <= _maxTime)
        {
            _remainingTime += time;
        }
        else
        {
            _remainingTime = _maxTime;
        }
    }

    public void DecreaseTime(float time)
    {
        // something to consider; nerf decreases based on player state
        _remainingTime -= time;
        
    }

    public static event DepleteEvent OnDepletion;


}
