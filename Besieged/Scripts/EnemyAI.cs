using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform castPoint;
    [SerializeField] public float detectionRange;
    [SerializeField] public float moveSpeed;

    Rigidbody2D rb;

    public Animator anim;

    public Transform objectDetection;
    public Transform[] patrolPoints;

    private bool atPatrolPoint;
    public bool isPatrolling = true;
    private bool isChasing = false;
    private bool isSearching = false;
    public bool canMove = true;
    private bool facingRight;

    private float waitTime;
    public float startWaitTime;

    private int randomPatrolPoint;
    public int chaseVision;
    private float vision = 0.5f;

    Vector2 lastKnownPlayerPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        waitTime = startWaitTime;
        randomPatrolPoint = Random.Range(0, patrolPoints.Length);
    }

    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (!CanSeeWall(vision))
        {
            if (distToPlayer < detectionRange)
            {
                isChasing = true;
            }
            else
            {
                if (isChasing)
                {
                    if (!isSearching)
                    {
                        isSearching = true;
                        Invoke("StopChasingPlayer", 3);
                    }
                }
            }
        }

        if (!CanSeeWall(vision))
        {
            if (CanSeePlayer(chaseVision))
            {
                isChasing = true;
            }
            else
            {
                if (isChasing)
                {
                    if (!isSearching)
                    {
                        isSearching = true;
                        Invoke("StopChasingPlayer", 3f);
                    }
                }
            }
        }

        if (isChasing)
        {
            if (!CanSeeWall(vision))
            {
                ChasePlayer();
            }
        }

        if (CanSeeWall(vision))
        {
            Invoke("StopChasingPlayer", 0.2f);
        }

        if (isPatrolling)
        {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[randomPatrolPoint].position, moveSpeed * Time.deltaTime);

                if (transform.position.x < patrolPoints[randomPatrolPoint].position.x)
                {
                    transform.localScale = new Vector2(0.02f, 0.02f);
                    anim.SetFloat("moveSpeed", 0f);
                    facingRight = true;
                }
                else if (transform.position.x > patrolPoints[randomPatrolPoint].position.x)
                {
                    transform.localScale = new Vector2(-0.02f, 0.02f);
                    anim.SetFloat("moveSpeed", 0f);
                    facingRight = false;
                }

                if (Vector2.Distance(transform.position, patrolPoints[randomPatrolPoint].position) > 0.5f || Vector2.Distance(transform.position, patrolPoints[randomPatrolPoint].position) < -0.5f)
                {
                    atPatrolPoint = false;
                }
                if (!atPatrolPoint)
                {
                    if (Vector2.Distance(transform.position, patrolPoints[randomPatrolPoint].position) < 0.001f)
                    {
                        StartCoroutine(NextPartolPoint());
                        isPatrolling = true;
                    }
                }
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        isPatrolling = false;
        moveSpeed = 5;

        anim.SetFloat("moveSpeed", 0f);

        if (Mathf.Round(transform.position.x) < Mathf.Round(player.position.x))
        {
            transform.localScale = new Vector2(0.02f, 0.02f);
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            facingRight = true;
        }
        else if (Mathf.Round(transform.position.x) > Mathf.Round(player.position.x))
        {
            transform.localScale = new Vector2(-0.02f, 0.02f);
            transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
            facingRight = false;
        }
    }

    void StopChasingPlayer()
    {
        isChasing = false;
        isSearching = false;
        isPatrolling = true;

        moveSpeed = 4;

        anim.SetFloat("moveSpeed", 1f);

        transform.Translate(-Vector2.right * 0 * Time.deltaTime);
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
            switch (collider.tag)
            {
                case "LargeObj":
                    rb.AddForce(Vector2.up * 400f);
                    break;

                case "SmallObj":
                    rb.AddForce(Vector2.up * 300f);
                    break;
            }
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        if(!facingRight)
        {
            castDist = -distance;
        }

        Vector2 endPos = castPoint.position + Vector3.right * castDist;

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Players"));

        if(hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
                Vector2 lastKnownPlayerPos = new Vector2(player.position.x, player.position.y);
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.green);
        }
        return val;
    }

    bool CanSeeWall(float distance)
    {
        bool val = false;
        float castDist = distance;

        if (!facingRight)
        {
            castDist = -distance;
        }

        Vector2 endPos = castPoint.position + Vector3.right * castDist;

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Obtructions"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Obtruction"))
            {
                val = true;
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.green);
        }
        return val;
    }

    public void CanMove(int canEnemyMove)
    {
        if (canEnemyMove == 1)
            canMove = true;

        if (canEnemyMove == 0)
            canMove = false;
    }

    IEnumerator NextPartolPoint()
    {
        float timeToWait = Random.Range(1, 4);

        atPatrolPoint = true;
        isPatrolling = false;

        anim.SetFloat("moveSpeed", 1f);


        yield return new WaitForSeconds(timeToWait);

        if (waitTime <= 0)
        {
            randomPatrolPoint = Random.Range(0, patrolPoints.Length);
            waitTime = startWaitTime;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
