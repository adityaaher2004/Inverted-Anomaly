using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    public bool walkPointSet = false;
    public float walkPointRange = 10f;

    public float timeBetweenAttacks = 4f;
    bool alreadyAttacked = false;

    public float sightRange = 10f, attackRange = 10f;
    public bool playerInSight = false, playerInAttackRange = false;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        float rand = Random.Range(0f, 20f);
        if (rand > 10)
        {
            AttackPlayer();
        }
        if (!playerInSight && !playerInAttackRange) Patrolling();
        if (playerInSight && !playerInAttackRange) ChasePlayer();
    }

    void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distance = transform.position - walkPoint;

        if (distance.magnitude < 1f) walkPointSet = false;

    }

    void SearchWalkPoint()
    {
        float randx = Random.Range(-walkPointRange, walkPointRange);
        float randz = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randx, transform.position.y, transform.position.z + randz);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        // agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Shoot();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void Shoot()
    {
        Vector3 ShootDirection = (player.position - transform.position).normalized;

        GameObject spawned = Instantiate(bullet, transform.position + 2 * ShootDirection, transform.rotation);
        spawned.GetComponent<Rigidbody>().AddForce(ShootDirection * 100f, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
