using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _body;
    private float _speed = 10f;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>(); // get reference to component
    }
    
    private void Update()
    {
        float xInput = 0f;
        float yInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            xInput -= 1;                   
        }
        if (Input.GetKey(KeyCode.D)) { 
            xInput += 1;                   
        }

        if (Input.GetKeyDown(KeyCode.Space)) { 
            yInput += 10;                   
        }
        
        _body.velocity = new Vector2(xInput * _speed, yInput);
        
    }
}
