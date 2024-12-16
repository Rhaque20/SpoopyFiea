using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementback : MonoBehaviour
{
    public enum MovementMode { Walk, Sprint, Crouch};

    private MovementMode _moveMode = MovementMode.Walk;
    private Vector3 velocity;
    private CharacterController characterController;
    private float gravity = -9.81f ;
    private const float groundCheckDistance = 0.1f;
    private bool isGrounded, _exhausted = false, _isMoving = false;

    public LayerMask groundLayer;
    public float speed = 5f, sprintSpeed = 10f, crouchSpeed = 2f;
    public float jumpHeight = 3f;
    public float staminaDrainRate = 2f, staminaRegenRate = 1f;

    private float _remainingStamina = 1f;
    private float _moveSpeed;
    private float _capsuleHeight;
    public AudioClip Exaust;
    public AudioSource source;
    public AudioClip step;
    public AudioSource stwomp;
    public float stepRate = 0.5f;
    public float sprintRate = 0.3f;
    public float stepCoolDown;

    [SerializeField,Range(0,1f),Tooltip("Determines the minimum amount of stamina required to regenerate from exhaustion to allow sprinting")]
    private float _exhaustionThreshold = 0.5f;

    public static Action<bool> sprintingEvent, crouchingEvent;


    public float staminaRatio
    {
        get { return _remainingStamina/1f; }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _moveSpeed = speed;
        _capsuleHeight = characterController.height;

    }

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    //    uiManager.sprintStatus += staminaRatio;
    //}

    public void SprintMode(bool isSprinting)
    {
        if (_exhausted || _moveMode == MovementMode.Crouch)
        {
            return;
        }

        if (isSprinting)
        {
            _moveSpeed = sprintSpeed;
            _moveMode = MovementMode.Sprint;
        }
        else
        {
            _moveSpeed = speed;
            _moveMode = MovementMode.Walk;
            sprintingEvent?.Invoke(false);
        }
    }

    public void CrouchingMode(bool isCrouching)
    {
        if(isCrouching)
        {
            _moveMode = MovementMode.Crouch;
            _moveSpeed = crouchSpeed;
            characterController.height = _capsuleHeight/2f;
            crouchingEvent?.Invoke(true);
        }
        else
        {
            _moveMode = MovementMode.Walk;
            _moveSpeed = speed;
            characterController.height = _capsuleHeight;
            crouchingEvent?.Invoke(false);
        }
    }

    public void ProcessMove(Vector2 input)
    {
        
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        _isMoving = !(input == Vector2.zero);
        characterController.Move(transform.TransformDirection(moveDirection) * _moveSpeed * Time.deltaTime);
        characterController.Move(Vector3.zero * Time.deltaTime);

        //What are we doing here?
        Vector3 groundCheckPosition = transform.position -
            Vector3.up * (characterController.height / 2);
        isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckDistance, groundLayer);

        Debug.DrawRay(groundCheckPosition, Vector3.down, Color.red);
        //Debug.Log($"Ground Check Position : {groundCheckPosition}");

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        characterController.Move(velocity * Time.deltaTime);
        Debug.Log(velocity.y);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 3.0f * Mathf.Abs(gravity));
        }
    }
    // Update is called once per frame
    void Update()
    {
        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f)
        {
            if (_moveMode == MovementMode.Sprint)
            {
                stwomp.pitch = 1f + UnityEngine.Random.Range(-0.2f, 0.2f);
                stwomp.PlayOneShot(step);
                stepCoolDown = sprintRate;
            }
            else
            {
                stwomp.pitch = 1f + UnityEngine.Random.Range(-0.2f, 0.2f);
                stwomp.PlayOneShot(step);
                stepCoolDown = stepRate;
            }
        }
        if (_moveMode == MovementMode.Sprint && _isMoving)
        {
            _remainingStamina -= Time.deltaTime * staminaDrainRate;
            if(_remainingStamina <= 0f && !_exhausted)
            {
                SprintMode(false);
                _exhausted = true;
                source.clip = Exaust;
                source.Play();
            }
            UIManager.sprintStatus?.Invoke(staminaRatio,_exhausted);
            sprintingEvent?.Invoke(true);
        }

        if(_moveMode != MovementMode.Sprint && _remainingStamina < 1f)
        {
            _remainingStamina += Time.deltaTime * staminaRegenRate;
            UIManager.sprintStatus?.Invoke(staminaRatio,_exhausted);
            
        }

        if (_exhausted)
        {
            if (_remainingStamina >= _exhaustionThreshold)
            {
                _exhausted = false;
            }
        }
    }
}
