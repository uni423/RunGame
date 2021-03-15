using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharactorType { Knight = 1, Gunner }

public class UIManager02 : MonoBehaviour
{
    [SerializeField]
    private List<CharactorData> charactorDatas;

    [SerializeField]
    private GameObject CharactorObj;
    public GameObject CharactorSelect;

    void Start()
    {
        GameManager.Instance.IsGamePlay = false;
        //StartCoroutine(TimeStop());
    }

    IEnumerator TimeStop()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }

    public void CharactorSelect_BT(int type)
    {
        CharactorObj.GetComponent<Player>().CharactorData = charactorDatas[type].StatList[0];
        CharactorSelect.SetActive(false);
        GameManager.Instance.IsGamePlay = true;
    }
}
