using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Object/PlayerStat", order = int.MaxValue)]
public class PlayerStat : ScriptableObject
{
    [Header("레벨")]
    [SerializeField] [Tooltip("현재 레벨")] 
    private int level = 0;
    public int Level { get { return level; } set { level = value; } }
    [SerializeField] [Tooltip("최대 경험치")] 
    private float maxExp;
    public float MaxExp { get { return maxExp; } set { maxExp = value; } }
    [SerializeField] [Tooltip("경험치증가치")] 
    private float addExp;
    public float AddExp { get { return addExp; } set { maxExp = value; } }
    [SerializeField] [Tooltip("현재 경험치")]
    private float nowExp = 0;
    public float NowExp { get { return nowExp; } set { nowExp = value; } }

    [Header("체력")]
    [SerializeField] [Tooltip("최대 체력")] 
    private float maxHp;
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    [SerializeField] [Tooltip("체력 증가치")] 
    private float addHp;
    public float AddHp { get { return addHp; } set { addHp = value; } }
    [SerializeField] [Tooltip("현재 체력")]
    private float nowHp;
    public float NowHp { get { return nowHp; } set { nowHp = value; } }

    [Header("스태미나")]
    [SerializeField] [Tooltip("최대 스태미나")] 
    private float maxSp;
    public float MaxSp { get { return maxSp; } set { maxSp = value; } }
    [SerializeField] [Tooltip("스태미나 증가치")]
    private float addSp;
    public float AddSp { get { return addSp; } set { addSp = value; } }
    [SerializeField] [Tooltip("현재 스태미나")]
    private float nowSp;
    public float NowSp { get { return nowSp; } set { nowSp = value; } }
    [SerializeField] [Tooltip("초당 회복 수치")]
    private float plusSp;
    public float PlusSp { get { return plusSp; } set { plusSp = value; } }

    [Header("공격")]
    [SerializeField] [Tooltip("공격력")] 
    private float ad;
    public float Ad { get { return ad; } set { ad = value; } }
    [SerializeField] [Tooltip("공격력 증가치")] 
    private float addad;
    public float Addad { get { return addad; } set { addad = value; } }
    [SerializeField] [Tooltip("공격 발동 사거리")] 
    private float adRange;
    public float AdRange { get { return adRange; } set { adRange = value; } }
    [SerializeField] [Tooltip("발동 후 몬스터와의 거리")] 
    private float adDistance;
    public float AdDistance { get { return adDistance; } set { adDistance = value; } }
    [SerializeField] [Tooltip("초에 때릴 수 있는 속도")] 
    private float adSpeed;
    public float AdSpeed { get { return adSpeed; } set { adSpeed = value; } }
    [SerializeField] [Tooltip("공격 속도 증가치")] 
    private float addadSpeed;
    public float AddadSpeed { get { return addadSpeed; } set { addadSpeed = value; } }

    [Header("이동")]
    [SerializeField] [Tooltip("초당 이동속도")] 
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }
    [SerializeField] [Tooltip("이동속도 증가치")] 
    private float addspeed;
    public float Addspeed { get { return addspeed; } set { addspeed = value; } }
    [SerializeField] [Tooltip("점프력")] 
    private float jumpPower;
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }


    //[Header("스킬")]
    //[Tooltip("특수공격 A")] public Skill skKnightA;
    //[Tooltip("특수공격 K")] public Skill skKnightK;

}
