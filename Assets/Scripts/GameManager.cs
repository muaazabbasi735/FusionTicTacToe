using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [Networked]
    public PlayerRef playersTurn {  get; set; }
    public static GameManager instance;
    public List<Button> availableButtons;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
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

}
