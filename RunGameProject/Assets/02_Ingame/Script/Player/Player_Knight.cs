using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;

public class Player_Knight : Player
{
    [Header("스킬")]
    [Tooltip("나이트 A 스킬")] public Skill skKnightA;
    [Tooltip("방어스킬 상태인가?")] public bool Is_Shild = false;
    [Tooltip("나이트 K 스킬")] public Skill skKnightK;
    [Tooltip("돌격스킬 상태인가?")] public bool Is_Carge = false;

    new void Start()
    {
        base.Start();

        skKnightA = new Skill(1f, 0, 0, 3.5f);
        skKnightK = new Skill(3f, 100, 300 + (2f * this.Stat.Speed), 20f);

        Debug.Log(this.Stat.Speed);
    }

    new void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        //스킬 입력 확인
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.D))
            if (!Is_Carge) Dash();
        if (Input.GetKeyUp(KeyCode.D))
            Dash_Quit();
        if (Input.GetKeyDown(KeyCode.J))
            Attack(); 
        if (Input.GetKeyDown(KeyCode.A))
            Shild();
        if (Input.GetKeyUp(KeyCode.A))
            if (Is_Shild) Shild_Quit();
        if (Input.GetKeyDown(KeyCode.K))
            SpecialSkill();
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Character_Swich();
        }

        base.Update();
    }

    public void Damage()
    {
        if (Is_Shild)
        {
            Shild_Quit(true);
            return;
        }
        if (Is_Carge)
            return;
    }

    public void Shild() //캐릭터 방어기
    {
        //키다운 중인 '지속시간' 동안 무적

        if (skKnightA.NowCoolTime > 0)
            return;

        Debug.Log("Shild");
        Is_Shild = true;
        StartCoroutine(Timer(skKnightA.Time, (() => { if (Is_Shild) Shild_Quit(); })));
    }

    public void Shild_Quit(bool Is_damage = false) //방어기 종료
    {
        Is_Shild = false;
        if (Is_damage)
        {
            skKnightA.NowCoolTime = skKnightA.MaxCoolTime;
            UIM_2.UI_Shiny(1, false);
            DOTween.To(() => skKnightA.NowCoolTime, x => skKnightA.NowCoolTime = x, 0, skKnightA.MaxCoolTime)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    UIM_2.UI_Shiny(1);
                });
        }
    }

    public void SpecialSkill() //캐릭터 특수기
    {
        if (Is_Carge || skKnightK.NowCoolTime > 0)
            return;
        if (Is_Dash)
            Dash_Quit();

        //'지속시간' 동안 무적, 1초마다 '사거리'만큼 돌진(이동), 닿는 적에겐 틱 1초마다 '대미지'만큼 입힌다
        Debug.Log("skSpecial");

        Is_Carge = true;
        StartCoroutine(Timer(skKnightK.Time,
            (() =>
            {
                Is_Carge = false;
                BG.In_Speed(Stat.Speed);
                RangeColli.SetActive(true);

                skKnightK.NowCoolTime = skKnightK.MaxCoolTime;
                UIM_2.UI_Shiny(2, false);
                DOTween.To(() => skKnightK.NowCoolTime, x => skKnightK.NowCoolTime = x, 0, skKnightK.MaxCoolTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        UIM_2.UI_Shiny(2);
                    });
            })
            ));
        RangeColli.SetActive(false);
        BG.In_Speed(skKnightK.Distance, true);
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        if (Is_Carge && other.gameObject.CompareTag("Enemy") && !other.transform.GetComponent<Enemy>().IsDead)
            Stat.NowExp += other.transform.GetComponent<Enemy>().Damage(skKnightK.Damage);

        base.OnTriggerStay2D(other);
    }
}
