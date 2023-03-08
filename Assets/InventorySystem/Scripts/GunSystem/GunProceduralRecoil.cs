using UnityEngine;

public class GunProceduralRecoil : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //GunProceduralRecoil - Code Update Version 0.4 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Class References -
    public GameObject Weapon => gameObject;
    public GameObject CameraPos => GameObject.FindGameObjectWithTag("PlayerCam");
    #endregion

    #region - Weapon Recoil Vectors -
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Vector3 currentPosition;
    private Vector3 targetPosition;
    #endregion

    #region - Recoil Values -
    [SerializeField] private float recoilX = -3;
    [SerializeField] private float recoilY = 3;
    [SerializeField] private float recoilZ = 2;

    [SerializeField] private float snappiness = 6;
    [SerializeField] private float returnSpeed = 20;

    #region - Aim Value Effector -
    public float RecoilReduct = 1;
    public float aimReductionFactor = 0.3f;
    #endregion

    public float Z_KickBack = 0.4f;
    float Z_Current_KickBack = 0.4f;
    public bool isAiming;
    #endregion

    #region - Recoil Calculation -
    private void Update() => RecoilCalculation();
    private void RecoilCalculation()//This method use the Vector3.Lerp to interpolate between two vectors to apply the recoil, one vector represents the weapon position with the recoil values, the other vector literally applis the recoil to the current position and rotation
    {
        if (isAiming) Z_Current_KickBack = Z_KickBack * aimReductionFactor;
        else Z_Current_KickBack = Z_KickBack;

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);
        Weapon.transform.localPosition = currentPosition;

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        var newRot = new Vector3(currentRotation.x * RecoilReduct, currentRotation.y * RecoilReduct, currentRotation.z * RecoilReduct);
        Weapon.transform.localRotation = Quaternion.Euler(currentRotation);
        CameraPos.transform.localRotation = Quaternion.Euler(newRot);
    }
    #endregion

    #region - Recoil Fire -
    public void RecoilFire(bool isAiming)//This method fires the recoil, add the recoil variables values to the current targetRotation and position
    {
        this.isAiming = isAiming;
        targetPosition -= Vector3.forward * Z_Current_KickBack;
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
    #endregion
}