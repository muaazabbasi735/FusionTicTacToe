using DG.Tweening;
using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public int playersTurn;
    public static GameManager instance;
    public List<Button> availableButtons;
    public TMP_Text statusMsg;
    public string[] playfield = new string[9];
    public Button backBtn;
    public GameObject gameBoard;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

        gameBoard.transform.localPosition = new Vector3(1920f, 0f,0f);
        gameBoard.transform.DOLocalMoveX(0f, 3f);

        backBtn.onClick.AddListener(GoToLobby);

        playersTurn = 1;

        if(FusionManager.instance.runner.LocalPlayer.PlayerId == 2)
        {
            statusMsg.text = "Please wait for your turn!";
        }

    }

    private void GoToLobby()
    {
        FusionManager.instance.runner.Shutdown(false, ShutdownReason.Ok, true);
    }

    public void startGame()
    {
        UpdateCellInteraction(true);
        statusMsg.text = "Its your turn!";
    }

    public void RemoveButton(Button button)
    {
        int i = availableButtons.IndexOf(button);
        RPC_RemoveButtonFromList(i);
    }

    [Rpc]
    public void RPC_RemoveButtonFromList(int i)
    {
        availableButtons.RemoveAt(i);
    }

    public void ChangePlayer()
    {
        playersTurn = (playersTurn == 1) ? 2 : 1;

        if(Runner.LocalPlayer.PlayerId == playersTurn) 
        {
            statusMsg.text = "Its your turn!";
            UpdateCellInteraction(true);
            
        }
        else
        {
            statusMsg.text = "Wait for your turn!";
            UpdateCellInteraction(false);
           
        }
    }

    private void UpdateCellInteraction(bool isIntrctve)
    {
        foreach (var button in availableButtons)
        {
            button.interactable = isIntrctve;
        }
    }

    public void UpdatePlayfield(int i, string s)
    {
        playfield.SetValue(s, i);
        CheckWinstate();
    }

    public void CheckWinstate()
    {
        // Check all possible winning combinations
        string winner = CheckRows() ?? CheckColumns() ?? CheckDiagonals();
        if (IsBoardFull())
        {

            Debug.Log("Draw!");
            statusMsg.text = "Its a draw!";
            backBtn.gameObject.SetActive(true);
            UpdateCellInteraction(false);

        }
        else
        {
            if (winner != null)
            {
                if (winner.Equals("X"))
                {
                    Debug.Log("X wins!");
                    statusMsg.text = "Player X Won!";
                    backBtn.gameObject.SetActive(true);
                    UpdateCellInteraction(false);


                }
                else
                {
                    Debug.Log("O wins!");
                    statusMsg.text = "Player O Won!";
                    backBtn.gameObject.SetActive(true);
                    UpdateCellInteraction(false);


                }
            }
            else
            {
                ChangePlayer();
            }

        }



    }

    private string CheckRows()
    {
        // Check row 1 (indices 0,1,2)
        if (playfield[0] != "" && playfield[0] == playfield[1] && playfield[1] == playfield[2])
            return playfield[0];

        // Check row 2 (indices 3,4,5)
        if (playfield[3] != "" && playfield[3] == playfield[4] && playfield[4] == playfield[5])
            return playfield[3];

        // Check row 3 (indices 6,7,8)
        if (playfield[6] != "" && playfield[6] == playfield[7] && playfield[7] == playfield[8])
            return playfield[6];

        return null;
    }

    private string CheckColumns()
    {
        // Check column 1 (indices 0,3,6)
        if (playfield[0] != "" && playfield[0] == playfield[3] && playfield[3] == playfield[6])
            return playfield[0];

        // Check column 2 (indices 1,4,7)
        if (playfield[1] != "" && playfield[1] == playfield[4] && playfield[4] == playfield[7])
            return playfield[1];

        // Check column 3 (indices 2,5,8)
        if (playfield[2] != "" && playfield[2] == playfield[5] && playfield[5] == playfield[8])
            return playfield[2];

        return null;
    }

    private string CheckDiagonals()
    {
        // Check diagonal 1 (indices 0,4,8)
        if (playfield[0] != "" && playfield[0] == playfield[4] && playfield[4] == playfield[8])
            return playfield[0];

        // Check diagonal 2 (indices 2,4,6)
        if (playfield[2] != "" && playfield[2] == playfield[4] && playfield[4] == playfield[6])
            return playfield[2];

        return null;
    }

    private bool IsBoardFull()
    {
        // Check if all cells are occupied (no empty strings)
        for (int i = 0; i < playfield.Length; i++)
        {
            if (playfield[i] == "")
                return false;
        }
        return true;
    }

    
}
