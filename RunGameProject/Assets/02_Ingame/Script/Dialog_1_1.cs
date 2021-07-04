using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;

public class Dialog_1_1 : MonoBehaviour
{
    public DialogManager DialogManager;

    private void Awake()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("그래, 이제 좀 정신이 들어?", "Gn"));

        dialogTexts.Add(new DialogData("구해줘서 고맙습니다...", "Kn"));

        dialogTexts.Add(new DialogData("약초따러 법을 어기면서 왕국 밖까지 나올리는 없고…", "Gn"));

        dialogTexts.Add(new DialogData("갑옷을 보니 너, 부활한 마왕을 토벌하러 가는거지?", "Gn"));

        dialogTexts.Add(new DialogData("아, 네..! 아직 부족하지만...", "Kn"));

        dialogTexts.Add(new DialogData("그럼 나도 합류하지", "Gn"));

        dialogTexts.Add(new DialogData("네???", "Kn"));

        DialogManager.Show(dialogTexts);
    }
}
