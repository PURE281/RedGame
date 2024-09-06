using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ����ab�����غ�ʹ�õĽű�
/// </summary>
public class ABMgr : MonoSington<ABMgr>
{
    string baseServerUrl = "https://www.pure81.site/work/Public/Upload/game";

    List<ABInfo> localABInfo = new List<ABInfo>();
    List<ABInfo> remoteABInfo = new List<ABInfo>();
    List<string> downList = new List<string>();
    string tem_localABCompareFile;

    public async void Init(Action<bool> isSuccess)
    {
        //�ȶ�ȡ���رȽ��ļ�,���뻺��
        //��ȡ������Դ�Ƚ��ļ�
        tem_localABCompareFile = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo.txt");
        string[] strs = tem_localABCompareFile.Split("|");
        string[] infos = null;
        foreach (string str in strs)
        {
            infos = str.Split(" ");
            ABInfo abInfo = new ABInfo(infos[0], infos[1], infos[2]);
            localABInfo.Add(abInfo);
        }
        //�ٴӷ�������ȡ�Ƚ��ļ�
        DownloadABCompareFile();
        //�Ƚ��ļ��Ƿ��и���,�����������µ�ab��Դ��
        for (int i = 0; i < localABInfo.Count; i++)
        {
            if (localABInfo[i].md5 != remoteABInfo[i].md5)
            {
                downList.Add(remoteABInfo[i].name);
            }
        }
        if (downList.Count == 0)
        {
            isSuccess(true);
            return;
        }
        int tryNum = 5;
        List<string> temLIst = new List<string>();
        int downNum = 0;
        int downListCount = downList.Count;
        while (downList.Count > 0 && tryNum > 0)
        {
            //���ر��غͷ�������ͬ��ab��Դ��
            bool isSuc = false;
            foreach (string str in downList)
            {
                string localPath = Application.persistentDataPath + "/" + str;
                await Task.Run(() =>
                {
                    isSuc = DownloadFile(str, localPath);
                });
                if (isSuc)
                {
                    downNum++;
                    Debug.Log($"����{str}�ɹ�...{downNum}/{downListCount}");
                    temLIst.Add(str);
                }
                else
                {
                    Debug.Log($"����{str}ʧ��...������������");
                }
            }
            for (int i = 0; i < temLIst.Count; i++)
            {
                downList.Remove(temLIst[i]);
            }
            tryNum--;
        }
        if (downList.Count == 0)
        {
            string info = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_tmp.txt");
            //���±Ƚ��ļ�
            File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt", info);
            isSuccess(true);
        }
        else
        {
            //�ָ��Ƚ��ļ�
            File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt", tem_localABCompareFile);
            isSuccess(false);
        }

    }
    public void DownloadABCompareFile()
    {
        //�ӷ�����������Դ�Ƚ��ļ�
        bool isSuc = DownloadFile("ABCompareInfo.txt", Application.persistentDataPath + "/ABCompareInfo_tmp.txt");
        if (!isSuc)
        {
            ToastManager.Instance?.CreatToast("���������쳣...");
            return;
        }
        //��ȡ������Դ�Ƚ��ļ�
        string info = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_tmp.txt");
        string[] strs = info.Split("|");
        string[] infos = null;
        foreach (string str in strs)
        {
            infos = str.Split(" ");
            ABInfo abInfo = new ABInfo(infos[0], infos[1], infos[2]);
            remoteABInfo.Add(abInfo);
        }
    }
    //private IEnumerator DownloadABFile()
    //{

    //}
    //private async Task<bool> DownloadFile(string filename, string localPath)
    //{
    //    string url = $"{baseServerUrl}/{filename}";
    //    try
    //    {
    //        using (UnityWebRequest request = UnityWebRequest.Get(url))
    //        {
    //            await request.SendWebRequest();
    //            if (request.isHttpError || request.isNetworkError)
    //            {
    //                //Debug.Log(request.error);
    //                ToastManager.Instance?.CreatToast("�ļ������쳣����������");
    //                return false;

    //            }
    //            File.WriteAllBytes(localPath, request.downloadHandler.data);
    //        }
    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        return false;
    //    }

    //}

    WebResponse response = null;
    Stream inStream = null;
    Stream outStream = null;
    HttpWebRequest request = null;
    private bool DownloadFile(string filename, string localPath)
    {
        try
        {
            Debug.Log($"����{filename}��...");
            string url = $"{baseServerUrl}/{filename}";
            request = (HttpWebRequest)WebRequest.Create(url);
            response = request.GetResponse();
            inStream = response.GetResponseStream();
            byte[] bytes = new byte[1024];
            FileInfo fileInfo = new FileInfo(localPath);
            outStream = fileInfo.Create();
            int readddCount = inStream.Read(bytes, 0, bytes.Length);
            while (readddCount > 0)
            {
                outStream.Write(bytes, 0, readddCount);
                readddCount = inStream.Read(bytes, 0, bytes.Length);
            }
            outStream.Close();
            inStream.Close();
            response.Close();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        request?.Abort();
        outStream?.Dispose();
        inStream?.Dispose();
        response?.Dispose();
    }

}
class ABInfo
{
    public string name;
    public long size;
    public string md5;

    public ABInfo(string name, string size, string md5)
    {
        this.name = name;
        this.size = long.Parse(size);
        this.md5 = md5;
    }
}


public enum PlatformType
{
    WEBGL, WINDOWS, ANDROIDD, IOS, EDITOR
}