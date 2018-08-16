

using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class VoiceRecognitionSystem : MonoBehaviour
{
     public string Result;  

    public string Commands;
    public string Response;
    public string Variable;
    string FactionName;

    public string ChatText;
    public string VariableText;
    public int ActiveMission = 0;
    public bool MissionSelected = false;
 
    void Update()
    {

        string[] VACommand = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low/Remlok/Intelligent HUD/VoiceAttackCommand.txt");

        if (VACommand.Length == 0)
        {

        }
        else
        {
            Response = VACommand[1];
            VariableText = VACommand[2];
            Invoke(Response, 0f);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low/Remlok/Intelligent HUD/VoiceAttackCommand.txt", "");

        }
    }

    void Menu()
    {

        GameObject Menu;
        Menu = GameObject.Find("MenuSystem");
        Menu.GetComponent<MenuAnimations>().ToggleMenu();
        Debug.Log("ShowingMissions");
    }
    void HighlightBox()
    {
        GameObject Menu;
        Menu = GameObject.Find("MenuSystem");
        Menu.GetComponent<MenuAnimations>().HighlightBox(VariableText);
    }

    void SelectMission()
    {
        GameObject Menu;
        Menu = GameObject.Find("MenuSystem");

        if (int.Parse(VariableText) > GetComponent<GrabLog>().ActiveMissionList.Count)
        {
            Menu.GetComponent<MenuAnimations>().SelectMission(int.Parse(VariableText), true);
        }
        else
        {
            Menu.GetComponent<MenuAnimations>().SelectMission(int.Parse(VariableText), false);
            ActiveMission = int.Parse(VariableText);
        }
    }


    //void SelectFaction()
    //{
    //    GameObject Menu;
    //    Menu = GameObject.Find("MenuSystem");
    //    int FactionCount=0;
    //    foreach (var faction in GetComponent<GrabLog>().FactionSystems)
    //    {
    //        if (faction.SystemName == GetComponent<GrabLog>().StarSystem)
    //        {
    //            FactionCount++;
    //            if(FactionCount == int.Parse(VariableText))
    //                {
    //                FactionName = faction.FactionName;
    //                }   
    //        }
    //    }
    //    if (int.Parse(VariableText) > FactionCount)
    //    {
    //        Menu.GetComponent<MenuAnimations>().SelectFaction("", true);
    //    }
    //    else
    //    {
    //        Menu.GetComponent<MenuAnimations>().SelectFaction(FactionName, false);
    //        ActiveMission = int.Parse(VariableText);
    //    }
    //}

    public void MissionSpeech()
   {
        WindowsVoice.speak("retrieving message...   message Reads: " + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].Story, 0);
   }
    
    public void OrderMissions()
    {

        if (VariableText == "Reward")
        {
            GetComponent<GrabLog>().ActiveMissionList = GetComponent<GrabLog>().ActiveMissionList.OrderByDescending(x => x.reward).ToList();
        }
        if (VariableText == "Distance")
        {
            GetComponent<GrabLog>().ActiveMissionList = GetComponent<GrabLog>().ActiveMissionList.OrderBy(x => x.distance).ToList();
        }
        if (VariableText == "Time")
        {
            GetComponent<GrabLog>().ActiveMissionList = GetComponent<GrabLog>().ActiveMissionList.OrderBy(x => x.Expiry).ToList();
        }

    }



}

