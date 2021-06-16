using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;

public class UIManager02 : MonoBehaviour
{
    public PlayerManager PlayerMgr;

    public GameObject HPnSP;
    public Image UI_hp;
    public Image UI_sp;

    public GameObject Skill;
    public Transform UI_Skill1;
    public Transform UI_Skill2;

    [Header("GameOver UI")]
    public Transform Over_Obj;
    public Transform Over_Image;
    public Transform Over_UIs;

    public void Start()
    {
        Over_Obj.gameObject.SetActive(false);

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
        UI_Skill1.Find("Filed").GetComponent<Image>().fillAmount =
            (PlayerMgr.Player.GetComponent<Player>().skKnightA.MaxCoolTime - PlayerMgr.Player.GetComponent<Player>().skKnightA.NowCoolTime)
            / PlayerMgr.Player.GetComponent<Player>().skKnightA.MaxCoolTime;

        UI_Skill2.Find("Filed").GetComponent<Image>().fillAmount =
            (PlayerMgr.Player.GetComponent<Player>().skKnightK.MaxCoolTime - PlayerMgr.Player.GetComponent<Player>().skKnightK.NowCoolTime)
            / PlayerMgr.Player.GetComponent<Player>().skKnightK.MaxCoolTime;
    }

    public void UI_Shiny(int num, bool IsPlay = true)
    {
        //몇번째 스킬아이콘을 Play할건지 num으로 받기.

        if (num == 1)
        {
            if (IsPlay)
            {
                UI_Skill1.Find("SkillUI_1").transform.GetComponent<UIShiny>().Play();
                UI_Skill1.Find("Filed").gameObject.SetActive(false);
            }
            else
            {
                UI_Skill1.Find("SkillUI_1").transform.GetComponent<UIShiny>().Stop();
                UI_Skill1.Find("Filed").gameObject.SetActive(true);
            }
        }

        else if (num == 2)
        {
            if (IsPlay)
            {
                UI_Skill2.Find("SkillUI_2").transform.GetComponent<UIShiny>().Play();
                UI_Skill2.Find("Filed").gameObject.SetActive(false);
            }
            else
            {
                UI_Skill2.Find("SkillUI_2").transform.GetComponent<UIShiny>().Stop();
                UI_Skill2.Find("Filed").gameObject.SetActive(true);
            }
        }
    }

    public void Game_Clear()
    {
    }

    #region GameOver

    public void Game_Over(string Enemy)
    {
        string enemyName;

        switch(Enemy)
        {
            case "Slime":
                enemyName = "슬라임"; break;
            case "SHurdle":
                enemyName = "그루터기"; break;
            case "LHurdle":
                enemyName = "나무"; break;
            default :
                enemyName = "원인불명"; break;
        }

        Over_UIs.Find("Over Reason").GetChild(2).GetComponent<Text>().text = enemyName;

        StartCoroutine(Game_Over_Anim());
    }

    public void ReStart()
    {
        LoadManager.Load(LoadManager.Scene.Ingame);
        LoadManager.LoaderCallback();
    }

    public void To_Title()
    {
        LoadManager.Load(LoadManager.Scene.Title);
        LoadManager.LoaderCallback();
    }

    #endregion

    public IEnumerator Game_Over_Anim()
    {
        //Active 켜주기, 탐험종료 이미지 위치 조정, UI투명
        Over_Obj.gameObject.SetActive(true);
        Over_Obj.position = new Vector3(0, 0);
        Over_Obj.localScale = new Vector3(0, 0);
        Over_Image.position = new Vector3(0, 0);
        Over_UIs.GetComponent<CanvasGroup>().alpha = 0;
        DOTween.To(() => new Vector3(0, 0), x => Over_Obj.GetComponent<RectTransform>().sizeDelta = x, new Vector3(1920, 1080), 0.3f);

        //탐험종료 이미지,텍스트 나타나기
        Over_Image.GetComponent<Image>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0f);
        Over_Image.GetComponent<Image>().DOFade(1f, 0.2f).SetEase(Ease.Linear);
        Over_Obj.GetChild(1).GetComponent<Text>().color = new Color(42 / 255f, 42 / 255f, 42 / 255f, 0f);
        Over_Obj.GetChild(1).GetComponent<Text>().DOFade(1f, 0.2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);

        DOTween.To(() => HPnSP.GetComponent<CanvasGroup>().alpha
        , x => HPnSP.GetComponent<CanvasGroup>().alpha = x, 0, 0.3f);
        DOTween.To(() => Skill.GetComponent<CanvasGroup>().alpha
        , x => Skill.GetComponent<CanvasGroup>().alpha = x, 0, 0.3f);

        //탐험종료 이미지 올라가기, UI 나타나기
        //Over_Image.DOMoveY(420f, 0.3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.2f);

        DOTween.To(() => Over_UIs.GetComponent<CanvasGroup>().alpha
        , x => Over_UIs.GetComponent<CanvasGroup>().alpha = x, 1, 0.3f);
    }
}