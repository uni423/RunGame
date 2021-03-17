using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;


public class UIManager02 : MonoBehaviour
{
    public CharDatas charDataObj;

    [SerializeField]
    private GameObject CharactorObj;
    public GameObject CharactorSelect;

    void Start()
    {
        GameManager.Instance.IsGamePlay = false;
    }

    public void CharactorSelect_BT(int type)
    {
        GameManager.Instance.CharactorCode = (CharType)type;
        CharactorObj.GetComponent<Player>().Char = charDataObj.GetStat((CharType)type);
        CharactorObj.GetComponent<Player>().StatUpgrade();
        CharactorSelect.SetActive(false);
        GameManager.Instance.IsGamePlay = true;
    }
}
