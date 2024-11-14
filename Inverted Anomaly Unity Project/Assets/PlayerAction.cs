using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector gunSelector;

    [SerializeField] private bool autoReload = true;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private PlayerIK InverseKinematics;

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
