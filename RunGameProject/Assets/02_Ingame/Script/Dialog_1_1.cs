using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Doublsb.Dialog;

public class Dialog_1_1 : MonoBehaviour
{
    public DialogManager DialogManager;

    private void Awake()
    {
        var dialogTexts = new List<DialogData>();
        if (GameManager.Instance.stage == Define.Stage.Stage1_1)
        {
            dialogTexts.Add(new DialogData("그래, 이제 좀 정신이 들어?", "Gn"));

            dialogTexts.Add(new DialogData("구해줘서 고맙습니다...", "Kn"));

            dialogTexts.Add(new DialogData("약초따러 법을 어기면서 왕국 밖까지 나올리는 없고…", "Gn"));

            dialogTexts.Add(new DialogData("갑옷을 보니 너, 부활한 마왕을 토벌하러 가는거지?", "Gn"));

            dialogTexts.Add(new DialogData("아, 네..! 아직 부족하지만...", "Kn"));

            dialogTexts.Add(new DialogData("그럼 나도 합류하지", "Gn"));

            dialogTexts.Add(new DialogData("네???", "Kn", () =>
            {
                GameManager.Instance.stage = Define.Stage.Stage1_2;

                DOTween.Kill(GameManager.Instance.playerMG.Player);
                LoadManager.Load(LoadManager.Scene.Ingame);
                LoadManager.LoaderCallback();
            }));
        }
        else if (GameManager.Instance.stage == Define.Stage.Stage1_2)
        {
            dialogTexts.Add(new DialogData("잠시만요..! 왜 절 도와주시는 거에요?", "Kn"));

            dialogTexts.Add(new DialogData("널 도와주는 게 아니다. 나도 확인하고 싶은 게 있거든", "Gn"));

            dialogTexts.Add(new DialogData("넌 우선 강해지는게 최우선 같은데?", "Gn"));

            dialogTexts.Add(new DialogData("그게 말처럼 쉽지 않아요…", "Kn"));

            dialogTexts.Add(new DialogData("뭐, 경험이 쌓이면 점점 강해지겠지", "Gn", () =>
            {
                GameManager.Instance.stage = Define.Stage.Stage1_3;

                DOTween.Kill(GameManager.Instance.playerMG.Player);
                LoadManager.Load(LoadManager.Scene.Ingame);
                LoadManager.LoaderCallback();
            }));
        }
        else if (GameManager.Instance.stage == Define.Stage.Stage1_3)
        {
            dialogTexts.Add(new DialogData("쉿…! 슬라임 킹이다! 조용히 지나가자", "Gn"));

            dialogTexts.Add(new DialogData("어?! 아까 만난 괴물이에요!", "Kn"));

            dialogTexts.Add(new DialogData("뭐? 젠장! 슬라임 킹은 냄새로 적을 기억한다고!", "Gn"));

            dialogTexts.Add(new DialogData("네에??!?!", "Kn"));

            dialogTexts.Add(new DialogData("자세를 잡아라! 이렇게 된 거 한 판 붙어보자고!", "Gn", () =>
            {
                GameManager.Instance.stage = Define.Stage.Stage1_Boss;

                DOTween.Kill(GameManager.Instance.playerMG.Player);
                LoadManager.Load(LoadManager.Scene.Boss);
                LoadManager.LoaderCallback();
            }));
        }
        else if (GameManager.Instance.stage == Define.Stage.Stage1_Boss)
        {
            dialogTexts.Add(new DialogData("드디어 해치웠다…", "Kn"));
            
            dialogTexts.Add(new DialogData("너, 그래도 꽤 하는구나?", "Gn"));

            dialogTexts.Add(new DialogData("솔직히 처음엔 부잣집 아가씨의 사춘기 정도로 생각했는데 말이야", "Gn"));

            dialogTexts.Add(new DialogData("네?! 어째서요!", "Kn"));
            
            dialogTexts.Add(new DialogData("그 최상위 클래스의 장비들은 너 같은 초짜가 다룰 수 없는 것들이야", "Gn"));
            
            dialogTexts.Add(new DialogData("이 장비들은….. 나중에 말해드릴게요", "Kn"));
            
            dialogTexts.Add(new DialogData("그래, 지금은 승리를 만끽하자고", "Gn", () =>
            {
                GameManager.Instance.uiMG.Game_Clear();
            }));
        }

        DialogManager.Show(dialogTexts);
    }
}
