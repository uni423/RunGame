using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Define;


public class UIManager02 : MonoBehaviour
{
    public PlayerManager PlayerMgr;

    public Image UI_hp;
    public Image UI_sp;

    private void Update()
    {
        UI_hp.fillAmount = 
            PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].NowHp
            / PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].MaxHp;
        UI_sp.fillAmount = 
            PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].NowSp
            / PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].MaxSp;
    }
}
