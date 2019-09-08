using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float ForwardSpeed = 1;
    public float SideSpeed = 1;
    public float BackSpeed = 1;
    public float SprintingSpeed = 1;
    public float JumpHeight = 1;

    private float Forward_Speed;
    private float Side_Speed;
    private float Back_Speed;
    private float Sprinting_Speed;
    private float Jump_Height;

    private void Start()
    {
        Forward_Speed = ForwardSpeed;
        Side_Speed = SideSpeed;
        Back_Speed = BackSpeed;
        Sprinting_Speed = SprintingSpeed;
        Jump_Height = JumpHeight;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            gameObject.transform.GetChild(0).GetComponent<MouseLook>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameObject.GetComponent<MouseLook>().enabled = false;
        }

        Vector3 newPosition = transform.position;
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        if (Input.GetKey(KeyCode.W))
        {
            newPosition += transform.forward * (Forward_Speed / 50);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition -= transform.forward * (Back_Speed / 50);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition += transform.right * (Side_Speed / 50);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPosition -= transform.right * (Side_Speed / 50);
        }

        

        newPosition.y = transform.position.y;

        transform.position = newPosition;
    }
}