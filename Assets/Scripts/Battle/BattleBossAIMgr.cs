using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理敌方Ai的脚本
/// </summary>
public class BattleBossAIMgr : MonoSington<BattleBossAIMgr>
{
    public void MoniAtk()
    {

        BattleSystemMgr.Instance?.ChangeBattleStatus(BattleStatus.PlayerTurn);
        BattleUIMgr.Instance?.EnableNextground();
    }
}
