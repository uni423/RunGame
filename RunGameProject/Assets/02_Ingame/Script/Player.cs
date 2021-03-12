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

    public Skill(float coolTime, float distance, float delay)
    {
        CoolTime = coolTime;
        Distance = distance;
        Delay = delay;
    }
}

public class Player : MonoBehaviour
{
    public BackGround BG;

    Vector3 StandardPosition;

    public Skill skDash = new Skill(0f, 100f, 0.3f);

    private Rigidbody2D myRigid;
    public int JumpPower;

    public bool IsJumping = false;

    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        myRigid = GetComponent<Rigidbody2D>();
        StandardPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.A))
            Shild();
        if (Input.GetKeyDown(KeyCode.S))
            Bottom();
        if (Input.GetKeyDown(KeyCode.D))
            Dash();
        if (Input.GetKeyDown(KeyCode.J))
            Attack();
        if (Input.GetKeyDown(KeyCode.K))
            SpecialSkill();
        if (Input.GetKeyDown(KeyCode.L))
        { }
        if (Input.GetKeyDown(KeyCode.Escape))
        { }

        //if (IsJumping && transform.position.y >= 37.5)
        //{
        //    myRigid.mass
        //}

    }

    public void Jump() //점프
    {
        if (!IsJumping)
            return;

        Debug.Log("Jump");

        myRigid.velocity = Vector2.zero;
        Vector2 JumpVelocity = new Vector2(0, JumpPower);
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
        if (!skDash.IsPractice)
        {
            Debug.Log("Dash");
            skDash.IsPractice = true;
            transform.position += new Vector3(skDash.Distance, 0);
            BG.Move();

            transform.DOMoveX(StandardPosition.x, skDash.Delay).OnComplete(() => {
                skDash.IsPractice = false;
            });
        }
    }

    public void Attack() //공격, 상호작용키
    {
        Debug.Log("Attack");
    }

    public void SpecialSkill() //캐릭터 특수기
    {
        Debug.Log("skSpecial");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            IsJumping = true;
    }
}
