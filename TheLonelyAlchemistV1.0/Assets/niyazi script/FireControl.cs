using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public ParticleSystem campfireParticles; // Campfire'ýn Particle System bileþeni
    public ParticleSystem campfireParticlesSmoke;
    public LayerMask HitLayer; // Raycast'ýn etkileþime geçeceði layer

    private bool isCampfireOn = true; // Campfire'ýn açýk/kapalý durumu

    void Update()
    {
        // "L" tuþuna basýldýðýnda
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Raycast'ý atarak etkileþime geçen bir nesne var mý kontrol et
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, HitLayer))
            {
                // Eðer Raycast campfire ile temas ettiyse
                if (hit.collider.CompareTag("Fire"))
                {
                    ToggleCampfire(); // Campfire'ý açýp kapat
                }
            }
        }
    }

    // Campfire'ý açýp kapatan fonksiyon
    void ToggleCampfire()
    {
        isCampfireOn = !isCampfireOn; // Durumu tersine çevir

        // Eðer campfire açýksa
        if (isCampfireOn)
        {
            // Particle System'i etkinleþtir
            campfireParticles.Play();
            campfireParticlesSmoke.Play();
            Debug.Log("Campfire açýldý!");

            // StartCoroutine(WaitAndStopCampfire());

        }
        else
        {
            // Particle System'i devre dýþý býrak
            campfireParticles.Stop();
            campfireParticlesSmoke.Stop();
            Debug.Log("Campfire kapatýldý!");
        }


    }
   
}
