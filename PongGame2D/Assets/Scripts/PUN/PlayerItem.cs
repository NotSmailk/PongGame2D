using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [field: SerializeField] private TextMeshProUGUI nickname;
    [field: SerializeField] private GameObject readyCheck;

    private Photon.Realtime.Player player;
    private Hashtable playerProperties = new Hashtable();

    public bool GetReadyState()
    {
        return (bool)playerProperties["RoomIsPlayerReady"];
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            if (targetPlayer.CustomProperties.ContainsKey("RoomIsPlayerReady"))
            {
                bool ready = (bool)targetPlayer.CustomProperties["RoomIsPlayerReady"];

                SetPlayerReadyCheck(ready);
            }
            else
            {
                SetPlayerReadyCheck(false);
            }
        }
    }

    public Photon.Realtime.Player GetPlayerInfo()
    {
        return player;
    }

    public void SetPlayerInfo(Photon.Realtime.Player player, bool ready)
    {
        this.player = player;

        nickname.text = player.NickName;

        SetPlayerReadyCheck(ready);
    }

    public void SetPlayerReadyCheck(bool ready, bool sendProperties = false)
    {
        playerProperties["RoomIsPlayerReady"] = ready;

        readyCheck.SetActive(ready);

        if (sendProperties)
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
}
