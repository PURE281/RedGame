using RandomElementsSystem.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����з�Ai�Ľű�
/// </summary>
public class BattleBossAIMgr : MonoSington<BattleBossAIMgr>
{
    public void MoniAtk()
    {
        int randType = new MinMaxRandomInt(0, 2).GetRandomValue();
        if (randType == 0)
        {
            this.AtkAll();
        }
        else
        {
            this.AtkOne();
        }
        BattleSystemMgr.Instance?.ChangeBattleStatus(BattleStatus.PlayerTurn);
        BattleUIMgr.Instance?.EnableNextground();
    }


    public void AtkAll()
    {
        List<CharacterController> playerSelects = BattleUIMgr.Instance?.PlayerSelects;
        float demage = new MinMaxRandomFloat(10, 30).GetRandomValue();
        ToastManager.Instance.CreatToast(string.Format("��ȫ�����{0}", demage));
        foreach (CharacterController character in playerSelects)
        {
            character.HandleSkill(SkillType.PAtked, demage);
        }

    }
    public void AtkOne()
    {
        List<CharacterController> playerSelects = BattleUIMgr.Instance?.PlayerSelects;
        int selectedCharacter = new MinMaxRandomInt(0, 7).GetRandomValue();
        float demage = new MinMaxRandomFloat(10, 30).GetRandomValue();
        playerSelects[selectedCharacter].HandleSkill(SkillType.PAtked, demage);
        ToastManager.Instance.CreatToast(string.Format("��{0}���{1}", playerSelects[selectedCharacter].character.Name, demage));
    }
}
