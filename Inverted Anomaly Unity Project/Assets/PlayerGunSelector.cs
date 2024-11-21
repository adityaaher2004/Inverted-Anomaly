using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType Gun;
    [SerializeField] private Transform GunParent;
    [SerializeField] private List<GunScriptableObject> Guns;
    [SerializeField] private PlayerIK InverseKinematics;

    [SerializeField] public Camera Camera;



    [Space]
    [Header("Runtime Filled")]

    public GunScriptableObject ActiveGun;

    private void Start()
    {
        GunScriptableObject gun = Guns.Find(gun => gun.Type == GunType.Glock);

        if (gun == null)
        {
            Debug.LogError("No GunScriptableObject Found");
            return;
        }

        if (Camera == null)
        {
            Camera = GameObject.FindFirstObjectByType<Camera>();
        }

        ActiveGun = gun;
        gun.populateAmmo();
        gun.Spawn(GunParent, this, Camera);

        InverseKinematics.SetGunStyle(ActiveGun.Type == GunType.Glock);
        InverseKinematics.Setup(GunParent);

    }
}
