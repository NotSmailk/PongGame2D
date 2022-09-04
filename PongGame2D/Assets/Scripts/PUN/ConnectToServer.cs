using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [field: SerializeField] private TMP_InputField usernameInput;
    [field: SerializeField] private TextMeshProUGUI buttonText;

    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1 && usernameInput.text.Length <= 12)
        {
            PhotonNetwork.NickName = usernameInput.text;

            buttonText.text = "Connecting...";

            PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
}
