using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardCell : NetworkBehaviour
{
    [SerializeField] private int cellIndex;
    private Button button;
    private TMP_Text text; 
    [SerializeField]
    private bool isX;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCellClicked);
        text = GetComponentInChildren<TMP_Text>();
    }

    private void OnCellClicked()
    {
        button.interactable = false;
        Debug.Log("Clicked!");
        if (FusionManager.instance.runner.LocalPlayer.PlayerId == 1)
        {
          
            RPC_SyncMove("X");
            RPC_UpdatePlayField(transform.GetSiblingIndex(), "X");

        }
        else
        {
            
            RPC_SyncMove("O");
            RPC_UpdatePlayField(transform.GetSiblingIndex(), "O");
        }

        GameManager.instance.RemoveButton(button);
    }

    [Rpc]
    public void RPC_SyncMove(string str)
    {
        text.text = str;
        button.interactable = false;
    }

    [Rpc]
    public void RPC_UpdatePlayField(int i, string s)
    {
        GameManager.instance.UpdatePlayfield(i, s);


    }
    
}