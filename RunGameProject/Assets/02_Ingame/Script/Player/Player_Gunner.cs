using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;

public class Player_Gunner : Player
{
    new void Start()
    {
        base.Start();

        skA = new Skill(1f, 0, 0, 3.5f);
        skK = new Skill(3f, 100, 300 + (2f * this.Stat.Speed), 20f);

        Debug.Log(this.Stat.Speed);
    }

    new void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        //스킬 입력 확인
        if (Input.GetKeyDown(KeyCode.J))
            Attack();
        if (Input.GetKeyDown(KeyCode.A))
            Shild();
        if (Input.GetKeyUp(KeyCode.A))
            if (Is_SkillA) Shild_Quit();
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
        if (Is_SkillA)
        {
            Shild_Quit(true);
            return;
        }
        if (Is_SkillK)
            return;
    }

    public void Shild() //캐릭터 방어기
    {
        //키다운 중인 '지속시간' 동안 무적

        if (skA.NowCoolTime > 0)
            return;

        Debug.Log("Shild");
        Is_SkillA = true;
        StartCoroutine(Timer(skA.Time, (() => { if (Is_SkillA) Shild_Quit(); })));
    }

    public void Shild_Quit(bool Is_damage = false) //방어기 종료
    {
        Is_SkillA = false;
        if (Is_damage)
        {
            skA.NowCoolTime = skA.MaxCoolTime;
            UIM_2.UI_Shiny(1, false);
            DOTween.To(() => skA.NowCoolTime, x => skA.NowCoolTime = x, 0, skA.MaxCoolTime)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    UIM_2.UI_Shiny(1);
                });
        }
    }

    public void SpecialSkill() //캐릭터 특수기
    {
        if (Is_SkillK || skK.NowCoolTime > 0)
            return;
        if (Is_Dash)
            Dash_Quit();

        //'지속시간' 동안 무적, 1초마다 '사거리'만큼 돌진(이동), 닿는 적에겐 틱 1초마다 '대미지'만큼 입힌다
        Debug.Log("skSpecial");

        Is_SkillK = true;
        StartCoroutine(Timer(skK.Time,
            (() =>
            {
                Is_SkillK = false;
                BG.In_Speed(Stat.Speed);
                RangeColli.SetActive(true);

                skK.NowCoolTime = skK.MaxCoolTime;
                UIM_2.UI_Shiny(2, false);
                DOTween.To(() => skK.NowCoolTime, x => skK.NowCoolTime = x, 0, skK.MaxCoolTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        UIM_2.UI_Shiny(2);
                    });
            })
            ));
        RangeColli.SetActive(false);
        BG.In_Speed(skK.Distance, true);
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        if (Is_SkillK && other.gameObject.CompareTag("Enemy") && !other.transform.GetComponent<Enemy>().IsDead)
            Stat.NowExp += other.transform.GetComponent<Enemy>().Damage(skK.Damage);

        base.OnTriggerStay2D(other);
    }
}
