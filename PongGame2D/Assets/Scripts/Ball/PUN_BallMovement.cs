using UnityEngine;
using Photon.Pun;

public class PUN_BallMovement : BallMovement
{
    [PunRPC]
    public override void Launch()
    {
        base.Launch();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (multiplyCoef < maxMultiplyCoef)
        {
            multiplyCoef += 0.1f;

            rb.velocity *= 1.1f;
        }

        if (collision.gameObject.TryGetComponent<GoalComponent>(out GoalComponent goal))
        {
            PUN_PongGameManager.Instance.photonView.RPC("AddScore", RpcTarget.AllBuffered, goal.playerLink);

            PhotonNetwork.Destroy(gameObject);

            PUN_PongGameSpawner.Instance.photonView.RPC("SpawnBall", RpcTarget.AllBuffered);
        }
    }
}
