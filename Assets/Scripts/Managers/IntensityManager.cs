using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public struct Intensity
    {
        public float Seconds;
        public float RequestInterval;
        public float RequestTime;
        public float DoubleChance;
    }
    
    public class IntensityManager : MonoBehaviour
    {
        [SerializeField] private List<Intensity> intensities;
        private int _currentIntensity;
        private float _timer;

        public Intensity Intensity => intensities[_currentIntensity];

        public delegate void IntensityEvent();

        public event IntensityEvent OnIntensityUp;
        
        public static IntensityManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            _timer = 0f;
            _currentIntensity = 0;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > Intensity.Seconds)
            {
                OnIntensityUp?.Invoke();
                _currentIntensity++;
                _timer = 0f;
            }
        }
    }
}