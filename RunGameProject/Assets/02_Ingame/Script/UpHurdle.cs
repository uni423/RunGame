using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpHurdle : MonoBehaviour
{
    public EnemyData stat;
    public bool IsUp;
    private Vector2 vector;

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
        if (transform.position.x <= 400 && IsUp)
        {
            vector = transform.localPosition;
            IsUp = false;
            transform.DOLocalJump(vector, 10f, 1, 0.3f);
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
