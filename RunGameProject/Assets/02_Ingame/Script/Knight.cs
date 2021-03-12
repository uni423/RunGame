using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Singleton<Knight>
{
    [Header("레벨")]
    public int   Level; //현재 레벨
    public float MaxExp; //레벨업 필요 경험치
    public float NowExp; //현재 경험치

    [Header("체력")]
    public float MaxHp; //최대 체력
    public float NowHp; //현재 체력

    [Header("스태미나")]
    public float MaxStamina; //최대 스태미나
    public float NowStamina; //현재 스태미나
    public float PlusStamina; //초당 회복 수치

    [Header("공격")]
    public float ad; 
    public float adRange; //공격 발동 사거리
    public float adDistance; //발동 후 몬스터와의 거리
    public float adSpeed; //초에 때릴 수 있는 속도

    [Header("이동속도")]
    public float sp; //초당 이동속도

    [Header("스킬")]
    public Skill skKnightA;
    public Skill skKnightK;

    private void Start()
    {
        Level = 1;
        MaxExp = 100; 
        NowExp = 0;

        MaxHp = 50;
        NowHp = MaxHp;

        MaxStamina = 1;
        NowStamina = MaxStamina;
        PlusStamina = 0.2f;

        ad = 30;
        adRange = 500; 
        adDistance = 100;
        adSpeed = 1.5f;

        sp = 300f;

        //키다운 중인 '지속시간' 동안 무적, '지속시간' 중 대미지를 입으면 해당 대미지만큼 반격한다
        skKnightA = new Skill(5f, 0, 1f);
        //'지속시간' 동안 무적, 1초마다 '사거리'만큼 돌진(이동), 닿는 적에겐 틱 1초마다 '대미지'만큼 입힌다
        skKnightK = new Skill(13f, 300 + sp / 2, 3f);
    }
}
