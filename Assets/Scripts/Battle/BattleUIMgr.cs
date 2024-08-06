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

    private CharacterController _curSelectCharacter;
    private CharacterController _curSelectedCharacter;
    private SkillInfo _curSelectSkillInfo;
    List<CharacterController> _playerSelects;

    public List<CharacterController> PlayerSelects { get => _playerSelects; }

    // Start is called before the first frame update
    public void Init()
    {
        _playerSelects = new List<CharacterController>();
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
        _bossPanel.GetComponentInChildren<CharacterController>().character = bossData;
        //��̬��Ӽ����¼�
        Toggle[] _playerSelectsbuttons = _playerSelectPanel.GetComponentsInChildren<Toggle>();
        Toggle[] _playerSelectedsbuttons = _playerSelectedPanel.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < _playerSelectsbuttons.Length; i++)
        {
            _playerSelectsbuttons[i].AddComponent<CharacterController>();
            CharacterController characterController = _playerSelectsbuttons[i].GetComponent<CharacterController>();
            _curSelectCharacter = characterController;
            _playerSelects.Add(characterController);
            SOCharacterData sOCharacterData = sOCharacterDatas[i];
            characterController.character = sOCharacterData;
            _playerSelectsbuttons[i].onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    _curSelectCharacter.character = sOCharacterData;
                    this.UpdateSelectCharacterUI(sOCharacterData);
                }
            });
            CharacterController selectedcharacterController = _playerSelectsbuttons[i].GetComponent<CharacterController>();
            _playerSelectedsbuttons[i].onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    _curSelectedCharacter = selectedcharacterController;
                    _curSelectedCharacter.character = sOCharacterData;
                }
            });
        }
        ///��ʼ��confirm�ļ����¼�
        Button[] _tembtns = _isExcutePanel.GetComponentsInChildren<Button>();
        _tembtns[0].onClick.AddListener(() =>
        {
            this.ExcuteSkill(_curSelectSkillInfo,_curSelectCharacter,_curSelectedCharacter);
        });
        _tembtns[1].onClick.AddListener(() =>
        {
            this.CloseIsExcutePanel();
        });
        this.UpdateSelectCharacterUI(sOCharacterDatas[0]);
        _curSelectCharacter.character = sOCharacterDatas[0];
    }


    /// <summary>
    /// �������µ�ǰѡ��Ľ�ɫ��Ϣ�ķ���
    /// </summary>
    /// <param name="characterInfo"></param>
    public void UpdateSelectCharacterUI(SOCharacterData characterInfo)
    {
        for (int i = 0; i < _playerCmdBtns.Length; i++)
        {
            if (i < characterInfo.skills.Count)
            {
                _playerCmdBtns[i].GetComponentInChildren<Text>().text = characterInfo.skills[i].Name;
                _playerCmdBtns[i].onClick.RemoveAllListeners();
                _playerCmdBtns[i].GetComponent<EventTrigger>().triggers.RemoveRange(0, _playerCmdBtns[i].GetComponent<EventTrigger>().triggers.Count);
                SkillInfo skillInfo = characterInfo.skills[i];
                this.AddEventTriggerFun(_playerCmdBtns[i].GetComponent<EventTrigger>(), EventTriggerType.PointerEnter, (data) =>
                {
                    _playerCmdDesc.text = (skillInfo.Desc);
                });
                _playerCmdBtns[i].onClick.AddListener(() =>
                {
                    _curSelectSkillInfo = skillInfo;
                    //����Ҫ���ݼ���ѡ��������Ƿ񷢶�
                    this.HintSkillForWho(skillInfo);
                });
            }
            else
            {
                _playerCmdBtns[i].gameObject.SetActive(false);
            }
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
            _curSelectedCharacter = null;
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
        if(skillInfo.skillType.ToString().Contains("One") && _curSelectedCharacter == null)
        {
            ToastManager.Instance?.CreatToast("��ѡ��һ��Ŀ���ɫ��");
            return;
        }
        BattleSystemMgr.Instance?.ExcuteSkill(skillInfo, selectCharacter, selectedCharacter);
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
