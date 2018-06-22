

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class VoiceRecognitionSystem : MonoBehaviour
{
     public string Result;  

    public string Commands;
    public string Response;
    public string Variable;

    public string ChatText;
    public string VariableText;
    public int ActiveMission = 0;
    public bool MissionSelected = false;

    private bool ChatModeToggle = false;

 
    void Update()
    {
        //GameObject LiveText;
        //LiveText = GameObject.Find("ActiveMissionDetails");
        //LiveText.GetComponent<Text>().text = Result;

        //    for (int i = 0; i < Commands.Length; i++)
        //    {
        //        Debug.Log("Text = " + Result + " Command = " + Commands[i]);
        //        if (Result.Contains(Commands[i].ToString()) == true)
        //        {

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

            //    }
            //}
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
    void ChatMode()
    {
        ChatModeToggle = true;
        WindowsVoice.speak("Chat Mode", 0);
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

