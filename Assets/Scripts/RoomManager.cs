using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loading;
    [SerializeField]
    private GameObject mainmenu;
    [SerializeField]
    private GameObject waitObj;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private Button playButton;

    private void Awake()
    {
        playButton.transform.localPosition = new Vector3(-1200f, 0,0);
        playButton.transform.DOLocalMoveX(0f, 3f);
    }

    public void CreateRoom()
    {
        waitObj.SetActive(true);
        createButton.interactable = false;
        FusionManager.instance.CreateSession();
        
    }

    public void OnPlay()
    {
        playButton.transform.DOLocalMoveX(1200f, 3f).OnComplete(() =>
        {
            loading.SetActive(true);
            mainmenu.SetActive(false);
            StartCoroutine(OnPlayCort());
        });
    }

    IEnumerator OnPlayCort()
    {
        yield return new WaitForSeconds(2f);
        loading.SetActive(false);
    }

    IEnumerator HideWaitCort()
    {
        yield return new WaitForSeconds(5f);
        waitObj.SetActive(false);
        createButton.interactable = true;

    }
}
