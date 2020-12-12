using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public Animator anim;
    public EnemyAI enemyAI;

    public float maxHealth = 50;
    float currentHealth;

    public Transform player;
    public Transform attackPoint;
    public LayerMask playerLayers;

    public float attackRange;

    public float attackDamage = 10;
    public float attackRate = 0.6f;
    float nextAttackTime = 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        enemyAI.CanMove(0);
        if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        enemyAI.CanMove(1);
    }

    void Attack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in hitPlayers)
        {
            anim.SetTrigger("Attack");
            player.GetComponent<PlayerStats>().TakeDamage(attackDamage);
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
        GetComponent<EnemyAI>().enabled = false;

        this.enabled = false;
    }
}
