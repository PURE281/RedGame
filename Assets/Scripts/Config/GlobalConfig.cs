using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalConfig:Singleton<GlobalConfig>
{
    private PlatformType platformType = PlatformType.WEBGL;


    /// <summary>
    /// 1代表全部卡,2代表玩家拥有的卡,3代表编辑卡
    /// </summary>
    public int DeckOption { get => _deckOption; set => _deckOption = value; }

    private int _deckOption = 1;

    private int _battleMode = 1;
    /// <summary>
    /// 1代表娱乐模式，娱乐模式下可抽到所有卡牌
    /// 2代表普通模式，普通模式下只可抽到玩家已有的卡牌
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
    public string GetPlatformABPath()
    {
        switch (platformType)
        {
            case PlatformType.WEBGL:
                return "WebGL/";
            case PlatformType.WINDOWS:
                return "PC/";
            case PlatformType.ANDROIDD:
                return "Andriod/";
            case PlatformType.IOS:
                return "IOS/";
            case PlatformType.EDITOR:
                return "PC/";
        }
        return null;
    }
}
