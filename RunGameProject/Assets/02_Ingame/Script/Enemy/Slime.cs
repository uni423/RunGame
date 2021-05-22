using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public bool IsAttack;

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        if (!IsAttack && transform.position.x <= 0 && !gameObject.GetComponent<Enemy>().IsDead)
        {
            IsAttack = true;
            transform.DOLocalJump(new Vector3(transform.localPosition.x, transform.localPosition.y), 300f, 1, 0.7f)
                .SetEase(Ease.Linear);
        }
    }
}
