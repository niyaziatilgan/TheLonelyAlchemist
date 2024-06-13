using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessController : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;

    public Vector2 targetCenter = new Vector2(0.5f, 0.5f);
    public float targetIntensity = 0f;
    public float targetSmoothness = 0.01f;
    public bool targetRounded = false;

    public float duration = 10f;

    void Start()
    {
        volume = GetComponent<Volume>();

        if (volume != null && volume.profile.TryGet(out vignette))
        {
            StartCoroutine(ChangeVignetteOverTime(targetCenter, targetIntensity, targetSmoothness, targetRounded, duration));
        }

    }
    private IEnumerator ChangeVignetteOverTime(Vector2 center, float intensity, float smoothness, bool rounded, float duration)
    {
        Vector2 initialCenter = vignette.center.value;
        float initialIntensity = vignette.intensity.value;
        float initialSmoothness = vignette.smoothness.value;
        bool initialRounded = vignette.rounded.value;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            vignette.center.value = Vector2.Lerp(initialCenter, center, t);
            vignette.intensity.value = Mathf.Lerp(initialIntensity, intensity, t);
            vignette.smoothness.value = Mathf.Lerp(initialSmoothness, smoothness, t);
            vignette.rounded.value = rounded;

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        vignette.center.value = center;
        vignette.intensity.value = intensity;
        vignette.smoothness.value = smoothness;
        vignette.rounded.value = rounded;
    }
}