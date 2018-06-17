

using UnityEngine;
using UnityEngine.UI;
using WindowsInput;
using WindowsInput.Native;



public class VoiceRecognitionSystem : MonoBehaviour
{
    public ActiveMissionDetails _activeMissionDetails;

    public string Result;  

    public string[] Commands;
    public string[] Response;
    public string[] Variable;

    public string ChatText;
    public string VariableText;
    public int ActiveMission = 0;

    private bool ChatModeToggle = false;


    InputSimulator IS;
 
    // Use this for initialization
    void Start()
    {
        IS = new InputSimulator();
    }



    public void RecognitionSystem()
    {
        GameObject LiveText;
        LiveText = GameObject.Find("ActiveMissionDetails");
        LiveText.GetComponent<Text>().text = Result;

        if (ChatModeToggle == true)
        {

          foreach (char c in Result)
            {
                string ChatText = c.ToString();
                if (ChatText == " ")
                {
                    IS.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                }
                else
                {
                    VirtualKeyCode ChatLetter = (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), "VK_" + ChatText.ToUpper());
                    IS.Keyboard.KeyPress(ChatLetter);
                    ChatModeToggle = false;
                }
            }
          //  GetComponent<WatsonSTT>().StopRecording();
        }

        else
        {
            for (int i = 0; i < Commands.Length; i++)
            {
                Debug.Log("Text = " + Result + " Command = " + Commands[i]);
                if (Result.Contains(Commands[i].ToString()) == true)
                {
                    Invoke(Response[i], 0f);
                    VariableText = Variable[i];
                 //   GetComponent<WatsonSTT>().StopRecording();
                 //   GetComponent<WatsonSTT>().PTT = false;
                }
            }

        }

    }

    void Menu()
    {

        GameObject Menu;
        Menu = GameObject.Find("MenuSystem");
        Menu.GetComponent<MenuAnimations>().ToggleMenu();
        Debug.Log("ShowingMissions");
     //   GetComponent<WatsonSTT>().StopRecording();
     //   GetComponent<WatsonSTT>().PTT = false;
    }
    void HighlightBox()
    {

        GameObject Menu;
        Menu = GameObject.Find("MenuSystem");
        Menu.GetComponent<MenuAnimations>().HighlightBox(VariableText);
     //   GetComponent<WatsonSTT>().StopRecording();
     //   GetComponent<WatsonSTT>().PTT = false;
    }
    void ChatMode()
    {
        ChatModeToggle = true;
        IS.Keyboard.KeyDown(VirtualKeyCode.VK_A);
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

    void MissionSpeech()
    {
        GetComponent<WatsonTTS>()._SpeechText = 
            "<speak version=\"1.0\">" +
            "<voice-transformation type=\"Soft\" " +
            "pitch=\"" + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].WatsonPitch + "%\" " +
            "pitch_range=\"" + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].WatsonPitch_Range + "%\" " +
            "rate=\"" + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].WatsonRate + "%\" " +
            "breathiness=\"" + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].WatsonBreathiness + "%\" " +
            "glottal_tension=\" " + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].WatsonTension + "%\"  " +
            "strength=\"" + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].WatsonStrength + "%\">" +
             "\"" + GetComponent<GrabLog>().ActiveMissionList[ActiveMission].Story +  "%\"" +
            "</voice-transformation></speak>";
        Debug.Log(GetComponent<WatsonTTS>()._SpeechText);

        GetComponent<WatsonTTS>().Voice = GetComponent<GrabLog>().ActiveMissionList[ActiveMission].WatsonVoice;
        GetComponent<WatsonTTS>().WatsonSpeech();
    }



}

