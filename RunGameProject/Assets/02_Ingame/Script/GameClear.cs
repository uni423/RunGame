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

    public Sequence Sequence;

    public Boss KingSlime;

    public void Game_Clear()
    {
        Debug.Log("GameClear");
        GameManager.Instance.IsGamePlay = false;
        if (GameManager.Instance.stage == Define.Stage.Stage1_1)
        {
            Sequence = DOTween.Sequence();
            Sequence
                .Append(GameManager.Instance.playerMG.Player.transform.DOMove(new Vector3(640, -300), 1f).SetEase(Ease.Linear))
                .Join(KingSlime.gameObject.transform.DOMove(new Vector3(640, -370), 1f).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    KingSlime.gameObject.SetActive(true);
                    KingSlime.enabled = false;
                    KingSlime.Ani.SetBool("Is_Jump_Down", true);
                })
                .Append(Camera.main.DOShakePosition(0.5f, 150))
                .AppendCallback(() => SoundManager.Instance.StopBGM())
                .AppendInterval(1.5f)
                .AppendCallback(() => UI_gameClear.gameObject.SetActive(true))
                .Append(UI_gameClear.DOFade(0f, 0.5f).From())
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    KingSlime.gameObject.SetActive(false);
                    Story();
                        //DOTween.Kill(GameManager.Instance.playerMG.Player);
                        //LoadManager.Load(LoadManager.Scene.Ingame);
                        //LoadManager.LoaderCallback();
                    });
        }
        else
        {
            GameManager.Instance.playerMG.Player.transform.DOMove(new Vector3(1000, -300), 1f)
                .SetEase(Ease.Linear);
            UI_gameClear.gameObject.SetActive(true);
            UI_gameClear.DOFade(0f, 0.5f)
                .From()
                .OnComplete(() =>
                {
                    Story();
                });
        }
    }

    public void Story()
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
