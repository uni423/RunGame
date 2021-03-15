using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Define;

[CreateAssetMenu(fileName = "Charactor Data", menuName = "Scriptable Object/Charactor Data", order = int.MaxValue)]

public class CharactorData : ScriptableObject
{
    public List<CharactorStat> StatList;
}