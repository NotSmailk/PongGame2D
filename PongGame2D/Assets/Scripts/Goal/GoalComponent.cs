using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GoalComponent : MonoBehaviour 
{
    [field: Header("Goal Size")]
    [field: SerializeField, Range(0.1f, 11f)] private float areaSize = 1f;

    public Player playerLink;

    private void OnValidate()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();

        box.size = new Vector2 (box.size.x, areaSize);
    }
}