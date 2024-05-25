
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
    public float sightRange, attackRange, canAttackRange, canMusicRange;
    public bool playerInSightRange, playerInAttackRange, playerCanAttackRange, musicRange;

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
            musicRange = Physics.CheckSphere(transform.position, canMusicRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange && !isDead) Patroling();
            if (playerInSightRange && !playerInAttackRange && !isDead) ChasePlayer();
            if (playerInAttackRange && playerInSightRange && !isDead) AttackPlayer();

        }
        if (musicRange && !isDead) PlayMusic();
        if (!musicRange && !isDead) MuteMusic();
        

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

                if (musicRange ) PlayDeathMusic();
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, canMusicRange);
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

    private void PlayMusic()
    {
        switch (thisBossType)
        {
            case BossType.Boss1:
                SoundManager.Instance.PlaySound(SoundManager.Instance.boss1BattleMusic);
                SoundManager.Instance.MuteSound(SoundManager.Instance.startingZoneMusic);
                break;
            case BossType.Boss2:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
        }

    }

    private void MuteMusic()
    {
        switch (thisBossType)
        {
            case BossType.Boss1:
                SoundManager.Instance.MuteSound(SoundManager.Instance.boss1BattleMusic);
                SoundManager.Instance.PlaySound(SoundManager.Instance.startingZoneMusic);
                break;
            case BossType.Boss2:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
        }

    }

    private void PlayDeathMusic()
    {
        switch (thisBossType)
        {
            case BossType.Boss1:
                SoundManager.Instance.PlaySound(SoundManager.Instance.boss1DeathMusic);
                SoundManager.Instance.MuteSound(SoundManager.Instance.boss1BattleMusic);
                SoundManager.Instance.MuteSound(SoundManager.Instance.startingZoneMusic);
                StartCoroutine(PlayZoneMusic());
                break;
            case BossType.Boss2:
                //soundChannel.PlayOneShot(); //lion sound clip
                break;
            default:
                break;
        }
    }

    IEnumerator PlayZoneMusic()
    {
        yield return new WaitForSeconds(6f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.startingZoneMusic);
    }


    

}

