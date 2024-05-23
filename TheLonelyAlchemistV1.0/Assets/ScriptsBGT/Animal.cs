using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    //For Movement
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    enum AnimalType
    {
        Rabbit,
        Lion,
        Snake,
    }

    [SerializeField] AnimalType thisAnimalType;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (!isDead)
        {
            Patroling();
        }
        
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossDies);
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossGetsDamage);
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
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }



}
