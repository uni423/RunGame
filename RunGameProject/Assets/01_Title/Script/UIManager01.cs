using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager01 : MonoBehaviour
{
    public GameObject Title;
    public GameObject Character;

    public void Start()
    {
        Title.SetActive(true);
        Character.SetActive(false);
    }

    public void Game_Start()
    {
        Title.SetActive(false);
        Character.SetActive(true);
    }

    public void Character_Select(int num)
    {
        GameManager.Instance.CharacterCode = (Define.CharType)num;
    }

    public void To_Ingame()
    {
        LoadManager.Load(LoadManager.Scene.Ingame);
        LoadManager.LoaderCallback();
    }
}
