using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Define;
using Coffee.UIExtensions;

public class UIManager02 : MonoBehaviour
{
    public PlayerManager PlayerMgr;

    public Image UI_hp;
    public Image UI_sp;

    public Image UI_Skill1;
    public Image UI_Skill2;
    public Image UI_Skill3;

    public void Start()
    {
        UI_Shiny(1);
        UI_Shiny(2);
    }

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

    public void UI_Shiny(int num, bool IsPlay = true)
    {
        //몇번째 스킬아이콘을 Play할건지 num으로 받기.

        if (num == 1)
        {
            if (IsPlay)
            {
                UI_Skill2.transform.GetComponent<UIShiny>().Play();
                UI_Skill2.color = new Color(170/255f, 1f, 182/255f);
            }
            else
            {
                UI_Skill2.transform.GetComponent<UIShiny>().Stop();
                UI_Skill2.color = new Color(100/255f, 150/255f, 100/255f);
            }    
        }

        else if (num == 2)
        {
            if (IsPlay)
            {
                UI_Skill3.transform.GetComponent<UIShiny>().Play();
                UI_Skill3.color = new Color(170 / 255f, 1f, 182 / 255f);
            }
            else
            {
                UI_Skill3.transform.GetComponent<UIShiny>().Stop();
                UI_Skill3.color = new Color(100 / 255f, 150 / 255f, 100 / 255f);
            }
        }
    }
}