using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss_sk1_Effect : MonoBehaviour
{
    public Boss boss;

    private void Start()
    {
        Sequence mySequence = DOTween.Sequence()
        .SetAutoKill(false)
        .SetLink(this.gameObject, LinkBehaviour.RestartOnEnable)
        .AppendCallback(() => { this.transform.position = new Vector3(300, -230); } )
        .Join(this.transform.DOMoveX(-1200f, 0.7f)
            .SetEase(Ease.InSine))
        .Join(this.transform.DOLocalMoveY(-30f, 0.7f)
            .SetEase(Ease.InSine))
        .Join(this.transform.DOScale(0.9f, 0.7f)
            .SetEase(Ease.InSine))
        .OnComplete(() => {
            gameObject.SetActive(false);
        } );
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        if (!boss.Is_Dead)
    //            PlayerManager.Instance.Damage(boss.sk1.Damage, boss.gameObject.name);
    //    }
    //}
}
