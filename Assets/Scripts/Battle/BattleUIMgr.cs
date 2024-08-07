using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BattleUIMgr : MonoSington<BattleUIMgr>
{
    private GameObject _mainCanvas;
    private GameObject _playerSelectPanel;
    [SerializeField]
    private GameObject _playerSelectedPanel;
    [SerializeField]
    private GameObject _isExcutePanel;
    private GameObject _playerPanel;
    [SerializeField]
    private GameObject _playerDetailPanel;
    [SerializeField]
    private GameObject _MenuPanel;
    private GameObject _bossPanel;
    private Button[] _playerCmdBtns;
    private Text _playerCmdDesc;

    public GameObject PlayerSelectPanel { get => _playerSelectPanel; }



    // Start is called before the first frame update
    public void Init()
    {
        //��̬��Ӳ˵����ܼ�
        _MenuPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            //������¿�ʼ
            SceneManager.LoadScene("BattleScene");
        });
        _MenuPanel.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
        {
            //������¿�ʼ
            SceneManager.LoadScene("MainScene");
        });
        //��̬�����һ�غϵĹ��ܼ�
        GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("NextGround").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("NextGround").GetComponent<Button>().interactable = false;
            BattleSystemMgr.Instance?.ChangeBattleStatus(BattleStatus.BossTurn);
        });
        ///��̬��ӽű�
        _mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        _playerSelectPanel = _mainCanvas.transform.Find("PlayerSelectPanel").gameObject;
        _playerPanel = _mainCanvas.transform.Find("PlayerPanel").gameObject;
        _bossPanel = _mainCanvas.transform.Find("BossPanel").gameObject;
        GameObject _playerCmdPanel = _mainCanvas.transform.Find("PlayerCmdPanel").gameObject;
        _playerCmdBtns = _playerCmdPanel.GetComponentsInChildren<Button>();
        _playerCmdDesc = _playerCmdPanel.transform.Find("DescPanel").GetComponentInChildren<Text>();

        //��̬���ؽ�ɫ��Ϣ
        SOCharacterData[] sOCharacterDatas = BattleSystemMgr.Instance?.AllCharacterDatas;

        //��̬����boss��Ϣ
        SOCharacterData bossData = BattleSystemMgr.Instance?.BossDatas;
        BattleSystemMgr.Instance.BossController = _bossPanel.GetComponentInChildren<CharacterController>();
        BattleSystemMgr.Instance.BossController.CharacterHp = _bossPanel.GetComponentInChildren<Slider>(); ;
        _bossPanel.GetComponentInChildren<CharacterController>().Character = bossData;
        _bossPanel.GetComponentInChildren<CharacterController>().CharacterHp = _bossPanel.GetComponentInChildren<Slider>();
        _bossPanel.GetComponentInChildren<CharacterController>().AddSliderListener();

        //��̬��Ӽ����¼�
        Toggle[] _playerSelectsbuttons = PlayerSelectPanel.GetComponentsInChildren<Toggle>();
        Toggle[] _playerSelectedsbuttons = _playerSelectedPanel.GetComponentsInChildren<Toggle>();
        Slider[] _playerHpSliders = _playerPanel.GetComponentsInChildren<Slider>();
        for (int i = 0; i < _playerSelectsbuttons.Length; i++)
        {
            //��ӽ�ɫ�����࣬ͬʱ����ӦUI
            _playerSelectsbuttons[i].AddComponent<CharacterController>();
            CharacterController characterController = _playerSelectsbuttons[i].GetComponent<CharacterController>();
            BattleSystemMgr.Instance.CurSelectCharacter = characterController;
            BattleSystemMgr.Instance.PlayerSelects.Add(characterController);
            SOCharacterData sOCharacterData = sOCharacterDatas[i];
            characterController.Character = sOCharacterData;
            characterController.CharacterHp = _playerHpSliders[i];
            characterController.AddSliderListener();
            _playerSelectsbuttons[i].onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    characterController.transform.DOScale(1.2f, 0.3f);
                    characterController.CharacterHp.transform.parent.DOScale(1.2f, 0.3f);
                    BattleSystemMgr.Instance.CurSelectCharacter = characterController;
                    //_curSelectCharacter.Character = sOCharacterData;
                    this.UpdateSelectCharacterUI(sOCharacterData);
                }
                else
                {
                    characterController.transform.DOScale(1f, 0.3f);
                    characterController.CharacterHp.transform.parent.DOScale(1f, 0.3f);
                }
            });
            CharacterController selectedcharacterController = _playerSelectsbuttons[i].GetComponent<CharacterController>();
            _playerSelectedsbuttons[i].onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    BattleSystemMgr.Instance.CurSelectedCharacter = selectedcharacterController;
                    //_curSelectedCharacter.Character = sOCharacterData;
                }
            });
        }
        ///��ʼ��confirm�ļ����¼�
        Button[] _tembtns = _isExcutePanel.GetComponentsInChildren<Button>();
        _tembtns[0].onClick.AddListener(() =>
        {
            this.ExcuteSkill(BattleSystemMgr.Instance.CurSelectSkillInfo, BattleSystemMgr.Instance.CurSelectCharacter, BattleSystemMgr.Instance.CurSelectedCharacter);
        });
        _tembtns[1].onClick.AddListener(() =>
        {
            this.CloseIsExcutePanel();
        });
        this.UpdateSelectCharacterUI();
    }


    /// <summary>
    /// �������µ�ǰѡ��Ľ�ɫ��Ϣ�ķ���
    /// </summary>
    /// <param name="characterInfo"></param>
    public void UpdateSelectCharacterUI(SOCharacterData characterInfo = null)
    {
        MusicManager.Instance?.PlayClipByIndex(0);
        for (int i = 0; i < _playerCmdBtns.Length; i++)
        {
            _playerCmdBtns[i].onClick.RemoveAllListeners();
            _playerCmdBtns[i].GetComponent<EventTrigger>().triggers.RemoveRange(0, _playerCmdBtns[i].GetComponent<EventTrigger>().triggers.Count);

            if (characterInfo == null)
            {
                _playerCmdBtns[i].interactable = false;
                _playerCmdBtns[i].GetComponentInChildren<Text>().text = "";
                _playerCmdDesc.text = "";
                continue;
            }
            else
            {
                _playerCmdBtns[i].interactable = true;
            }
            _playerCmdBtns[i].GetComponentInChildren<Text>().text = characterInfo.skills[i].Name;
            SkillInfo skillInfo = characterInfo.skills[i];
            this.AddEventTriggerFun(_playerCmdBtns[i].GetComponent<EventTrigger>(), EventTriggerType.PointerEnter, (data) =>
            {
                _playerCmdDesc.text = (skillInfo.Desc);
            });
            _playerCmdBtns[i].onClick.AddListener(() =>
            {
                BattleSystemMgr.Instance.CurSelectSkillInfo = skillInfo;
                //����Ҫ���ݼ���ѡ��������Ƿ񷢶�
                this.HintSkillForWho(skillInfo);
            });
            #region ԭ�ƻ����ܲ�д�����߼�
            //if (i < characterInfo.skills.Count)
            //{
            //    _playerCmdBtns[i].GetComponentInChildren<Text>().text = characterInfo.skills[i].Name;
            //    _playerCmdBtns[i].onClick.RemoveAllListeners();
            //    _playerCmdBtns[i].GetComponent<EventTrigger>().triggers.RemoveRange(0, _playerCmdBtns[i].GetComponent<EventTrigger>().triggers.Count);
            //    SkillInfo skillInfo = characterInfo.skills[i];
            //    this.AddEventTriggerFun(_playerCmdBtns[i].GetComponent<EventTrigger>(), EventTriggerType.PointerEnter, (data) =>
            //    {
            //        _playerCmdDesc.text = (skillInfo.Desc);
            //    });
            //    _playerCmdBtns[i].onClick.AddListener(() =>
            //    {
            //        BattleSystemMgr.Instance.CurSelectSkillInfo = skillInfo;
            //        //����Ҫ���ݼ���ѡ��������Ƿ񷢶�
            //        this.HintSkillForWho(skillInfo);
            //    });
            //}
            //else
            //{
            //    _playerCmdBtns[i].gameObject.SetActive(false);
            //}
            #endregion
        }
    }/// <summary>
     /// �������µ�ǰѡ��Ľ�ɫ��Ϣ�ķ���
     /// </summary>
     /// <param name="characterInfo"></param>
    public void UpdateSelectedCharacterUI(SOCharacterData characterInfo)
    {
        Toggle[] toggles = _playerSelectedPanel.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {

        }
    }
    public void ShowPlayerDetailInfo(string info)
    {
        _playerDetailPanel.SetActive(true);
        _playerDetailPanel.GetComponentInChildren<Text>().text = info;
    }
    public void ClosePlayerDetailInfo()
    {
        _playerDetailPanel.GetComponentInChildren<Text>().text = "";
        _playerDetailPanel.SetActive(false);
    }
    private void AddEventTriggerFun(EventTrigger eventTrigger, EventTriggerType eventTriggerType, UnityEngine.Events.UnityAction<BaseEventData> unityAction)
    {
        // ������Ҫ�󶨵��¼�����
        EventTrigger.Entry entry = new EventTrigger.Entry();
        // �����¼�����
        entry.eventID = eventTriggerType;
        // ��ʼ���ص�����
        entry.callback = new EventTrigger.TriggerEvent();
        // ����ص�����
        UnityEngine.Events.UnityAction<BaseEventData> callBack = new UnityEngine.Events.UnityAction<BaseEventData>(unityAction);
        // �󶨻ص�����
        entry.callback.AddListener(callBack);
        eventTrigger.triggers.Add(entry);
    }

    public void HintSkillForWho(SkillInfo skill)
    {
        if (skill.skillType == SkillType.BoostOnePA ||
            skill.skillType == SkillType.BoostOneMA ||
            skill.skillType == SkillType.BoostOnePD ||
            skill.skillType == SkillType.BoostOneMD ||
            skill.skillType == SkillType.HealOneHp ||
            skill.skillType == SkillType.HealOneTp ||
            skill.skillType == SkillType.RebornOne)
        {
            //��Ҫѡ�����
            this.ShowChooseSelectedCharacterPanel();
        }
        else
        {
            //ֻ��Ҫ��ʾ�Ƿ�ִ��
            BattleSystemMgr.Instance.CurSelectedCharacter = null;
            this.ShowIsExcutePanel();
        }

    }

    public void ShowChooseSelectedCharacterPanel()
    {
        _playerSelectedPanel.SetActive(true);
        PopupManager.Instance?.AddUIInPopup(_playerSelectedPanel);
        this.ShowIsExcutePanel();
    }
    public void ExcuteSkill(SkillInfo skillInfo, CharacterController selectCharacter, CharacterController selectedCharacter)
    {

        BattleSystemMgr.Instance?.ExcuteSkill(skillInfo, selectCharacter, selectedCharacter);
        selectCharacter.gameObject.GetComponent<Toggle>().interactable = false;
        this.CloseIsExcutePanel();
    }
    public void ShowIsExcutePanel()
    {
        _isExcutePanel.SetActive(true);
        PopupManager.Instance?.AddUIInPopup(_isExcutePanel);
    }
    public void CloseIsExcutePanel()
    {
        _isExcutePanel.SetActive(false);
        _playerSelectedPanel.SetActive(false);
        PopupManager.Instance?.RemoveUIInPopup(_isExcutePanel);
        PopupManager.Instance?.RemoveUIInPopup(_playerSelectedPanel);
    }

    public void EnableNextground()
    {
        GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("NextGround").GetComponent<Button>().interactable = true;
    }



}
