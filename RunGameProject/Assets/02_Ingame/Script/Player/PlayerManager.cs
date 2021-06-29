using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

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
    public float swith_NowCollTime;

    public void Init()
    {
        for(int i = 0; i < StatDatas.Count; i++)
        {
            Player = transform.GetChild(i).gameObject;
            Player.SetActive(false);
            Chars.Add(Player);

            StatSaves.Add(ScriptableObject.CreateInstance<PlayerStat>());
            InitStat((CharType)i);

            Chars[i].GetComponent<Player>().Stat = StatSaves[i];
            Chars[i].GetComponent<Player>().StatUpgrade();
        }

        Player = Chars[(int)GameManager.Instance.CharacterCode];
        Debug.Log("PlayerManager Init 완료");

        Player.SetActive(true);

        GameManager.Instance.IsGamePlay = true;
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

    public void Character_Swich(CharType type)
    {
        if (type == GameManager.Instance.CharacterCode)
            return;
    }

    public void Damage(float damage, string Enemy)
    {
        switch(GameManager.Instance.CharacterCode)
        {
            case CharType.Knight:
                if (Player.GetComponent<Player_Knight>().Is_SkillA)
                {
                    Player.GetComponent<Player_Knight>().Shild_Quit(true);
                    return;
                }
                else if (Player.GetComponent<Player_Knight>().Is_SkillK)
                    return;
                Player.GetComponent<Player>().Damage(damage, Enemy);
                break;

            case CharType.Gunner:
                if (Player.GetComponent<Player_Knight>().Is_SkillA)
                {
                    Player.GetComponent<Player_Knight>().Shild_Quit(true);
                    return;
                }
                else if (Player.GetComponent<Player_Knight>().Is_SkillK)
                    return;
                Player.GetComponent<Player>().Damage(damage, Enemy);
                break;

        }
    }
}
