using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using zFramework.Extension;
using static EnumMgr;

public class CsvManager : Singleton<CsvManager>
{


    public List<SlotsBean> GetSlots()
    {
        try
        {
            return null;
            //return CsvUtility.Read<SlotsBean>((GlobalConfig.Instance?.GetPath() + "/StreamingAssets") + "/" + "CardData.csv");
        }
        catch (Exception ex)
        {
            Debug.Log($"Êý¾ÝÎªÁã{ex.Message}");
            return new List<SlotsBean>();
        }
    }
}

[Serializable]
public class SlotsBean
{

}
