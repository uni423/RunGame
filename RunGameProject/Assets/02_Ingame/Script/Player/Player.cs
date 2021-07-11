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
    [Tooltip("스위치점프 중인 상태인가?")] public bool Is_SwichJumping = false;
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

    public int Combo = 0;

    public IEnumerator Lerp;
    public IEnumerator ComboTimer;
    public delegate void Delegate();

    public Animator Anim;
    public Animator Effect_Anim;
    public Animator Effect_Hitting_Anim;

    [Space(10f)]
    public BGManager BG;
    public UIManager02 UIM_2;

    private Rigidbody2D myRigid;
    public GameObject JumpColli;
    public GameObject RangeColli;

    public bool Is_BossStage = false;
    public bool Is_CantJump = false;

    #endregion

    public void init()
    {
        if ((int)GameManager.Instance.stage % 10 == 9)
            Is_BossStage = true;

        BG = GameManager.Instance.bgMG;
        UIM_2 = GameManager.Instance.uiMG;

        myRigid = transform.GetComponent<Rigidbody2D>();
        Anim = this.GetComponent<Animator>();

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
            if (Is_Dash) Dash_Quit();
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
            GameManager.Instance.bgMG.In_Speed(Stat.Speed);
            Stat.Level = 1;

            Is_Damage = false;
            RangeEnemyObj = null;
            Is_SkillA = false;
            Is_SkillK = false;
            return;
        }

        Stat.NowExp -= Stat.MaxExp;
        Stat.MaxExp += Stat.AddExp;
        Stat.MaxHp += Mathf.Round(Stat.MaxHp * Stat.AddHp);
        Stat.NowHp += Mathf.Round(Stat.MaxHp * Stat.AddHp);
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
        if (transform.position.x != -600 && Lerp == null)
        {
            //Lerp = lerp(transform.position.x, 0.2f, () => Lerp = null) ;
            //StartCoroutine(Lerp);
        }

        //점프 후 하강할때 가속도 추가
        if (!Is_Jumping && myRigid.velocity.y < 10 && Is_Down)
        {
            Vector2 vel = new Vector2(myRigid.velocity.x, (Physics2D.gravity * 8f).y);
            myRigid.velocity = vel;
            Is_Down = false;
            Anim.SetBool("Jump_Up", false);
            Anim.SetBool("Jump_Down", true);
        }
    }

    public void Damage(float damage, string Enemy)
    {
        Is_Damage = true;
        if (Stat.NowHp - damage <= 0)
        {

            if (GameManager.Instance.CharacterCode == CharType.Knight)
                SoundManager.Instance.PlaySound("Knight_Dead");
            else if (GameManager.Instance.CharacterCode == CharType.Gunner)
            {
                SoundManager.Instance.PlaySound("Gunner_Dead");
                Debug.Log("으엥");
            }

            GameManager.Instance.IsGamePlay = false;
            UIM_2.Game_Over(Enemy);
        }

        StartCoroutine(Timer(0.5f, () => { Is_Damage = false; }));
        Dash_Quit();
        DOTween.To(() => Stat.NowHp, x => Stat.NowHp = x, Stat.NowHp - damage, 0.2f).OnComplete(() => { });
        Camera.main.DOShakePosition(0.3f, 100);
        Camera.main.transform.DOMove(new Vector3(0, 0, -10), 0.1f).SetDelay(0.3f);
        BG.Move(Stat.Speed * 0.5f, 2f);
    }

    public float NowHpSp(int num)
    {
        switch (num)
        {
            case 1: return Stat.NowHp / Stat.MaxHp;
            case 2: return Stat.NowSp / Stat.MaxSp;
            default:
                return 0;
        }
    }

    public float SkillNowCool(int num)
    {
        switch (num)
        {
            case 1:
                return (skA.MaxCoolTime - skA.NowCoolTime) / skA.MaxCoolTime;
            case 2:
                return (skK.MaxCoolTime - skK.NowCoolTime) / skK.MaxCoolTime;
            default:
                return 0;
        }
    }

    #region Skills
    public void Jump(bool swich = false) //점프
    {
        if (!Is_Jumping && !swich && !Is_SkillK || (Is_SkillK && transform.position.y >= 400f) && Is_CantJump)
            return;

        if (!Is_Attack)
            StopCoroutine(Lerp);

        if (swich)
        {
            Vector2 vel = new Vector2(myRigid.velocity.x, (Stat.JumpPower * 0.7f) * 5.5f);
            myRigid.velocity = vel;
            Is_SwichJumping = true;
        }
        else
        {
            Vector2 vel = new Vector2(myRigid.velocity.x, Stat.JumpPower * 5.5f);
            myRigid.velocity = vel;
            Is_SwichJumping = false;
            Is_Jumping = false;
            Is_Down = true;

            Anim.SetBool("Jump_Up", true);
        }

        if (!Is_Attack)
            StartCoroutine(lerp(transform.position.x, 0.2f,
                () =>
                {
                    Is_Attack = true;
                    RangeDistance = 10000;
                    RangeEnemyObj = null;
                    Lerp = null;
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

    private void Character_Swich()
    {
        if (Is_SkillK || GameManager.Instance.stage == Stage.Stage1_1)
            return;

        if (GameManager.Instance.CharacterCode == CharType.Knight)
        {
            if (GameManager.Instance.playerMG.Character_Swich(CharType.Gunner))
                SoundManager.Instance.PlaySound("Gunner_Swich");
        }
        else if (GameManager.Instance.CharacterCode == CharType.Gunner)
        {
            if (GameManager.Instance.playerMG.Character_Swich(CharType.Knight))
                SoundManager.Instance.PlaySound("Knight_Swich");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Is_SwichJumping = false;
            Is_Jumping = true;
            Anim.SetBool("Jump_Down", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (Is_SwichJumping)
                Is_Jumping = true;
            else
                Is_Jumping = false;

            if (transform.position.y > -300)
            {
                Anim.SetBool("Jump_Up", false);
                Anim.SetBool("Jump_Down", true);
            }
        }
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Drop"))
        {
            GameManager.Instance.playerMG.Damage(99999, "Drop");
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
                if (RangeDistance < 0 || RangeEnemyObj.transform.GetComponent<Enemy>().Is_Dead)
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

    public IEnumerator lerp(float startvalue, float duration, Delegate dele = null)
    {
        transform.position = new Vector3(startvalue, transform.position.y);

        Vector2 vector = (startvalue > -600) ? Vector2.left : Vector2.right;

        float i = duration;
        float velocity = Vector2.Distance(new Vector2(startvalue, 0), new Vector2(-600, 0));
        velocity *= 3 - duration;
        if (velocity < 0)
            velocity = 0;

        while (i > 0)
        {
            yield return null;
            i -= Time.deltaTime;

            Vector2 vel = new Vector2(vector.x * velocity, myRigid.velocity.y/* + 500f*/);
            myRigid.velocity = vel;

            if ((vector.x > 0 && transform.position.x >= -600) ||
                (vector.x < 0 && transform.position.x <= -600))
                break;
        }

        myRigid.velocity = new Vector2(0, myRigid.velocity.y);
        transform.position = new Vector3(-600, transform.position.y);
        yield return new WaitForSeconds(i);

        if (dele != null) dele();
    }

    protected virtual IEnumerator Timer(float time, Delegate dele)
    {
        yield return new WaitForSeconds(time);
        dele();
    }
}