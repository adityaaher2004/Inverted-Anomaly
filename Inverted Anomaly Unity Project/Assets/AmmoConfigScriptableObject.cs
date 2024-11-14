using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns / Ammo Config", order = 3)]
public class AmmoConfigScriptableObject : ScriptableObject
{
    public int maxAmmo = 60;
    public int clipSize = 12;

    public int currentAmmo = 60;
    public int currentClipAmmo = 12;

    public void Reload()
    {
        int reloadAmt = Mathf.Min(clipSize, currentAmmo);
        currentClipAmmo = reloadAmt;
        currentAmmo -= reloadAmt;
    }

    public bool canReload()
    {
        return currentAmmo > 0 && currentClipAmmo < clipSize;
    }

    public void populateAmmo()
    {
        currentClipAmmo = 12;
        currentAmmo = 60;
    }

}
