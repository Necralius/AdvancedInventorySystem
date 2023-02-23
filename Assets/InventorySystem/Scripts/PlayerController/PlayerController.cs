using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region - View Data -
    [SerializeField, Range(1, 10)] private float mouseSensitivity = 3;

    private float xRotation = 0f;

    [HideInInspector] public Vector2 lookInput;
    [HideInInspector] public Vector2 movmentInput;
    #endregion

    #region - References on Instantiation -
    private Transform playerBody => transform;
    public Camera currentCamera => GetComponentInChildren<Camera>();
    private CharacterController playerController => GetComponent<CharacterController>();
    public ObjectPooler pooler => ObjectPooler.Instance;
    #endregion

    #region - Movment Data -
    public float currentSpeed;
    public float walkSpeed;
    public float runSpeed;
   
    public float gravity = -9.81f;
    public float jumpHeight;
    Vector3 velocity;

    #endregion

    #region - State Bools -
    public bool isWalking;
    public bool isRunning;
    public bool isGrounded;

    public bool isCrouched;
    public bool isProning;
    public bool isAiming;

    public Animator controllerAnimator;

    public WeaponSystem equippedGun;

    #endregion

    #region - Stance Data -
    public PlayerState currentState;
    [Space]
    public PlayerState playerStandState;
    public PlayerState playerCrouchState;
    public PlayerState playerProneState;

    public Transform headPosition;
    public LayerMask checkMask;
    public float checkRadius;

    public float stateHeightChangeSpeed;

    #endregion

    public Transform cameraBody;
    public Transform aimObject;
    public GameObject weaponSwayObject;

    #region - UI System -
    [Header("UI System")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI gunStateText;

    #endregion

    #region - Key Code System -
    [Header("Key Code System")]
    public KeyCode crouchKeyCode;
    public KeyCode proneKeyCode;
    public KeyCode inventoryKeyCode;
    public KeyCode sprintKeyCode;
    #endregion

    #region - BuildIn Methods -
    private void Start() => Stand();
    void Update()
    {
        InputGet();
        if (GameManager.Instance.inventoryIsOpen) return;
        CalculateMovment();
        CalculateView();
        CalculateState();
    }
    #endregion

    #region - Input Gathering -
    private void InputGet()
    {
        if (Input.GetKeyDown(inventoryKeyCode)) GameManager.Instance.ChangeInventoryState();

        if (GameManager.Instance.inventoryIsOpen) return;

        lookInput = new Vector2(Input.GetAxis("Mouse X") * (mouseSensitivity * 100) * Time.deltaTime, Input.GetAxis("Mouse Y") * (mouseSensitivity * 100) * Time.deltaTime);
        movmentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(crouchKeyCode) && CheckPlayerState()) Crouch();
        if (Input.GetKeyDown(proneKeyCode) && CheckPlayerState()) Prone();

        isGrounded = playerController.isGrounded;
        isRunning = Input.GetKey(sprintKeyCode) && isWalking;
        isWalking = movmentInput != Vector2.zero && isGrounded;
    }
    #endregion

    #region - Movment Calculations -
    private void CalculateMovment()
    {
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        Vector3 movmentVector = transform.right * movmentInput.x + transform.forward * movmentInput.y;

        playerController.Move(movmentVector * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Stand();
        }

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity);
    }
    #endregion

    #region - View Calculations -
    private void CalculateView()
    {
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraBody.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookInput.x);
    }
    #endregion

    #region - Player State Calculation -
    private void CalculateState()
    {
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        playerController.height = Mathf.Lerp(playerController.height, currentState.StateHeight, stateHeightChangeSpeed * Time.deltaTime);
        currentSpeed *= currentState.StateSpeedModifier;

        controllerAnimator.SetBool("IsWalking", isWalking);
        controllerAnimator.SetBool("IsRunning", isRunning);
    }
    private void Crouch()
    {
        if (currentState.state == playerCrouchState.state)
        {
            currentState.SetUp(playerStandState);
            return;
        }
        else currentState.SetUp(playerCrouchState);
    }
    private void Prone()
    {
        if (currentState.state == playerProneState.state)
        {
            currentState.SetUp(playerCrouchState);
            return;
        }
        else currentState.SetUp(playerProneState);
    }
    private void Stand() => currentState.SetUp(playerStandState);
    private bool CheckPlayerState() => Physics.CheckSphere(headPosition.position, checkRadius, checkMask);
    #endregion
}