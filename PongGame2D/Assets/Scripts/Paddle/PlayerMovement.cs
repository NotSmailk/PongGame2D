using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [field: Header("Player Options")]
    [field: SerializeField] private float speed = 5f;
    [field: SerializeField] private Player player = Player.One;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float yAxis = player == Player.One ? Inputs.player_1_yAxis : Inputs.player_2_yAxis;

        rb.velocity = new Vector2(rb.velocity.x, speed * yAxis);
    }
}

public enum Player
{ 
    One,
    Two
}
