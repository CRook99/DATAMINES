using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("I am awake!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            Debug.Log("Hello!");

        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 upTranslation = new Vector3(0f, 5f, 0f);
            transform.position += upTranslation;
        }
    }
}
