/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;

public class WatsonTTS : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [SerializeField]
    private string _username;
    [SerializeField]
    private string _password;
    [SerializeField]
    private string _url;
    #endregion

    TextToSpeech _textToSpeech;
    public string _SpeechText; 
    string _createdCustomizationId;
    CustomVoiceUpdate _customVoiceUpdate;
    string _customizationName = "unity-example-customization";
    string _customizationLanguage = "en-US";
    string _customizationDescription = "A text to speech voice customization created within Unity.";
    string _testWord = "Watson";
    public string Voice;


    private bool _synthesizeTested = false;
  //  private bool _getVoicesTested = false;
  //  private bool _getVoiceTested = false;
  //  private bool _getPronuciationTested = false;
  //  private bool _getCustomizationsTested = false;
  //  private bool _createCustomizationTested = false;
  //  private bool _deleteCustomizationTested = false;
  //  private bool _getCustomizationTested = false;
  //  private bool _updateCustomizationTested = false;
    //private bool _getCustomizationWordsTested = false;
    //private bool _addCustomizationWordsTested = false;
  //  private bool _deleteCustomizationWordTested = false;
  //  private bool _getCustomizationWordTested = false;

    void Start()
    {
        LogSystem.InstallDefaultReactors();

        //  Create credential and instantiate service
        Credentials credentials = new Credentials(_username, _password, _url);

        _textToSpeech = new TextToSpeech(credentials);
        Debug.Log("TTS Online");

     //   Runnable.Run(Examples());
    }

    public void WatsonSpeech()
    {
        Runnable.Run(Examples());
    }

    private IEnumerator Examples()
    {
        //  Synthesize

            if(Voice == "Allison")
        {
            _textToSpeech.Voice = VoiceType.en_US_Allison;
        }
        if (Voice == "Lisa")
        {
            _textToSpeech.Voice = VoiceType.en_US_Lisa;
        }
        if (Voice == "Michael")
        {
            _textToSpeech.Voice = VoiceType.en_US_Michael;
        }


        _textToSpeech.ToSpeech(HandleToSpeechCallback, OnFail, _SpeechText, true);
        while (!_synthesizeTested)
            yield return null;

    }

    void HandleToSpeechCallback(AudioClip clip, Dictionary<string, object> customData = null)
    {
        PlayClip(clip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
           // GameObject audioObject = new GameObject("AudioObject");
            AudioSource source = GetComponent<AudioSource>();
            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();

            _synthesizeTested = true;
        }
    }

   

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleTextToSpeech.OnFail()", "Error received: {0}", error.ToString());
    }
}
