
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionEngine : MonoBehaviour
{
    public string[] keyword = new string[] { "Remlok" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    public string SpokenText;

    //protected DictationRecognizer recognizer;
    protected PhraseRecognizer trigger;
   // protected SpeechRecognitionEngine trigger;
        
        //PhraseRecognizer recognizer;
    string word;

    private void Start()
    {
        
        Application.runInBackground = true;
        if (keyword != null)
        {
            trigger = new KeywordRecognizer(keyword, confidence);
            trigger.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            trigger.Start();
        }
        
    }

    public void KeywordListener()
    {
        PhraseRecognitionSystem.Restart();
    }

public void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        TriggerEvent();
    }


    void TriggerEvent()
    {
        WindowsVoice.speak("Yes Commander", 0);
     //   PhraseRecognitionSystem.Shutdown();
     //   GetComponent<VoiceRecognitionSystem>().VoiceListening();
        
    }
    private void OnApplicationQuit()
    {
        if (trigger != null && trigger.IsRunning)
        {
            trigger.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            trigger.Stop();
        }
    }
        
}
