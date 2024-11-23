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
        public static RequestManager Instance { get; private set; }

        private List<DropPoint> _dropPoints;
        [SerializeField] private List<ResourceScriptableObject> possibleResources;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _dropPoints = FindObjectsOfType<DropPoint>().ToList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var resource1 = possibleResources.Random();
                var resource2 = possibleResources.Random();
                var point = _dropPoints.Random();
                point.NewRequest(resource1, resource2);
            }
        }
    }
}