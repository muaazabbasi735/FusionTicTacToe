using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
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
        if (FusionManager.instance.runner.LocalPlayer.PlayerId == 0)
        {
            text.text = "X";
        }
        else
        {
            text.text = "O";
        }

        GameManager.instance.RemoveButton(button);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}