using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.Presets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public float movementSpeed;
    [SerializeField] public float movementDrag;

    public float playerHeight;
    public LayerMask whatIsGround;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    bool _readyToJump;

    bool _grounded;
    bool _crouching;

    public Transform orientation;

    float _horizontalInput;
    float _verticalInput;

    Vector3 _movementDirection;

    [SerializeField] Rigidbody _rb;

    private void Start()
    {
        _rb.freezeRotation = true;
        _readyToJump = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        UpdateInput();
        SpeedControl();

        if (_grounded)
        {
            _rb.drag = movementDrag;
        } else
        {
            _rb.drag = 0;
        }
    }

    private void UpdateInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(_grounded && !_crouching && Input.GetKeyDown(crouchKey))
        {
            Crouch(true);
        }

        if (_crouching && Input.GetKeyUp(crouchKey))
        {
            Crouch(false);
        }
    }

    private void Move()
    {
        _movementDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if(_grounded)
        {
            _rb.AddForce(_movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        } else if(!_grounded)
        {
            _rb.AddForce(_movementDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        if(flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
        }
    }
    private void Crouch(bool pressed)
    {
        if(pressed)
        {
            _crouching = true;
            transform.localScale = new Vector3(1f, transform.localScale.y / 2f, 1f);
        } else
        {
            _crouching = false;
            transform.localScale = new Vector3(1f, transform.localScale.y * 2f, 1f);
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }
}
