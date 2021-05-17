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

    public Image UI_Skill1;
    public Image UI_Skill2;
    public Image UI_Skill3;

    private void Update()
    {
        //Hp, Sp
        UI_hp.fillAmount =
            PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].NowHp
            / PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].MaxHp;
        UI_sp.fillAmount =
            PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].NowSp
            / PlayerMgr.statSaves[(int)GameManager.Instance.CharactorCode].MaxSp;

        //Skill
        UI_Skill2.fillAmount =
            (PlayerMgr.Player.GetComponent<Player>().skKnightA.MaxCoolTime - PlayerMgr.Player.GetComponent<Player>().skKnightA.NowCoolTime)
            / PlayerMgr.Player.GetComponent<Player>().skKnightA.MaxCoolTime;
    }
}