using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    [SerializeField] private int cellIndex;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}