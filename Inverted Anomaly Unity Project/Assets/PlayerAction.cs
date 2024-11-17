using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector gunSelector;

    [SerializeField] private bool autoReload = true;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private PlayerIK InverseKinematics;

    [SerializeField] private RawImage Crosshair;

    private bool isReloading;

    private void Update()
    {
        if (Input.GetMouseButton(0) && gunSelector.ActiveGun != null)
        {
            if (gunSelector.ActiveGun.AmmoConfig.currentClipAmmo > 0 && !isReloading)
            {
                gunSelector.ActiveGun.Shoot();
            }
        }

        if (doAutoReload() || doManualReload())
        {
            playerAnimator.SetTrigger("Reload");
            InverseKinematics.HandIKAmount = 0.25f;
            InverseKinematics.ElbowIKAmount = 0.25f;
            isReloading = true;
        }
        UpdateCrosshair();
    }

    private void UpdateCrosshair()
    {
        if (gunSelector.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
        {
            Vector3 gunTipPoint = gunSelector.ActiveGun.GetRaycastOrigin();
            Vector3 gunForward = gunSelector.ActiveGun.GetGunForward();
            Vector3 hitPoint = gunTipPoint + gunForward * 10;

            if (Physics.Raycast(
                gunTipPoint, gunForward, out RaycastHit hit, float.MaxValue, gunSelector.ActiveGun.ShootConfig.HitMask))
            {
                hitPoint = hit.point;
            }

            Vector3 screenSpaceLocation = gunSelector.camera.WorldToScreenPoint(hitPoint);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)Crosshair.transform.parent,
                screenSpaceLocation,
                null,
                out Vector2 localPosition))
            {
                Crosshair.rectTransform.anchoredPosition = localPosition;
            }
            else
            {
                Crosshair.rectTransform.anchoredPosition = Vector2.zero;
            }

        }
    }

    private void EndReload()
    {
        gunSelector.ActiveGun.endReload();
        InverseKinematics.HandIKAmount = 1f;
        InverseKinematics.ElbowIKAmount = 1f;
        isReloading = false;
    }

    private bool doAutoReload()
    {
        return !isReloading && autoReload && gunSelector.ActiveGun.AmmoConfig.currentClipAmmo == 0 && gunSelector.ActiveGun.AmmoConfig.canReload();
    }

    private bool doManualReload()
    {
        return !isReloading && Input.GetKeyUp(KeyCode.R) && gunSelector.ActiveGun.AmmoConfig.canReload();
    }
}
