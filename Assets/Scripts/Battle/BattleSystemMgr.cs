using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// ����ս��ϵͳ�Ľű�
/// </summary>
public class BattleSystemMgr : MonoSington<BattleSystemMgr>
{
    [SerializeField]
    private SOCharacterData[] allCharacterDatas;
    [SerializeField]
    private SOCharacterData bossDatas;

    [SerializeField]
    private BattleStatus battleStatus;

    public SOCharacterData[] AllCharacterDatas { get => allCharacterDatas; }
    public SOCharacterData BossDatas { get => bossDatas; }
    public BattleStatus BattleStatus { get => battleStatus; }

    public void Init()
    {
        ///��ʼ��˫���Ľ�ɫ��Ϣ
        foreach (var item in allCharacterDatas)
        {
            item.CurHp = item.MaxHp;
            item.CurTp = item.MaxTp;
            item.CurPA = item.OriPA;
            item.CurPD = item.OriPD;
            item.CurMA = item.OriMA;
            item.CurMD = item.OriMD;
        }
        bossDatas.CurHp = bossDatas.MaxHp;
        bossDatas.CurTp = bossDatas.MaxTp;
        bossDatas.CurPA = bossDatas.OriPA;
        bossDatas.CurPD = bossDatas.OriPD;
        bossDatas.CurMA = bossDatas.OriMA;
        bossDatas.CurMD = bossDatas.OriMD;
    }


    /// <summary>
    /// ���ݼ������ݺͶ�����м����߼�����
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="skillType"></param>
    /// <param name="character2Data"></param>
    public void ExcuteSkill(SkillInfo skill, CharacterController selectCharacter, CharacterController selectedCharacter = null)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.HandleSkill(skill.skillType,skill.Value);
            Debug.Log(string.Format("{0}��{1}ʹ�� {2}", selectCharacter.character.Name, selectedCharacter.character.Name, skill.skillType));
        }
        else
        {
            foreach (var character in BattleUIMgr.Instance?.PlayerSelects)
            {
                character.HandleSkill(skill.skillType, skill.Value);
                Debug.Log(string.Format("{0}��ȫ��ʹ�� {1}", selectCharacter.character.Name, skill.skillType));
            }
        }
    }

    public void ChangeBattleStatus(BattleStatus status)
    {
        battleStatus = status;
        switch (battleStatus)
        {
            case BattleStatus.Init:
                //��ʼ��������˫����Ϣ�����ض������������ֵ���Դ
                BattleUIMgr.Instance?.Init();
                break;
            case BattleStatus.Start:
                //��ʾ��ʼ
                break;
            case BattleStatus.PlayerTurn:
                //��һغ�,����Խ�ɫ���п���
                break;
            case BattleStatus.BossTurn:
                //�з��غ�,AI
                //todo AI
                BattleBossAIMgr.Instance?.MoniAtk();
                break;
            case BattleStatus.Win:
                //���ʤ��,չʾʤ������
                break;
            case BattleStatus.Lose:
                //�з�ʤ��,չʾʧ�ܽ���
                break;
            case BattleStatus.End:
                //ս������
                break;
        }
    }
    public void CheckisWin()
    {

    }


}
