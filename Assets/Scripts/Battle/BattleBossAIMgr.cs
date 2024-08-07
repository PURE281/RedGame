using RandomElementsSystem.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理敌方Ai的脚本
/// </summary>
public class BattleBossAIMgr : MonoSington<BattleBossAIMgr>
{
    bool isSleep = false;
    public void MoniAtk()
    {
        if (isSleep)
        {
            string tem = string.Format("停止活动一回合");
            ToastManager.Instance.CreatToast(tem);
            Debug.Log(tem);
            isSleep = false;
        }
        else
        {
            MusicManager.Instance?.PlayClipByIndex(1);
            int randType = new MinMaxRandomInt(0, 2).GetRandomValue();
            if (randType == 0)
            {
                this.AtkAll();
            }
            else
            {
                this.AtkOne();
            }
        }
        BattleSystemMgr.Instance?.ChangeBattleStatus(BattleStatus.PlayerTurn);
        BattleUIMgr.Instance?.EnableNextground();
    }


    public void AtkAll()
    {
        List<CharacterController> playerSelects = BattleSystemMgr.Instance?.PlayerSelects;
        float demage = new MinMaxRandomFloat(10, 30).GetRandomValue();
        string tem = string.Format("对全体造成{0}", demage);
        ToastManager.Instance.CreatToast(tem);
        Debug.Log(tem);
        foreach (CharacterController character in playerSelects)
        {
            character.HandleSkill(SkillType.PAtked, demage);
        }

    }
    public void AtkOne()
    {
        List<CharacterController> playerSelects = BattleSystemMgr.Instance?.PlayerSelects;
        int selectedCharacter = new MinMaxRandomInt(0, 7).GetRandomValue();
        float demage = new MinMaxRandomFloat(10, 30).GetRandomValue();
        playerSelects[selectedCharacter].HandleSkill(SkillType.PAtked, demage);
        string tem = string.Format("对{0}造成{1}", playerSelects[selectedCharacter].Character.Name, demage);
        ToastManager.Instance.CreatToast(tem);
        Debug.Log(tem);
    }

    public void SleepOneRound()
    {
        this.isSleep = true;
    }
}
