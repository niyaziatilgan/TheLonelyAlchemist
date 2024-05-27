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
        if (Input.GetKeyDown(KeyCode.L))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, HitLayer))
            {
                if (hit.collider.CompareTag("Fire"))
                {
                    ToggleCampfire(); // Campfire'ý açýp kapat
                }
            }
        }
    }

    void ToggleCampfire()
    {
        isCampfireOn = !isCampfireOn; 

        if (isCampfireOn)
        {
            campfireParticles.Play();
            campfireParticlesSmoke.Play();
            Debug.Log("Campfire açýldý!");

            // StartCoroutine(WaitAndStopCampfire());

        }
        else
        {
            campfireParticles.Stop();
            campfireParticlesSmoke.Stop();
            Debug.Log("Campfire kapatýldý!");
        }


    }
   
}
