using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AiScript : MonoBehaviour
{
    [SerializeField] public Transform player;
    [HideInInspector] public PlayerScript playerScript;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public CapsuleCollider capsuleCollider;
    public float health = 100f;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject gui;
    [SerializeField] public GameObject guiMinimap;
    private float maxHealth;
    public float healthBarShowTime = 1f;
    private bool isDead;

    public float walkSpeed = 2;
    public float runSpeed = 4;

    [HideInInspector] public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public Vector3 previousPos;
    public float nextCheckMoveTime;
    public float checkMoveRateSec = 2.5f;

    //Attacking
    public int attackDamage = 10;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private enum AiMovement { idle, walk, run }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        maxHealth = health;
        HideHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) { Patroling(); }
            if (playerInSightRange && !playerInAttackRange) { ChasePlayer(); }
            if (playerInSightRange && playerInAttackRange) { AttackPlayer(); }

            gui.transform.LookAt(player);
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) { SearchWalkPoint(); }

        if (walkPointSet) { agent.SetDestination(walkPoint); }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Reached Walk Point
        if (distanceToWalkPoint.magnitude < 1f) { walkPointSet = false; animator.SetInteger("state", (int)AiMovement.idle); agent.speed = 0;  agent.acceleration = 0; }

        if (Time.time > nextCheckMoveTime)
        {
            nextCheckMoveTime = Time.time + checkMoveRateSec;
            CheckMove();
        }
    }

    private void SearchWalkPoint()
    {
        
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) { walkPointSet = true; animator.SetInteger("state", (int)AiMovement.walk); agent.speed = walkSpeed;  agent.acceleration = 0.075f; }
    }

    private void CheckMove()
    {
        Vector3 distanceTraveled = transform.position - previousPos;
        if (distanceTraveled.magnitude < 1f) {
            walkPointSet = false;
            SearchWalkPoint();
        }
        previousPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
        animator.SetInteger("state", (int)AiMovement.run);
        agent.speed = runSpeed;
        agent.acceleration = 4f;
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            animator.SetBool("isAttacking", true);

            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (playerInSightRange && playerInAttackRange) { playerScript.Hit(attackDamage); }

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("isAttacking", false);

    }

    public void setHealth(float value)
    {
        if (health <= 0) return;
        if (health - value <= 0) { Die(); }
        health -= value;
    }

    public void ShowHealth()
    {
        gui.SetActive(true);
        Invoke("HideHealth", healthBarShowTime);
    }

    public void HideHealth()
    {
        gui.SetActive(false);
    }

    public void Hit(int value)
    {
        setHealth(value);
        healthBar.GetComponent<Image>().fillAmount = health / maxHealth;
    }

    public void Die()
    {
        if (alreadyAttacked)
        {
            CancelInvoke();
            ResetAttack();
        }
        isDead = true;
        gameManager.DecreseEnemies();
        gui.SetActive(false);
        guiMinimap.SetActive(false);
        animator.Play("Z_FallingBack");
        float delay = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("StopMoveing", delay);
        Destroy(agent);
    }

    public void StopMoveing()
    {
        Destroy(animator);
        Destroy(capsuleCollider);
    }

}
