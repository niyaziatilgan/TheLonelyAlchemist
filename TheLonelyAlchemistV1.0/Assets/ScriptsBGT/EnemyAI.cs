
using System.Collections;

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
        if (!isDead)
        {
            agent.SetDestination(player.position);
        }
    }

    public void AttackPlayer()
    {
        //Make sure enemy doesn't move
        if (!alreadyAttacked && isDead == false)
        {
            //agent.SetDestination(player.position);

            transform.LookAt(player);

            animator.SetTrigger("hit");
            SoundManager.Instance.PlaySound(SoundManager.Instance.bossAttacksPlayer);
            PlayerState.Instance.currentHealth -= 10;

            alreadyAttacked = true;

            StartCoroutine(betweenAttacks(timeBetweenAttacks));
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    IEnumerator betweenAttacks(float duration)
    {

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            ChasePlayer();
            Debug.Log(timer);
            yield return null;
        }
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossDies);
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.bossGetsDamage);
                break;
            case BossType.Boss2:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
        }

    }

}

