using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Slime : MonoBehaviour
{
    //public float Hp;
    //public EnemyData stat;
    //public Sprite dead;

    //[Header("Stat Check")]
    public bool IsAttack;
    //public bool IsDead;

    //    public void Start()
    //    {
    //        Hp = 10;
    //        IsDead = false;
    //        IsAttack = false;
    //    }

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        if (!IsAttack && transform.position.x <= -500)
        {
            IsAttack = true;
            transform.DOLocalJump(new Vector3(transform.localPosition.x, transform.localPosition.y), 300f, 1, 1 / 0.78f)
                .SetEase(Ease.Linear);
        }
    }

//    public float Damage(float damage)
//    {
//        Hp -= damage;
//        transform.DOMoveX(transform.position.x + 100, 0.2f);
//        if (Hp <= 0)
//        {
//            StartCoroutine(Dead());
//            return stat.Exp;
//        }

//        return 0;
//    }

//    public void PlayerAattack(float value)
//    {
//        transform.position += new Vector3(value, 0);
//    }

//    public IEnumerator Dead()
//    {
//        IsDead = true;
//        transform.GetComponent<SpriteRenderer>().sprite = dead;
//        yield return new WaitForSeconds(0.5f);
//        Destroy(gameObject);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.tag == "Player")
//        {
//            if (!IsDead)
//                collision.GetComponent<Player>().Damage(stat.Ad);
//        }
//    }
}
