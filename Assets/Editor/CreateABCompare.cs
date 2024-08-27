using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CreateABCompare
{
    [MenuItem("AB������/�����Ա��ļ�")]
    public static void CreateCompareFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory($"{Application.dataPath}/AB/{GlobalConfig.Instance?.GetPlatformABPath()}");
        FileInfo[] fileInfos = directory.GetFiles();
        string abCompareInfo = "";
        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension == "")
            {
                abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GetMD5(fileInfo.FullName);
                abCompareInfo += "|";
            }
        }
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        File.WriteAllText($"{Application.dataPath}/AB/{GlobalConfig.Instance?.GetPlatformABPath()}" + "ABCompareInfo.txt", abCompareInfo);
        AssetDatabase.Refresh();
        Debug.Log("����AB���Ա��ļ��ɹ�");
    }
    static string GetMD5(string filePath)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            byte[] bytes = md5.ComputeHash(file);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
