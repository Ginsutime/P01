using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action Jumping = delegate { };
    public event Action Falling = delegate { };

    [SerializeField] CharacterController controller = null;
    [SerializeField] Transform cam = null;

    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float speed = 6f;

    float turnSmoothVelocity;
    bool _isMoving = false;
    bool _isJumping = false;

    private bool groundedPlayer;
    private float verticalVelocity;
    private float gravity = 10.0f;
    private float jumpForce = 7.0f;

    private void Start()
    {
        Idle?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveVector = new Vector3(0f, verticalVelocity, 0f);

        groundedPlayer = controller.isGrounded;

        // Code for gravity and jumping
        if (_isJumping)
        {
            verticalVelocity -= gravity * Time.deltaTime;

            if (groundedPlayer)
            {
                Idle?.Invoke();
                _isJumping = false;
            }
        }
        else if (horizontal > 0 || horizontal < 0 || vertical > 0 || vertical < 0)
        {
            CheckIfStartedMoving();

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                Debug.Log("Vert velocity is greater than 0");
                Invoke("CheckIfStartedJumping", 0f);
                Invoke("CheckIfLanded", 1f);
                _isJumping = true;
            }
        }
        else
        {
            CheckIfStoppedMoving();
            Debug.Log("Vert velocity is 0");

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                Debug.Log("Vert velocity is greater than 0");
                Invoke("CheckIfStartedJumping", 0f);
                Invoke("CheckIfLanded", 1f);
                _isJumping = true;
            }
        }

        controller.Move(moveVector * Time.deltaTime);

        // Code for moving
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            CheckIfStoppedMoving();
        }
    }

    private void CheckIfStartedMoving()
    {
        if (_isMoving == false)
        {
            StartRunning?.Invoke();
            Debug.Log("Started");
        }

        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isMoving == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }

        _isMoving = false;
    }

    private void CheckIfStartedJumping()
    {
        //if (_isJumping == false)
       // {
            Jumping?.Invoke();
            Debug.Log("Jumping");
       // }

        //_isJumping = true;
    }

    private void CheckIfLanded()
    {
        //if (_isJumping == true)
        //{
            Falling?.Invoke();
            Debug.Log("Landed");
        //}

        //_isJumping = false;
    }
}
