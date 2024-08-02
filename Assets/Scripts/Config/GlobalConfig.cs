using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalConfig:Singleton<GlobalConfig>
{
    private int _platform = 1;

    public int Platform { get => _platform; set => _platform = value; }
    /// <summary>
    /// 1����ȫ����,2�������ӵ�еĿ�,3����༭��
    /// </summary>
    public int DeckOption { get => _deckOption; set => _deckOption = value; }

    private int _deckOption = 1;

    private int _battleMode = 1;
    /// <summary>
    /// 1��������ģʽ������ģʽ�¿ɳ鵽���п���
    /// 2������ͨģʽ����ͨģʽ��ֻ�ɳ鵽������еĿ���
    /// </summary>
    public int BattleMode { get => _battleMode; set => _battleMode = value; }

    public string GetPath()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {

        }
#if UNITY_EDITOR
        return Application.persistentDataPath;
#elif UNITY_ANDROID
			return Application.persistentDataPath;
#elif UNITY_IPHONE
			return GetiPhoneDocumentsPath();
#else
			return Application.dataPath;
#endif
    }
}
