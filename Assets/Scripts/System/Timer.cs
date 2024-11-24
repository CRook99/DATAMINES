using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField] private float _maxTime;
    [SerializeField] private float _decreaseMultiplier;
    [SerializeField] private Image fill;
    private float _remainingTime;

    public float Percent => _remainingTime / _maxTime;
    
    public static Timer Instance { get; private set; }
    
    public delegate void DepleteEvent();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        _remainingTime = _maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Add speed in which time decreases 
        _remainingTime -= _decreaseMultiplier * Time.deltaTime;
        fill.fillAmount = (_remainingTime / _maxTime);
        
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
