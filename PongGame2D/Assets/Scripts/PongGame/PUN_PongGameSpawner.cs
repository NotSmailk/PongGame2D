using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PUN_PongGameSpawner : MonoBehaviour
{
    [field: Header("Prefabs")]
    [field: SerializeField] private GameObject player1Prefab;
    [field: SerializeField] private GameObject player2Prefab;

    [field: Header("Spawns")]
    [field: SerializeField] private Transform player1Spawn;
    [field: SerializeField] private Transform player2Spawn;
    [field: SerializeField] private Transform ballSpawn;

    [field: HideInInspector] public PhotonView photonView;

    public static PUN_PongGameSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    [PunRPC]
    public void SpawnBall()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        GameObject newBall = PhotonNetwork.Instantiate("ball_PUN", ballSpawn.position, Quaternion.identity);

        if (!PUN_PongGameManager.Instance.GameIsOver)
            newBall.GetComponent<BallMovement>().Launch();
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("player_1_PUN", player1Spawn.position, player1Prefab.transform.rotation);
            PhotonNetwork.Instantiate("ball_PUN", ballSpawn.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("player_2_PUN", player2Spawn.position, player2Prefab.transform.rotation);
        }        
    }
}
