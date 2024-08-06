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
        //Debug.Log($"{JsonUtility.ToJson(character)}>>>���ڱ��鿴>>>");
        string showInfo = string.Format("��ɫ��{0}\n " +
            "��ǰHP:{1}\n ��ǰTP:{2}\n ��ǰ�﹥:{3}\n ��ǰ���:{4}\n ��ǰħ��:{5}\n ��ǰħ��:{6}\n "
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
                //�����﹥
                character.CurPA += value;
                break;
            case SkillType.BoostAllPA:
                //�����﹥
                character.CurPA += value;
                break;
            case SkillType.BoostOneMA:
                //����ħ��
                character.CurMA += value;
                break;
            case SkillType.BoostAllMA:
                //����ħ��
                character.CurMA += value;
                break;
            case SkillType.BoostOnePD:
                //�������
                character.CurPD += value;
                break;
            case SkillType.BoostAllPD:
                //�������
                character.CurPD += value;
                break;
            case SkillType.BoostOneMD:
                //����ħ��
                character.CurMD += value;
                break;
            case SkillType.BoostAllMD:
                //����ħ��
                character.CurMD += value;
                break;
            case SkillType.BoostAllPAMA:
                //������ħ��
                character.CurPA += value;
                character.CurMA += value;
                break;
            case SkillType.BoostAllPDMD:
                //������ħ��
                character.CurMD += value;
                character.CurPD += value;
                break;
            case SkillType.DownPA:
                //�����﹥
                character.CurPA -= value;
                break;
            case SkillType.DownMA:
                //����ħ��
                character.CurMA -= value;
                break;
            case SkillType.DownPD:
                //�������
                character.CurPD -= value;
                break;
            case SkillType.DownMD:
                //����ħ��
                character.CurMD -= value;
                break;
            case SkillType.HealOneHp:
                //����Ѫ��
                character.CurHp += value;
                break;
            case SkillType.HealAllHp:
                //����Ѫ��
                character.CurHp += value;
                break;
            case SkillType.RebornOne:
                //����
                character.CurHp += value;
                break;
            case SkillType.RebornAll:
                //����
                character.CurHp += value;
                break;
            case SkillType.HealOneTp:
                //����tp
                character.CurTp += value;
                break;
            case SkillType.HealAllTp:
                //����tp
                character.CurTp += value;
                break;
            case SkillType.Sleep:
                //����һ�غ�
                break;
            case SkillType.None:
                //������Ч��
                character.CurHp -= value;
                break;
        }
    }

}
