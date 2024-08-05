using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BattleUIMgr : MonoSington<BattleUIMgr>
{
    private GameObject _mainCanvas;
    private GameObject _playerSelectPanel;
    private GameObject _playerPanel;
    [SerializeField]
    private GameObject _playerDetailPanel;
    private GameObject _bossPanel;
    private Button[] _playerCmdBtns;
    private Text _playerCmdDesc;
    // Start is called before the first frame update
    void Start()
    {
        ///动态添加脚本
        _mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        _playerSelectPanel = _mainCanvas.transform.Find("PlayerSelectPanel").gameObject;
        _playerPanel = _mainCanvas.transform.Find("PlayerPanel").gameObject;
        _bossPanel = _mainCanvas.transform.Find("BossPanel").gameObject;
        GameObject _playerCmdPanel = _mainCanvas.transform.Find("PlayerCmdPanel").gameObject;
        _playerCmdBtns = _playerCmdPanel.GetComponentsInChildren<Button>();
        _playerCmdDesc = _playerCmdPanel.transform.Find("DescPanel").GetComponentInChildren<Text>();
        //动态加载角色信息
        SOCharacterData[] sOCharacterDatas = BattleSystemMgr.Instance?.AllCharacterDatas;
        //动态添加监听事件
        Toggle[] _playerSelectsbuttons = _playerSelectPanel.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < _playerSelectsbuttons.Length; i++)
        {
            _playerSelectsbuttons[i].AddComponent<CharacterController>();
            CharacterController characterController = _playerSelectsbuttons[i].GetComponent<CharacterController>();
            SOCharacterData sOCharacterData = sOCharacterDatas[i];
            characterController.character = sOCharacterData;
            _playerSelectsbuttons[i].onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    this.UpdateCharacterUI(sOCharacterData);
                }
            });
        }

        this.UpdateCharacterUI(sOCharacterDatas[0]);
    }

    
    /// <summary>
    /// 用来更新当前选择的角色信息的方法
    /// </summary>
    /// <param name="characterInfo"></param>
    public void UpdateCharacterUI(SOCharacterData characterInfo)
    {
        for (int i = 0; i < _playerCmdBtns.Length; i++)
        {
            if (i<characterInfo.skills.Count)
            {
                _playerCmdBtns[i].GetComponentInChildren<Text>().text = characterInfo.skills[i].Name;
                _playerCmdBtns[i].onClick.RemoveAllListeners();
                SkillInfo skillInfo = characterInfo.skills[i];
                _playerCmdBtns[i].onClick.AddListener(() =>
                {
                    _playerCmdDesc.text = (skillInfo.Desc);
                });
            }
            else
            {
                _playerCmdBtns[i].gameObject.SetActive(false);
            }
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
}
