using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEnums
{
}

public enum BattleStatus
{
    Init, Start, PlayerTurn, BossTurn, Win, Lose, End
}
public enum SkillType
{
    BoostOnePA,BoostAllPA,BoostOneMA,BoostAllMA,
    BoostOnePD,BoostAllPD,BoostOneMD,BoostAllMD,
    BoostAllPAMA,BoostAllPDMD,
    DownPA,DownMA,DownPD,DownMD,
    HealOneHp,HealAllHp,RebornOne,RebornAll,
    HealOneTp,HealAllTp,
    Sleep,
    PAtked,
    MAtked,
    None
}

public enum CharacterType
{
    Player,Boss,Npc
}