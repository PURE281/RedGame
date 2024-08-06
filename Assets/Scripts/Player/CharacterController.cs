using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoSington<CharacterController>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public SOCharacterData character;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"{JsonUtility.ToJson(character)}>>>正在被查看>>>");
        string showInfo = string.Format("角色：{0}\n " +
            "当前HP:{1}\n 当前TP:{2}\n 当前物攻:{3}\n 当前物防:{4}\n 当前魔攻:{5}\n 当前魔防:{6}\n "
            , character.Name
            , character.CurHp, character.CurTp
            , character.CurPA, character.CurPD, character.CurMA, character.CurMD);
        BattleUIMgr.Instance?.ShowPlayerDetailInfo(showInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleUIMgr.Instance?.ClosePlayerDetailInfo();
    }

    public void HandleSkill(SkillType skillType, float value)
    {
        switch (skillType)
        {
            case SkillType.BoostOnePA:
                //增加物攻
                character.CurPA += value;
                break;
            case SkillType.BoostAllPA:
                //增加物攻
                character.CurPA += value;
                break;
            case SkillType.BoostOneMA:
                //增加魔攻
                character.CurMA += value;
                break;
            case SkillType.BoostAllMA:
                //增加魔攻
                character.CurMA += value;
                break;
            case SkillType.BoostOnePD:
                //增加物防
                character.CurPD += value;
                break;
            case SkillType.BoostAllPD:
                //增加物防
                character.CurPD += value;
                break;
            case SkillType.BoostOneMD:
                //增加魔防
                character.CurMD += value;
                break;
            case SkillType.BoostAllMD:
                //增加魔防
                character.CurMD += value;
                break;
            case SkillType.BoostAllPAMA:
                //增加物魔攻
                character.CurPA += value;
                character.CurMA += value;
                break;
            case SkillType.BoostAllPDMD:
                //增加物魔防
                character.CurMD += value;
                character.CurPD += value;
                break;
            case SkillType.DownPA:
                //降低物攻
                character.CurPA -= value;
                break;
            case SkillType.DownMA:
                //降低魔攻
                character.CurMA -= value;
                break;
            case SkillType.DownPD:
                //降低物防
                character.CurPD -= value;
                break;
            case SkillType.DownMD:
                //降低魔防
                character.CurMD -= value;
                break;
            case SkillType.HealOneHp:
                //增加血量
                character.CurHp += value;
                break;
            case SkillType.HealAllHp:
                //增加血量
                character.CurHp += value;
                break;
            case SkillType.RebornOne:
                //复活
                character.CurHp += value;
                break;
            case SkillType.RebornAll:
                //复活
                character.CurHp += value;
                break;
            case SkillType.HealOneTp:
                //增加tp
                character.CurTp += value;
                break;
            case SkillType.HealAllTp:
                //增加tp
                character.CurTp += value;
                break;
            case SkillType.Sleep:
                //跳过一回合
                break;
            case SkillType.None:
                //无特殊效果
                character.CurHp -= value;
                break;
        }
    }

}
