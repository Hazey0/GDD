using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Animator animtor;
    public Transform player;
    public AudioSource  walk;
    public AudioSource roar;
    bool haveRoared=false;
    public float moveSpeed = 3f;
    public float chaseDistance = 8f;
    public float attackDistance = 2f;
    public float damage = 4f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;

    void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            animtor.SetBool("isChasing", false);
            walk.Stop();
            AttackPlayer();

        }
        else if (distance <= chaseDistance)
        {
            if (!haveRoared)
            {
                roar.Play();
                haveRoared = true;
            }
            animtor.SetBool("isChasing", true);
            if (!walk.isPlaying)
            {
                
                walk.Play();
            }
            ChasePlayer();
        }

        else
        {haveRoared = false;
            walk.Stop();
            animtor.SetBool("isChasing", false);
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void AttackPlayer()
    {
 
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            
            lastAttackTime = Time.time;


            Health playerHealth = player.GetComponent<Health>();
            

            if (playerHealth != null)
            {
                playerHealth.ApplyDamage(damage);
                animtor.SetTrigger("attack");
                Debug.Log("Enemy attacked player. Player health = " + playerHealth.healthPoints);
            }
        }
    }
}