using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public float Hp;
    public EnemyData stat;
    public Sprite dead;

    public bool IsDead;

    public void Start()
    {
        Hp = 10;
        IsDead = false;
    }

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
    }

    public float Damage(float damage)
    {
        Hp -= damage;
        transform.DOMoveX(transform.position.x + 100, 0.2f);
        if (Hp <= 0)
        {
            StartCoroutine(Dead());
            return stat.Exp;
        }

        return 0;
    }
    
    public void PlayerAattack(float value)
    {
        transform.position += new Vector3(value, 0);
    }

    public IEnumerator Dead()
    {
        IsDead = true;
        transform.GetComponent<SpriteRenderer>().sprite = dead;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!IsDead)
                collision.GetComponent<Player>().Damage(stat.Ad);
                //Debug.Log(collision.gameObject.name);
        }
    }
}
