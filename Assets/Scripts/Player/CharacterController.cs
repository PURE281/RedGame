using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoSington<CharacterController>,IPointerEnterHandler,IPointerExitHandler
{
    public SOCharacterData character;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"{JsonUtility.ToJson(character)}>>>正在被查看>>>");
        BattleUIMgr.Instance?.ShowPlayerDetailInfo(JsonUtility.ToJson(character));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleUIMgr.Instance?.ClosePlayerDetailInfo();
    }

}
