using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class PopulateBoards : MonoBehaviour {

    GrabLog datadump;
    private void Update()
    {
        if(Input.GetKeyDown("v") == true)
        {
            MissionBoard();
        }
        if (Input.GetKeyDown("r") == true)
        {
            storeJSON();
        }
    }

    void MissionBoard()
    {
        GameObject MainBox;
        MainBox = GameObject.Find("HighlightedBox");
        MainBox.GetComponent<Text>().text = "Hello";

    }



    public void storeJSON()
    {
        string path = Application.dataPath + "/Resources/missionlist.json";

        datadump = GetComponent<GrabLog>();

       string json = JsonConvert.SerializeObject(datadump.ActiveMissionList);
        System.IO.File.WriteAllText(path, json );
    }

}
