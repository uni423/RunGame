using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameClear : MonoBehaviour
{
    public Image UI_gameClear;

    public GameObject DialogAsset;
    public GameObject DialogObject;

    public void Game_Clear()
    {
        if (GameManager.Instance.stage == Define.Stage.Stage1_1)
        {
            Debug.Log("GameClear");
            GameManager.Instance.IsGamePlay = false;
            GameManager.Instance.playerMG.Player.transform.DOMove(new Vector3(1000, -300), 1f)
                .SetEase(Ease.Linear);
            UI_gameClear.gameObject.SetActive(true);
            UI_gameClear.DOFade(0f, 0.5f)
                .From()
                .OnComplete(() =>
                {
                    //DOTween.Kill(PlayerManager.Instance.Player);
                    //LoadManager.Load(LoadManager.Scene.Boss);
                    //LoadManager.LoaderCallback();
                    Beta();
                });
        }
    }

    public void Beta()
    {
        GameManager.Instance.playerMG.Player.SetActive(false);
        DialogAsset.SetActive(true);
        DialogObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Game_Clear();
        }
    }
}
