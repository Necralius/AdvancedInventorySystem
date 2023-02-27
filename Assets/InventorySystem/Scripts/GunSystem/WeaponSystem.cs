using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    #region - References on Instantiation -
    public PlayerController playerController => GetComponentInParent<PlayerController>();
    public Animator gunAnimator => GetComponent<Animator>();
    public GunProceduralRecoil recoilAsset => GetComponent<GunProceduralRecoil>();
    #endregion

    public GunType gunType;

    [Space]

    #region - Gun States -
    public GunState currentGunState;
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

    #region - Sound System
    [Header("Sound System")]
    public AudioClip shootClip;
    public AudioClip emptyClip;
    public AudioClip drawWeaponClip;
    public AudioClip aimClip;
    public AudioClip reloadClip;
    #endregion

    #region - Ammo -
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
    private Vector3 originalPosition = Vector3.zero;
    public float reloadingOffset_Z = 0.2f;
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
    #endregion

    #region - BuildIn Methods -
    private void Start()
    {
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
        if (GameManager.Instance.inventoryIsOpen) return;
        ShootInput();
        InputGathering();
        CalculateAiming();
        CalculateWeaponSway();
    }
    private void InstatiateGun()
    {
        gunStates.Clear();
        if (gunType.Equals(GunType.SemiAndAuto)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire, GunState.AutoFire };
        else if (gunType.Equals(GunType.Shotgun)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire };
        else if (gunType.Equals(GunType.OnlySemi)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire };
        else if (gunType.Equals(GunType.SniperType)) gunStates = new List<GunState> { GunState.Locked, GunState.SemiFire };

        if (!GetComponent<GunProceduralRecoil>()) gameObject.AddComponent<GunProceduralRecoil>();
    }
    private void OnValidate() => InstatiateGun();
    #endregion

    private void CalculateWeaponSway()
    {
        #region - Aim Effectors -
        float calcSwayAmount = isAiming ? swayAmount / 10 : swayAmount;
        float calcMaxAmount = isAiming ? maxAmount / 5 : maxAmount;
        float calcMovmentSwayXAmount = isAiming ? movmentSwayXAmount / 10 : movmentSwayXAmount;
        float calcMovmentSwayYAmount = isAiming ? movmentSwayYAmount / 10 : movmentSwayYAmount;
        float calcMaxMovmentSwayAmount = isAiming ? maxMovmentSwayAmount / 5 : maxMovmentSwayAmount;
        float calcRotationSwayAmount = isAiming ? rotationSwayAmount / 5 : rotationSwayAmount;
        float calcMaxRotationSwayAmount = isAiming ? maxRotationSwayAmount / 5 : maxRotationSwayAmount;
        #endregion

        #region - Weapon Position Sway Calculations -
        float lookInputX = -playerController.lookInput.x * calcSwayAmount;
        float lookInputY = -playerController.lookInput.y * calcSwayAmount;

        lookInputX = Mathf.Clamp(lookInputX, -calcMaxAmount, calcMaxAmount);
        lookInputY = Mathf.Clamp(lookInputY, -calcMaxAmount, calcMaxAmount);

        Vector3 finalPosition = new Vector3(lookInputX, lookInputY, 0);
        playerController.weaponSwayObject.transform.localPosition = Vector3.Lerp(playerController.weaponSwayObject.transform.localPosition, finalPosition + initialPosition, smoothAmount * Time.deltaTime);
        #endregion

        #region - Movment Sway Calculations -
        float movmentInputX = -playerController.movmentInput.x * calcMovmentSwayXAmount;
        float movmentInputY = -playerController.movmentInput.y * calcMovmentSwayYAmount;

        movmentInputX = Mathf.Clamp(movmentInputX, -calcMaxMovmentSwayAmount, calcMaxMovmentSwayAmount);
        movmentInputY = Mathf.Clamp(movmentInputY, -calcMaxMovmentSwayAmount, calcMaxMovmentSwayAmount);

        Vector3 movmentSwayFinalPosition = new Vector3(movmentInputX, movmentInputY, 0);
        playerController.weaponSwayObject.transform.localPosition = Vector3.Lerp(playerController.weaponSwayObject.transform.localPosition, movmentSwayFinalPosition + initialPosition, movmentSwaySmooth * Time.deltaTime);
        #endregion

        #region - Weapon Rotation Sway Calculations -
        float rotationX = Mathf.Clamp(playerController.lookInput.y * calcRotationSwayAmount, -calcMaxRotationSwayAmount, calcMaxRotationSwayAmount);
        float rotationY = Mathf.Clamp(playerController.lookInput.x * calcRotationSwayAmount, -calcMaxRotationSwayAmount, calcMaxRotationSwayAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(swayOnX ? -rotationX : 0f, swayOnY ? -rotationY : 0f, swayOnZ ? -rotationY : 0f));

        playerController.weaponSwayObject.transform.localRotation = Quaternion.Slerp(playerController.weaponSwayObject.transform.localRotation, finalRotation * initialRotation, smoothRotationAmount * Time.deltaTime);
        #endregion
    }

    #region - Input Gathering -
    private void InputGathering() 
    {
        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("ReloadKey"))) ReloadWeapon();

        if (gunType.Equals(GunType.SniperType))
        {
            if (canShoot)
            {
                gunAnimator.SetLayerWeight(1, isReloading ? 1f : 0f);
            }
            else gunAnimator.SetLayerWeight(1, !canShoot ? 1f : 0f);
        }
        else gunAnimator.SetLayerWeight(1, isReloading ? 1f : 0f);
        if (Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("ChangeGunStateKey"))) StateChange();

        isAiming = Input.GetMouseButton(1) && !playerController.isRunning;
    }
    #endregion

    #region - Aiming Calculations -
    private void CalculateAiming()
    {
        targetFOV = isAiming ? aimingFOV : normalFOV;

        playerController.currentCamera.fieldOfView = Mathf.Lerp(playerController.currentCamera.fieldOfView, targetFOV, aimSpeed * Time.deltaTime);

        if (isAiming && !isReloading) playerController.aimObject.transform.localPosition = Vector3.Lerp(playerController.aimObject.transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
        else if (isAiming && isReloading) playerController.aimObject.transform.localPosition = Vector3.Lerp(playerController.aimObject.transform.localPosition, new Vector3(aimPosition.x, aimPosition.y, aimPosition.z + reloadingOffset_Z), aimSpeed * Time.deltaTime);
        else playerController.aimObject.transform.localPosition = Vector3.Lerp(playerController.aimObject.transform.localPosition, originalPosition, aimSpeed * Time.deltaTime);
    }
    #endregion

    #region - Reload System -
    private void ReloadWeapon()
    {
        if (currentMagAmmo == maxMagAmmo) return;

        amountToNeeded = maxMagAmmo - currentMagAmmo;

        gunAnimator.SetInteger("ReloadIndex", amountToNeeded == maxMagAmmo ? 0 : 1);
        isReloading = true;
        gunAnimator.SetBool("IsReloading", true);
    }
    public void StopReloadAnim() => gunAnimator.SetBool("IsReloading", false);
    public void EndReload()
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
    private void StateChange()
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
    private void ShootInput()
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
                    canShoot = false;
                    StartCoroutine(ShootBehavior(rateOfFire));
                }
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
    private IEnumerator ShootBehavior(float rateOfFire)
    {
        if (gunType.Equals(GunType.SniperType))
        {
            gunAnimator.SetTrigger("Shooted");
        }
        ShootGun();
        yield return new WaitForSeconds(rateOfFire);
        if (!gunType.Equals(GunType.SniperType)) canShoot = true;
    }
    public void ShootGun()
    {
        currentMagAmmo--;
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
    private void RaycastCalculation()
    {
        if (Physics.Raycast(playerController.transform.position, playerController.transform.forward, out hit, shootRange, enemyMask))
        {
            //playerController.pooler.SpawnFromPool(hit.transform.tag, hit.point, Quaternion.Euler(-hit.normal));
            
        }
    }
    #endregion

    #region - Animation Events -
    public void DrawWeapon() => AudioManager.Instance.PlayEffectSound(drawWeaponClip);
    public void SetCanShoot()
    {
        canShoot = true; gunAnimator.SetLayerWeight(1, 0);
    }
    #endregion
}