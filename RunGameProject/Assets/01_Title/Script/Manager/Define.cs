namespace Define
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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