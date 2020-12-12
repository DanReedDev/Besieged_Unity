using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Animator anim;
    public PlayerMovement playerMovement;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange;

    public float maxHealth = 50;
    float currentHealth;

    public float attackDamage = 20;
    public float attackRate = 2f;
    float nextAttackTime = 0;

    [SerializeField]
    private Transform player;

    public HealthBar healthBar;

    public float playerHealth;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        playerMovement.CanMove(0);
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            playerMovement.CanMove(1);
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyStats>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetBool("IsDead", true);

        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<CharacterController2D>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        this.enabled = false;
    }
}
