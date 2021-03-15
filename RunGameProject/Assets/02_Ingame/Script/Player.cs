using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Skill
{
    public bool IsPractice = false;
    public float Distance;
    public float Delay;
    public float CoolTime;
    public float Time;

    public Skill(float coolTime, float distance, float delay, float time)
    {
        CoolTime = coolTime;
        Distance = distance;
        Delay = delay;
        Time = time;
    }
}

public class Player : MonoBehaviour
{
    public BackGround BG;

    Vector3 StandardPosition;
    private Rigidbody2D myRigid;

    public int JumpPower;
    public bool IsJumping = false;

    public Skill skDash = new Skill(0f, 100f, 0.3f, 0f);

    float skDelay = 0f;

    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        myRigid = GetComponent<Rigidbody2D>();
        StandardPosition = transform.position;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGamePlay) 
            return;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
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
    }

    #region Skills
    public void Jump() //점프
    {
        if (!IsJumping)
            return;

        myRigid.velocity = Vector2.zero;
        Vector2 JumpVelocity = new Vector2(0, JumpPower * 140);
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
        BG.Setsp(400f * 2.3f);
    }
    public void Dash_Quit() //대쉬
    {
        BG.Setsp(400f);
    }

    public void Attack() //공격, 상호작용키
    {
        Debug.Log("Attack");
    }

    public void SpecialSkill() //캐릭터 특수기
    {
        if (!IsJumping)
            return;
        //if (Knight.Instance.skKnightK.IsPractice)
        //    return;

        Debug.Log("skSpecial");

        if (skDelay <= 0f && !Knight.Instance.skKnightK.IsPractice) //처음 시작
        {
            skDelay = Knight.Instance.skKnightK.Time;
            Knight.Instance.skKnightK.IsPractice = true;
        }
        else if (skDelay <= 0f && Knight.Instance.skKnightK.IsPractice) //끝
        {
            skDelay = 0f;
            Knight.Instance.skKnightK.IsPractice = false;
            return;
        }

        Sequence skSeq = DOTween.Sequence();

        skSeq.AppendCallback(() => BG.Move(0.3f));
        skSeq.Append(transform.DOMoveX(transform.position.x + Knight.Instance.skKnightK.Distance, 0.3f).From());
        skSeq.AppendInterval(Knight.Instance.skKnightK.Delay - 0.3f);
        skSeq.AppendCallback(() => skDelay -= 1f);
        skSeq.AppendCallback(() => SpecialSkill());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            IsJumping = true;
    }

    #endregion
}
