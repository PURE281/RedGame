using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ս��ϵͳ�Ľű�
/// </summary>
public class BattleSystemMgr : MonoSington<BattleSystemMgr>
{
    [SerializeField]
    private SOCharacterData[] allCharacterDatas;

    public SOCharacterData[] AllCharacterDatas { get => allCharacterDatas; }


    /// <summary>
    /// ���ݼ������ݺͶ�����м����߼�����
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="skillType"></param>
    /// <param name="character2Data"></param>
    public void ExcuteSkill(int skillType, SOCharacterData characterData, SOCharacterData character2Data)
    {

    }
}
