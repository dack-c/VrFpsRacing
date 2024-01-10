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

    public List<Record> records; //기록 데이터들

    private string m_filePath;
    private BinaryFormatter binaryFormatter = new BinaryFormatter();

    void Start()
    {
        m_filePath = Application.persistentDataPath + @"\Records.dat"; //기록데이터가 저장될 파일 경로
        List<Record> records = new List<Record>();
    }

    public bool LoadRecordData() //Record데이터를 파일로부터 불러옴
    {
        if (!File.Exists(m_filePath))
        {
            return false;
        }

        using (Stream rs = new FileStream(m_filePath, FileMode.Open))
        {
            records = (List<Record>)binaryFormatter.Deserialize(rs);
        }
        return true;
    }

    public bool SaveRecordData(Record record) //Record데이터를 파일로 저장함
    {
        records.Add(record);
        try
        {
            using (Stream ws = new FileStream(m_filePath, FileMode.Create))
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
}
