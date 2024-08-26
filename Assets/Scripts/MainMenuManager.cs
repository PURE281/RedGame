using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MainMenuManager : MonoBehaviour
{
    public Button b_Start, Continue, Options, Exit, AboutUs;

    private void Awake()
    {
        b_Start?.onClick.AddListener(StartNewGame);
        Continue?.onClick.AddListener(ContinueGame);
        //Options?.onClick.AddListener(OpenOption);
        Exit?.onClick.AddListener(ExitGame);
        AboutUs?.onClick.AddListener(OpenAbout);
    }


    private void Start()
    {
        ABMgr.Instance?.Init((isSuc) =>
        {
            if (isSuc)
            {
                Debug.Log("资源更新下载完成");
            }
            else
            {
                Debug.Log("请检查网络");
            }
        });
    }

    void StartNewGame()
    {
        ProcessController.Instance?.GoNextScene("0-1");
    }

    void ContinueGame()
    {
        ProcessController.Instance?.GoNextScene("BattleScene");
    }

    void OpenOption()
    {

    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OpenAbout()
    {
        SceneManager.LoadScene("About");
    }

}
