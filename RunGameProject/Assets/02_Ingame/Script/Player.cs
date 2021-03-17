using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharStat Char;

    public int Level = 0; //현재 레벨
    public float NowExp; //현재 경험치
    public float NowHp; //현재 체력
    public float NowSp; //현재 스태미나

    public bool IsJumping = false; //점프 가능이면 true

    public bool IsAttackRange = false; //공격 발동 사거리 안에 몬스터가 있으면 true
    public float RangeDistance = 1000f;
    public Enemy RangeEnemyObj; //공격 발동 사거리 안에 있는 몬스터

    public Skill skDash = new Skill(0f, 100f, 0.3f, 0f);

    float skDelay = 0f;

    public BackGround BG;
    public Enemy enemy;
    public UIManager02 UIM_2;
    public Image UI_hp;
    public Image UI_sp;

    private Rigidbody2D myRigid;

    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
    }

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
    }

    public void StatUpdate()
    {
        if (NowExp >= Char.MaxExp && Level < 10)
        {
            StatUpgrade();
        }

        UI_hp.fillAmount = NowHp / Char.MaxHp;
        UI_sp.fillAmount = NowSp / Char.MaxSp;
    }

    public void StatUpgrade()
    {
        if (Level <= 0)
        {
            NowExp = 0f;
            NowHp = Char.MaxHp;
            NowSp = Char.MaxSp;
            BG.Setsp(Char.speed);
            Level = 1;
            return;
        }

        NowExp -= Char.MaxExp;
        Char.MaxExp += Char.AddExp; //+100
        Char.MaxHp += Mathf.Round(Char.MaxHp * Char.AddHp); //+0.2%
        Char.MaxSp += Char.AddSp; //+1
        Char.ad += Char.Addad; //+10
        Char.speed += Mathf.Round(Char.speed * Char.Addspeed); //+ 0.05%
        Char.adSpeed += Mathf.Round(Char.adSpeed * Char.AddadSpeed); //+ 0.02%
        Level++;
        BG.Setsp(Char.speed);
    }

    #region Skills
    public void Jump() //점프
    {
        if (!IsJumping)
            return;

        myRigid.velocity = Vector2.zero;
        Vector2 JumpVelocity = new Vector2(0, Char.JumpPower * 140);
        myRigid.AddForce(JumpVelocity, ForceMode2D.Impulse);
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
        BG.Setsp(Char.speed * 2.3f);
        enemy.Speed = 2.3f;
    }
    public void Dash_Quit() //대쉬
    {
        BG.Setsp(Char.speed);
        enemy.Speed = 1f;
    }

    public void Attack() //공격, 상호작용키
    {
        if (!IsAttackRange) 
            return;
        Debug.Log("Attack");

        //100px 당 0.02추가
        //BG.Offset += RangeDistance / 100f * 0.02f;
        BG.Move(Char.speed * 5f, RangeDistance / 200f * 0.2f);
        RangeEnemyObj.gameObject.transform.position += new Vector3(RangeDistance, 0);
        RangeEnemyObj.Damage(Char.ad);
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

        skSeq.AppendCallback(() => BG.Move(Char.speed, 0.3f));
        //skSeq.Append(transform.DOMoveX(transform.position.x + Knight.Instance.skKnightK.Distance, 0.3f).From());
        //skSeq.AppendInterval(Knight.Instance.skKnightK.Delay - 0.3f);
        skSeq.AppendCallback(() => skDelay -= 1f);
        skSeq.AppendCallback(() => SpecialSkill());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            IsJumping = true;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        IsAttackRange = false;
        if (other.gameObject.CompareTag("Enemy"))
        {
            IsAttackRange = true;
            if (RangeDistance > 600 + other.gameObject.transform.localPosition.x)
            {
                Debug.Log("콜라이더");
                RangeDistance = 600 + other.gameObject.transform.localPosition.x;
                RangeEnemyObj = other.gameObject.GetComponent<Enemy>();
            }
        }
    }

    #endregion
}
