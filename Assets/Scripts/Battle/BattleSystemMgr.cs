using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private CharacterController _curSelectCharacter;
    private CharacterController _curSelectedCharacter;
    private CharacterController _bossController;
    private SkillInfo _curSelectSkillInfo;
    List<CharacterController> _playerSelects;


    public List<CharacterController> PlayerSelects { get => _playerSelects; }
    public SOCharacterData[] AllCharacterDatas { get => allCharacterDatas; }
    public SOCharacterData BossDatas { get => bossDatas; }
    public BattleStatus BattleStatus { get => battleStatus; }
    public CharacterController CurSelectedCharacter { get => _curSelectedCharacter; set => _curSelectedCharacter = value; }
    public CharacterController CurSelectCharacter { get => _curSelectCharacter; set => _curSelectCharacter = value; }
    public SkillInfo CurSelectSkillInfo { get => _curSelectSkillInfo; set => _curSelectSkillInfo = value; }
    public CharacterController BossController { get => _bossController; set => _bossController = value; }

    public void Init()
    {
        _playerSelects = new List<CharacterController>();
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
    public void ExcuteSkill(SkillInfo skillInfo, CharacterController selectCharacter, CharacterController selectedCharacter = null)
    {
        if (selectCharacter.Character.CurTp < skillInfo.Cost)
        {
            ToastManager.Instance?.CreatToast("tp值不足，无法使用该技能");
            return;
        }
        //消耗cost
        selectCharacter.Character.CurTp -= skillInfo.Cost;
        ;
        if (skillInfo.skillType.ToString().Contains("One") &&CurSelectedCharacter == null)
        {
            ToastManager.Instance?.CreatToast("请选择一个目标角色！");
            return;
        }
        if (BattleStatus == BattleStatus.PlayerTurn)
        {
            if (skillInfo.skillType == SkillType.PAtked || skillInfo.skillType == SkillType.MAtked)
            {
                selectedCharacter = BossController;
            }
        }
        if (selectedCharacter != null)
        {
            selectedCharacter.HandleSkill(skillInfo.skillType, skillInfo.Value);
            Debug.Log(string.Format("{0}对{1}使用 {2}", selectCharacter.Character.Name, selectedCharacter.Character.Name, skillInfo.skillType));
        }
        else
        {
            foreach (var character in PlayerSelects)
            {
                character.HandleSkill(skillInfo.skillType, skillInfo.Value);
                Debug.Log(string.Format("{0}对全体使用 {1}", selectCharacter.Character.Name, skillInfo.skillType));
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
                //恢复所有的toggle的interactable
                List<CharacterController> playerSelects = PlayerSelects;
                foreach (var character in playerSelects)
                {
                    if (character.CharacterHp.value > 0)
                    {
                        character.gameObject.GetComponent<Toggle>().interactable = true;
                    }
                }
                break;
            case BattleStatus.BossTurn:
                //敌方回合,AI
                //todo AI
                BattleBossAIMgr.Instance?.MoniAtk();
                break;
            case BattleStatus.Win:
                //玩家胜利,展示胜利界面
                ToastManager.Instance?.CreatToast("player win");
                break;
            case BattleStatus.Lose:
                //敌方胜利,展示失败界面
                ToastManager.Instance?.CreatToast("player lose");
                break;
            case BattleStatus.End:
                //战斗结束，结算画面

                break;
        }
    }
    public void CheckWin(CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterType.Player:
                List<CharacterController> playerSelects = PlayerSelects;
                foreach (var item in playerSelects)
                {
                    if (item.GetComponent<Toggle>().interactable)
                    {
                        return;
                    }
                }
                //player lose
                this.ChangeBattleStatus(BattleStatus.Lose);
                break;
            case CharacterType.Boss:
                //player win 
                this.ChangeBattleStatus(BattleStatus.Win);
                break;
            case CharacterType.Npc:
                break;
        }
    }


}
