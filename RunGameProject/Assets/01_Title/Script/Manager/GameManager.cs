using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Define;

public class GameManager : Singleton<GameManager>
{
    public CharType CharacterCode = CharType.Knight;
    public Stage stage = Stage.Stage1_1;

    public UIManager02 uiMG;
    public PlayerManager playerMG;
    public BGManager bgMG;

    public bool IsGamePlay = false;

    //new void Awake()
    //{
    //    base.Awake();
    //}
     
    public void Ingame_Init()
    {
        Debug.Log("GameManager Init Start");

        if (SceneManager.GetActiveScene().name == "02_Ingame")
        {
            playerMG = GameObject.FindObjectOfType<PlayerManager>();
            uiMG = GameObject.FindObjectOfType<UIManager02>();
            bgMG = GameObject.FindObjectOfType<BGManager>();

            bgMG.Init();
            playerMG.Init();
            //uiMG.Init();

            stage = Stage.Stage1_1;

            IsGamePlay = true;
        }
        if (SceneManager.GetActiveScene().name == "02_Ingame_1-Boss")
        {
            playerMG = GameObject.FindObjectOfType<PlayerManager>();
            uiMG = GameObject.FindObjectOfType<UIManager02>();
            bgMG = GameObject.FindObjectOfType<BGManager>();

            bgMG.Init();
            playerMG.Init();
            //uiMG.Init();

            stage = Stage.Stage1_Boss;

            IsGamePlay = true;
        }
    }
}