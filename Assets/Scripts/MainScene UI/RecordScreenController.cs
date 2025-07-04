using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordScreenController : MonoBehaviour
{
    public GameObject recordRawPrefab;
    public GameObject recordRawParent;
    // Start is called before the first frame update

    private List<Record> records;
    void Start()
    {
        DataManager.I.LoadRecordData();

        records = new List<Record>(DataManager.I.records); //records 데이터 깊은 복사
        foreach (Record record in records)
        {
            GameObject recordRawObj = Instantiate(recordRawPrefab, recordRawParent.transform);
            recordRawObj.transform.GetChild(0).GetComponent<TMP_Text>().text = record.dateTime;
            recordRawObj.transform.GetChild(1).GetComponent<TMP_Text>().text = record.result;
            recordRawObj.transform.GetChild(2).GetComponent<TMP_Text>().text = record.labTime.ToString();
            recordRawObj.transform.GetChild(3).GetComponent<TMP_Text>().text = record.destroyedCarNum.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
