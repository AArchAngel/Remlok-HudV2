using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetWatsonCredentials : MonoBehaviour {

    public string FilePath = "Desktop/WatsonCredentials.JSON";
    string FileContents;

    void Start ()
            {
        FileStream fs = new FileStream(FilePath, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        using (StreamReader read = new StreamReader(fs, true))
        {
            FileContents = read.ReadToEnd();
        }
        UserSettingsJSONClass settings = JsonConvert.DeserializeObject<UserSettingsJSONClass>(FileContents);
        GetComponent<WatsonSTT>()._username = settings.STTWatsonUsername;
        GetComponent<WatsonSTT>()._password = settings.STTWatsonPassword;
        GetComponent<WatsonSTT>()._url = settings.STTWatsonURL;
    }
}
