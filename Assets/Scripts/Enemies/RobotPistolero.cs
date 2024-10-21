using System.Collections;
using UnityEngine;

public class RobotPistolero : MonoBehaviour, IDamageable
{
    public float maxHP = 20.0f;
    private float currentHP;
    public float speed = 2.0f;
    public bool horizontal = false;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    private bool movingRight = true;
    private float direction = 1f;
    private bool canShoot = true;

    private bool shootmode = false;

    //Referencias para la animacion y el sprite
    //public Animator animator;
    public SpriteRenderer spriteR;

    private void Awake()
    {
        currentHP = maxHP;
    }
    private void Update()
    {
        Move();
        CheckForPlayer();
    }

    private void Move()
    {
        if (!shootmode)
        {
            if (horizontal)
            {
                transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, 0, movingRight ? -90 : 90);
            }
            else
            {
                transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, 0, movingRight ? 0 : 180);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            movingRight = !movingRight;
            transform.position += new Vector3(0, direction * 0.1f, 0); // Que no se atore en la pared.
        }
    }

    private void CheckForPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, transform.right, 10f);


        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            shootmode = true;
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else
            shootmode= false;
        
    }

    void OnDrawGizmos()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.red; // Establece el color del Gizmo a rojo
            Vector3 direction = transform.right * 10f; // Ajusta la longitud de tu raycast seg�n sea necesario
            Gizmos.DrawRay(shootPoint.position, direction);
        }
    }


    IEnumerator Shoot()
    {
        canShoot = false;

        //Obtener una bala del pool en lugar de instanciar una nueva
        GameObject bullet = BulletPool.Instance.GetBullet();

        //Configurar la posicion y la velocidad de la bala
        bullet.transform.position = shootPoint.position;

        //GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * 10f; // Ajusta la velocidad seg�n sea necesario
        yield return new WaitForSeconds(3);
        canShoot = true;
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
        //Marco Anotnio 
        //Reproducir animacion de muerte
        //if(animator != null)
        //{
        //    animator.SetTrigger("Die");
        //}

        //Desactivar el sprite para que desaparezca visualmente
        if(spriteR != null)
        {
            spriteR.enabled = false;
        }
        //------------------------------------------

        Debug.Log("El enemigo ha muerto.");

        //Destruir el objeto despues de un peque�o retraso
        Destroy(gameObject, 2f);
    }
}
