using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public int playersTurn;
    public static GameManager instance;
    public List<Button> availableButtons;

    public string[] playfield = new string[9];

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
        playersTurn = 1;

        if(FusionManager.instance.runner.LocalPlayer.PlayerId == 2)
        {
            FusionManager.instance.statusMessage.text = "Wait for your turn!";
        }
        availableButtons = FusionManager.instance.cellsParent.GetComponentsInChildren<Button>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startGame()
    {
        foreach (var button in availableButtons)
        {
            button.interactable = true;
        }

        FusionManager.instance.statusMessage.text = "Its your turn!";
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

        ChangePlayer();
    }

    public void ChangePlayer()
    {
        if(playersTurn == 1)
        {
            playersTurn = 2;
        }
        else
        {
            playersTurn = 1;
        }

        if(Runner.LocalPlayer.PlayerId == playersTurn) 
        {
            FusionManager.instance.statusMessage.text = "Its your turn!";

            foreach (var button in availableButtons)
            {
                button.interactable = true;
            }
        }
        else
        {
            FusionManager.instance.statusMessage.text = "Wait for your turn!";

            foreach (var button in availableButtons)
            {
                button.interactable = false;
            }
        }
    }

    

}
