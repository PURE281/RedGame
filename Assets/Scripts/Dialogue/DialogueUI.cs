using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour,IPointerClickHandler
{
    public static DialogueUI Instance { get; private set; }

    public bool endFlag = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private DialogueData_SO currentData;//��ǰ�Ի�����
    private int currentIndex;//�Ի����

    [SerializeField]
    private Canvas dialogueCanvas;
    [SerializeField]
    private Canvas comCanvas;
    private Sprite bgSprite;
    private bool canContinue;

    [Header("===== Dialogue Basic Element =====")]
    [SerializeField]
    private Image standA;//�������
    [SerializeField]
    private Image standB;//Npc����
    [SerializeField]
    private Text speakerName;//˵��������
    [SerializeField]
    private Text mainText;//�Ի��ı�

    private void Start()
    {
        bgSprite = comCanvas.transform.Find("Background").GetComponent<Image>().sprite;
    }
    public void UpdateDialogue(DialogueData_SO data)//�˺���Ϊÿ�ο����Ի��ĵ�һ��ˢ��
    {
        currentData = data;
        currentIndex = 0;
    }
    public void UpdateMainDialogue(DialoguePiece piece)//�˺���Ϊÿ�μ����Ի�ʱ������һ������
    {
        canContinue = true;
        dialogueCanvas.enabled = true;
        standA.enabled = false;
        standB.enabled = false;

        if (!piece.isNpc)
        {
            standA.enabled = true;
            standA.sprite = piece.image;
        }
        else if (piece.isNpc)
        {
            standB.enabled = true;
            standB.sprite = piece.image;
        }

        mainText.text = "";
        speakerName.text = "";
        speakerName.text = piece.speakerName;
        if (piece.bgImage!=null)
        {
            comCanvas.transform.Find("Background").GetComponent<Image>().sprite = piece.bgImage;
        }
        if (piece.bgmClip != null)
        {
            MusicManager.Instance?.PlayBgmByClip(piece.bgmClip);
        }
        mainText.DOText(piece.text,((piece.text.Length/20)));

        if (currentData.dialoguePieces.Count > 0)
        {
            currentIndex++;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentIndex < currentData.dialoguePieces.Count)
        {
            mainText.DOKill();
            mainText.text = "";
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        }
        else
        {
            Control0.Instance.Num++;
            if (Control0.Instance.Num >= Control0.Instance.dialogueData_SOs.Length)
            {
                ToastManager.Instance?.CreatToast("�������Ķ����");
                SceneManager.LoadScene("BattleScene");
                return;
            }
            currentIndex = 0;
            Control0.Instance?.UpdateDialogue();
            //dialogueCanvas.enabled = false;

        }
    }
}
