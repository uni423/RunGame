﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;

public class Player : MonoBehaviour
{
    #region Declaration
    [Header("플레이어 상태")]
    [Tooltip("스탯")] public PlayerStat Stat;
    [Tooltip("공격 받은 상태인가?")] public bool Is_Damage;

    [Header("점프")]
    [Tooltip("점프가 가능한 상태인가?")] public bool Is_Jumping = false;
    [Tooltip("하강하고 있는 상태인가?")] public bool Is_Down = false;

    [Header("대쉬")]
    [Tooltip("대쉬 상태인가?")] public bool Is_Dash = false;

    [Header("공격")]
    [Tooltip("사거리 안에 몬스터가 있는 상태인가?")] public bool Is_AttackRange = false;
    [Tooltip("공격이 가능한 상태인가?")] public bool Is_Attack = true;
    [Tooltip("최단 사거리")] public float RangeDistance = 1000f;
    [Tooltip("사거리 안에 있는 몬스터 오브젝트")] public Enemy RangeEnemyObj;

    [Header("스킬")]
    [Tooltip("나이트 A 스킬")] public Skill skA;
    [Tooltip("방어스킬 상태인가?")] public bool Is_SkillA = false;
    [Tooltip("나이트 K 스킬")] public Skill skK;
    [Tooltip("돌격스킬 상태인가?")] public bool Is_SkillK = false;

    private IEnumerator Lerp;
    public delegate void Delegate();

    [Space(10f)]
    public BGManager BG;
    public EnemyData Enemy;
    public UIManager02 UIM_2;

    private Rigidbody2D myRigid;
    public GameObject JumpColli;
    public GameObject RangeColli;

    #endregion

    public void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();

        JumpColli.SetActive(true);
        RangeColli.SetActive(true);
    }

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.D))
            if (!Is_SkillK) Dash();
        if (Input.GetKeyUp(KeyCode.D))
            Dash_Quit();
        if (Input.GetKeyDown(KeyCode.L))
            Character_Swich();

        StatUpdate();
        PositionUpdate();
    }

    public void StatUpdate()
    {
        if (Stat.NowExp >= Stat.MaxExp && Stat.Level < 10)
        {
            StatUpgrade();
        }

        if (Is_Dash)
        {
            Stat.NowSp -= Time.deltaTime;
            if (Stat.NowSp <= 0)
                Dash_Quit();
        }

        if (Stat.NowSp < Stat.MaxSp)
            Stat.NowSp += Time.deltaTime * Stat.PlusSp;
    }

    public void StatUpgrade()
    {
        if (Stat.Level <= 0)
        {
            Stat.NowExp = 0f;
            Stat.NowHp = Stat.MaxHp;
            Stat.NowSp = Stat.MaxSp;
            BG.In_Speed(Stat.Speed);
            Stat.Level = 1;
            return;
        }

        Stat.NowExp -= Stat.MaxExp;
        Stat.MaxExp += Stat.AddExp;
        Stat.MaxHp += Mathf.Round(Stat.MaxHp * Stat.AddHp);
        Stat.MaxSp += Stat.AddSp;
        Stat.Ad += Stat.Addad;
        Stat.Speed += Mathf.Round(Stat.Speed * Stat.Addspeed);
        Stat.AdSpeed += Mathf.Round(Stat.AdSpeed * Stat.AddadSpeed);
        Stat.Level++;
        BG.In_Speed(Stat.Speed);
    }

    public void PositionUpdate()
    {
        //지정된 위치 벗어나면 제자리로
        if (transform.localPosition.x != -600)
            lerp(transform.localPosition.x, 0.1f);

        //점프 후 하강할때 가속도 추가
        if (!Is_Jumping && myRigid.velocity.y < 10 && Is_Down)
        {
            myRigid.velocity = Vector2.up * Physics2D.gravity.y * 8f;
            Is_Down = false;
        }
    }

    public void Damage(float damage, string Enemy)
    {
        Is_Damage = true;
        StartCoroutine(Timer(2, () => { Is_Damage = false; }));

        Dash_Quit();
        if (Stat.NowHp - damage <= 0)
        {
            GameManager.Instance.IsGamePlay = false;
            UIM_2.Game_Over(Enemy);
        }
        DOTween.To(() => Stat.NowHp, x => Stat.NowHp = x, Stat.NowHp - damage, 0.2f).OnComplete(() => { });
        Camera.main.DOShakePosition(0.3f, 100);
        Camera.main.transform.DOMove(new Vector3(0, 0, -10), 0.1f).SetDelay(0.3f);
        BG.Move(Stat.Speed * 0.5f, 2f);
    }

    #region Skills
    public void Jump(bool swich = false) //점프
    {
        if (!Is_Jumping && !swich)
            return;

        if (!Is_Attack)
            StopCoroutine(Lerp);

        if (swich)
            myRigid.velocity = Vector2.up * (Stat.JumpPower * 0.5f) * 5.5f;
        else
            myRigid.velocity = Vector2.up * Stat.JumpPower * 5.5f;

        Is_Jumping = false;
        Is_Down = true;

        if (!Is_Attack)
            StartCoroutine(lerp(transform.position.x, 0.2f,
                () =>
                {
                    Is_Attack = true;
                    RangeDistance = 10000;
                    RangeEnemyObj = null;
                }));
    }

    public void Dash() //대쉬
    {
        if (Is_Damage)
            return;

        BG.In_Speed(Stat.Speed * 2.3f);
        Is_Dash = true;
    }

    public void Dash_Quit() //대쉬 끝
    {
        BG.In_Speed(Stat.Speed);
        Is_Dash = false;
    }

    public void Attack() //공격, 상호작용키
    {
        if (!Is_AttackRange || !Is_Jumping || !Is_Attack)
            return;
        Debug.Log("Attack" + Stat.Ad);

        Stat.NowExp += RangeEnemyObj.Damage(Stat.Ad);

        Is_Attack = false;
        Lerp = lerp(RangeEnemyObj.gameObject.transform.position.x - 100, 1 / Stat.AdSpeed,
            () =>
            {
                Is_Attack = true;
                RangeDistance = 10000;
                RangeEnemyObj = null;
            });
        StartCoroutine(Lerp);

        Camera.main.DOShakePosition(0.3f, 10)
            .OnComplete(() => Camera.main.transform.position = new Vector3(0, 0, -10));
    }

    private void Character_Swich()
    {
        if (GameManager.Instance.CharacterCode == CharType.Knight)
            PlayerManager.Instance.Character_Swich(CharType.Gunner);
        else if (GameManager.Instance.CharacterCode == CharType.Gunner)
            PlayerManager.Instance.Character_Swich(CharType.Knight);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            Is_Jumping = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            Is_Jumping = false;
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Drop"))
        {
            PlayerManager.Instance.Damage(99999, "Drop");
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Is_AttackRange = true;
            if (RangeDistance > other.gameObject.transform.position.x + 600)
            {
                RangeDistance = other.gameObject.transform.position.x + 600;
                RangeEnemyObj = other.gameObject.GetComponent<Enemy>();
                if (RangeDistance < 0 || RangeEnemyObj.transform.GetComponent<Enemy>().IsDead)
                {
                    RangeDistance = 10000f;
                    RangeEnemyObj = null;
                }
            }
        }
        else if (RangeDistance >= 10000f)
            Is_AttackRange = false;
    }
    #endregion

    IEnumerator lerp(float startvalue, float duration, Delegate dele = null)
    {
        transform.position = new Vector3(startvalue, transform.position.y);

        myRigid.velocity += Vector2.left * 1500f;
        while (transform.position.x > -590)
        {
            yield return null;
        }

        myRigid.velocity = new Vector2(0, myRigid.velocity.y);
        transform.position = new Vector3(-600, transform.position.y);

        if (dele != null) StartCoroutine(Timer(duration, dele));
    }

    protected virtual IEnumerator Timer(float time, Delegate dele)
    {
        yield return new WaitForSeconds(time);
        dele();
    }
}