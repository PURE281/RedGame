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
                //�ж��Ƿ��жϳɹ�
                BattleSystemMgr.Instance?.CheckWin(this.character.characterType);
            }
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string showInfo = string.Format("��ɫ��{0}\n " +
            "��ǰHP:{1}\n ��ǰTP:{2}\n ��ǰ�﹥:{3}\n ��ǰ���:{4}\n ��ǰħ��:{5}\n ��ǰħ��:{6}\n "
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
                //�����﹥
                Character.CurPA += value;
                break;
            case SkillType.BoostAllPA:
                //�����﹥
                Character.CurPA += value;
                break;
            case SkillType.BoostOneMA:
                //����ħ��
                Character.CurMA += value;
                break;
            case SkillType.BoostAllMA:
                //����ħ��
                Character.CurMA += value;
                break;
            case SkillType.BoostOnePD:
                //�������
                Character.CurPD += value;
                break;
            case SkillType.BoostAllPD:
                //�������
                Character.CurPD += value;
                break;
            case SkillType.BoostOneMD:
                //����ħ��
                Character.CurMD += value;
                break;
            case SkillType.BoostAllMD:
                //����ħ��
                Character.CurMD += value;
                break;
            case SkillType.BoostAllPAMA:
                //������ħ��
                Character.CurPA += value;
                Character.CurMA += value;
                break;
            case SkillType.BoostAllPDMD:
                //������ħ��
                Character.CurMD += value;
                Character.CurPD += value;
                break;
            case SkillType.DownPA:
                //�����﹥
                Character.CurPA -= value;
                break;
            case SkillType.DownMA:
                //����ħ��
                Character.CurMA -= value;
                break;
            case SkillType.DownPD:
                //�������
                Character.CurPD -= value;
                break;
            case SkillType.DownMD:
                //����ħ��
                Character.CurMD -= value;
                break;
            case SkillType.HealOneHp:
                //����Ѫ��
                Character.CurHp += value;
                characterHp.DOValue((Character.CurHp / Character.MaxHp), 0.5f);
                break;
            case SkillType.HealAllHp:
                //����Ѫ��
                Character.CurHp += value;
                characterHp.DOValue((Character.CurHp / Character.MaxHp), 0.5f);
                break;
            case SkillType.RebornOne:
                //����
                Character.CurHp += value;
                characterHp.DOValue((Character.CurHp / Character.MaxHp), 0.5f);
                this.gameObject.GetComponent<Toggle>().interactable = true;
                break;
            case SkillType.RebornAll:
                //����
                Character.CurHp += value;
                characterHp.DOValue((10 / Character.MaxHp), 0.5f);
                this.gameObject.GetComponent<Toggle>().interactable = true;
                break;
            case SkillType.HealOneTp:
                //����tp
                Character.CurTp += value;
                break;
            case SkillType.HealAllTp:
                //����tp
                Character.CurTp += value;
                break;
            case SkillType.Sleep:
                //����һ�غ�
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
                //������Ч��
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
