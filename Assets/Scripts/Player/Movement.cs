using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour, IDamageable, IExperience
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float dashSpeedMultiplier = 3f; // Multiplicador de la velocidad de dash puede que este algo alto.
    [SerializeField]
    private float dashDuration = 0.2f;
    [SerializeField]
    private float dashCooldown = 1.5f;
    [SerializeField]
    private float attackDuration = 1f;
    [SerializeField]
    private float secundaryCooldown = 6f;
    [SerializeField]
    private GameObject hitbox;
    [SerializeField]
    private BoxCollider2D hit;
    [SerializeField]
    private GameObject fishPrefab;
    [SerializeField]
    private Transform shootPoint;

    private Transform me;
    private Vector2 movementInput;
    private Vector2 lastDirection = Vector2.right;
    private bool isDashing;
    private float dashEndTime;
    private float nextDashTime;
    private float exp = 0.0f;
    private int level = 0;
    private bool secundaryactivate = false;
    private bool canShoot = true;


    private float hp;
    private float maxHp = 100f;
    private bool invencible = false;

    bool attacking = false;

    private SpriteRenderer spriteRenderer;

    //Agregar el controlador de animacion y asignar el GameOver screen desde el inspector
    //public Animator animator;
    public GameObject gameOverScreen;

    private void Awake()
    {
        hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        me = transform;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>().normalized;
        if (movementInput != Vector2.zero)
        {
            lastDirection = movementInput; // Guarda la última dirección no cero
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextDashTime && !isDashing)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!attacking)
                StartCoroutine(ActivateHitbox());
        }
    }

    public void OnSecundario(InputAction.CallbackContext context)
    {
        if (secundaryactivate)
        {
            if (context.performed && canShoot && !attacking)
                StartCoroutine(Shoot());
        }
    }

    public void ChangeHP(float amount)
    {
        if (!invencible)
        {
            invencible = true;
            hp += amount;
            hp = Mathf.Clamp(hp, 0, maxHp);

            if (hp <= 0)
            {
                HandleDeath();
            }

            StartCoroutine(Vencible());
        }
    }

    public void HandleDeath()
    {
        // TODO: Reproducir una animación de muerte, desactivar el sprite, Gameover Screen.
        Debug.Log("El personaje ha muerto.");

        //Reproduce la animacion de muerte
        //animator.SetTrigger("Die");

        //Desactiva el sprite del jugador
        spriteRenderer.enabled = false;

        //Mostrar la pantalla de Game Over despues de un breve retraso
        StartCoroutine(ShowGameOverScreen());
    }

    private IEnumerator ShowGameOverScreen()
    {
        //Espera a que termine la animacion(ajusta el tiempo la duracion de la animacion)
        yield return new WaitForSeconds(2.0f);
        //Mostrar pantalla de Game Over
        gameOverScreen.SetActive(true);
    }

    private IEnumerator ActivateHitbox()
    {
        attacking = true;
        hit.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        hit.enabled = false;
        attacking = false;
    }


    IEnumerator Shoot()
    {
        canShoot = false;

        GameObject fish = Instantiate(fishPrefab, shootPoint.position, Quaternion.identity);
        fish.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;
        yield return new WaitForSeconds(secundaryCooldown);
        canShoot = true;
    }


    private IEnumerator Vencible()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        yield return new WaitForSeconds(0.2f);
        invencible = false;
    }

    public void AddEXP(float amount)
    {
        Debug.Log("Exp Increased by " +  amount);
        exp += amount;

        if (exp >= 10)
        {
            Debug.Log("level up time!");
            exp = 0;
            LvlUp();
        }
    }

    public void LvlUp()
    {
        if (level == 0)
        {
            Debug.Log("LevelUP!");
            secundaryactivate = true;
            level = 1;
            return;
        }

        if (level == 1)
        {
            dashCooldown /= 2f;
            level = 2;
            return;
        }

        if (level == 2)
        {
            secundaryCooldown /= 2f;
            level = 3;
            return;
        }

        if (level == 3)
            return;

    }

    void Update()
    {
        if (isDashing)
        {
            if (Time.time >= dashEndTime)
            {
                isDashing = false;
            }
            me.position += (Vector3)(lastDirection * speed * dashSpeedMultiplier * Time.deltaTime);
        }
        else
        {
            me.position += (Vector3)(movementInput * speed * Time.deltaTime);
        }

        if (lastDirection == Vector2.right)
            me.rotation = Quaternion.Euler(0, 0, 0);
        else if (lastDirection == Vector2.up)
            me.rotation = Quaternion.Euler(0, 0, 90);
        else if (lastDirection == Vector2.down)
            me.rotation = Quaternion.Euler(0, 0, -90);
        else if (lastDirection == Vector2.left)
            me.rotation = Quaternion.Euler(0, 180, 0);
    }
}
