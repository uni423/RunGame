using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;

public class UIManager02 : MonoBehaviour
{
    float time_Start;
    float time_Current;

    public GameObject HPnSP;
    public Image UI_hp;
    public Image UI_sp;
    public Text UI_level;

    public GameObject Skill;
    public Transform UI_Skill1;
    private Image Skill1_Filed;
    public Transform UI_Skill2;
    private Image Skill2_Filed;

    [Header("GameOver UI")]
    public Transform Over_Obj;
    public Transform Over_Image;
    public Transform Over_UIs;

    public Image GameClear;

    public GameObject UI_CutScene;

    public PlayerManager playerMG;

    public void Start()
    {
        GameManager.Instance.Ingame_Init();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.StopSFX();

        switch(GameManager.Instance.stage)
        {
            case Define.Stage.Stage1_1:
            case Define.Stage.Stage1_2:
            case Define.Stage.Stage1_3:
                SoundManager.Instance.PlayBGM("BGM_Stage_1");
                break;
            case Define.Stage.Stage1_Boss:
                SoundManager.Instance.PlayBGM("BGM_Stage_1_Boss");
                break;
            default:
                break;
        }
        SoundManager.Instance.SetBGM(0.5f);
        SoundManager.Instance.SetSFX(0.5f);

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        time_Start = Time.time;
        time_Current = 0f;

        Over_Obj.gameObject.SetActive(false);

        Skill1_Filed = UI_Skill1.Find("Filed").GetComponent<Image>();
        Skill2_Filed = UI_Skill2.Find("Filed").GetComponent<Image>();

        UI_Shiny(1);
        UI_Shiny(2);

        playerMG = GameManager.Instance.playerMG;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        time_Current = Time.time - time_Start;

        //Level
        UI_level.text = playerMG.Player.GetComponent<Player>().Stat.Level.ToString();

        //Hp, Sp
        UI_hp.fillAmount = playerMG.Player.GetComponent<Player>().NowHpSp(1);
        UI_sp.fillAmount = playerMG.Player.GetComponent<Player>().NowHpSp(2);

        //Skill
        switch (GameManager.Instance.CharacterCode)
        {
            case Define.CharType.Knight:
                Skill1_Filed.fillAmount = playerMG.Player.GetComponent<Player>().SkillNowCool(1);
                Skill2_Filed.fillAmount = playerMG.Player.GetComponent<Player>().SkillNowCool(2);
                break;
            case Define.CharType.Gunner:
                Skill1_Filed.fillAmount = playerMG.Player.GetComponent<Player>().SkillNowCool(1);
                Skill2_Filed.fillAmount = playerMG.Player.GetComponent<Player>().SkillNowCool(2);
                break;
        }
    }

    public void UI_Shiny(int num, bool IsPlay = true)
    {
        //몇번째 스킬아이콘을 Play할건지 num으로 받기.

        if (num == 1)
        {
            if (IsPlay)
            {
                UI_Skill1.Find("SkillUI_1").transform.GetComponent<UIShiny>().Play();
                Skill1_Filed.gameObject.SetActive(false);
            }
            else
            {
                UI_Skill1.Find("SkillUI_1").transform.GetComponent<UIShiny>().Stop();
                Skill1_Filed.gameObject.SetActive(true);
            }
        }

        else if (num == 2)
        {
            if (IsPlay)
            {
                UI_Skill2.Find("SkillUI_2").transform.GetComponent<UIShiny>().Play();
                Skill2_Filed.gameObject.SetActive(false);
            }
            else
            {
                UI_Skill2.Find("SkillUI_2").transform.GetComponent<UIShiny>().Stop();
                Skill2_Filed.gameObject.SetActive(true);
            }
        }
    }

    public void CutScene()
    {
        switch(GameManager.Instance.CharacterCode)
        {
            case Define.CharType.Knight:
                UI_CutScene.transform.Find("Character").Find("Knight").gameObject.SetActive(true);
                UI_CutScene.transform.Find("Character").Find("Gunner").gameObject.SetActive(false);
                break;
            case Define.CharType.Gunner:
                UI_CutScene.transform.Find("Character").Find("Knight").gameObject.SetActive(false);
                UI_CutScene.transform.Find("Character").Find("Gunner").gameObject.SetActive(true);
                break;
        }

        UI_CutScene.transform.Find("Character").position = new Vector3(0, 0, 0);
        UI_CutScene.transform.position = new Vector3(-500, 400, 0);
        UI_CutScene.gameObject.SetActive(true);
        UI_CutScene.transform.DOMove(new Vector3(0, 0, 0), 1f);
        //UI_CutScene.transform.Find("Character").DOMove(new Vector3(829, 376), 1f);
        //UI_CutScene.transform.DOMove(new Vector3(-960, -540), 2f);
    }

    #region GameOver

    public void Game_Over(string Enemy)
    {
        string enemyName;
        switch (Enemy)
        {
            case "Slime":
                enemyName = "슬라임"; break;
            case "SHurdle":
                enemyName = "그루터기"; break;
            case "LHurdle":
                enemyName = "나무"; break;
            case "Drop":
                enemyName = "낙사"; break;
            default:
                enemyName = Enemy; break;
        }

        string stageName;
        switch (GameManager.Instance.stage)
        {
            case Define.Stage.Stage1_1:
                stageName = "숲 스테이지 1"; break;
            case Define.Stage.Stage1_2:
                stageName = "숲 스테이지 2"; break;
            case Define.Stage.Stage1_3:
                stageName = "숲 스테이지 3"; break;
            case Define.Stage.Stage1_Boss:
                stageName = "숲 스테이지 보스"; break;
            default:
                stageName = GameManager.Instance.stage.ToString(); break;
        }
        Over_UIs.Find("Time").GetChild(2).GetComponent<Text>().text = time_Current.ToString("N2") + "초";
        Over_UIs.Find("Map").GetChild(2).GetComponent<Text>().text = stageName;
        Over_UIs.Find("Score").GetChild(2).GetComponent<Text>().text = GameManager.Instance.score.ToString();

        Over_UIs.Find("Journal").GetChild(2).GetComponent<Text>().text 
            = "몬스터들을 " + GameManager.Instance.DeadMonsters + "마리 잡았다.";
        Over_UIs.Find("Over Reason").GetChild(2).GetComponent<Text>().text = enemyName;
        StartCoroutine(Game_Over_Anim());
    }

    public void Game_Clear()
    {
        Over_UIs.Find("Over Reason").GetChild(2).GetComponent<Text>().text = "탐험 성공";
        Over_UIs.Find("Time").GetChild(2).GetComponent<Text>().text = time_Current.ToString("N2") + "초";
        Over_UIs.Find("Journal").GetChild(2).GetComponent<Text>().text = "여기까지 베타테스트 였습니다. \n플레이해주셔서 감사합니다.";
        GameClear.gameObject.SetActive(false);
        StartCoroutine(Game_Over_Anim());
    }

    public void ReStart()
    {
        GameManager.Instance.score = 0;
        GameManager.Instance.DeadMonsters = 0;
        GameManager.Instance.playerMG.ReleaseStat();
        LoadManager.Load(LoadManager.Scene.Ingame);
        LoadManager.LoaderCallback();
    }

    public void To_Title()
    {
        GameManager.Instance.score = 0;
        GameManager.Instance.DeadMonsters = 0;
        GameManager.Instance.playerMG.ReleaseStat();
        LoadManager.Load(LoadManager.Scene.Title);
        LoadManager.LoaderCallback();
    }

    #endregion

    public IEnumerator Game_Over_Anim()
    {
        SoundManager.Instance.StopBGM();

        //Active 켜주기, UI투명
        Over_Obj.gameObject.SetActive(true);
        Over_UIs.GetComponent<CanvasGroup>().alpha = 0;

        //탐험종료 이미지,텍스트 나타나기
        DOTween.To(() => new Vector3(1920, 0, 0), x => Over_Image.GetComponent<RectTransform>().sizeDelta = x, new Vector3(1920, 300, 0), 0.3f);
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
        DOTween.To(() => new Vector3(1920, 300, 0), x => Over_Image.GetComponent<RectTransform>().sizeDelta = x, new Vector3(1920, 1080, 0), 0.2f)
            .SetEase(Ease.OutSine);
        Over_Obj.GetChild(1).DOMoveY(430, 0.2f)
            .SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.2f);

        DOTween.To(() => Over_UIs.GetComponent<CanvasGroup>().alpha
        , x => Over_UIs.GetComponent<CanvasGroup>().alpha = x, 1, 0.3f);
    }
}