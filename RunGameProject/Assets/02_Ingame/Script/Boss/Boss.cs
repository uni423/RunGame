using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Define;

public class Boss : MonoBehaviour
{
    public EnemyData stat;
    public float NowHp;
    public bool Is_Dead;
    public bool Is_Damage;

    public bool Is_SkillPlay = true;
    public bool Is_Rage = false;
    public Skill sk1;
    public Skill sk2;
    public Sequence sk_Sequence;
    [SerializeField] private AnimationCurve sk2_Jump1;
    [SerializeField] private AnimationCurve sk2_Jump2;

    public delegate void Delegate();

    public MeshRenderer myren;
    public Collider2D myColl;

    public GameObject sk1_Effect;

    public Collider2D skill2_Coll;

    public Animator Ani;


    public void Start()
    {
        NowHp = stat.MaxHp;
        Is_Dead = false;
        Is_Rage = false;
        Is_Damage = false;

        skill2_Coll.gameObject.SetActive(false);

        sk1 = new Skill(2f, 30, 0, 1f);
        sk2 = new Skill(2f, 40, 0, 1f);
    }

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
        if (!Is_SkillPlay)
        {
            int rand = Random.Range(0, 100);
            if (rand > 40)
                Skill_1(); //60%확률로 발동
            else
                Skill_2(); //40%확률로 발동

            Is_SkillPlay = true;
        }
    }

    public void Damage(float damage, string Enemy)
    {
        Is_Damage = true;
        myren.material.DOColor(Color.red, 0.5f)
            .From();
        Ani.SetTrigger("Hiting");

        StartCoroutine(Timer(0.5f, () => { Is_Damage = false; }));
    }

    public void Skill_1()
    {
        //1초간 공중에 뜨고, 내려오는데 1초. 땅에 닿을 때부터 충격파가 날라간다.
        //충격파는 거리비례 크기가 커진다. 충격파의 초기 Y범위는 200이며, 거리 100당 y범위가 50씩 늘어난다
        //60%확률로 발동

        Ani.SetTrigger("Jumping");
        this.transform.DOLocalJump(this.transform.position, 500, 1, 1f)
            .SetEase(Ease.Linear)
            .SetDelay(0.3f);
        StartCoroutine(Timer(1.3f, () =>
        {
            Camera.main.DOShakePosition(0.5f, 150);
            sk1_Effect.gameObject.SetActive(true);
            StartCoroutine(Timer(1f, () => { Is_SkillPlay = false; }));
        }));
    }

    public void Skill_2()
    {
        //1.5초간 공중에 뜨고, 내려오는데 0.5초, 600 이라는 X범위를 가졌다.찍고 난 다음 0.5초간 경직, 다시 원래 자리로 점프한다.
        //40%확률로 발동

        if (sk_Sequence == null)
        {
            sk_Sequence = DOTween.Sequence();
            sk_Sequence
                .SetAutoKill(false)
                .AppendCallback(() => Ani.SetBool("Is_Jump_Press", true))
                .AppendInterval(1f)
                .Append(this.transform.DOJump(GameManager.Instance.playerMG.Player.transform.position + new Vector3(0, 300f), 0f, 1, 1f)
                    .SetEase(sk2_Jump1))
                .AppendCallback(() => 
                { 
                    Ani.SetBool("Is_Jump_Up", true); Ani.SetBool("Is_Jump_Press", false);
                    myColl.enabled = false; skill2_Coll.gameObject.SetActive(true); 
                })
                .Append(this.transform.DOMoveY(-300f, 0.5f)
                    .SetEase(Ease.OutBack, 2f))
                .Join(Camera.main.DOShakePosition(0.5f, 150))
                .AppendInterval(0.2f)
                .AppendCallback(() => { Ani.SetTrigger("Jumping"); })
                .AppendInterval(0.3f)
                .Append(this.transform.DOJump(new Vector3(640, -370), 500f, 1, 1f)
                    .SetEase(sk2_Jump2))
                .AppendCallback(() => { myColl.enabled = true; skill2_Coll.gameObject.SetActive(false); 
                    StartCoroutine(Timer(1.5f, () => { Is_SkillPlay = false; })); });
        }
        else
            sk_Sequence.Restart();
    }

    public void Skill_3()
    {
        //(추후 발동 안함) 폴짝! 과 쿠궁! 의 후딜레이를 없앤다
    }

    protected virtual IEnumerator Timer(float time, Delegate dele)
    {
        yield return new WaitForSeconds(time);
        dele();
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        if (!Is_Dead)
    //        {
    //            if (skill2_Coll.gameObject.activeSelf)
    //                PlayerManager.Instance.Damage(sk2.Damage, this.name);
    //            else
    //                PlayerManager.Instance.Damage(stat.TriAd, this.name);
    //        }
    //    }
    //}
}
