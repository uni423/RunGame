using System.Collections;
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
    [Tooltip("나이트 A 스킬")] public Skill skKnightA;
    [Tooltip("방어스킬 상태인가?")] public bool Is_Shild = false;
    [Tooltip("나이트 K 스킬")] public Skill skKnightK;
    [Tooltip("돌격스킬 상태인가?")] public bool Is_Carge = false;

    private IEnumerator Lerp;
    private delegate void Delegate();

    [Space(10f)]
    public BGManager BG;
    public EnemyData Enemy;
    public UIManager02 UIM_2;

    private Rigidbody2D myRigid;
    public GameObject JumpColli;
    public GameObject RangeColli;

    #endregion

    private void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();

        skKnightA = new Skill(1f, 0, 0, 5f);
        skKnightK = new Skill(3f, 100, 300 + (0.5f * Stat.Speed), 20f);

        JumpColli.SetActive(true);
        RangeColli.SetActive(true);
    }

    void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        //스킬 입력 확인
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.A))
            Shild();
        if (Input.GetKeyUp(KeyCode.A))
            if (Is_Shild) Shild_Quit();
        if (Input.GetKeyDown(KeyCode.S))
            Bottom();
        if (Input.GetKeyDown(KeyCode.D))
            Dash();
        if (Input.GetKeyUp(KeyCode.D))
            Dash_Quit();
        if (Input.GetKeyDown(KeyCode.J))
            Attack();
        if (Input.GetKeyDown(KeyCode.K))
            SpecialSkill();
        if (Input.GetKeyDown(KeyCode.L))
        { }
        if (Input.GetKeyDown(KeyCode.Escape))
        { }

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
        if (Is_Shild)
        {
            Shild_Quit(true);
            return;
        }
        if (Is_Carge && !Is_Attack)
            return;

        Is_Damage = true;
        StartCoroutine(Timer(2, () => { Is_Damage = false; }));

        Dash_Quit();
        DOTween.To(() => Stat.NowHp, x => Stat.NowHp = x, Stat.NowHp - damage, 0.2f).OnComplete(() => { });
        Camera.main.DOShakePosition(0.3f, 100);
        Camera.main.transform.DOMove(new Vector3(0, 0, -10), 0.1f).SetDelay(0.3f);
        BG.Move(Stat.Speed * 0.5f, 2f);
        if (Stat.NowHp <= 0)
        {
            GameManager.Instance.IsGamePlay = false;
            UIM_2.Game_Over(Enemy);
        }
    }

    #region Skills
    public void Jump() //점프
    {
        if (!Is_Jumping)
            return;

        if (!Is_Attack)
            StopCoroutine(Lerp);

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

    public void Bottom() //하단키
    {
        Debug.Log("Bottom");
    }

    public void Dash() //대쉬
    {
        if (Is_Damage || Is_Carge)
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

    public void Shild() //캐릭터 방어기
    {
        //키다운 중인 '지속시간' 동안 무적, '지속시간' 중 대미지를 입으면 해당 대미지만큼 반격한다

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
        Debug.Log(skKnightK.Distance);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Is_Carge && other.gameObject.CompareTag("Enemy") && !other.transform.GetComponent<Enemy>().IsDead)
            Stat.NowExp += other.transform.GetComponent<Enemy>().Damage(skKnightK.Damage);

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

    IEnumerator Timer(float time, Delegate dele)
    {
        yield return new WaitForSeconds(time);
        dele();
    }
}