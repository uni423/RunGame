namespace Define
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class CharStat
    {
        //레벨업 필요 경험치
        public float MaxExp;
        //필요 경험치 증가치
        public float AddExp;

        [Header("체력")]
        [Tooltip("최대 체력")] public float MaxHp;
        [Tooltip("체력 증가치")] public float AddHp;

        [Header("스태미나")]
        [Tooltip("최대 스태미나")] public float MaxSp;
        [Tooltip("스태미나 증가치")] public float AddSp;
        [Tooltip("초당 회복 수치")] public float PlusSp;

        [Header("공격")]
        [Tooltip("공격력")] public float ad;
        [Tooltip("공격력 증가치")] public float Addad;
        [Tooltip("공격 발동 사거리")] public float adRange;
        [Tooltip("발동 후 몬스터와의 거리")] public float adDistance;
        [Tooltip("초에 때릴 수 있는 속도")] public float adSpeed;
        [Tooltip("공격 속도 증가치")] public float AddadSpeed;

        [Header("이동")]
        [Tooltip("초당 이동속도")] public float speed;
        [Tooltip("이동속도 증가치")] public float Addspeed;
        [Tooltip("점프력")] public float JumpPower;


        [Header("스킬")]
        [Tooltip("특수공격 A")] public Skill skKnightA;
        [Tooltip("특수공격 K")] public Skill skKnightK;
    }

    public class Skill
    {
        public bool IsPractice = false;
        public float Distance;
        public float Delay;
        public float CoolTime;
        public float Time;

        public Skill(float coolTime, float distance, float delay, float time)
        {
            CoolTime = coolTime;
            Distance = distance;
            Delay = delay;
            Time = time;
        }
    }

    public enum CharType { Knight = 0, Gunner }
}