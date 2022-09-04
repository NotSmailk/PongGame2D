using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [field: SerializeField] private TMP_InputField roomInput;
    [field: SerializeField] private GameObject roomPanel;
    [field: SerializeField] private GameObject lobbyPanel;
    [field: SerializeField] private TextMeshProUGUI roomName;

    [field: SerializeField] private RoomItem roomItemPrefab;
    [field: SerializeField] private Transform contentObject;

    [field: SerializeField] private float timeBetweenUpdates = 1.5f;

    [field: SerializeField] private List<PlayerItem> playerItemList = new List<PlayerItem>();
    [field: SerializeField] private PlayerItem playerItemPrefab;
    [field: SerializeField] private Transform playersContent;
    [field: SerializeField] private UnityEngine.UI.Button readyButton;

    private List<RoomItem> roomItemList = new List<RoomItem>();
    private float nextUpdateTime;

    public static LobbyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.JoinLobby();

        readyButton.onClick.AddListener(OnClickPlayerReady);
    }

    private void OnGUI()
    {
        KeyCode keyCode = Event.current.keyCode;

        if (keyCode == KeyCode.Escape && Event.current.type == EventType.KeyDown)
        {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();

        if (PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void OnClickCreate()
    {
        if (roomInput.text.Length >= 1 && roomInput.text.Length <= 12)
        {
            PhotonNetwork.CreateRoom(roomInput.text, new Photon.Realtime.RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
        }
        else if (roomInput.text.Length == 0)
        {
            PhotonNetwork.CreateRoom($"{PhotonNetwork.NickName}'s room", new Photon.Realtime.RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);

        roomPanel.SetActive(true);

        UpdatePlayerList();

        roomName.text = $"Room name: {PhotonNetwork.CurrentRoom.Name}";

        readyButton.GetComponentInChildren<TextMeshProUGUI>().text = PhotonNetwork.IsMasterClient ? "Start Game" : "Ready";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);

            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomItem item in roomItemList) 
        {
            Destroy(item.gameObject);
        }

        roomItemList.Clear();

        foreach (RoomInfo room in roomList)
        {
            if (room.PlayerCount == 0)
                continue;

            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);

            newRoom.SetRoomName(room.Name);
            newRoom.activePlayersText.text = $"{room.PlayerCount}/{room.MaxPlayers}";

            roomItemList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        roomInput.text = string.Empty;

        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer.GetNext());
        }

        roomPanel.SetActive(false);

        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        roomInput.text = string.Empty;

        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";

        UpdatePlayerList();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.Disconnect();
    }

    private void OnClickPlayerReady()
    {
        UpdatePlayerStates();
    }

    private void UpdatePlayerStates()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (playerItemList.Count <= 1)
                return;

            foreach (PlayerItem item in playerItemList)
            {
                if (!item.GetReadyState())
                    return;
            }

            PhotonNetwork.LoadLevel("MultiplayerPvP");
        }
        else
        {
            foreach (PlayerItem item in playerItemList)
            {
                if (item.GetPlayerInfo() == PhotonNetwork.LocalPlayer)
                {
                    item.SetPlayerReadyCheck(!item.GetReadyState(), true);
                }
            }
        }
    }

    private void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }

        playerItemList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
            return;

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        { 
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playersContent);

            playerItemList.Add(newPlayerItem);

            if (player.Value == PhotonNetwork.MasterClient)
                newPlayerItem.SetPlayerInfo(player.Value, true);
            else
                newPlayerItem.SetPlayerInfo(player.Value, false);
        }
    }
}
