using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager02 : MonoBehaviour
{
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

    public void CharactorSelect_BT(int value)
    {
        GameManager.Instance.CharactorCode = value;
        CharactorSelect.SetActive(false);
        GameManager.Instance.IsGamePlay = true;
    }
}
