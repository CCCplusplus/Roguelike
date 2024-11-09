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
    private float expValue = 20.0f;

    private bool invencible = false;

    private SpriteRenderer spriteRenderer;

    public static event System.Action OnEnemyDeath;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (!invencible)
        {
            invencible = true;
            currentHP += amount;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);

            if (currentHP <= 0)
                HandleDeath();

            StartCoroutine(Vencible());
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

        playerG.GetComponent<Movement>().AddEXP(expValue);

        Debug.Log("El enemigo ha muerto.");
        // Destruir el objeto después de un pequeño retraso 
        //quite el retraso para hacer test al spawn lo devolvemos cuando tengamos animaciones de muertes.
        Destroy(gameObject);
        OnEnemyDeath?.Invoke();
    }

    private IEnumerator Vencible()
    {

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(0.2f);
        }

        invencible = false;

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }
}
