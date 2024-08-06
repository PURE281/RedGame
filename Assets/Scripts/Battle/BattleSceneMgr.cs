using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneMgr : MonoSington<BattleSceneMgr>
{
    // Start is called before the first frame update
    void Start()
    {
        PopupManager.Instance?.Init();
        BattleSystemMgr.Instance?.Init();
        BattleSystemMgr.Instance?.ChangeBattleStatus(BattleStatus.Init);
    }

}
