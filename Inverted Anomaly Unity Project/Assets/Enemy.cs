using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

        if (!playerInSight && !playerInAttackRange) Patrolling();
        if (playerInSight && !playerInAttackRange) ChasePlayer();
        if (playerInSight && playerInAttackRange) AttackPlayer();
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
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
