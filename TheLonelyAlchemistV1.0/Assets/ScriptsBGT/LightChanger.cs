using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    public Light light;
    public bool playerRange;
    public float smoothSpeed = 2.0f;
    public float maxIntensity = 100f;
    public float minIntensity = 0f;

    private bool increasing = true;
    void Start()
    {
        if (light == null)
        {
            light = GetComponent<Light>();
        }

    }


    void Update()
    {
        if (playerRange)
        {
            if (increasing)
            {
                light.intensity += smoothSpeed * Time.deltaTime;
                if (light.intensity >= maxIntensity)
                {
                    light.intensity = maxIntensity;
                    increasing = false;
                }
            }
            else
            {
                light.intensity -= smoothSpeed * Time.deltaTime;
                if (light.intensity <= minIntensity)
                {
                    light.intensity = minIntensity;
                    increasing = true;
                }
            }
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
            playerRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
        }
    }
}
