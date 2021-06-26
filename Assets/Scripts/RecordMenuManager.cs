using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordMenuManager : MonoBehaviour
{
    [SerializeField] public GameObject prefabTableRow;
    [SerializeField] public GameObject recordsMenu;

    private const float offsetYpos = -40;
    private const string lineNumTagObj = "NumLine";
    private const string nameTagObj = "Name";
    private const string scoreTagObj = "Score";
    private int currentNumRow = 1;
    private float currentRowPosY = 145f;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ShowRecordsTable(gameManager.records.records);
    }

    private void ShowRecordsTable(Dictionary<string,int> records)
    {
        if (records.Count > 0)
        {
            GameObject table = GameObject.FindGameObjectWithTag("Table");
            
            foreach (KeyValuePair<string, int> r in records )
            {
                ShowRow(r, table);
            }
        }
    }

    private void ShowRow(KeyValuePair<string, int> rowData, GameObject table)
    {
        Vector3 tablePos = table.GetComponent<RectTransform>().position;
        GameObject row = Instantiate(prefabTableRow, tablePos, Quaternion.identity);
        row.transform.SetParent(table.transform);
        row.transform.localScale = prefabTableRow.transform.localScale;
        row.transform.localPosition = new Vector3(0, currentRowPosY, 0);
        row.transform.Find(lineNumTagObj).GetComponent<TMP_Text>().text = currentNumRow++.ToString();
        row.transform.Find(nameTagObj).GetComponent<TMP_Text>().text = rowData.Key;
        row.transform.Find(scoreTagObj).GetComponent<TMP_Text>().text = rowData.Value.ToString();
        currentRowPosY += offsetYpos;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("start_menu");
    }

}
