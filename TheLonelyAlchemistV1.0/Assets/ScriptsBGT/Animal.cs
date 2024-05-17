using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    public bool isDead;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip destroy;

    public Animator animator;

    [SerializeField] ParticleSystem hitParticle;

    enum AnimalType
    {
        Rabbit,
        Lion,
        Snake,
    }

    [SerializeField] AnimalType thisAnimalType;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public string GetItemName()
    {
        return animalName;
    }

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;
            hitParticle.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();

                //ANIMATOR GELCEK animator.SetTrigger("DIE");
                //StartCoroutine(DestroyingObject());

                isDead = true;
            }
            else
            {
                PlayHitSound();
            } 
        }
    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit :
                soundChannel.PlayOneShot(destroy);
                break;
            case AnimalType.Lion:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
        }

    }

    private void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(hit);
                break;
            case AnimalType.Lion:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
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

    //IEnumerator DestroyingObject()
    //{
    //    yield return new WaitForSeconds(1f);
    //    Destroy(gameObject);
    //}



}
