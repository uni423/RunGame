using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private List<GameObject> Chars = new List<GameObject>();

    [SerializeField] //원본 데이터 값
    private List<PlayerStat> StatDatas = new List<PlayerStat>();
    [SerializeField] //게임 중 데이터 값을 넣을 Scriptable Obj
    private List<PlayerStat> StatSaves = new List<PlayerStat>();
    public List<PlayerStat> statSaves { get { return StatSaves; }}

    public GameObject Player;

    public float swith_MaxCollTime;
    public float swith_NowCollTime = 0;
    public bool Is_Invincibility = false;

    public delegate void Delegate();

    public void Init()
    {
        for (int i = 0; i < StatDatas.Count; i++)
        {
            Player = transform.GetChild(i).gameObject;
            Player.SetActive(false);
            Player.transform.position = new Vector3(-600, -300);

            if (StatSaves.Count < i+1)
            {
                Chars.Add(Player);
                StatSaves.Add(ScriptableObject.CreateInstance<PlayerStat>());
                InitStat((CharType)i);
                Chars[i].GetComponent<Player>().Stat = StatSaves[i];
                Chars[i].GetComponent<Player>().StatUpgrade();
            }
        }
        Chars[0].GetComponent<Player_Knight>().Init();
        Chars[1].GetComponent<Player_Gunner>().Init();

        Player = Chars[(int)GameManager.Instance.CharacterCode];

        Player.SetActive(true);
    }

    public void InitStat(CharType type)
    {
        StatSaves[(int)type].Level = StatDatas[(int)type].Level;
        StatSaves[(int)type].MaxExp = StatDatas[(int)type].MaxExp;
        StatSaves[(int)type].AddExp = StatDatas[(int)type].AddExp;
        StatSaves[(int)type].NowExp = StatDatas[(int)type].NowExp;

        StatSaves[(int)type].MaxHp = StatDatas[(int)type].MaxHp;
        StatSaves[(int)type].AddHp = StatDatas[(int)type].AddHp;
        StatSaves[(int)type].NowHp = StatDatas[(int)type].NowHp;

        StatSaves[(int)type].MaxSp = StatDatas[(int)type].MaxSp;
        StatSaves[(int)type].AddSp = StatDatas[(int)type].AddSp;
        StatSaves[(int)type].NowSp = StatDatas[(int)type].NowSp;
        StatSaves[(int)type].PlusSp = StatDatas[(int)type].PlusSp;

        StatSaves[(int)type].Ad = StatDatas[(int)type].Ad;
        StatSaves[(int)type].Addad = StatDatas[(int)type].Addad;
        StatSaves[(int)type].AdRange = StatDatas[(int)type].AdRange;
        StatSaves[(int)type].AdDistance = StatDatas[(int)type].AdDistance;
        StatSaves[(int)type].AdSpeed = StatDatas[(int)type].AdSpeed;
        StatSaves[(int)type].AddadSpeed = StatDatas[(int)type].AddadSpeed;

        StatSaves[(int)type].Speed = StatDatas[(int)type].Speed;
        StatSaves[(int)type].Addspeed = StatDatas[(int)type].Addspeed;
        StatSaves[(int)type].JumpPower = StatDatas[(int)type].JumpPower;
    }

    public void ReleaseStat()
    {
        for (int i = 0; i < Chars.Count; i++)
        {
            Chars[i].SetActive(false);
            Chars[i].GetComponent<Player>().Stat = null;
        }
        StatSaves.Clear();
        Chars.Clear();
        Debug.LogError("초기화 완료");
    }

    public bool Character_Swich(CharType type)
    {
        if (swith_NowCollTime > 0)
            return false;

        Player.SetActive(false);
        Vector3 vector = Player.transform.position;

        GameManager.Instance.CharacterCode = type;
        Player = Chars[(int)type];
        Player.transform.position = vector;

        Player.SetActive(true);

        //쿨타임
        swith_NowCollTime = swith_MaxCollTime;
        DOTween.To(() => swith_NowCollTime, x => swith_NowCollTime = x, 0, swith_MaxCollTime)
                     .SetEase(Ease.Linear);
        //무적
        Is_Invincibility = true;
        StartCoroutine(Timer(0.5f, () => { Is_Invincibility = false; }));

        //점프
        Player.GetComponent<Player>().Jump(true);

        return true;
    }

    public void Damage(float damage, string Enemy)
    {
        if (Is_Invincibility)
            return;

        switch (GameManager.Instance.CharacterCode)
        {
            case CharType.Knight:
                if (Player.GetComponent<Player_Knight>().Is_SkillA && Enemy != "Drop")
                {
                    Debug.Log("Shild");
                    Player.GetComponent<Player_Knight>().Shild_Quit(true);
                    return;
                }
                else if (Player.GetComponent<Player_Knight>().Is_SkillK && Enemy != "Drop")
                    return;
                Player.GetComponent<Player>().Damage(damage, Enemy);
                break;

            case CharType.Gunner:
                if (Player.GetComponent<Player_Gunner>().Is_SkillA && Enemy != "Drop")
                {
                    Debug.Log("Shild");
                    Player.GetComponent<Player_Gunner>().Shild_Quit(true);
                    return;
                }
                else if (Player.GetComponent<Player_Gunner>().Is_SkillK && Enemy != "Drop")
                    return;
                Player.GetComponent<Player>().Damage(damage, Enemy);
                break;

        }
    }
    IEnumerator Timer(float time, Delegate dele)
    {
        yield return new WaitForSeconds(time);
        dele();
    }
}
