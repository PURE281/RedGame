using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterController : MonoSington<CharacterController>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private SOCharacterData character;

    private Slider characterHp;

    public Slider CharacterHp { get => characterHp; set => characterHp = value; }
    public SOCharacterData Character { get => character; set => character = value; }

    public void AddSliderListener()
    {

        characterHp.onValueChanged.AddListener((value) =>
        {
            if (value <= 0)
            {
                this.gameObject.GetComponent<Toggle>().interactable = false;
                //判断是否判断成功
                BattleSystemMgr.Instance?.CheckWin(this.character.characterType);
            }
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string showInfo = string.Format("角色：{0}\n " +
            "当前HP:{1}\n 当前TP:{2}\n 当前物攻:{3}\n 当前物防:{4}\n 当前魔攻:{5}\n 当前魔防:{6}\n "
            , Character.Name
            , Character.CurHp, Character.CurTp
            , Character.CurPA, Character.CurPD, Character.CurMA, Character.CurMD);
        BattleUIMgr.Instance?.ShowPlayerDetailInfo(showInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleUIMgr.Instance?.ClosePlayerDetailInfo();
    }

    public void HandleSkill(SkillType skillType, float value)
    {
        float damege = 0;
        switch (skillType)
        {
            case SkillType.BoostOnePA:
                //增加物攻
                Character.CurPA += value;
                break;
            case SkillType.BoostAllPA:
                //增加物攻
                Character.CurPA += value;
                break;
            case SkillType.BoostOneMA:
                //增加魔攻
                Character.CurMA += value;
                break;
            case SkillType.BoostAllMA:
                //增加魔攻
                Character.CurMA += value;
                break;
            case SkillType.BoostOnePD:
                //增加物防
                Character.CurPD += value;
                break;
            case SkillType.BoostAllPD:
                //增加物防
                Character.CurPD += value;
                break;
            case SkillType.BoostOneMD:
                //增加魔防
                Character.CurMD += value;
                break;
            case SkillType.BoostAllMD:
                //增加魔防
                Character.CurMD += value;
                break;
            case SkillType.BoostAllPAMA:
                //增加物魔攻
                Character.CurPA += value;
                Character.CurMA += value;
                break;
            case SkillType.BoostAllPDMD:
                //增加物魔防
                Character.CurMD += value;
                Character.CurPD += value;
                break;
            case SkillType.DownPA:
                //降低物攻
                Character.CurPA -= value;
                break;
            case SkillType.DownMA:
                //降低魔攻
                Character.CurMA -= value;
                break;
            case SkillType.DownPD:
                //降低物防
                Character.CurPD -= value;
                break;
            case SkillType.DownMD:
                //降低魔防
                Character.CurMD -= value;
                break;
            case SkillType.HealOneHp:
                //增加血量
                Character.CurHp += value;
                characterHp.DOValue((Character.CurHp / Character.MaxHp), 0.5f);
                break;
            case SkillType.HealAllHp:
                //增加血量
                Character.CurHp += value;
                characterHp.DOValue((Character.CurHp / Character.MaxHp), 0.5f);
                break;
            case SkillType.RebornOne:
                //复活
                Character.CurHp += value;
                characterHp.DOValue((Character.CurHp / Character.MaxHp), 0.5f);
                this.gameObject.GetComponent<Toggle>().interactable = true;
                break;
            case SkillType.RebornAll:
                //复活
                Character.CurHp += value;
                characterHp.DOValue((10 / Character.MaxHp), 0.5f);
                this.gameObject.GetComponent<Toggle>().interactable = true;
                break;
            case SkillType.HealOneTp:
                //增加tp
                Character.CurTp += value;
                break;
            case SkillType.HealAllTp:
                //增加tp
                Character.CurTp += value;
                break;
            case SkillType.Sleep:
                //跳过一回合
                if (character.characterType==CharacterType.Player)
                {
                    BattleBossAIMgr.Instance?.SleepOneRound();
                }
                else
                {
                    this.GetComponent<Toggle>().interactable = false;
                }
                break;
            case SkillType.None:
                //无特殊效果
                break;
            case SkillType.PAtked:
                damege = (value - character.CurPD)<=0?0: (value - character.CurPD);
                Character.CurHp -= damege;
                if (Character.CurHp<=0)
                {
                    Character.CurHp = 0;
                }
                characterHp.DOValue((Character.CurHp/Character.MaxHp),0.5f);
                break;
            case SkillType.MAtked:
                damege = (value - character.CurMD) <= 0 ? 0 : (value - character.CurMD);
                Character.CurHp -= damege;
                if (Character.CurHp <= 0)
                {
                    Character.CurHp = 0;
                }
                characterHp.DOValue((Character.CurHp / Character.MaxHp), 0.5f);
                break;
        }
    }

}
