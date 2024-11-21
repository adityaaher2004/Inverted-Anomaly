using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShakeEffect : MonoBehaviour
{
    public float effectDuration;
    public float rotationAmount = 5f;
    public float rotationSpeed = 15f;

    private Quaternion rotation;
    private Vector3 position;
    private Vector3 targetScale;
    private Coroutine coroutine;

    private float maxHeight = 12f;
    private float step = 0.05f;
    private bool isActivated = false;

    Rigidbody rb;

    Rewindable rewinder;

    GlobalIsRewindingScript globalRewinder;



    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation;
        targetScale = transform.localScale;
        position = transform.position;
        rb = GetComponent<Rigidbody>();
        rewinder = new Rewindable(gameObject);
        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!globalRewinder.fireStartRewind && maxHeight >= transform.position.y && isActivated)
        {
            transform.position += new Vector3(0f, step, 0f);
        }
        // Need this snippet to check if rewinding
        // --------------------------------
        if (globalRewinder.fireStartRewind)
        {
            rewinder.StartRewind();
        }
        if (globalRewinder.fireStopRewind)
        {
            rewinder.StopRewind();
        }
        // --------------------------------   
    }

    private void FixedUpdate()
    {
        // Need this line to apply rewinding
        // --------------------------------
        rewinder.physUpdate();
        // --------------------------------
    }

    public void StartShake()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(ShakeCoroutine());
        }

        Debug.Log("Shaking Started");
    }

    public IEnumerator ShakeCoroutine()
    {
        float startTime = Time.time;

        while (Time.time - startTime < effectDuration)
        {
            float rotationOffset = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;

            transform.rotation = rotation * Quaternion.Euler(0f, 0f, rotationOffset);

            yield return null;
        }

        transform.rotation = rotation;
        transform.position = position;
        coroutine = null;
    }

    private void OnTriggerExit()
    {
        isActivated = true;
    }

    public void ActivateGravity()
    {
        rb.useGravity = true;
    }

    public void DeactivateGravity()
    {
        rb.useGravity = false;
    }
}
