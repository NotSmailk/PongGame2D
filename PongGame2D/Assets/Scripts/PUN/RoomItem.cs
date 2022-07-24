using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI roomNameText;
    [field: SerializeField] public TextMeshProUGUI activePlayersText;

    private LobbyManager lobbyManager;

    private void Start()
    {
        lobbyManager = LobbyManager.Instance;
    }

    public void SetRoomName(string roomName)
    {
        roomNameText.text = roomName;
    }

    public void OnClickItem()
    {
        lobbyManager.JoinRoom(roomNameText.text);
    }
}
