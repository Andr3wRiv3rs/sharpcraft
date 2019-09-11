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
    private bool Frozen = false;

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

    public void LockPlayer()
    {
        Frozen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.GetComponent<MouseLook>().enabled = false;
        Camera.main.gameObject.GetComponent<MouseLook>().enabled = false;
    }

    public void UnlockPlayer()
    {
        Frozen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.GetComponent<MouseLook>().enabled = true;
        Camera.main.gameObject.GetComponent<MouseLook>().enabled = true;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = transform.position;
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        if (!Frozen)
        {
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
        }


        

        newPosition.y = transform.position.y;

        transform.position = newPosition;
    }
}