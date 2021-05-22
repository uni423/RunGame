using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    [Tooltip("최대 체력")] private float maxHp;
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    [Tooltip("공격력")] private float ad;
    public float Ad { get { return ad; } set { ad = value; } }
    [Tooltip("사거리")] private float adRange;
    public float AdRange { get { return adRange; } set { adRange = value; } }
    [SerializeField]
    [Tooltip("초에 때릴 수 있는 속도")]
    private float adSpeed;
    public float AdSpeed { get { return adSpeed; } set { adSpeed = value; } }

    public float Exp;
}
