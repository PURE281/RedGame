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

        BattleSystemMgr.Instance?.ChangeBattleStatus(BattleStatus.PlayerTurn);
        BattleUIMgr.Instance?.EnableNextground();
    }
}
