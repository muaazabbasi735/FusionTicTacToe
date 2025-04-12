using DG.Tweening;
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
        Debug.Log("Clicked!");
        if (FusionManager.instance.runner.LocalPlayer.PlayerId == 1)
        {
            RPC_UpdatePlayField(transform.GetSiblingIndex(), "X");

            RPC_SyncMove("X");

        }
        else
        {
            RPC_UpdatePlayField(transform.GetSiblingIndex(), "O");

            RPC_SyncMove("O");
        }

        GameManager.instance.RemoveButton(button);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SyncMove(string str)
    {
        button.interactable = false;
        text.text = str;
        text.transform.DOScale(Vector3.one, 1f);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_UpdatePlayField(int i, string s)
    {
        GameManager.instance.UpdatePlayfield(i, s);


    }
    
}