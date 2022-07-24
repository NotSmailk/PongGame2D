using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AutoPaddle : MonoBehaviour
{
    private Transform ballTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ballTransform = FindObjectOfType<BallMovement>().transform;
    }

    public void SetBall(Transform ball)
    {
        ballTransform = ball;
    }

    private void Update()
    {
        Vector2 newPosition = new Vector2(transform.position.x, ballTransform.position.y);

        rb.position = Vector2.MoveTowards(rb.position, newPosition, 2f);
    }
}
