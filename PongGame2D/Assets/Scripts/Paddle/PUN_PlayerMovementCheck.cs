using UnityEngine;
using Photon.Pun;

public class PUN_PlayerMovementCheck : MonoBehaviour
{
    [field: Header("Player Options")]
    [field: SerializeField] private Player player = Player.One;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        player = PhotonNetwork.IsMasterClient ? Player.One : Player.Two;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        float yAxis = Inputs.player_1_yAxis;

        if (yAxis != 0)
        {
            PUN_PongGameManager.Instance.photonView.RPC("PlayerReady", RpcTarget.AllBuffered, player);

            Destroy(this);
        }
    }
}
