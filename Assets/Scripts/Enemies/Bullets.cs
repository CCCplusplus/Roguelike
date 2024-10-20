using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField]
    private float attackDamage = 30.0f;

    //TODO: No destruir balas usar un pool.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                GameObject player = collision.gameObject;
                player.GetComponent<IDamageable>()?.ChangeHP(-attackDamage);
                BulletPool.Instance.ReturnBullet(this.gameObject);
            }
            else BulletPool.Instance.ReturnBullet(this.gameObject);
        } 
    }
}
