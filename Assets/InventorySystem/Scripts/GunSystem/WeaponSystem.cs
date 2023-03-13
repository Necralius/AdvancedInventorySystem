using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //WeaponSystem - Code Update Version 0.6 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - References on Instantiation -
    public PlayerController playerController => GetComponentInParent<PlayerController>();
    public Animator gunAnimator => GetComponent<Animator>();
    public GunProceduralRecoil recoilAsset => GetComponent<GunProceduralRecoil>();
    public GameObject gun3D_Model;
    #endregion

    #region - Gun States -
    [Space, Header("Gun State System")]
    public GunType gunType;
    public GunTypeIndexer gunTypeIndex;
    public GunState currentGunState;
    public bool shootingGun = false;
    private int currentGunStateIndex = 0;
    public List<GunState> gunStates;
    #endregion

    #region - Gun Bools -
    [Header("Gun Bools")]
    public bool canShoot = true;
    public bool GunHolded = false;
    public bool isAiming;
    public bool isReloading;
    #endregion

    #region - Shoot System -
    [Header("Shoot Settings")]

    [SerializeField] private float shootDamage = 25;
    [SerializeField] private float shootRange = 200;
    [SerializeField] private float rateOfFire = 0.12f;

    [HideInInspector, Range(1,10)] public int shootsPerTap = 1;
    [HideInInspector] public float bulletSpread = 1;

    RaycastHit hit;
    public LayerMask enemyMask;
    public ParticleSystem muzzleFlash;
    #endregion

    #region - Sound System -
    [Header("Sound System")]
    public AudioClip shootClip;
    public AudioClip emptyClip;
    public AudioClip drawWeaponClip;
    public AudioClip aimClip;
    public AudioClip reloadClip;
    public AudioClip fullReloadClip;

    [InspectorLabel("Only Assing if the gun is an Sniper")]
    public AudioClip boltActionSound;
    #endregion

    #region - Ammo System -
    [Header("Ammo Settings")]
    [SerializeField] private int currentMagAmmo = 31;
    [SerializeField] private int maxMagAmmo = 31;

    [SerializeField] private int maxInventoryAmmo = 240;
    [SerializeField] private int currentInventoryAmmo = 240;
    int amountToNeeded;
    #endregion

    #region - Aim System -
    [Space, Header("Aiming System")]

    public Vector3 aimPosition;
    [SerializeField] private Vector3 originalPosition = Vector3.zero;
    public float reloadingOffset_Z = 0.2f;
    public float defaultOffset_Z = 0f;
    public float aimingFOV = 45;
    public float normalFOV = 60;
    private float targetFOV;
    public float aimSpeed = 10;
    #endregion

    #region - Weapon Sway System -
    [Header("Weapon Sway System")]

    [Header("Position Sway")]
    public float swayAmount = 0.01f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    private Vector3 initialPosition;

    [Header("Rotation Sway")]
    public float rotationSwayAmount = 4f;
    public float maxRotationSwayAmount = 5f;
    public float smoothRotationAmount = 12f;

    private Quaternion initialRotation;

    public bool swayOnX = true;
    public bool swayOnY = true;
    public bool swayOnZ = true;

    [Header("Movment Sway")]
    public float movmentSwayXAmount = 0.05f;
    public float movmentSwayYAmount = 0.05f;

    public float movmentSwaySmooth = 6f;
    public float maxMovmentSwayAmount = 0.5f;

    [Header("Breathing Weapon Sway")]
    public float swayAmountA = 4;
    public float swayAmountB = 2;

    public float swayScale = 600;
    public float aimSwayScale = 6000;

    public float swayLerpSpeed = 14f;

    public float swayTime;
    public Vector3 swayPosition;
    #endregion

    //======================================//

    #region - BuildIn Methods -
    private void Start()
    {
        canShoot = true;
        initialPosition = playerController.weaponSwayObject.transform.localPosition;
        initialRotation = playerController.weaponSwayObject.transform.localRotation;

        currentGunStateIndex = 0;
        currentInventoryAmmo = maxInventoryAmmo;
        currentGunState = gunStates[currentGunStateIndex];

        playerController.ammoText.text = string.Format("{0}/{1}", currentMagAmmo, currentInventoryAmmo);
        playerController.gunStateText.text = currentGunState.ToString();
    }
    private void Update()
    {
        if (GameManager.Instance.inventoryIsOpen) return;//This statement lock the player position and inputs if the inventory is active
        ShootInput();
        InputGathering();
        CalculateAiming();
        CalculateWeaponSway();
    }
    public void SetGunState(bool state)
    {
        gameObject.SetActive(state);
        gun3D_Model.SetActive(state);
    }
    #endregion

    #region - Gun State Manegment -
    private void InstatiateGun()//This method manage automatically the gun states based in his type
    {
        gunStates.Clear();
        if (gunType.Equals(GunType.SemiAndAuto)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire, GunState.AutoFire };
        else if (gunType.Equals(GunType.Shotgun)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire };
        else if (gunType.Equals(GunType.OnlySemi)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire };
        else if (gunType.Equals(GunType.SniperType)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire };

        if (!GetComponent<GunProceduralRecoil>()) gameObject.AddComponent<GunProceduralRecoil>();
    }
    private void OnValidate() => InstatiateGun();//This
                                                 //
                                                 //call the state manegment every time that the inspector is changed
    #endregion

    #region - Weapon Sway Calculations -
    private void CalculateWeaponSway()
    {
        #region - Aim Effectors -
        //This statements change all the sway mechanics values whether or not the player is aiming
        float calcSwayAmount = isAiming ? swayAmount / 10 : swayAmount;
        float calcMaxAmount = isAiming ? maxAmount / 5 : maxAmount;
        float calcMovmentSwayXAmount = isAiming ? movmentSwayXAmount / 10 : movmentSwayXAmount;
        float calcMovmentSwayYAmount = isAiming ? movmentSwayYAmount / 10 : movmentSwayYAmount;
        float calcMaxMovmentSwayAmount = isAiming ? maxMovmentSwayAmount / 5 : maxMovmentSwayAmount;
        float calcRotationSwayAmount = isAiming ? rotationSwayAmount / 5 : rotationSwayAmount;
        float calcMaxRotationSwayAmount = isAiming ? maxRotationSwayAmount / 5 : maxRotationSwayAmount;
        #endregion

        #region - Weapon Position Sway Calculations -
        //This statements represent the weapon look movment sway calculations 
        float lookInputX = -playerController.lookInput.x * calcSwayAmount;
        float lookInputY = -playerController.lookInput.y * calcSwayAmount;

        lookInputX = Mathf.Clamp(lookInputX, -calcMaxAmount, calcMaxAmount);
        lookInputY = Mathf.Clamp(lookInputY, -calcMaxAmount, calcMaxAmount);

        Vector3 finalPosition = new Vector3(lookInputX, lookInputY, 0);
        playerController.weaponSwayObject.transform.localPosition = Vector3.Lerp(playerController.weaponSwayObject.transform.localPosition, finalPosition + initialPosition, smoothAmount * Time.deltaTime);
        #endregion

        #region - Movment Sway Calculations -
        //This statementes represent the weapon sway based on the player movment
        float movmentInputX = -playerController.movmentInput.x * calcMovmentSwayXAmount;
        float movmentInputY = -playerController.movmentInput.y * calcMovmentSwayYAmount;

        movmentInputX = Mathf.Clamp(movmentInputX, -calcMaxMovmentSwayAmount, calcMaxMovmentSwayAmount);
        movmentInputY = Mathf.Clamp(movmentInputY, -calcMaxMovmentSwayAmount, calcMaxMovmentSwayAmount);

        Vector3 movmentSwayFinalPosition = new Vector3(movmentInputX, movmentInputY, 0);
        playerController.weaponSwayObject.transform.localPosition = Vector3.Lerp(playerController.weaponSwayObject.transform.localPosition, movmentSwayFinalPosition + initialPosition, movmentSwaySmooth * Time.deltaTime);
        #endregion

        #region - Weapon Rotation Sway Calculations -
        //This statementes represent the weapon rotational sway calculations
        float rotationX = Mathf.Clamp(playerController.lookInput.y * calcRotationSwayAmount, -calcMaxRotationSwayAmount, calcMaxRotationSwayAmount);
        float rotationY = Mathf.Clamp(playerController.lookInput.x * calcRotationSwayAmount, -calcMaxRotationSwayAmount, calcMaxRotationSwayAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(swayOnX ? -rotationX : 0f, swayOnY ? -rotationY : 0f, swayOnZ ? -rotationY : 0f));

        playerController.weaponSwayObject.transform.localRotation = Quaternion.Slerp(playerController.weaponSwayObject.transform.localRotation, finalRotation * initialRotation, smoothRotationAmount * Time.deltaTime);
        #endregion

        #region - Breathing Idle Sway -
        //This statements use the LissajousCurve calculation to make an breathing idle procedural animation
        Vector3 targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAiming ? aimSwayScale : swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);

        playerController.weaponSwayObject.transform.localPosition = swayPosition;

        swayTime += Time.deltaTime;

        if (swayTime > 6.3f) swayTime = 0;
        #endregion
    }
    private Vector3 LissajousCurve(float Time, float A, float B) => new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));//This method return an calculation that is used to make an procedural horizontal and vertical wave that represent an breathing idle animation
    #endregion

    #region - Input Gathering -
    private void InputGathering()//This method nest all input calls 
    {
        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("ReloadKey")) && canShoot && !isReloading) ReloadWeapon();

        if (gunType.Equals(GunType.SniperType))
        {
            if (canShoot) gunAnimator.SetLayerWeight(1, isReloading ? 1f : 0f);
            else gunAnimator.SetLayerWeight(1, !canShoot ? 1f : 0f);
        }
        else gunAnimator.SetLayerWeight(1, isReloading ? 1f : 0f);
        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("ChangeGunStateKey"))) StateChange();

        isAiming = Input.GetMouseButton(1) && !playerController.isRunning;
        if (Input.GetMouseButtonDown(1) && !playerController.isRunning) AudioManager.Instance.PlayEffectSound(aimClip);
    }
    #endregion

    #region - Aiming Calculations -
    private void CalculateAiming()//This Method calculates and set the whole aim functionality 
    {
        targetFOV = isAiming ? aimingFOV : normalFOV;//This statement change the player aim FOV between some conditions

        playerController.currentCamera.fieldOfView = Mathf.Lerp(playerController.currentCamera.fieldOfView, targetFOV, aimSpeed * Time.deltaTime);//This stament set the field of view using the Mathf.Lerp, whitch means that the FOV value is linearly

        if (isAiming && !isReloading) playerController.aimObject.transform.localPosition = Vector3.Lerp(playerController.aimObject.transform.localPosition, new Vector3(aimPosition.x, aimPosition.y, aimPosition.z + defaultOffset_Z), aimSpeed * Time.deltaTime);
        else if (isAiming && isReloading) playerController.aimObject.transform.localPosition = Vector3.Lerp(playerController.aimObject.transform.localPosition, new Vector3(aimPosition.x, aimPosition.y, aimPosition.z + reloadingOffset_Z), aimSpeed * Time.deltaTime);
        else playerController.aimObject.transform.localPosition = Vector3.Lerp(playerController.aimObject.transform.localPosition, originalPosition, aimSpeed * Time.deltaTime);
    }
    #endregion

    #region - Reload System -
    private void ReloadWeapon()//This Method execut the reload calculations
    {
        if (currentMagAmmo == maxMagAmmo) return;//This statement verifies if the ammo mag is full

        amountToNeeded = maxMagAmmo - currentMagAmmo;//This statement calculates the needed ammo for the reload action

        if (amountToNeeded == maxMagAmmo) AudioManager.Instance.PlayEffectSound(fullReloadClip);//This statement verifies if the mag will be fully replaced by a new one an plays a difrent reload sound
        else if (amountToNeeded < maxMagAmmo) AudioManager.Instance.PlayEffectSound(reloadClip);//This statement play an reload sound 

        gunAnimator.SetInteger("ReloadIndex", amountToNeeded == maxMagAmmo ? 0 : 1);//This statement changes the reload Index based on his type (Full reloadType || Default Reload Type)
        isReloading = true;
        gunAnimator.SetBool("IsReloading", true);
    }
    public void StopReloadAnim() => gunAnimator.SetBool("IsReloading", false);//This method is used in an animation event to stop the reload animation
    public void EndReload()//This statement set the reload values and update the gun UI
    {
        if (currentInventoryAmmo - amountToNeeded > 0)
        {
            currentInventoryAmmo -= amountToNeeded;
            currentMagAmmo = maxMagAmmo;
        }
        else if (currentInventoryAmmo - amountToNeeded == 0)
        {
            currentMagAmmo += currentInventoryAmmo;
            currentInventoryAmmo = 0;
        }
        else if (currentInventoryAmmo - amountToNeeded < 0)
        {
            currentInventoryAmmo = 0;
            currentMagAmmo = amountToNeeded;
        }

        amountToNeeded = 0;
        playerController.ammoText.text = string.Format("{0}/{1}", currentMagAmmo, currentInventoryAmmo);

        isReloading = false;
    }
    #endregion

    #region - Gun State Transitioning -
    private void StateChange()//This method linearly change the gun state
    {
        if (GunHolded)
        {
            if (currentGunStateIndex < gunStates.Count)
            {
                currentGunStateIndex++;
                if (currentGunStateIndex == gunStates.Count) currentGunStateIndex = 0;
            }
            currentGunState = gunStates[currentGunStateIndex];
            playerController.gunStateText.text = currentGunState.ToString();
        }
    }
    #endregion

    #region - Shoot Behavior -
    private void ShootInput()//This method executes the shoot behavior limitations, calls and input
    {
        if (canShoot && !playerController.isRunning && !isReloading)
        {
            if (currentMagAmmo <= 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    AudioManager.Instance.PlayEffectSound(emptyClip);
                    return;
                }
            }
            else if (currentGunState == GunState.AutoFire)
            {
                if (Input.GetMouseButton(0))
                {
                    shootingGun = true;
                    canShoot = false;
                    StartCoroutine(ShootBehavior(rateOfFire));
                }
                else shootingGun = false;
            }
            else if (currentGunState == GunState.SemiFire || currentGunState == GunState.BurstFire)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    canShoot = false;
                    StartCoroutine(ShootBehavior(rateOfFire));
                }
            }
        }
    }
    private IEnumerator ShootBehavior(float rateOfFire)//This method fires the raycast, sound, particle and all shoot behavior of the weapon and manage the Rate of Fire.
    {
        if (gunType.Equals(GunType.SniperType)) gunAnimator.SetTrigger("Shooted");
        ShootGun();
        yield return new WaitForSeconds(rateOfFire);
        if (!gunType.Equals(GunType.SniperType)) canShoot = true;
    }
    public void ShootGun()
    {
        currentMagAmmo--;
        PlayerController.Instance.isMoving = true;
        muzzleFlash.Emit(1);
        recoilAsset.RecoilFire(isAiming);

        playerController.ammoText.text = string.Format("{0}/{1}", currentMagAmmo, currentInventoryAmmo);

        AudioManager.Instance.PlayShootSound(shootClip);

        if (gunType == GunType.Shotgun) for (int i = 0; i < shootsPerTap; i++) RaycastCalculation();
        else if (currentGunState == GunState.BurstFire) for (int i = 0; i < shootsPerTap; i++) RaycastCalculation();
        else RaycastCalculation();
    }
    #endregion

    #region - Raycast Method -
    private void RaycastCalculation()//This method execute an raycast calculation based on the player camera
    {
        if (Physics.Raycast(playerController.currentCamera.transform.position, playerController.currentCamera.transform.forward, out hit, shootRange))
        {
            try
            {
                if (hit.transform.CompareTag("Item")) return;

                GameObject impactParticle= playerController.particlesDatabase.GetParticleByBaseTagAndType(hit.transform.tag, "Impact");
                GameObject decalParticle = playerController.particlesDatabase.GetParticleByBaseTagAndType(hit.transform.tag, "Decal");

                //This statementes represent the particles get from particle DataBase and the Instatiation
                if (!(impactParticle.Equals(null))) Instantiate(impactParticle, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                if (!(decalParticle.Equals(null))) Instantiate(decalParticle, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    AudioManager.Instance.PlayEffectSound(playerController.hitMarkerSound);
                    StartCoroutine(HitMarker());
                }
            }
            catch(System.Exception ex)
            {
                Debug.Log("An error Ocurred!");
            }
        }
    }
    #endregion

    #region - Hit Marker System -

    IEnumerator HitMarker()//This method execute the HitMarker action
    {
        playerController.hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        playerController.hitMarker.SetActive(false);
    }
    #endregion

    #region - UI Update -
    public void UpdateGunUI()//This Method update whole gun UI assets
    {
        playerController.ammoText.text = string.Format("{0}/{1}", currentMagAmmo, currentInventoryAmmo);
        playerController.gunStateText.text = currentGunState.ToString();
    }
    #endregion

    #region - Animation Events -
    //This methods represent animations events methods used to very especific functionalities
    public void PlaySniperBoltAction() => AudioManager.Instance.PlayEffectSound(boltActionSound);
    public void DrawWeapon() => AudioManager.Instance.PlayEffectSound(drawWeaponClip);
    public void SetCanShoot()
    {
        canShoot = true; gunAnimator.SetLayerWeight(1, 0);
    }
    #endregion
}