using System.Collections;
using UnityEngine;

public class RobotLuchador : MonoBehaviour, IDamageable
{
    public float maxHP = 20.0f;
    public float attackDamage = 20.0f;
    public float speed = 3.0f; // Velocidad de persecución
    private Transform player; // Referencia al jugador
    public float detectionRange = 10.0f;
    private float currentHP;
    private bool isAttacking = false;

    //Referecnias para la animacion y el sprite
    //public Animator animator;
    public SpriteRenderer spriteR;

    private void Awake()
    {
       currentHP = maxHP;

       GameObject playerG = GameObject.FindGameObjectWithTag("Player");
       player = playerG.GetComponent<Transform>();
    }

    private void Update()
    {
        if (player != null)
        {
            
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            
            if (distanceToPlayer < detectionRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * speed * Time.deltaTime;

                
                if (distanceToPlayer < 0.5f) // Distancia de ataque se necesitará modificar a partir de los sprites y animaciones.
                {
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        StartCoroutine(DamagePlayer());
                    }
                }
                else
                {
                    isAttacking = false;
                    StopCoroutine(DamagePlayer());
                }
            }
            else
            {
                isAttacking = false;
                StopCoroutine(DamagePlayer());
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }


    IEnumerator DamagePlayer()
    {
        while (isAttacking)
        {
            player.GetComponent<IDamageable>()?.ChangeHP(-attackDamage);
            yield return new WaitForSeconds(1);
        }
    }

    public void ChangeHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP <= 0)
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        //MarcoAntonio
        //Reprodurcir animacion de muerte
        //if(animator != null)
        //{
        //    animator.SetTrigger("Die");
        //}

        //Desactivar el sprite para que de3saparezca visualmente
        if(spriteR != null)
        {
            spriteR.enabled = false;
        }
        //---------------------------------------

        Debug.Log("El enemigo ha muerto.");

        //Destruir el objeto despues de un pequeño retraso
        Destroy(gameObject, 2f);
    }
}
