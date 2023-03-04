using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region - Singleton Pattern -
    public static PlayerController Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - View Data -
    [Header("Player Aspects")]
    public PlayerStats playerStats;

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
    [SerializeField] private float currentSpeed;
   
    public float gravity = -9.81f;
    public float jumpHeight;
    Vector3 velocity;

    #endregion

    #region - State Bools -

    [Header("Player State")]
    public bool isWalking;
    public bool canRun;
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

    #region - Aim System -
    public Transform aimObject;
    #endregion

    #region - CameraHolder -
    public Transform cameraBody;
    #endregion

    #region - Weapon Sway System -
    public GameObject weaponSwayObject;
    #endregion

    #region - UI System -
    [Header("UI System")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI gunStateText;

    #endregion

    //======================================//

    #region - Method Area -

    #region - BuildIn Methods -
    private void Start()
    {
        Stand();
        playerStats.mouseSensitivity *= 100;
    }
    void Update()
    {
        InputGet();
        CalculateView();
        CalculateState();
        CalculateMovment();
    }
    #endregion

    #region - Input Gathering -
    private void InputGet()
    {
        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("InventoryKey"))) GameManager.Instance.ChangeInventoryState();

        if (GameManager.Instance.inventoryIsOpen)
        {
            lookInput = Vector2.zero;
            movmentInput = Vector2.zero;

            isWalking = false;
            isRunning = false;
            isAiming = false;

            return;
        }

        lookInput = new Vector2(Input.GetAxis("Mouse X") * playerStats.mouseSensitivity * Time.deltaTime, Input.GetAxis("Mouse Y") * playerStats.mouseSensitivity * Time.deltaTime);
        movmentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("CrouchKey")) && !CheckPlayerState()) Crouch();
        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("ProneKey")) && !CheckPlayerState()) Prone();

        isGrounded = playerController.isGrounded;
        isRunning = Input.GetKey(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("SprintKey")) && isWalking && canRun;
        isWalking = movmentInput != Vector2.zero && isGrounded;
        isAiming = Input.GetKey(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("AimKey")) && !isRunning;

        #region - Speed Effector Set -

        currentSpeed = isRunning ? playerStats.runSpeed : playerStats.walkSpeed;
        currentSpeed *= playerStats.currentSpeedEffector;

        #endregion

        if (equippedGun) equippedGun.isAiming = this.isAiming;
    }
    #endregion

    #region - Movment Calculations -
    private void CalculateMovment()
    {
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        Vector3 movmentVector = transform.right * movmentInput.x + transform.forward * movmentInput.y;

        playerController.Move(movmentVector * currentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("JumpKey")) && isGrounded)
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
        playerController.height = Mathf.Lerp(playerController.height, currentState.StateHeight, stateHeightChangeSpeed * Time.deltaTime);

        if (isAiming) playerStats.currentSpeedEffector = playerStats.aimSpeedEffector;
        else if (currentState.Equals(playerStandState)) playerStats.currentSpeedEffector = playerStats.standSpeedEffector;
        else if (currentState.Equals(playerCrouchState)) playerStats.currentSpeedEffector = playerStats.crouchSpeedEffector;
        else if (currentState.Equals(playerProneState)) playerStats.currentSpeedEffector = playerStats.proneSpeedEffector;

        controllerAnimator.speed = playerStats.currentSpeedEffector;
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

    #endregion
}