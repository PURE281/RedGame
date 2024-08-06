using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control0 : MonoSington<Control0>
{
    public DialogueData_SO[] dialogueData_SOs;
    private int num = 0;

    public int Num { get => num; set => num = value; }

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        DialogueUI.Instance.UpdateDialogue(dialogueData_SOs[Num]);
        DialogueUI.Instance.UpdateMainDialogue(dialogueData_SOs[Num].dialoguePieces[Num]);
    }

    // Update is called once per frame
    public void UpdateDialogue()
    {
        DialogueUI.Instance.UpdateDialogue(dialogueData_SOs[Num]);
        DialogueUI.Instance.UpdateMainDialogue(dialogueData_SOs[Num].dialoguePieces[0]);
    }
}
