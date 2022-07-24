using UnityEngine;

public class PongGameSpawner : MonoBehaviour
{
    [field: SerializeField] private bool againstBot = false;

    [field: Header("Prefabs")]
    [field: SerializeField] private GameObject player1Prefab;
    [field: SerializeField] private GameObject player2Prefab;
    [field: SerializeField] private GameObject autoPaddle;
    [field: SerializeField] private GameObject ballPrefab;

    [field: Header("Spawns")]
    [field: SerializeField] private Transform player1Spawn;
    [field: SerializeField] private Transform player2Spawn;
    [field: SerializeField] private Transform ballSpawn;

    public static PongGameSpawner Instance;
    public bool AgainstBot { get => againstBot; }

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBall()
    {
        GameObject newBall = Instantiate(ballPrefab, ballSpawn.position, Quaternion.identity);

        if (!PongGameManager.Instance.GameIsOver)
            newBall.GetComponent<BallMovement>().Launch();

        if (againstBot)
            FindObjectOfType<AutoPaddle>().SetBall(newBall.transform);
    }

    private void Start()
    {
        GameObject secondPaddle = againstBot ? autoPaddle : player2Prefab;

        Instantiate(player1Prefab, player1Spawn.position, player1Prefab.transform.rotation);
        Instantiate(secondPaddle, player2Spawn.position, secondPaddle.transform.rotation);
        Instantiate(ballPrefab, ballSpawn.position, Quaternion.identity);
    }
}
