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

    public bool IsAttackRange = false; //공격 발동 사거리 안에 몬스터가 있으면 true
    public float RangeDistance = 1000f;
    public Enemy RangeEnemyObj; //공격 발동 사거리 안에 있는 몬스터

    public Skill skDash = new Skill(0f, 100f, 0.3f, 0f);

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

        if (myRigid.velocity.y < 0)
        {
            myRigid.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        }
    }

    public void StatUpdate()
    {
        if (Stat.NowExp >= Stat.MaxExp && Stat.Level < 10)
        {
            StatUpgrade();
        }
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

    #region Skills
    public void Jump() //점프
    {
        if (!IsJumping)
            return;

        myRigid.velocity = Vector2.zero;
        Vector2 JumpVelocity = new Vector2(0, Stat.JumpPower * 5f);
        //myRigid.AddForce(JumpVelocity, ForceMode2D.Impulse);
        myRigid.velocity = Vector2.up * JumpVelocity;
        IsJumping = false;
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
    }
    public void Dash_Quit() //대쉬
    {
        BG.In_Speed(Stat.Speed);
        Enemy.Speed = 1f;
    }

    public void Attack() //공격, 상호작용키
    {
        if (!IsAttackRange) 
            return;
        Debug.Log("Attack");

        //100px 당 0.02추가
        //BG.Offset += RangeDistance / 100f * 0.02f;
        BG.Move(Stat.Speed * 5f, (RangeDistance / 200f) * 0.3f);
        RangeEnemyObj.gameObject.transform.position += new Vector3(RangeDistance, 0);
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

        Sequence skSeq = DOTween.Sequence();

        skSeq.AppendCallback(() => BG.Move(Stat.Speed, 0.3f));
        //skSeq.Append(transform.DOMoveX(transform.position.x + Knight.Instance.skKnightK.Distance, 0.3f).From());
        //skSeq.AppendInterval(Knight.Instance.skKnightK.Delay - 0.3f);
        skSeq.AppendCallback(() => skDelay -= 1f);
        skSeq.AppendCallback(() => SpecialSkill());
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
            if (RangeDistance > 600 + other.gameObject.transform.localPosition.x)
            {
                Debug.Log("충동체 연결 완료");
                RangeDistance = 600 + other.gameObject.transform.localPosition.x;
                RangeEnemyObj = other.gameObject.GetComponent<Enemy>();
            }
        }
    }

    #endregion
}