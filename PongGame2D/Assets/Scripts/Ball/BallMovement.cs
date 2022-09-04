using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    [field: Header("Ball Options")]
    [field: SerializeField] protected float maxMultiplyCoef = 1.0f;

    protected float speed = 5f;
    protected float multiplyCoef = 0.0f; 
    protected Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Launch()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        float xDirection = Random.Range(-1.0f, 1.0f) > 0 ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);
        float yDirection = Random.Range(-1.0f, 1.0f);

        Vector2 direction = new Vector2(xDirection, yDirection);

        rb.velocity = direction * speed;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (multiplyCoef < maxMultiplyCoef)
        {
            multiplyCoef += 0.1f;

            rb.velocity *= 1.1f;
        }

        if (collision.gameObject.TryGetComponent<GoalComponent>(out GoalComponent goal))
        {
            PongGameManager.Instance.AddScore(goal.playerLink);

            Destroy(gameObject);

            PongGameSpawner.Instance.SpawnBall();
        }
    }
}
