using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChangerNew : MonoBehaviour
{
    public Light light;
    public Transform player;
    public float smoothSpeed = 2.0f;
    public float maxIntensity = 100f;
    public float minIntensity = 0f;
    public float maxDistance = 10f;

    public bool playerInRange;

    void Start()
    {
        if (light == null)
        {
            light = GetComponent<Light>();
        }
    }

    void Update()
    {
        if (player != null && playerInRange)
        {

            float distance = Vector3.Distance(transform.position, player.position);


            float targetIntensity = Mathf.Lerp(maxIntensity, minIntensity, distance / maxDistance);


            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, smoothSpeed * Time.deltaTime);
        }
        else
        {
            light.intensity = Mathf.Lerp(light.intensity, minIntensity, smoothSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
