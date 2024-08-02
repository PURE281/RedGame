using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����Լ��TΪSingleton����������ࣨwhere <ҪԼ���ķ��ͷ�������T> : <Լ���ľ�������>��
//����Լ�������ù���˼�����Լ�����ʹ�������ͣ�һ��������Լ����ʽ���ﲻչ������
public abstract class MonoSington<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = false;

    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType<T>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (global)
        {
            if (instance != null && instance != this.gameObject.GetComponent<T>())
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            instance = this.gameObject.GetComponent<T>();
        }
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}