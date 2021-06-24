using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Define;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public CharType CharacterCode = CharType.Knight;
    public PlayerManager playerMG;
    public UIManager02 uiMG;

    public bool IsGamePlay = false;

    new void Awake()
    {
        base.Awake();

        //uiMG.Init();
        //playerMG.Init();
    }
}