using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PongGameManager : MonoBehaviour
{
    [field: Header("Score UI")]
    [field: SerializeField] private TextMeshProUGUI player1ScoreText;
    [field: SerializeField] private TextMeshProUGUI player2ScoreText;
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

    public bool GameIsOver { get => gameIsOver; }

    public static PongGameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreText();
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
            RestartGame();
        }
    }

    public void AddScore(Player playerLink)
    {
        if (playerLink == Player.One)
            player1Lifes--;

        if (playerLink == Player.Two)
            player2Lifes--;

        UpdateScoreText();
    }

    public void PlayerReady(Player playerLink)
    {
        if (playerLink == Player.One)
            player1Ready = true;

        if (playerLink == Player.Two)
            player2Ready = true;

        UpdateReadyMenu();
    }

    private void UpdateScoreText()
    {
        player1ScoreText.text = player1Lifes.ToString();
        player2ScoreText.text = player2Lifes.ToString();

        if (player1Lifes == 0 || player2Lifes == 0)
        {
            Player player = player1Lifes == 0 ? Player.Two : Player.One;

            winPanel.SetActive(true);

            winText.text = $"Player {player} Wins";

            gameIsOver = true;
        }
    }

    private void UpdateReadyMenu()
    {
        player1readyImage.gameObject.SetActive(player1Ready);
        player2readyImage?.gameObject.SetActive(player2Ready);

        gameStarted = player1Ready && (player2Ready || PongGameSpawner.Instance.AgainstBot);

        if (gameStarted)
        {
            Destroy(readyMenu);

            FindObjectOfType<BallMovement>().Launch();
        }
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
