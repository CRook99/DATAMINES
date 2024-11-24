using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Resources;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class RequestManager : MonoBehaviour
    {
        [SerializeField] private List<ResourceScriptableObject> possibleResources;
        
        public static RequestManager Instance { get; private set; }

        private List<DropPoint> _dropPoints;
        private float _interval;
        private float _requestTime;
        private float _timer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _dropPoints = FindObjectsOfType<DropPoint>().ToList();

            _timer = 0f;
            IntensityManager.Instance.OnIntensityUp += Reload;

            Reload();
            
            GenerateRequest();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GenerateRequest();
            }

            _timer += Time.deltaTime;
            if (_timer > _interval)
            {
                _timer = 0f;
                GenerateRequest();
            }
        }
        
        public void GenerateRequest()
        {
            if (_dropPoints.TryGetRandomElement(p => !p.HasRequest, out DropPoint point))
            {
                var resource1 = possibleResources.Random();
                var resource2 = (Random.value < IntensityManager.Instance.Intensity.DoubleChance) ? possibleResources.Random() : null;
                point.NewRequest(_requestTime, resource1, resource2);
            }
        }

        private void Reload()
        {
            _interval = IntensityManager.Instance.Intensity.RequestInterval;
            _requestTime = IntensityManager.Instance.Intensity.RequestTime;
        }
    }
}