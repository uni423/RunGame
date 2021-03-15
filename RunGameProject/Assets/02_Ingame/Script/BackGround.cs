using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BackGround : MonoBehaviour
{
    Renderer Renderer;
    public Player Player;

    public float Speed;
    public float Offset;

    public void Start()
    {
        Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;

        Offset += Time.deltaTime * (Speed / 2000);
        Renderer.material.SetTextureOffset("_MainTex", new Vector2(Offset, 0));
    }

    public void Setsp(float sp)
    {
        Speed = sp;
    }

    public void Move(float duration)
    {
        Speed = 400f * 1.7f;
        DOTween.To(() => Speed, x => Speed = x, 400f, duration);
    }
}
