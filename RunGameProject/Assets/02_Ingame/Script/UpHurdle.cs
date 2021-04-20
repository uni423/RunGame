using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpHurdle : MonoBehaviour
{
    public EnemyData stat;
    public bool IsUp;
    private Vector2 vector;

    private void Start()
    {
        transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    }


    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
        if (transform.position.x <= 550 && IsUp)
        {
            vector = transform.localPosition;
            IsUp = false;
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            transform.position = new Vector3(transform.position.x, -500);
            transform.DOLocalJump(vector, 300f, 1, 0.7f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
                collision.GetComponent<Player>().Damage(stat.Ad);
        }
    }
}
