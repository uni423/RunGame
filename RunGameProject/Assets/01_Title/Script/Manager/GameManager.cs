using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Define;

public class GameManager : Singleton<GameManager>
{
    public CharType CharacterCode = CharType.Knight;

    public UIManager02 uiMG; 
    public PlayerManager playerMG;

    public bool IsGamePlay = false;

    new void Awake()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);

        base.Awake();

        if (SceneManager.GetActiveScene().name == "02_Ingame")
        {
            uiMG = GameObject.FindObjectOfType<UIManager02>();
            playerMG = GameObject.FindObjectOfType<PlayerManager>();

            uiMG.Init();
            playerMG.Init();

            IsGamePlay = true;
        }
    }
}