using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPlatform : MonoBehaviour
{
    //[DllImport("__Internal")]
    //private static extern int GetUA();
    public Text text;
    public GameObject _pcPanel;
    public GameObject _mobilePanel;

    void Awake()
    {

        GlobalConfig.Instance.Platform = 1;
//#if UNITY_EDITOR_WIN
//#else
//        int a = GetUA();
//        if (a == 1)
//        {
//            Debug.Log("���ǵ���1");
//            text.text = "���ǵ���";
//            GlobalConfig.Instance.Platform = 1;
//            _pcPanel.SetActive(true);
//            _mobilePanel.SetActive(false);
//        }
//        if (a == 2)
//        {
//            Debug.Log("�����ֻ�");
//            text.text = "�����ֻ�"; 
//            GlobalConfig.Instance.Platform = 2;
//            _pcPanel.SetActive(false);
//            _mobilePanel.SetActive(true);
//        }
//#endif
    }
}
