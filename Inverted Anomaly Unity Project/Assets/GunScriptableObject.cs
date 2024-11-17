using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns / Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public GameObject BulletModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;

    public ShootConfigurationScriptableObject ShootConfig;
    public AmmoConfigScriptableObject AmmoConfig;
    public TrailConfigScriptableObject TrailConfig;

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    private Camera ActiveCamera;


    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour, Camera ActiveCamera = null)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        this.ActiveCamera = ActiveCamera;

        LastShootTime = 0;
        TrailPool= new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.SetLocalPositionAndRotation(SpawnPoint, Quaternion.Euler(SpawnRotation));

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
    }

    public void UpdateCamera(Camera ActiveCamera)
    {
        this.ActiveCamera = ActiveCamera;
    }

    public void Shoot(){
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            LastShootTime = Time.time;

            ShootSystem.Play();

            Vector3 ShootDirection = Vector3.zero;

            if (ShootConfig.ShootType == ShootType.FromGun)
            {
                ShootDirection = ShootSystem.transform.forward;
            }
            else
            {
                ShootDirection = ActiveCamera.transform.forward + ActiveCamera.transform.TransformDirection(ShootDirection);
            }

            ShootDirection.Normalize();

            GameObject spawnedBullet =  Instantiate(BulletModelPrefab, ShootSystem.transform.position, ShootSystem.transform.rotation);
            spawnedBullet.transform.Rotate(90, 0, 0);


            ActiveMonoBehaviour.StartCoroutine(
                   PlayTrail(
                       spawnedBullet.transform.position,
                       spawnedBullet.transform.position + (ShootDirection * TrailConfig.MissDistance * 2),
                       new RaycastHit()
                       ));

            AmmoConfig.currentClipAmmo--;

            // Only for HitScan Shots, for Projectile Shots, we do ProjectileShoot
            /*if (Physics.Raycast(ShootSystem.transform.position, ShootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootSystem.transform.position,
                        hit.point,
                        hit
                        )
                    );
            }
            else
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootSystem.transform.position,
                        ShootSystem.transform.position + (ShootDirection * TrailConfig.MissDistance),
                        new RaycastHit()
                        )
                    );
            }*/

        }

    }

    private void DoProjectileShoot(Vector3 ShootDirection)
    {
        Bullet bullet = CreateBullet();
        bullet.transform.Rotate(90, 0, 0);
        bullet.gameObject.SetActive(true);
        bullet.OnCollision += HandleBulletCollision;

        TrailRenderer trail = TrailPool.Get();
        if (trail != null)
        {
            trail.transform.SetParent(bullet.transform, false);
            trail.gameObject.SetActive(true);
            trail.transform.localPosition = Vector3.zero;
            trail.emitting = true;
        }
        
    }

    public Vector3 GetGunForward()
    {
        return Model.transform.forward;
    }

    private void HandleBulletCollision(Bullet bullet, Collision collision)
    {
        TrailRenderer trail = bullet.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.transform.SetParent(null, true);
            ActiveMonoBehaviour.StartCoroutine(DelayedDisableTrail(trail));
        }
    }

    public void endReload()
    {
        AmmoConfig.Reload();
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;

        yield return null; // Wait one frame

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
                );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;

        instance.emitting = false;
        instance.gameObject.SetActive(false);

        TrailPool.Release(instance);

    }

    public Vector3 GetRaycastOrigin()
    {
        Vector3 origin = ShootSystem.transform.position;

        if (ShootConfig.ShootType == ShootType.FromCamera)
        {
            origin = ActiveCamera.transform.position +
                ActiveCamera.transform.forward * Vector3.Distance(
                    ActiveCamera.transform.position,
                    ShootSystem.transform.position);
        }

        return origin;

    }

    private IEnumerator DelayedDisableTrail(TrailRenderer trail)
    {
        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        trail.emitting = false;
        trail.gameObject.SetActive(false);
        TrailPool.Release(trail);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");

        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;

    }

    private Bullet CreateBullet()
    {
        return Instantiate(ShootConfig.BulletPrefab);
    }

    public void populateAmmo()
    {
        AmmoConfig.populateAmmo();
    }

}
