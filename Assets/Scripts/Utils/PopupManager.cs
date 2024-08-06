using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoSington<PopupManager>
{
    public GameObject PopupCanvas;

    //public GameObject PopupMaskCanvas;


    private Dictionary<string, Transform> temParentList;
    
    private Dictionary<string, Transform> temGOList;

    public void Init()
    {
        temGOList = new Dictionary<string, Transform>();
        temParentList = new Dictionary<string, Transform>();
        PopupCanvas.SetActive(false);
    }

    public void AddUIInPopup(GameObject UI)
    {
        PopupCanvas.SetActive(true);
        //先查询集合里有没有,有的话进行更新操作,没有的话进行添加操作
        if (temGOList.ContainsKey(UI.name))
        {
            Transform temGO = (Transform)temGOList[UI.name];
            //存在则更新该预制体,并且显示出来
            temGOList[UI.name] = UI.transform;
            //显示popupmask
            //ShowPopupMask();
            temGO.transform.SetParent(PopupCanvas.transform, false);
        }
        else
        {
            //通过临时变量存储即将放在popupui层的ui原先的父级
            temParentList.Add(UI.name, UI.transform.parent);
            temGOList.Add(UI.name, UI.transform);
            //显示popupmask
            //ShowPopupMask();
            UI.transform.SetParent(PopupCanvas.transform, false);
        }
    }
    public void RemoveUIInPopup(GameObject UI,bool isDestroy = false)
    {
        //先判断移除的ui层是否在popupcanvas里,有则移除,没有则不处理
        if (temGOList.ContainsKey(UI.name))
        {
            Transform temGO = temGOList[UI.name];
            Transform temGOParent = temParentList[UI.name];
            temGO.SetParent(temGOParent);
            //清除集合里的对应数据
            if (isDestroy)
            {
                temGOList.Remove(UI.name);
                temParentList.Remove(UI.name);
            }
            PopupCanvas.SetActive(false);
        }
        else return;
    }

    //private void ShowPopupMask()
    //{
    //    PopupMaskCanvas.transform.GetChild(0).gameObject.SetActive(true);
    //}

    //private void HidePopupMask()
    //{
    //    PopupMaskCanvas.transform.GetChild(0).gameObject.SetActive(false);
    //}

}
