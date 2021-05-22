using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Middleground: MonoBehaviour
{
    Renderer Renderer;

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
}
