using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class PUN_PongGameManager : MonoBehaviourPunCallbacks
{
    [field: Header("Score UI")]
    [field: SerializeField] private TextMeshProUGUI player1ScoreText;
    [field: SerializeField] private TextMeshProUGUI player2ScoreText;
    [field: SerializeField] private TextMeshProUGUI player1Nickname;
    [field: SerializeField] private TextMeshProUGUI player2Nickname;
    [field: SerializeField] private TextMeshProUGUI winText;
    [field: SerializeField] private GameObject winPanel;

    [field: Header("Ready Menu UI")]
    [field: SerializeField] private GameObject readyMenu;
    [field: SerializeField] private Image player1readyImage;
    [field: SerializeField] private Image player2readyImage;

    private int player1Lifes = 3;
    private int player2Lifes = 3;
    private bool player1Ready = false;
    private bool player2Ready = false;
    private bool gameStarted = false;
    private bool gameIsOver = false;
    private Photon.Realtime.Player player1;
    private Photon.Realtime.Player player2;

    public bool GameIsOver { get => gameIsOver; }

    public static PUN_PongGameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreText();

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.IsMasterClient)
                player1 = player.Value;
            else
                player2 = player.Value;
        }

        photonView.RPC("UpdateNicknames", RpcTarget.AllBuffered);
    }

    private void OnGUI()
    {
        KeyCode keyCode = Event.current.keyCode;

        if (keyCode == KeyCode.Escape && Event.current.type == EventType.KeyDown)
        {
            LoadMainMenu();
        }

        if (keyCode == KeyCode.R && gameStarted && Event.current.type == EventType.KeyDown)
        {
            photonView.RPC("RestartGame", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void UpdateNicknames()
    {
        player1Nickname.text = player1.NickName;
        player2Nickname.text = player2.NickName;
    }

    [PunRPC]
    public void AddScore(Player playerLink)
    {
        if (playerLink == Player.One)
            player1Lifes--;

        if (playerLink == Player.Two)
            player2Lifes--;

        UpdateScoreText();
    }
   
    [PunRPC]
    public void PlayerReady(Player playerLink)
    {
        if (playerLink == Player.One)
            player1Ready = true;

        if (playerLink == Player.Two)
            player2Ready = true;

        UpdateReadyMenu();
    }

    [PunRPC]
    private void UpdateScoreText()
    {
        player1ScoreText.text = player1Lifes.ToString();
        player2ScoreText.text = player2Lifes.ToString();

        if (player1Lifes == 0 || player2Lifes == 0)
        {
            string playerNickname = player1Lifes == 0 ? player2Nickname.text : player1Nickname.text;

            winPanel.SetActive(true);

            winText.text = $"{playerNickname} Wins";

            gameIsOver = true;
        }
    }

    [PunRPC]
    private void UpdateReadyMenu()
    {
        player1readyImage.gameObject.SetActive(player1Ready);
        player2readyImage.gameObject.SetActive(player2Ready);

        gameStarted = player1Ready && player2Ready;

        if (gameStarted)
        {
            Destroy(readyMenu);

            FindObjectOfType<BallMovement>().Launch();
        }
    }

    [PunRPC]
    private void RestartGame()
    {
        PhotonNetwork.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void LoadMainMenu()
    {
        PhotonNetwork.Disconnect();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        LoadMainMenu();
    }
}
