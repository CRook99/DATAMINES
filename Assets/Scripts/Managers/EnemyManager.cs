using System;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public GameObject enemyPrefab;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Instantiate(enemyPrefab);
            }
        }
    }
}