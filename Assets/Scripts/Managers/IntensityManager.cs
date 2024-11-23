using System;
using UnityEngine;

namespace Managers
{
    public class IntensityManager : MonoBehaviour
    {
        public static IntensityManager Instance { get; private set; }

        public float RequestTimeLimit = 45f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
        
        
    }
}