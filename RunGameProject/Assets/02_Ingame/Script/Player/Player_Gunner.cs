using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;

public class Player_Gunner : Player
{
    public void Init()
    {
        base.init();

        skA = new Skill(1.5f, 0, 200, 3.5f);
        skA.NowCoolTime = 0f;
        skK = new Skill(7f, 100 + (0.5f * Stat.Speed), 500, 25f);
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
            Back();
        if (Input.GetKeyDown(KeyCode.K))
            SpecialSkill();

        base.Update();
    }

    public void Attack() //공격, 상호작용키
    {
        if (!Is_AttackRange || !Is_Jumping || !Is_Attack)
            return;

        Stat.NowExp += RangeEnemyObj.Damage(Stat.Ad);

        Combo += 1;
        switch (Combo)
        {
            case 1:
                SoundManager.Instance.PlaySound("Gunner_1");
                break;
            case 2:
                SoundManager.Instance.PlaySound("Gunner_2");
                break;
            case 3:
                SoundManager.Instance.PlaySound("Gunner_3");
                break;
            case 4:
                SoundManager.Instance.PlaySound("Gunner_4");
                Combo = 0;
                break;
            default:
                break;
        }
        SoundManager.Instance.PlaySound("SFX_Gunner_Attack", false);
        ComboTimer = Timer(5f, () => { Combo = 0; });
        StartCoroutine(ComboTimer);

        Is_Attack = false;
        Anim.SetBool("Is_Attack", true);
        Effect_Anim.SetTrigger("Is_Attack");
        Effect_Hitting_Anim.SetTrigger("Is_Hitting");
        Effect_Hitting_Anim.gameObject.transform.SetParent(RangeEnemyObj.gameObject.transform);
        Effect_Hitting_Anim.gameObject.transform.localPosition = new Vector3(0, 0);
        StartCoroutine(Timer(0.5f, () => Effect_Hitting_Anim.gameObject.transform.SetParent(this.transform)));

        float distance = RangeEnemyObj.gameObject.transform.position.x - Stat.AdDistance;
        if (distance < -600)
            distance = -600;

        Lerp = lerp(RangeEnemyObj.gameObject.transform.position.x - Stat.AdDistance, 1 / Stat.AdSpeed,
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

    public void Back() //캐릭터 방어기
    {
        // X좌표 100만큼 뒤로. 1.5초동안 제자리로. 복귀 도중 대시하면 제자리로 순간 이동. 점프 도중 입력 안됨.

        if (skA.NowCoolTime > 0 || !Is_Jumping)
            return;

        Is_SkillA = true;
        Anim.SetBool("Is_A", true);
        SoundManager.Instance.PlaySound("Gunner_A");

        transform.DOJump(new Vector3(transform.position.x - skA.Distance, -300), 100f, 1, 0.4f)
            .OnComplete(() =>
            {
                Lerp = lerp(transform.position.x, 1.5f,
                    () =>
                    {
                        BG.In_Speed(Stat.Speed, true);
                        Is_SkillA = false;
                        skA.NowCoolTime = skA.MaxCoolTime;
                        UIM_2.UI_Shiny(1, false);

                        DOTween.To(() => skA.NowCoolTime, x => skA.NowCoolTime = x, 0, skA.MaxCoolTime)
                            .SetEase(Ease.Linear)
                            .OnComplete(() =>
                            {
                                UIM_2.UI_Shiny(1);
                            });
                        Lerp = null;
                    });
                StartCoroutine(Lerp);
            });
    }

    public void SpecialSkill() //캐릭터 특수기
    {
        if (Is_SkillK || skK.NowCoolTime > 0)
            return;
        if (Is_Dash)
            Dash_Quit();

        //'지속시간'동안 '사거리'안에 들어오는 적에게 틱 0.5초마다 '대미지'만큼 입힌다
        Debug.Log("skSpecial");

        Effect_Anim.SetBool("Is_K", true);
        SoundManager.Instance.PlaySound("SFX_Gunner_Attack");
        SoundManager.Instance.PlaySound("SFX_Gunner_K", false);
        Is_SkillK = true;
        StartCoroutine(Timer(skK.Time,
            (() =>
            {
                Is_SkillK = false;
                Effect_Anim.SetBool("Is_K", false);
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
        BG.In_Speed(Stat.Speed, true);
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        if (Is_SkillK && other.gameObject.CompareTag("Enemy") && !other.transform.GetComponent<Enemy>().Is_Dead)
            Stat.NowExp += other.transform.GetComponent<Enemy>().Damage(skK.Damage);

        base.OnTriggerStay2D(other);
    }
}
