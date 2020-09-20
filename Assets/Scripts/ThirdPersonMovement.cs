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
    public event Action Sprinting = delegate { };
    public event Action Casting = delegate { };
    public event Action Death = delegate { };
    public event Action Injured = delegate { };

    [SerializeField] CharacterController controller = null;
    [SerializeField] Transform cam = null;

    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float normspeed = 6f;
    [SerializeField] float sprintspeed = 10f;

    float turnSmoothVelocity;
    bool _isMoving = false;
    bool _isJumping = false;
    bool _isSprinting = false;
    bool isInjured = false;
    bool isCasting = false;

    public bool movingPlayer = false;
    public bool groundedPlayer;
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
        if (isCasting == false && isInjured == false)
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
            // Code for checking if casting
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (groundedPlayer)
                {
                    Invoke("CheckIfStartedCasting", 0f);
                    Invoke("CheckIfStoppedCasting", 2f);
                }
            }
            else if (horizontal > 0 || horizontal < 0 || vertical > 0 || vertical < 0)
            {
                verticalVelocity -= gravity * Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    movingPlayer = true;
                    CheckIfStartedMoving();
                }
                else
                {
                    movingPlayer = false;
                }

                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    CheckIfStartedSprinting();
                }

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
                {
                    verticalVelocity = jumpForce;
                    Debug.Log("Jump and Land");
                    Invoke("CheckIfStartedJumping", 0f);
                    Invoke("CheckIfLanded", 1f);
                    _isJumping = true;
                }
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
                CheckIfStoppedMoving();
                CheckIfStoppedSprinting();
                Debug.Log("Idle");

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
                {
                    verticalVelocity = jumpForce;
                    Debug.Log("Jump and Land");
                    Invoke("CheckIfStartedJumping", 0f);
                    Invoke("CheckIfLanded", 1f);
                    _isJumping = true;
                }
                else
                {
                    _isJumping = false;
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

                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    controller.Move(moveDir.normalized * sprintspeed * Time.deltaTime);
                }
                else
                {
                    controller.Move(moveDir.normalized * normspeed * Time.deltaTime);
                }
            }
            else
            {
                CheckIfStoppedMoving();
                CheckIfStoppedSprinting();
            }
        }
    }

    private void CheckIfDead()
    {
        Death?.Invoke();
        Debug.Log("Dead");

        isInjured = true;
    }

    private void CheckIfInjured()
    {
        Injured?.Invoke();
        Debug.Log("Injured");

        isInjured = true;
    }

    private void CheckIfNotInjured()
    {
        Idle?.Invoke();
        Debug.Log("Out of injured anims");

        isInjured = false;
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

    private void CheckIfStartedSprinting()
    {
        if (_isSprinting == false)
        {
            Sprinting?.Invoke();
            Debug.Log("Started sprinting");
        }

        _isSprinting = true;
    }

    private void CheckIfStoppedSprinting()
    {
        if (_isSprinting == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped sprinting");
        }

        _isSprinting = false;
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
        Jumping?.Invoke();
        Debug.Log("Jumping");
    }

    private void CheckIfLanded()
    {
        Falling?.Invoke();
        Debug.Log("Landed");
    }

    private void CheckIfSprinting()
    {
        Sprinting?.Invoke();
        Debug.Log("Sprinting");
    }

    private void CheckIfStartedCasting()
    {
        Casting?.Invoke();
        Debug.Log("Casting");

        isCasting = true;
    }

    private void CheckIfStoppedCasting()
    {
        Idle?.Invoke();
        Debug.Log("Casting");

        isCasting = false;
    }
}
