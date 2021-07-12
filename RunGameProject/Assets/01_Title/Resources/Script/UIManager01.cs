using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager01 : MonoBehaviour
{
    public GameObject Title;
    public GameObject Key;

    public void Awake()
    {
        Screen.SetResolution(1920, 1080, true); 
        Screen.fullScreen = true;
        SoundManager.Instance.PlayBGM("BGM_Title");
        SoundManager.Instance.SetBGM(0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Esc();
    }

    public void Esc()
    {
        Application.Quit();
    }

    public void To_Ingame()
    {
        GameManager.Instance.CharacterCode = Define.CharType.Knight;
        GameManager.Instance.stage = Define.Stage.Stage1_1;

        LoadManager.Load(LoadManager.Scene.Ingame);
        LoadManager.LoaderCallback();
    }

    public void Key_OnOff()
    {
        if (Key.activeSelf)
            Key.SetActive(false);
        else
            Key.SetActive(true);
    }
}
