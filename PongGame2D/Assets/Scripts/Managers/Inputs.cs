using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static int player_1_yAxis = 0;
    public static int player_2_yAxis = 0;

    private void Update()
    {
        GetPlayer1YAxis();
        GetPlayer2YAxis();
    }

    private void GetPlayer1YAxis()
    {
        if (Input.GetKey(KeyCode.W) && player_1_yAxis > -0.5)
        {
            player_1_yAxis = 1;

            return;
        }

        if (Input.GetKey(KeyCode.S) && player_1_yAxis < 0.5)
        {
            player_1_yAxis = -1;

            return;
        }

        player_1_yAxis = 0;
    }

    private void GetPlayer2YAxis()
    {
        if (Input.GetKey(KeyCode.UpArrow) && player_2_yAxis > -0.5)
        {
            player_2_yAxis = 2;

            return;
        }

        if (Input.GetKey(KeyCode.DownArrow) && player_2_yAxis < 0.5)
        {
            player_2_yAxis = -1;

            return;
        }

        player_2_yAxis = 0;
    }
}
