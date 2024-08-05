using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理战斗系统的脚本
/// </summary>
public class BattleSystemMgr : MonoSington<BattleSystemMgr>
{
    [SerializeField]
    private SOCharacterData[] allCharacterDatas;

    public SOCharacterData[] AllCharacterDatas { get => allCharacterDatas; }


    /// <summary>
    /// 根据技能内容和对象进行技能逻辑处理
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="skillType"></param>
    /// <param name="character2Data"></param>
    public void ExcuteSkill(int skillType, SOCharacterData characterData, SOCharacterData character2Data)
    {

    }
}
