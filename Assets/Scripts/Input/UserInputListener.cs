using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputListener : MonoBehaviour
{
    public event Action LMBClicked = delegate { };
    public event Action LMBHeld = delegate { };
    public event Action LMBUp = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LMBClicked?.Invoke();
            
        }
        if (Input.GetMouseButton(0))
        {
            LMBHeld?.Invoke();

        }
        if (Input.GetMouseButtonUp(0))
        {
            LMBUp?.Invoke();

        }

    }


    
}
