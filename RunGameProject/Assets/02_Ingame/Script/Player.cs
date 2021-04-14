using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerStat Stat;

    private float skDelay = 0f;

    public bool IsJumping = false; //점프 가능이면 true

    public bool IsDash = false;

    public bool IsAttackRange = false; //공격 발동 사거리 안에 몬스터가 있으면 true
    public bool Is_Psb_Attack = true; //공격 가능하면 true
    public float RangeDistance = 1000f;
    public Enemy RangeEnemyObj; //공격 발동 사거리 안에 있는 몬스터
    public Transform HurdleGrid;

    public Skill skDash = new Skill(0f, 100f, 0.3f, 0f);

    public IEnumerator Lerp;

    [Space(10f)]
    public BGManager BG;
    public EnemyData Enemy;
    public UIManager02 UIM_2;

    public Rigidbody2D myRigid;
    
    void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.A))
            Shild();
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
        PositionUpdata();
    }

    public void StatUpdate()
    {
        if (Stat.NowExp >= Stat.MaxExp && Stat.Level < 10)
        {
            StatUpgrade();
        }

        if (IsDash)
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
        Stat.MaxExp += Stat.AddExp; //+100
        Stat.MaxHp += Mathf.Round(Stat.MaxHp * Stat.AddHp); //+0.2%
        Stat.MaxSp += Stat.AddSp; //+1
        Stat.Ad += Stat.Addad; //+10
        Stat.Speed += Mathf.Round(Stat.Speed * Stat.Addspeed); //+ 0.05%
        Stat.AdSpeed += Mathf.Round(Stat.AdSpeed * Stat.AddadSpeed); //+ 0.02%
        Stat.Level++;
        BG.In_Speed(Stat.Speed);
    }

    public void PositionUpdata()
    {
        if (myRigid.velocity.y < 0)
        {
            myRigid.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        }
    }

    public void Damage(float damage)
    {
        //Stat.NowHp -= damage;
        DOTween.To(() => Stat.NowHp, x => Stat.NowHp = x, Stat.NowHp - damage, 0.2f);
        Camera.main.DOShakePosition(0.3f, 100);
        BG.Move(Stat.Speed * 0.5f, 2f, 0);
        if (Stat.NowHp <= 0)
        {
            //죽음
        }
    }

    #region Skills
    public void Jump() //점프
    {
        if (!IsJumping)
            return;

        if (!Is_Psb_Attack)
            StopCoroutine(Lerp);
            //transform.DOKill();

        //Vector2 JumpVelocity = new Vector2(0, Stat.JumpPower * 5f);
        myRigid.velocity = Vector2.up * Stat.JumpPower * 5f;
        IsJumping = false;

        if (!Is_Psb_Attack)
        {
            StartCoroutine(lerp(transform.position.x, 0.2f));
            //transform.DOMoveX(-600, 0.2f)
            //    .SetDelay(0.3f)
            //    .OnComplete(() =>
            //    {
            //        Is_Psb_Attack = true;
            //        RangeDistance = 10000;
            //    });
        }
    }

    public void Shild() //캐릭터 방어기
    {
        Debug.Log("Shild");
    }

    public void Bottom() //하단키
    {
        Debug.Log("Bottom");
    }

    public void Dash() //대쉬
    {
        BG.In_Speed(Stat.Speed * 2.3f);
        Enemy.Speed = 2.3f;
        IsDash = true;
    }

    public void Dash_Quit() //대쉬
    {
        BG.In_Speed(Stat.Speed);
        Enemy.Speed = 1f;
        IsDash = false;
    }

    public void Attack() //공격, 상호작용키
    {
        if (!IsAttackRange || !IsJumping || !Is_Psb_Attack) 
            return;
        Debug.Log("Attack");

        Is_Psb_Attack = false;
        Lerp = lerp(RangeEnemyObj.gameObject.transform.position.x - 100, 1 / Stat.AdSpeed);
        StartCoroutine(Lerp);
        //transform.DOMoveX(RangeEnemyObj.gameObject.transform.position.x - 100, 1 / Stat.AdSpeed)
        //    .From()
        //    .OnComplete(() => {
        //        Is_Psb_Attack = true;
        //        RangeDistance = 10000; } );

        Camera.main.DOShakePosition(0.3f, 10)
            .OnComplete(() => Camera.main.transform.position = new Vector3(0, 0, -10));

        Stat.NowExp += RangeEnemyObj.Damage(Stat.Ad);
    }

    public void SpecialSkill() //캐릭터 특수기
    {
        ////키다운 중인 '지속시간' 동안 무적, '지속시간' 중 대미지를 입으면 해당 대미지만큼 반격한다
        //skKnightA = new Skill(5f, 0, 1f, 0.1f);
        ////'지속시간' 동안 무적, 1초마다 '사거리'만큼 돌진(이동), 닿는 적에겐 틱 1초마다 '대미지'만큼 입힌다
        //skKnightK = new Skill(13f, 300 + sp / 2, 1f, 3f);
        if (!IsJumping)
            return;
        //if (Knight.Instance.skKnightK.IsPractice)
        //    return;

        Debug.Log("skSpecial");

        //if (skDelay <= 0f && !Knight.Instance.skKnightK.IsPractice) //처음 시작
        //{
        //    skDelay = Knight.Instance.skKnightK.Time;
        //    Knight.Instance.skKnightK.IsPractice = true;
        //}
        //else if (skDelay <= 0f && Knight.Instance.skKnightK.IsPractice) //끝
        //{
        //    skDelay = 0f;
        //    Knight.Instance.skKnightK.IsPractice = false;
        //    return;
        //}

        //Sequence skSeq = DOTween.Sequence();

        //skSeq.AppendCallback(() => BG.Move(Stat.Speed, 0.3f, 1f));
        //skSeq.Append(transform.DOMoveX(transform.position.x + Knight.Instance.skKnightK.Distance, 0.3f).From());
        //skSeq.AppendInterval(Knight.Instance.skKnightK.Delay - 0.3f);
        //skSeq.AppendCallback(() => skDelay -= 1f);
        //skSeq.AppendCallback(() => SpecialSkill());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsJumping = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsJumping = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IsAttackRange = false;
        if (other.gameObject.CompareTag("Enemy"))
        {
            IsAttackRange = true;
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
    }
    #endregion

    IEnumerator lerp(float startvalue, float duration)
    {
        transform.position = new Vector3(startvalue, transform.position.y);

        myRigid.velocity += Vector2.left * 1500f;
        while (transform.position.x > -590)
        {
            yield return null;
        }

        myRigid.velocity = new Vector2(0, myRigid.velocity.y);
        transform.position = new Vector3(-600, transform.position.y);
        Is_Psb_Attack = true;
        RangeDistance = 10000;
    }
}