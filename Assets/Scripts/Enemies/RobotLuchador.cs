using System.Collections;
using UnityEngine;

public class RobotLuchador : MonoBehaviour, IDamageable
{
    public float maxHP = 20.0f;
    public float attackDamage = 20.0f;
    public float speed = 3.0f;
    private Transform player;
    public float detectionRange = 10.0f;
    public float attackDistance = 2.5f;
    private float currentHP;
    private bool isAttacking = false;
    private bool canAttack = true;
    private GameObject playerG;
    private float expValue = 2.0f;

    // Referencias para la animacion y el sprite
    // public Animator animator;
    public SpriteRenderer spriteR;
    public GameObject attackHitbox;

    private void Awake()
    {
        currentHP = maxHP;
        playerG = GameObject.FindGameObjectWithTag("Player");
        player = playerG.transform;
        attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            
            if (distanceToPlayer < detectionRange && distanceToPlayer > attackDistance && canAttack)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * speed * Time.deltaTime;
            }
            else if (distanceToPlayer <= attackDistance && canAttack)
                StartCoroutine(ActivateHitbox());
            
        }
    }

    IEnumerator ActivateHitbox()
    {
        canAttack = false;
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);

        attackHitbox.SetActive(true); 
        yield return new WaitForSeconds(1.0f);

        attackHitbox.SetActive(false); 
        yield return new WaitForSeconds(1.0f); 

        isAttacking = false;
        canAttack = true; 
    }

    public void ChangeHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP <= 0)
            HandleDeath();
    }

    public void HandleDeath()
    {
        // MarcoAntonio
        // Reproducir animacion de muerte
        // if (animator != null)
        // {
        //     animator.SetTrigger("Die");
        // }

        // Desactivar el sprite para que desaparezca visualmente
        if (spriteR != null)
        {
            spriteR.enabled = false;
        }

        playerG.GetComponent<Movement>().AddEXP(expValue);

        Debug.Log("El enemigo ha muerto.");
        // Destruir el objeto después de un pequeño retraso
        Destroy(gameObject, 2f);
    }
}
