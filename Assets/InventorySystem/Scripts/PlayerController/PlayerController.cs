using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer
    //Player Controller - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

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
    public CharacterController playerController => GetComponent<CharacterController>();
    public ObjectPooler pooler => ObjectPooler.Instance;
    public TerrainTextureCheck terrainTextureChecker;
    public ParticlesDatabase particlesDatabase;

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
    public bool isMoving;
    public bool canRun;
    public bool isRunning;
    public bool isCrouched;
    public bool isProning;
    public bool isAiming;

    public bool isOnNonTerrainGround;
    public bool isGrounded;
    public bool isOnTerrain;

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

    #region - Dynamic Crosshair -
    [Space, Header("Dynamic Crosshair")]
    public RectTransform reticle;

    public float restingSize;
    public float maxSize;
    public float changeSpeed;
    private float currentSize;

    private bool crosshairState;
    #endregion

    #region - UI System -
    [Header("UI System")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI gunStateText;
    #endregion

    //======================================//

    #region - Method Area -

    #region - Dynamic Cross Hair -
    private void UpdateCrossHair()//This method execute the CrossHair functionality
    {
        isMoving = MovingCheck();
        crosshairState = CrossHairStateManegment();

        if (isMoving) currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * changeSpeed);
        else currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * changeSpeed);

        reticle.sizeDelta = new Vector2(currentSize, currentSize);
        reticle.gameObject.SetActive(crosshairState);
    }
    private bool CrossHairStateManegment()//This method change the CrossHair state based in some conditions
    {
        if (GameManager.Instance.inventoryIsOpen) return false;
        else if (isAiming) return false;
        return true;
    }
    private bool MovingCheck() => movmentInput != Vector2.zero || lookInput != Vector2.zero;//This method verifies if the player is moving
    #endregion

    #region - BuildIn Methods -
    private void Start()
    {
        Stand();
        terrainTextureChecker = GetComponent<TerrainTextureCheck>();
        playerStats.mouseSensitivity *= 100;
    }
    void Update()
    {
        InputGet();
        CalculateView();
        CalculateState();
        CalculateMovment();
        GroundChecks();
        UpdateCrossHair();
    }
    #endregion

    #region - Input Gathering -
    private void InputGet()//This method nest all interaction inputs
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

        isRunning = Input.GetKey(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("SprintKey")) && isWalking && canRun;
        isWalking = movmentInput != Vector2.zero && isGrounded;
        isAiming = Input.GetKey(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("AimKey")) && !isRunning;

        #region - Speed Effector Set -
        //This statments changes the speed effectors based in the actual player state
        currentSpeed = isRunning ? playerStats.runSpeed : playerStats.walkSpeed;
        currentSpeed *= playerStats.currentSpeedEffector;
        #endregion

        if (equippedGun) equippedGun.isAiming = this.isAiming;
    }
    #endregion

    #region - Ground Checking -
    private void GroundChecks()//This method verifies if the player is on ground and if it is on a terrain ground
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerController.bounds.extents.y + 0.5f))
        {
            isGrounded = true;
            isOnTerrain = hit.collider != null && hit.collider.CompareTag("GrassTerrain");
        }
        else isGrounded = false;
    }
    #endregion

    #region - Movment Calculations -
    private void CalculateMovment()//This method calculate and set the movment values, considering gravity and considering the current input
    {
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        Vector3 movmentVector = transform.right * movmentInput.x + transform.forward * movmentInput.y;

        playerController.Move(movmentVector * currentSpeed * Time.deltaTime);

        GetComponent<FootstepSystem>().currentSpeed = playerController.velocity.magnitude;//This statment transfer the current player velocity magnitude to the FootStep audio system

        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("JumpKey")) && isGrounded)//This statment execute the jump calculations
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Stand();
        }

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity);
    }
    #endregion

    #region - View Calculations -
    private void CalculateView()//This method execute and set the view calculations, considering the current player look input (Mouse movment) 
    {
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraBody.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookInput.x);
    }
    #endregion

    #region - Player State Calculation -
    private void CalculateState()//This method calculates the current player stance state (verifies if it is crouching, proning or if it is standing)
    {
        playerController.height = Mathf.Lerp(playerController.height, currentState.StateHeight, stateHeightChangeSpeed * Time.deltaTime);//This statment linearly changes the player controller collider height

        //This statmentes changes the players speed effectores based in the current player stance state
        if (isAiming) playerStats.currentSpeedEffector = playerStats.aimSpeedEffector;
        else if (currentState.Equals(playerStandState)) playerStats.currentSpeedEffector = playerStats.standSpeedEffector;
        else if (currentState.Equals(playerCrouchState)) playerStats.currentSpeedEffector = playerStats.crouchSpeedEffector;
        else if (currentState.Equals(playerProneState)) playerStats.currentSpeedEffector = playerStats.proneSpeedEffector;

        //This statments execute some values set
        controllerAnimator.speed = playerStats.currentSpeedEffector;
        controllerAnimator.SetBool("IsWalking", isWalking);
        controllerAnimator.SetBool("IsRunning", isRunning);
    }
    private void Crouch()//This method tries to execute the crouch stance state
    {
        if (currentState.state == playerCrouchState.state)
        {
            currentState.SetUp(playerStandState);
            return;
        }
        else currentState.SetUp(playerCrouchState);
    }
    private void Prone()//This method tries to execute the prone stance state
    {
        if (currentState.state == playerProneState.state)
        {
            currentState.SetUp(playerCrouchState);
            return;
        }
        else currentState.SetUp(playerProneState);
    }
    private void Stand() => currentState.SetUp(playerStandState);//This method tries to execute the stand stance state
    private bool CheckPlayerState() => Physics.CheckSphere(headPosition.position, checkRadius, checkMask);//This method check if the player can change the current stance state to an higher stance state
    #endregion

    #endregion
}