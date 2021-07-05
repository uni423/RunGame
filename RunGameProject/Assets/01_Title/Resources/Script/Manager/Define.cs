namespace Define
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Skill
    {
        public bool IsPractice = false;
        public float Distance;
        public float Damage;
        public float MaxCoolTime;
        public float NowCoolTime = 0;
        public float Time;

        public Skill(float time, float damage, float distance, float coolTime)
        {
            Time = time;
            Damage = damage;
            Distance = distance;
            MaxCoolTime = coolTime;
        }
    }

    public enum Stage
    {
        Stage1_1 = 1,
        Stage1_2,
        Stage1_3,
        Stage1_Boss = 9,
        Stage2_1 = 11,
        Stage2_2,
        Stage2_3,
        Stage2_Boss = 19,
        Stage3_1 = 21,
        Stage3_2,
        Stage3_3,
        Stage3_Boss = 29,
    }

    public enum CharType { Knight = 0, Gunner }
}