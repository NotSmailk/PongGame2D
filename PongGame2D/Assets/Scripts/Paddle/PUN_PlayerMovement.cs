using UnityEngine;
using Photon.Pun;

public class PUN_PlayerMovement : MonoBehaviour
{
    [field: Header("Player Options")]
    [field: SerializeField] private float speed = 5f;

    private Rigidbody2D rb;

    private PhotonView photonView;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        float yAxis = Inputs.player_1_yAxis;

        rb.velocity = new Vector2(rb.velocity.x, speed * yAxis);
    }
}
