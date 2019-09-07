using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.GetComponent<MouseLook>().enabled = true;
    }

    public float ForwardSpeed = 1;
    public float SideSpeed = 1;
    public float BackSpeed = 1;
    
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gameObject.GetComponent<MouseLook>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameObject.GetComponent<MouseLook>().enabled = false;
        }

        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            newPosition += transform.forward * ForwardSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition -= transform.forward * BackSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition += transform.right * SideSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPosition -= transform.right * SideSpeed;
        }

        newPosition.y = 0;

        transform.position = newPosition;
    }
}