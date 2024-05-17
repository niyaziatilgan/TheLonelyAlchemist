using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public ParticleSystem campfireParticles, campfireParticlesSmoke;
    public GameObject particle1, particle2;

    private bool isCampfireOn = true; 

    public bool playerInRange;

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

    public void ToggleCampfire()
    {
        isCampfireOn = !isCampfireOn;

        if (isCampfireOn)
        {

            particle1.SetActive(true);
            particle2.SetActive(true);

            //campfireParticles.Play();
            //campfireParticlesSmoke.Play();
        }
        else
        {
            particle1.SetActive(false);
            particle2.SetActive(false);

            //campfireParticles.Stop();
            //campfireParticlesSmoke.Stop();
        }


    }


}
