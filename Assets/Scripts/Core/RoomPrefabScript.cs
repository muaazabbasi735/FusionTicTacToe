using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPrefabScript : MonoBehaviour
{
    public TMP_Text roomName;
    public TMP_Text playerCount;
    public Button joinButton;

    private void Awake()
    {
        joinButton.onClick.AddListener(JoinSession);
    }

    private void JoinSession()
    {
        FusionManager.instance.JoinSession(roomName.text);
    }
}
