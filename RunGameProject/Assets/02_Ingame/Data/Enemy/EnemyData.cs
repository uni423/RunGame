using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    [Tooltip("최대 체력")] private float maxHp;
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    [SerializeField]
    [Tooltip("공격력")] private float ad;
    public float Ad { get { return ad; } set { ad = value; } }
    [SerializeField]
    [Tooltip("사거리")] private float adRange;
    public float AdRange { get { return adRange; } set { adRange = value; } }
    [SerializeField]
    [Tooltip("초에 때릴 수 있는 속도")]
    private float adSpeed;
    public float AdSpeed { get { return adSpeed; } set { adSpeed = value; } }
    [SerializeField]
    [Tooltip("피격 공격력")] private float triAd;
    public float TriAd { get { return triAd; } set { triAd = value; } }
    [SerializeField]
    [Tooltip("경험치")] private float exp;
    public float Exp { get { return exp; } set { exp = value; } }

    //공격력 != 피격 공격력
    //공격력은 Enemy가 공격을 시도했을 떄의 공격력, 
    // 피격 공격력은 Player와 Enemy가 충돌했을 때의 공격력
}
