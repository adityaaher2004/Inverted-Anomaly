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
    public TrailConfigScriptableObject TrailConfig;

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0;
        TrailPool= new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.SetLocalPositionAndRotation(SpawnPoint, Quaternion.Euler(SpawnRotation));

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot(){
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            LastShootTime = Time.time;

            ShootSystem.Play();

            Vector3 ShootDirection = ShootSystem.transform.forward /* +
                                        new Vector3(
                                            Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
                                            Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y),
                                            Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z)
            ) */ ;

            ShootDirection.Normalize();

            GameObject spawnedBullet =  Instantiate(BulletModelPrefab, ShootSystem.transform.position, ShootSystem.transform.rotation);

            ActiveMonoBehaviour.StartCoroutine(
                   PlayTrail(
                       spawnedBullet.transform.position,
                       spawnedBullet.transform.position + (ShootDirection * TrailConfig.MissDistance * 2),
                       new RaycastHit()
                       ));



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

    private IEnumerator DelayedDisableTrail(TrailRenderer trail)
    {
        yield return new WaitForSeconds(TrailConfig.Duration * 2);
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
}
