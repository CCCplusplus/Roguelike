using System.Collections;
using UnityEngine;

public class RobotLuchador : MonoBehaviour, IDamageable
{
    public float maxHP = 20.0f;
    public float attackDamage = 20.0f;
    public float speed = 3.0f; // Velocidad de persecución
    private Transform player; // Referencia al jugador
    public float detectionRange = 10.0f;
    public float attackDistance = 2.5f; // Distancia para detenerse y atacar
    private float currentHP;
    private bool isAttacking = false;
    private bool canAttack = true; // Controlar el cooldown del ataque

    // Referencias para la animacion y el sprite
    // public Animator animator;
    public SpriteRenderer spriteR;
    public GameObject attackHitbox; // GameObject que representa el hitbox de ataque

    private void Awake()
    {
        currentHP = maxHP;
        GameObject playerG = GameObject.FindGameObjectWithTag("Player");
        player = playerG.transform;
        attackHitbox.SetActive(false); // Asegurarse de que el hitbox está inicialmente desactivado
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Verificar si el enemigo puede atacar y está dentro del rango de detección
            if (distanceToPlayer < detectionRange && distanceToPlayer > attackDistance && canAttack)
            {
                // Perseguir al jugador
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * speed * Time.deltaTime;
            }
            else if (distanceToPlayer <= attackDistance && canAttack)
            {
                // Detenerse y prepararse para atacar
                StartCoroutine(ActivateHitbox());
            }
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
        {
            HandleDeath();
        }
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

        Debug.Log("El enemigo ha muerto.");
        // Destruir el objeto después de un pequeño retraso
        Destroy(gameObject, 2f);
    }
}
