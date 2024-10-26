using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool shooter;
    private int counter = 0;

    [SerializeField]
    private GameObject fighterPrefab;
    [SerializeField]
    private GameObject shooterPrefab;

    private void OnEnable()
    {
        RobotLuchador.OnEnemyDeath += DecreaseCounter;
        RobotPistolero.OnEnemyDeath += DecreaseCounter;
    }

    private void OnDisable()
    {
        RobotLuchador.OnEnemyDeath -= DecreaseCounter;
        RobotPistolero.OnEnemyDeath -= DecreaseCounter;
    }

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        if (counter != 0) return;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (shooter)
            Instantiate(shooterPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(fighterPrefab, transform.position, Quaternion.identity);

        counter++;
    }

    private void DecreaseCounter()
    {
        counter--;
    }
}
