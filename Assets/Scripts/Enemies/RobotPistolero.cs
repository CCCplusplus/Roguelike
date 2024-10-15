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
                transform.Translate(Vector2.up * direction * speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, 0, movingRight ? -90 : 90);
            }
            else
            {
                transform.Translate(Vector2.up * direction * speed * Time.deltaTime);
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
        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, transform.up, 10f);


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
            Vector3 direction = transform.up * 10f; // Ajusta la longitud de tu raycast según sea necesario
            Gizmos.DrawRay(shootPoint.position, direction);
        }
    }


    IEnumerator Shoot()
    {
        canShoot = false;
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * 10f; // Ajusta la velocidad según sea necesario
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
        // TODO: Reproducir una animación de muerte, desactivar el sprite, Gameover Screen.
        Debug.Log("El enemigo ha muerto.");
        Destroy(gameObject);
    }
}
