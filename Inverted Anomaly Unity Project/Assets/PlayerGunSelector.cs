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

    [SerializeField] public Camera PlayerCamera;



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

        if (PlayerCamera == null)
        {
            PlayerCamera = gameObject.GetComponent<Camera>();
        }

        ActiveGun = gun;
        gun.populateAmmo();
        gun.Spawn(GunParent, this, PlayerCamera);

        Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();
        InverseKinematics.LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        InverseKinematics.RightElbowIKTarget= allChildren.FirstOrDefault(child => child.name == "RightElbow");
        InverseKinematics.LeftHandIKTarget= allChildren.FirstOrDefault(child => child.name == "LeftHand");
        InverseKinematics.RightHandIKTarget= allChildren.FirstOrDefault(child => child.name == "RightHand");


    }
}
