using UnityEngine;

public class PlayerMovementCheck : MonoBehaviour
{
    [field: Header("Player Options")]
    [field: SerializeField] private Player player = Player.One;

    private void Update()
    {
        float yAxis = player == Player.One ? Inputs.player_1_yAxis : Inputs.player_2_yAxis;

        if (yAxis != 0)
        {
            PongGameManager.Instance.PlayerReady(player);

            Destroy(this);
        }
    }
}
