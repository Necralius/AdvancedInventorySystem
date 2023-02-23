using UnityEngine;

public class GunProceduralRecoil : MonoBehaviour
{
    public GameObject Weapon => gameObject;
    public GameObject CameraPos;
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Vector3 currentPosition;
    private Vector3 targetPosition;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness = 6;
    [SerializeField] private float returnSpeed = 2;

    public float RecoilReduct = 1;
    public float aimReductionFactor;

    public float Z_KickBack = 1;
    [SerializeField] float Z_Current_KickBack;
    public bool isAiming;

    private void Update()
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
    public void RecoilFire(bool isAiming)
    {
        this.isAiming = isAiming;
        targetPosition -= Vector3.forward * Z_Current_KickBack;
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}