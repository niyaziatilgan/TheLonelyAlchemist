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



    public Animator animator;

    [SerializeField] ParticleSystem hitParticle;

    //For Movement
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public bool isWalking;


    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;

    enum AnimalType
    {
        Horse,
        WhiteHorse,
        Bull,
        Wolf,
        Stag,
        Deer,
        Cow,
        Fox,
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

        SetNewTimes();

        isWalking = true;
        walkCounter = walkTime;
    }

    public void Update()
    {

            if (!isDead && isWalking)
            {
                walkCounter -= Time.deltaTime;

                if (walkCounter <= 0)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                    animator.SetBool("isWalking", false);
                }
                else
                {
                    Patroling();
                    animator.SetBool("isWalking", true);
                }
            }
            else
            {
                waitCounter -= Time.deltaTime;

                if (waitCounter <= 0)
                {
                    isWalking = true;
                    walkCounter = walkTime;
                    SetNewTimes();
                    SearchWalkPoint();
                }
            }

        
    }

    private void Patroling()
    {
        if (!isDead)
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;

            //walkCounter = walkTime;
        }


    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;


        //waitCounter = waitTime;
    }

    private void SetNewTimes()
    {
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);
        walkCounter = walkTime;
        waitCounter = waitTime;
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

                animator.SetTrigger("DIE");
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
            case AnimalType.Horse :
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossDies);
                break;
            case AnimalType.WhiteHorse:
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossDies);
                break;
            case AnimalType.Bull:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            case AnimalType.Wolf:
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
            case AnimalType.Horse:
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossDies);
                break;
            case AnimalType.WhiteHorse:
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossDies);
                break;
            case AnimalType.Bull:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            case AnimalType.Wolf:
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



}
    
