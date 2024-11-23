using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 10f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>(); // get reference to component
    }
    // // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    //
    // Update is called once per frame
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
        
        body.velocity = new Vector2(xInput * speed, yInput);
        
    }
}
