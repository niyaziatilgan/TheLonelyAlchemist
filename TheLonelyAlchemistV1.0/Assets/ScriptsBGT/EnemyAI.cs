
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public string bossName;

    //public float health;

    public bool isDead;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip destroy;

    public Animator animator;

    [SerializeField] ParticleSystem hitParticle;

    enum BossType
    {
        Boss1,
        Boss2,
        Boss3,
    }

    [SerializeField] BossType thisBossType;

    public string GetItemName()
    {
        return bossName;
    }

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange, canAttackRange;
    public bool playerInSightRange, playerInAttackRange, playerCanAttackRange;

    //shoot

    //public Transform firePoint;
    //public GameObject FireParticle;
    //public GameObject HitParticle;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            playerCanAttackRange = Physics.CheckSphere(transform.position, canAttackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange && !isDead) Patroling();
            if (playerInSightRange && !playerInAttackRange && !isDead) ChasePlayer();
            if (playerInAttackRange && playerInSightRange && !isDead) AttackPlayer();
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

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        if (!alreadyAttacked && isDead == false)
        {
            agent.SetDestination(player.position);

            transform.LookAt(player);
            ///Attack code here

            PlayerState.Instance.currentHealth -= 10;
            Debug.Log("playere saldirdim");

            //End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
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
                playerInSightRange = false;
                playerInAttackRange = false;
                

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
    //private void DestroyEnemy()
    //{
    //    Destroy(gameObject);
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, canAttackRange);
    }

    private void PlayDyingSound()
    {
        switch (thisBossType)
        {
            case BossType.Boss1:
                soundChannel.PlayOneShot(destroy);
                break;
            case BossType.Boss2:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
        }

    }

    private void PlayHitSound()
    {
        switch (thisBossType)
        {
            case BossType.Boss1:
                soundChannel.PlayOneShot(hit);
                break;
            case BossType.Boss2:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
        }

    }

}

