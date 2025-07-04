using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Record
{
    public string dateTime;
    public string result;
    public float labTime;
    public int destroyedCarNum;
}

[Serializable]
public class SelectedItemInfo //속성등 수정될 여지 존재.
{
    public string name;//None이 올수도
}

public class DataManager : MonoBehaviour
{
    public static DataManager I { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (I)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject); //씬이 바뀌어도 DataManger는 삭제되지 않음. 즉, Record 데이터는 씬이 전환되도 계속 유지됨
    }

    public List<Record> records { get; private set; } //기록 데이터들
    public SelectedItemInfo[] selectedItemInfos; //선택된 아이템 정보들

    public const int maxItemNum = 3; //가질 수 있는 아이템 개수

    private string m_recordFilePath;
    private string m_selectedItemInfoFilePath;

    private BinaryFormatter binaryFormatter = new BinaryFormatter();

    void Start()
    {
        m_recordFilePath = Application.persistentDataPath + @"\Records.dat"; //기록데이터가 저장될 파일 경로
        records = new List<Record>();

        m_selectedItemInfoFilePath = Application.persistentDataPath + @"\SelectedItemInfos.dat"; //아이템 데이터가 저장될 파일 경로
        selectedItemInfos = new SelectedItemInfo[maxItemNum];

        LoadRecordData(); //LoadRecordData() 하기 전에 SaveRecordData()를 해버려 기록이 덮어씌워져버리는 문제가 생길 수 있으므로, 처음부터 미리 load함.

        //test
        /*Record record = new Record
        {
            dateTime = DateTime.Now.ToString(),
            result = "testResult",
            labTime = 231.12f,
            destroyedCarNum = 2
        };
        SaveRecordData(record);*/
    }

    public bool LoadRecordData() //Record데이터를 파일로부터 불러옴
    {
        if (!File.Exists(m_recordFilePath))
        {
            return false;
        }

        using (Stream rs = new FileStream(m_recordFilePath, FileMode.Open))
        {
            records = (List<Record>)binaryFormatter.Deserialize(rs);
        }
        return true;
    }

    public bool SaveRecordData(Record record) //Record데이터를 파일로 저장함
    {
        //records.Add(record);
        records.Insert(0, record);
        try
        {
            using (Stream ws = new FileStream(m_recordFilePath, FileMode.Create))
            {
                binaryFormatter.Serialize(ws, records);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }

    public bool LoadSelectedItemData()
    {
        if (!File.Exists(m_selectedItemInfoFilePath))
        {
            return false;
        }

        using (Stream rs = new FileStream(m_selectedItemInfoFilePath, FileMode.Open))
        {
            selectedItemInfos = (SelectedItemInfo[])binaryFormatter.Deserialize(rs);
        }
        return true;
    }

    public bool SaveSelectedItemData(SelectedItemInfo[] itemInfosToSave)
    {
        for(int i = 0; i < maxItemNum; i++)
        {
            selectedItemInfos[i] = itemInfosToSave[i];
        }

        try
        {
            using (Stream ws = new FileStream(m_selectedItemInfoFilePath, FileMode.Create))
            {
                binaryFormatter.Serialize(ws, selectedItemInfos);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }
}
