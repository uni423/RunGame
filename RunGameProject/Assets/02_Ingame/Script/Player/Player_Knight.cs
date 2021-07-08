using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;

public class Player_Knight : Player
{
    public void Init()
    {
        base.init();

        skA = new Skill(1f, 0, 0, 3.5f);
        skA.NowCoolTime = 0f;
        skK = new Skill(3f, 100, 300 + (2f * this.Stat.Speed), 20f);
        skK.NowCoolTime = 0f;
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

        base.Update();
    }

    public void Attack() //공격, 상호작용키
    {
        if (!Is_AttackRange || !Is_Jumping || !Is_Attack)
            return;

        if (RangeEnemyObj == null)
        {
            Debug.Log("Null Object");
            return;
        }

        Stat.NowExp += RangeEnemyObj.Damage(Stat.Ad);

        Combo += 1;
        switch (Combo)
        {
            case 1:
                SoundManager.Instance.PlaySound("Knight_1");
                break;
            case 2:
                SoundManager.Instance.PlaySound("Knight_2");
                break;
            case 3:
                SoundManager.Instance.PlaySound("Knight_3");
                break;
            case 4:
                SoundManager.Instance.PlaySound("Knight_4");
                Combo = 0;
                break;
            default:
                break;
        }
        SoundManager.Instance.PlaySound("SFX_Knight_Attack", false);
        ComboTimer = Timer(3f, () => { Combo = 0; });
        StartCoroutine(ComboTimer);

        Is_Attack = false;
        Effect_Anim.SetTrigger("Is_Attack");
        Anim.SetBool("Is_Attack", true);
        Lerp = lerp(RangeEnemyObj.gameObject.transform.position.x - 100, 1 / Stat.AdSpeed,
            () =>
            {
                Is_Attack = true;
                RangeDistance = 10000;
                RangeEnemyObj = null;
                Lerp = null;
            });
        StartCoroutine(Lerp);

        StartCoroutine(Timer(0.1f, () => Anim.SetBool("Is_Attack", false)));

        Camera.main.DOShakePosition(0.3f, 10)
            .OnComplete(() => Camera.main.transform.position = new Vector3(0, 0, -10));
    }

    public void Shild() //캐릭터 방어기
    {
        //키다운 중인 '지속시간' 동안 무적

        if (skA.NowCoolTime > 0)
            return;

        Is_SkillA = true;
        Anim.SetBool("Is_Shild", true);
        StartCoroutine(Timer(skA.Time, (() => { if (Is_SkillA) Shild_Quit(); })));
    }

    public void Shild_Quit(bool Is_damage = false) //방어기 종료
    {
        Is_SkillA = false;
        Anim.SetBool("Is_Shild", false);
        if (Is_damage)
        {
            SoundManager.Instance.PlaySound("Knight_A");
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

        Effect_Anim.SetBool("Is_K", true);
        SoundManager.Instance.PlaySound("Knight_K");
        SoundManager.Instance.PlaySound("SFX_Knight_K", false);
        Is_SkillK = true;
        StartCoroutine(Timer(skK.Time,
            (() =>
            {
                Effect_Anim.SetBool("Is_K", false);
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
        if (Is_SkillK && other.gameObject.CompareTag("Enemy") && !other.transform.GetComponent<Enemy>().Is_Dead)
            Stat.NowExp += other.transform.GetComponent<Enemy>().Damage(skK.Damage);

        base.OnTriggerStay2D(other);
    }
}
