using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// 管理战斗系统的脚本
/// </summary>
public class BattleSystemMgr : MonoSington<BattleSystemMgr>
{
    [SerializeField]
    private SOCharacterData[] allCharacterDatas;
    [SerializeField]
    private SOCharacterData bossDatas;

    [SerializeField]
    private BattleStatus battleStatus;

    public SOCharacterData[] AllCharacterDatas { get => allCharacterDatas; }
    public SOCharacterData BossDatas { get => bossDatas; }
    public BattleStatus BattleStatus { get => battleStatus; }

    public void Init()
    {
        ///初始化双方的角色信息
        foreach (var item in allCharacterDatas)
        {
            item.CurHp = item.MaxHp;
            item.CurTp = item.MaxTp;
            item.CurPA = item.OriPA;
            item.CurPD = item.OriPD;
            item.CurMA = item.OriMA;
            item.CurMD = item.OriMD;
        }
        bossDatas.CurHp = bossDatas.MaxHp;
        bossDatas.CurTp = bossDatas.MaxTp;
        bossDatas.CurPA = bossDatas.OriPA;
        bossDatas.CurPD = bossDatas.OriPD;
        bossDatas.CurMA = bossDatas.OriMA;
        bossDatas.CurMD = bossDatas.OriMD;
    }


    /// <summary>
    /// 根据技能内容和对象进行技能逻辑处理
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="skillType"></param>
    /// <param name="character2Data"></param>
    public void ExcuteSkill(SkillInfo skill, CharacterController selectCharacter, CharacterController selectedCharacter = null)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.HandleSkill(skill.skillType,skill.Value);
            Debug.Log(string.Format("{0}对{1}使用 {2}", selectCharacter.character.Name, selectedCharacter.character.Name, skill.skillType));
        }
        else
        {
            foreach (var character in BattleUIMgr.Instance?.PlayerSelects)
            {
                character.HandleSkill(skill.skillType, skill.Value);
                Debug.Log(string.Format("{0}对全体使用 {1}", selectCharacter.character.Name, skill.skillType));
            }
        }
    }

    public void ChangeBattleStatus(BattleStatus status)
    {
        battleStatus = status;
        switch (battleStatus)
        {
            case BattleStatus.Init:
                //初始化，加载双方信息，加载动画，加载音乐等资源
                BattleUIMgr.Instance?.Init();
                break;
            case BattleStatus.Start:
                //提示开始
                break;
            case BattleStatus.PlayerTurn:
                //玩家回合,允许对角色进行控制
                break;
            case BattleStatus.BossTurn:
                //敌方回合,AI
                //todo AI
                BattleBossAIMgr.Instance?.MoniAtk();
                break;
            case BattleStatus.Win:
                //玩家胜利,展示胜利界面
                break;
            case BattleStatus.Lose:
                //敌方胜利,展示失败界面
                break;
            case BattleStatus.End:
                //战斗结束
                break;
        }
    }
    public void CheckisWin()
    {

    }


}
