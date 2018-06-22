using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GrabLog : MonoBehaviour
{

    public string JournalEndPath;
    private string JournalDump;
    private string SystemDump;
    public string directory;
    public string CurrentUser;
    private DateTime ExpiryTime;
    private string Countdown;
    private Vector3 PlayerLocation;
    private string MissionType;
    private int InSystemMissionsCount;
    public string CurrentTarget;
    public string TargetName;
    public string TargetShip;
    public bool TargetWanted;
    public int TargetBounty;
    public string TargetRank;
    public string TargetDetail;
    public bool MissionTarget = false;
    string Sentiment;
    bool FactionFound;
    public string StarSystem;
    public string remainingTargets;

    private int ActiveMissionCount;

    private int FileNumber = 0;
    private int JournalStartMissions = 0;
    private int JournalLine = 0;
    private int MissionsFound = 0;
    private int LastLineNumber = 0;
    private int FirstFileLastLineNumber = 0;



    public List<DataDump> JournalContents = new List<DataDump>();
    public List<MissionAdd> ActiveMissionList = new List<MissionAdd>();
    public List<MissionEnd> EndedMissionList = new List<MissionEnd>();
    public List<Systems> EDDBData = new List<Systems>();
    public List<KillList> killlist = new List<KillList>();
    public List<PlayerInfo> playerinfo = new List<PlayerInfo>();
    public List<MissionStory> StoryItems = new List<MissionStory>();
    public List<RNGRoles> RNGRoles = new List<RNGRoles>();
    public List<FactionVariables> FactionContacts = new List<FactionVariables>();

    public Transform distance;
    bool MatchedMission;
    public Sprite Active;
    public Sprite Mission;
    public Sprite Blank;
    public Sprite ActiveMission;

    public string MissionPath;

    private void Start()
    {
        MissionPath = Application.dataPath + "/Resources/missionlist.json";
        WindowsVoice.speak("Welcome To The Remlok Intelligent H U D", 0);
        //   GetEDDB();
        //   GetFile();
        Debug.Log("should start updates");
        InvokeRepeating("UpdateLists", 0, 1f);
       
   
    }

    private void Update()
    {
        if (Input.GetKeyDown("w") == true)
        {
            ActiveMissionList = ActiveMissionList.OrderByDescending(x => x.reward).ToList();
       
            Debug.Log("Manual Mission 0 is " + ActiveMissionList[0].LocalisedName);
            Debug.Log("Manual Mission 1 is " + ActiveMissionList[1].LocalisedName);
        }

}

    void UpdateLists()
    {

            Debug.Log("getfile running");
            GetFile();
       
    }

    // Gett EDDB info (only run once during startup)

    void GetEDDB()
    {
  
        StartCoroutine(GetText());
    }
    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://eddb.io/archive/v5/systems_populated.jsonl");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            SystemDump = www.downloadHandler.text;
            string[] systemdata = SystemDump.Split("\n"[0]);
            foreach (string line in systemdata)
            {
                try
                {
                    Systems eddb = JsonConvert.DeserializeObject<Systems>(line);
                    EDDBData.Add(new Systems { name = eddb.name, x = eddb.x, y = eddb.y, z = eddb.z });
                }
                catch (Exception)
                {
                }
            }

        }
        WindowsVoice.speak("Cartographic Data Loaded", 0);

    }


    //One off Read journal files and populate lists

    public void GetJournalData()
    {
      //  Debug.Log("Running Update status is " + JournalLine);
        // Generate file information
        CurrentUser = System.Environment.UserName.ToString();
        directory = "C:/Users/" + CurrentUser + "/Saved Games/Frontier Developments/Elite Dangerous";

        DirectoryInfo dir = new DirectoryInfo(directory);
        FileInfo[] info = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

        JournalEndPath = info[FileNumber].ToString();

        Debug.Log(JournalEndPath);
        // Read file lines using filestream to avoid access issues from readalllines

        FileStream fs = new FileStream(JournalEndPath, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        using (StreamReader read = new StreamReader(fs, true))
        {
            JournalDump = read.ReadToEnd();
        }
    }

    public void GetFile()
    {
        if (new FileInfo(MissionPath).Length == 0)
        {
         //   Debug.Log("File Empty");
        }
        else
        {
            string missiondata = File.ReadAllText(MissionPath);
            ActiveMissionList = JsonConvert.DeserializeObject<List<MissionAdd>>(missiondata);
        }


        GetJournalData();
        string[] JournalData = JournalDump.Split("\n"[0]);
      
        if(FileNumber>0)
        {
            LastLineNumber = 0;
        }
        else
        {
            LastLineNumber = FirstFileLastLineNumber;
        }
        
        for (int j = LastLineNumber; j < JournalData.Length; j++)
        {
            Debug.Log(LastLineNumber + " " + JournalData.Length);
                //Handle extra blank line at end of file
                if (JournalData[j].Length == 0)
                { }
                else
                {
                if (FileNumber == 0)
                {
                    FirstFileLastLineNumber = j;
                }

                // Json Deserialise
                DataDump datadump = JsonConvert.DeserializeObject<DataDump>(JournalData[j]);

                //Start up 0 = no file read


                if (JournalLine > 0)
                {

                    //timestamp check
               //     Debug.Log("Reading Line " + j);

                    // Game load = 1 (when Commander event found)
                    if (JournalLine == 1 && FileNumber == 0)
                    {
                        if (datadump.@event == "Missions")
                        {
                            //Set to 2 when Missions event found
                            Debug.Log("Missions event found " + ActiveMissionList.Count + " Missions found");

                            try
                            {
                                if (JournalStartMissions == 0)
                                {
                                    for (int i = 0; i < datadump.Active.Length; i++)
                                    {
                                        if (datadump.Active[i].Expires > 0)
                                        {
                                            foreach (var File in ActiveMissionList)
                                            {
                                                if(File.MissionID == datadump.Active[i].MissionID)
                                                {
                                                    MatchedMission = true;
                                                }
                                                else
                                                {
                                                    MatchedMission = false;
                                                }
                                            }
                                            if (MatchedMission == false)
                                            {
                                                JournalStartMissions = JournalStartMissions + 1;
                                                ActiveMissionList.Add(new MissionAdd { MissionID = datadump.Active[i].MissionID.ToString() });
                                                MissionsFound = 1;
                                            }
                                        }
                                    }
                                    Debug.Log("Missions event found " + ActiveMissionList.Count + " Missions found");
                                    foreach(var mission in ActiveMissionList)
                                    {
                                        Debug.Log(mission.MissionID + " In list");
                                    }
                                }

                            }
                            catch (Exception)
                            {

                            }

                        }
                    }

                    // Populate accepted missions


                    if (datadump.@event == "FSDJump")
                    {
                        PlayerLocation = new Vector3(float.Parse(datadump.StarPos[0]), float.Parse(datadump.StarPos[1]), float.Parse(datadump.StarPos[2]));
        
                        StarSystem = datadump.StarSystem;
                    }

                    if (datadump.@event == "Bounty")
                    {
                        killlist.Add(new KillList { KillTime = datadump.timestamp, Faction = datadump.VictimFaction });
                    }
                    if (datadump.@event == "Location" && FileNumber == 0)
                    {
                        StarSystem = datadump.StarSystem;
                    }

                    if (datadump.@event == "MissionAccepted")
                    {

                        try
                        {

                            if (datadump.name.StartsWith("Mission_Massacre"))
                            {
                                MissionType = "Kill";
                            }
                            if (datadump.name.StartsWith("Mission_Delivery") || datadump.name.Contains("HelpFinishTheOrder"))
                            {
                                MissionType = "Deliver";
                            }
                            if (datadump.name.StartsWith("Mission_Assass"))
                            {
                                MissionType = "Assassinate";
                                
                            }
                            if (datadump.name.StartsWith("Mission_Courier"))
                            {
                                MissionType = "Deliver Data";
                            }
                            if (datadump.name.StartsWith("Mission_Disable"))
                            {
                                MissionType = "Take out";
                            }
                            if (datadump.name.StartsWith("Mission_Mining"))
                            {
                                MissionType = "Mine";
                            }
                            if (datadump.name.StartsWith("Mission_Collect"))
                            {
                                MissionType = "Source";
                            }
                            if (datadump.name.StartsWith("Mission_Hack"))
                            {
                                MissionType = "Hack";
                            }
                            if (datadump.name.StartsWith("MISSION_Salvage"))
                            {
                                MissionType = "Retrieve";
                            }
                        }
                        catch (Exception)
                        {

                        }

                        if (FileNumber == 0 && ActiveMissionList.Count > 0)
                        {
                            foreach (var File in ActiveMissionList)
                            {
                        
                                if (File.MissionID == datadump.MissionID)
                                {
                                    MatchedMission = true;
                                    break;
                                }
                                else
                                {
                                    MatchedMission = false;
                                }
                            }


                            
                            if (MatchedMission == false)
                            {
                                ActiveMissionList.Add(new MissionAdd { MissionID = datadump.MissionID.ToString() });
                                Debug.Log("Mission number " + datadump.MissionID + "Added");
                            }
     
                        }
                        else if (FileNumber == 0 && ActiveMissionList.Count == 0)
                        {
                            ActiveMissionList.Add(new MissionAdd { MissionID = datadump.MissionID.ToString() });
                            Debug.Log("Mission number " + datadump.MissionID + "Added");
                        }


                        foreach (var mission in ActiveMissionList)
                        {
                         
                            if (mission.MissionID == datadump.MissionID)
                            {
                                Debug.Log(mission.MissionID + " added detail");
                                mission.Reputation = datadump.Reputation;
                                mission.Influence = datadump.Influence;
                                mission.Faction = datadump.Faction;
                                break;
                            }
                        }
                        if (datadump.name.StartsWith("Mission_Altr"))
                        {
                            ActiveMissionList.Add(new MissionAdd { MissionID = datadump.MissionID, LocalisedName = datadump.LocalisedName , Department = "Logistics "});
                        }

                        //****** ASSASSINATE MISSIONS*******
                        else if (datadump.name.StartsWith("Mission_Assass"))

                        {
                            foreach (var mission in ActiveMissionList)
                            {

              
                                if (mission.MissionID == datadump.MissionID)
                                {
             
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.Target = datadump.Target;
                                    mission.TargetType_Localised = datadump.TargetType_Localised;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.DestinationSystem = datadump.DestinationSystem;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.Sentiment = "Neg";
                                    mission.Department = "Security";
                                                                        if(datadump.TargetType_Localised.Contains("pirate") == true)
                                    {
                                        mission.TargetEffect = " increase economic prosperity by stemming the ongoing crime wave ";
                                    }
                                    else
                                    {
                                        mission.TargetEffect = " Avoid a lockdown situation being caused by an upcoming attack by " + datadump.Target + "'s people ";
                                    }
                                    break;
                                }
                            }

                        }
                        //****** MASSACRE MISSIONS*******
                        else if (datadump.name.StartsWith("Mission_Massa"))
                        {

                            foreach (var mission in ActiveMissionList)
                            {
                   
                                if (mission.MissionID == datadump.MissionID)
                                {
           
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.Target = datadump.KillCount.ToString();
                                    mission.TargetType_Localised = datadump.TargetType_Localised;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.DestinationSystem = datadump.DestinationSystem;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.Sentiment = "Neg";
                                    if(datadump.LocalisedName.Contains("Pirates") == true)
                                    {
                                        mission.Department = "Security";
                                        mission.TargetEffect = " reduce the amount of crime sweeping across the system ";
                                    }
                                    else
                                    {
                                        mission.Department = "Military strategy";
                                        mission.TargetEffect = " swing the tides of this war in our favour ";
                                    }
                                    break;
                                }
                            }
                        }
                        else if (datadump.name.StartsWith("Mission_Courier"))

                        {

                            foreach (var mission in ActiveMissionList)

                            {
               
                                if (mission.MissionID == datadump.MissionID)
                                {
                                 //   FoundMissions = FoundMissions + 1;
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.DestinationSystem = datadump.DestinationSystem;
                                    mission.DestinationStation = datadump.DestinationStation;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.Sentiment = "Pos";
                                    mission.Department = "Communications";
                                    mission.TargetEffect = " help establish trade routes and trigger an economic boom";
                                    break;
                                }
                            }

                        }
                        else if (datadump.name.StartsWith("Mission_Deliver") || datadump.name.Contains("HelpFinishTheOrder"))

                        {

                            foreach (var mission in ActiveMissionList)
                            {
                    
                                if (mission.MissionID == datadump.MissionID)
                                {
                                //    FoundMissions = FoundMissions + 1;
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.DestinationSystem = datadump.DestinationSystem;
                                    mission.DestinationStation = datadump.DestinationStation;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.KillCount = datadump.Count;
                                    mission.Target = datadump.Commodity_Localised;
                                    if(datadump.Commodity_Localised == "illegal weapons" || datadump.Commodity_Localised == "illegal drugs" || datadump.Commodity_Localised == "drugs")
                                    {
                                        mission.Sentiment = "Neg";
                                        mission.TargetEffect = "Move this system into an econominc crisis and instigate marshall law ";
                                    }
                                    else if (datadump.Commodity_Localised == "biowaste")
                                    {
                                        mission.Sentiment = "Neg";
                                        mission.TargetEffect = "cause a viral outbreak by crippling their sanitation systems";
                                    }
                                    else
                                    {
                                        mission.Sentiment = "Pos";
                                        mission.TargetEffect = "capitalise on the economic success of the system and build the economy to new levels of prosperity";
                                    }
                                    mission.Department = "Logistics";
                                    break;
                                }
                            }

                        }

                        else if (datadump.name.StartsWith("Mission_Disable"))

                        {
               
                            foreach (var mission in ActiveMissionList)
                            {
                                if (mission.MissionID == datadump.MissionID)
                                {
                                 //   FoundMissions = FoundMissions + 1;
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.Target = datadump.Target_Localised;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.DestinationSystem = "Unknown";
                                    mission.Sentiment = "Neg";
                                    mission.Department = "Military Strategy";
                                    mission.TargetEffect = "cripple their defenses and cause a state of marshal law or even trigger a civil war";
                                    break;
                                }
                            }

                        }
                        else if (datadump.name.StartsWith("Mission_Collect"))

                        {

                            foreach (var mission in ActiveMissionList)
                            {
                 
                                if (mission.MissionID == datadump.MissionID)
                                {
                                  //  FoundMissions = FoundMissions + 1;
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.KillCount = datadump.Count;
                                    mission.TargetType_Localised = datadump.Commodity_Localised;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.Target = datadump.Commodity_Localised;
                                    mission.Sentiment = "Pos";
                                    mission.Department = "Logistics";
                                    mission.TargetEffect = "help maintain the status quo around here";
                                    break;
                                }
                            }

                        }

                        else if (datadump.name.StartsWith("Mission_Mining"))

                        {

                            foreach (var mission in ActiveMissionList)
                            {
                                if (mission.MissionID == datadump.MissionID)
                                {
                                 //   FoundMissions = FoundMissions + 1;
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.DestinationSystem = datadump.DestinationSystem;
                                    mission.DestinationStation = datadump.DestinationStation;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.KillCount = datadump.Count;
                                    mission.Target = datadump.Commodity_Localised;
                                    mission.Sentiment = "Pos";
                                    mission.Department = "Mining Operations";
                                
                                    mission.TargetEffect = "capitalise on the economic success of the system and build the economy to new levels of prosperity";
                                    break;
                                }
                            }

                        }
                        else if (datadump.name.StartsWith("Mission_Hack"))

                        {
  
                            foreach (var mission in ActiveMissionList)
                            {
                                if (mission.MissionID == datadump.MissionID)
                                {
                                 //   FoundMissions = FoundMissions + 1;
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.Target = datadump.Target_Localised;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.DestinationSystem = "Unknown";
                                    mission.Sentiment = "Neg";
                                    mission.Department = "Military Strategy";
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.TargetEffect = "cripple their defenses and cause a state of marshal law or even trigger a civil war";
                                    break;
                                }
                            }

                        }
                        else if (datadump.name.StartsWith("MISSION_Salvage"))

                        {
                            foreach (var mission in ActiveMissionList)
                            {
                                if (mission.MissionID == datadump.MissionID)
                                {
                                 //   FoundMissions = FoundMissions + 1;
                                    mission.AcceptedTime = datadump.timestamp;
                                    mission.LocalisedName = datadump.LocalisedName;
                                    mission.reward = datadump.Reward;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.KillCount = datadump.Count;
                                    mission.Target = datadump.Commodity_Localised;
                                    mission.Expiry = datadump.Expiry;
                                    mission.type = MissionType;
                                    mission.TargetFaction = datadump.TargetFaction;
                                    mission.Sentiment = "Neg";
                                    mission.Department = "Logistics";
                                    mission.TargetEffect = "solve a little problem they have";
                                    break;

                                }
                            }

                        }
                    }

                    if (datadump.@event == "MissionAbandoned" || datadump.@event == "MissionFailed")
                    {
                        EndedMissionList.Add(new MissionEnd { MissionID = datadump.MissionID });
                        Debug.Log("Mission Failed");
                    }
                    if (datadump.@event == "MissionCompleted" )
                    {
                        Debug.Log("Mission Completed!");
                        EndedMissionList.Add(new MissionEnd { MissionID = datadump.MissionID });
                    }
                    if (datadump.@event == "MissionRedirected")
                    {
                        foreach (var mission in ActiveMissionList)
                        {
                            if (datadump.MissionID == mission.MissionID)
                            {
                                mission.DestinationStation = datadump.NewDestinationStation;
                                mission.DestinationSystem = datadump.NewDestinationSystem;
                            }
                        }
                    }

                    if (datadump.@event == "ShipTargeted")
                        if (datadump.TargetLocked == true)
                        {
                            
                                CurrentTarget = datadump.Ship_Localised;
                                TargetName = datadump.PilotName_Localised;
                                TargetRank = datadump.PilotRank;
                                TargetBounty = datadump.Bounty;
                                TargetShip = datadump.Ship_Localised;


                                if (datadump.LegalStatus == "Wanted")
                                {
                                    TargetWanted = true;
                                }
                                else
                                {
                                    TargetWanted = false;
                                }

                                foreach (var mission in ActiveMissionList)
                                {
                                    if (datadump.PilotName_Localised == mission.Target)
                                    {
                                        GameObject MissionTargetNotify;
                                        MissionTargetNotify = GameObject.Find("MissionTargetNotify");
                                        MissionTargetNotify.GetComponent<Text>().text = "Mission Target \n Locked";
                                        MissionTarget = true;
                                        break;
                                    }
                                    else
                                    {
                                        GameObject MissionTargetNotify;
                                        MissionTargetNotify = GameObject.Find("MissionTargetNotify");
                                        MissionTargetNotify.GetComponent<Text>().text = "";
                                    }
                                }
                                GameObject TargetBoard;
                                TargetBoard = GameObject.Find("TargetText");
                                if (TargetWanted == true)
                                {
                                    TargetBoard.GetComponent<Text>().text = "Name: " + TargetName + "\n Ship: " + TargetShip +
                                        "\n Rank: " + TargetRank + "\nStatus: " + datadump.LegalStatus + "\n Bounty: " + TargetBounty;
                                }
                                else
                                {
                                    TargetBoard.GetComponent<Text>().text = "Name: " + TargetName + "\n Ship: " + TargetShip +
                                                                    "\n Rank: " + TargetRank + "\n Status: " + datadump.LegalStatus;
                                }

                            GameObject TargetDetails;
                            TargetDetails = GameObject.Find("CurrentTargetDetails");
                            TargetDetails.GetComponent<Text>().text = "Name: " + TargetName + "\nStatus: " + datadump.LegalStatus;
                        }
                        else
                        {
                            GameObject MissionTargetNotify;
                            MissionTargetNotify = GameObject.Find("MissionTargetNotify");
                            MissionTargetNotify.GetComponent<Text>().text = "";

                            GameObject TargetBoard;
                            TargetBoard = GameObject.Find("TargetText");
                            TargetBoard.GetComponent<Text>().text = "";
                        }
                }
                else
                {
                    if (datadump.@event == "Commander" || datadump.part > 1)
                    {
         
                        LastLineNumber = j;
                        JournalLine = 1;
                    }
                }
                }

        }
    
        ScrollJournals();
    }

    public void ScrollJournals()
    {
        JournalLine = 2;

            for (int item = 0; item < ActiveMissionList.Count; item++)
            {
                if (ActiveMissionList[item].LocalisedName == null)
                {
                    FileNumber = FileNumber + 1;
                    GetFile();
                }
            }

            FileNumber = 0;

            MissionRemoval();
            MissionCleanse();
            KillCountUpdate();
            

    }
    void KillCountUpdate()
    {
        foreach (var item in ActiveMissionList)
        {
            foreach (var kills in killlist)
            {
                if(item.TargetFaction == kills.Faction && item.AcceptedTime < kills.KillTime)
                {
                    item.TotalKills++;

                }
            }
        }
    }

    void MissionRemoval()
    {

        foreach (var item in EndedMissionList)
        {
            //Compare mission ID in active missions to ID's in Ended missions
            MissionAdd matcheditem = ActiveMissionList.Find(x => x.MissionID.Contains(item.MissionID));
            try
            {
                //Remove any completed missions from active mission list
                ActiveMissionList.Remove(matcheditem);
            }
            catch (Exception)
            {

            }
        }
    }

    void MissionCleanse()
        {

        foreach (var sys in ActiveMissionList)
        {
            try
            {
                Systems location = EDDBData.Find(x => x.name.Contains(sys.DestinationSystem));
                sys.Location = new Vector3(float.Parse(location.x), float.Parse(location.y) , float.Parse(location.z));
                float dist = Vector3.Distance(sys.Location, PlayerLocation);
                sys.distance = dist;
            }
            catch(Exception)
            {
                
            }
        }

        string path = Application.dataPath + "/Resources/Story.json";
        string StoryBlocks = File.ReadAllText(path);
        StoryItems = JsonConvert.DeserializeObject<List<MissionStory>>(StoryBlocks);

        string Rolepath = Application.dataPath + "/Resources/Roles.json";
        string Roles = File.ReadAllText(Rolepath);
        RNGRoles = JsonConvert.DeserializeObject<List<RNGRoles>>(Roles);

        string Factionpath = Application.dataPath + "/Resources/Factions.json";
        string Factions = File.ReadAllText(Factionpath);
        if (Factions != "")
        {
            FactionContacts = JsonConvert.DeserializeObject<List<FactionVariables>>(Factions);
        }
        GameObject MissionListDisplay;
        MissionListDisplay = GameObject.Find("MissionText");
        MissionListDisplay.GetComponent<Text>().text = "";

        

        for (int Mission = 0; Mission < ActiveMissionList.Count; Mission++)
        {
            if (ActiveMissionList[Mission].Story == null)
            {
                int Opening = UnityEngine.Random.Range(0, StoryItems.Count);
                int MissionGoal = UnityEngine.Random.Range(0, StoryItems.Count);
                int FactionGoal = UnityEngine.Random.Range(0, StoryItems.Count);
                int MissionSentimentPos = UnityEngine.Random.Range(0, StoryItems.Count);
                int MissionSentimentNeg = UnityEngine.Random.Range(0, StoryItems.Count);
                int NegEventsRand = UnityEngine.Random.Range(0, StoryItems.Count);
             //   int JobRole = UnityEngine.Random.Range(0, RNGRoles.Count);

                if (ActiveMissionList[Mission].Sentiment == "Pos")
                {
                    Sentiment = StoryItems[MissionSentimentPos].MissionSentimentPositive;
                }
                else
                {
                    Sentiment = StoryItems[MissionSentimentNeg].MissionSentimentNegative;
                }

                string story = StoryItems[Opening].Opening + StoryItems[MissionGoal].MissionGoal + StoryItems[FactionGoal].FactionGoal + Sentiment;
                story = story.Replace("-mission type-", ActiveMissionList[Mission].type);
                story = story.Replace("-target-", ActiveMissionList[Mission].Target + " " + ActiveMissionList[Mission].TargetType_Localised);
                story = story.Replace("-mission type-", ActiveMissionList[Mission].type);
                story = story.Replace("-RNG date-", UnityEngine.Random.Range(3250, 3304).ToString());
                story = story.Replace("-effect on target-", ActiveMissionList[Mission].TargetEffect);
                story = story.Replace("-target faction-", ActiveMissionList[Mission].TargetFaction);
                story = story.Replace("-rng negative event-", StoryItems[NegEventsRand].Event);

                try
                {

                    for (int faction = 0; faction < FactionContacts.Count; faction++)
                    {
                        if (FactionContacts[faction].FactionName == ActiveMissionList[Mission].Faction && FactionContacts[faction].ContactDepartment == ActiveMissionList[Mission].Department)
                        {

                            ActiveMissionList[Mission].FactionContact = FactionContacts[faction].FactionContact;
                            ActiveMissionList[Mission].ContactRole = FactionContacts[faction].ContactRole;
                            break;
                        }
                    }
               
                }
                catch (Exception) { }
                if(ActiveMissionList[Mission].FactionContact == null)
                {
                      GenerateFactionContact(ActiveMissionList[Mission].Department, ActiveMissionList[Mission].Faction);

                    ActiveMissionList[Mission].FactionContact = FactionContacts[FactionContacts.Count].FactionContact;
                    ActiveMissionList[Mission].ContactRole = FactionContacts[FactionContacts.Count].ContactRole;
                }
                                
                story = story.Replace("-RNG name-", ActiveMissionList[Mission].FactionContact);
                    story = story.Replace("-employing faction-", ActiveMissionList[Mission].Faction);
                    story = story.Replace("-RNG role-", ActiveMissionList[Mission].ContactRole + ActiveMissionList[Mission].Department);

                ActiveMissionList[Mission].Story = story;
            }
            if (ActiveMissionList[Mission].KillCount == 0)
            {
                MissionListDisplay.GetComponent<Text>().text = MissionListDisplay.GetComponent<Text>().text + ActiveMissionList[Mission].type + " " + ActiveMissionList[Mission].Target + "\n";
            }
            else
            {
                MissionListDisplay.GetComponent<Text>().text = MissionListDisplay.GetComponent<Text>().text + ActiveMissionList[Mission].type + " " + ActiveMissionList[Mission].KillCount + " " + ActiveMissionList[Mission].Target + "\n";
            }

            Debug.Log("Mission " + Mission + " is " + ActiveMissionList[Mission].LocalisedName);
            Debug.Log("Manual Mission 0 is " + ActiveMissionList[0].LocalisedName);
            Debug.Log("Manual Mission 1 is " + ActiveMissionList[1].LocalisedName);

            if (GetComponent<VoiceRecognitionSystem>().MissionSelected == true)
            {
                GameObject NextSteps;
                NextSteps = GameObject.Find("ActiveMissionNextStep");
                int ActiveMissionNumber = GetComponent<VoiceRecognitionSystem>().ActiveMission;

                if (StarSystem == ActiveMissionList[ActiveMissionNumber].DestinationSystem)
                {
                    if (ActiveMissionList[ActiveMissionNumber].DestinationStation != null)
                    {
                        NextSteps.GetComponent<Text>().text = "Go to " + ActiveMissionList[ActiveMissionNumber].DestinationStation;
                    }
                    else
                    {
                        if (ActiveMissionList[ActiveMissionNumber].KillCount == 0)
                        {
                            remainingTargets = null;
                        }
                        else
                        {
                            int remainingTargetMath = ActiveMissionList[ActiveMissionNumber].KillCount - ActiveMissionList[ActiveMissionNumber].TotalKills;
                            remainingTargets = remainingTargetMath.ToString();
                        }
                        NextSteps.GetComponent<Text>().text = ActiveMissionList[ActiveMissionNumber].type + " " + remainingTargets + " " + ActiveMissionList[ActiveMissionNumber].Target;
                    }
                }
                else
                {
                    NextSteps.GetComponent<Text>().text = "Go to " + ActiveMissionList[ActiveMissionNumber].DestinationSystem;
                        }
            }
        }
        GetComponent<PopulateBoards>().storeJSON();
    }

    public void GenerateFactionContact(string Department, string Faction)
    {
        GetComponent<GetRandomName>().GetName();

        string name = GetComponent<GetRandomName>().FirstName + " " + GetComponent<GetRandomName>().LastName;

        string Factionpath = Application.dataPath + "/Resources/Factions.json";
        string[] WatsonVoices = new string[] { "Allison", "Lisa" };
   

        FactionContacts.Add(new FactionVariables
        {
            FactionContact = name,
            ContactDepartment = Department,
            ContactRole = RNGRoles[UnityEngine.Random.Range(0, RNGRoles.Count)].JobTitle,
            FactionName = Faction,
        });

        string FactionJSON = JsonConvert.SerializeObject(FactionContacts);
        File.WriteAllText(Factionpath, FactionJSON);
    }

    //public void Reward()
    //{
    //    ActiveMissionList = ActiveMissionList.OrderByDescending(x => x.reward).ToList();
    //    UpdateMissionList();
    //}

    //public void Distance()
    //{
    //    ActiveMissionList = ActiveMissionList.OrderBy(x => x.distance).ToList();
    //    UpdateMissionList();
    //}
    //public void Time()
    //{
    //    ActiveMissionList = ActiveMissionList.OrderBy(x => x.Expiry).ToList();
    //    UpdateMissionList();
    //}

    //public void UpdateMissionList()
    //{
    //    GameObject MissionDetails;
    //    GameObject MissionImage;
    //    GameObject MissionActive;
    //    GameObject InSystemMissions;
    //    // Debug.Log(ActiveMissionCount);
    //    ActiveMissionCount = ActiveMissionList.Count;

    //    if (ActiveMissionCount < 3)
    //    {
    //        for (int i = ActiveMissionCount; i < 3; i++)
    //        {

    //            MissionDetails = GameObject.Find("Mission" + i);
    //            MissionImage = GameObject.Find("MissionImage" + i);
    //            MissionImage.GetComponent<Image>().overrideSprite = Blank;
    //            MissionDetails.GetComponent<Text>().text = "No mission available";
    //        }
    //    }
        
    //    if (ActiveMissionCount > 3)
    //    {
    //        ActiveMissionCount = 3;
    //    }

    //    for (int i = 0; i < ActiveMissionCount; i++)
    //    {
    //        TimeSpan countdown = ActiveMissionList[i].Expiry - DateTime.Now;
            
    //        MissionDetails = GameObject.Find("Mission" + i);
    //        MissionImage = GameObject.Find("MissionImage" + i);
    //        MissionActive= GameObject.Find("MissionActive" + i);
    //        Active = Resources.Load<Sprite>("Active");
    //        Blank = Resources.Load<Sprite>("Blank");
           

    //        if (ActiveMissionList[i].active == true)
    //        {
    //            MissionActive.GetComponent<Image>().overrideSprite = Active;
    //        }
    //        else
    //        {
    //            MissionActive.GetComponent<Image>().overrideSprite = Blank;
    //        }

    //        Mission = Resources.Load<Sprite>(ActiveMissionList[i].type);
    //        MissionImage.GetComponent<Image>().overrideSprite = Mission;

    //        MissionDetails.GetComponent<Text>().text = ActiveMissionList[i].type +  " " + ActiveMissionList[i].Target + " " +ActiveMissionList[i].TargetType_Localised +
    //            " System: " + ActiveMissionList[i].DestinationSystem + " - " + ActiveMissionList[i].DestinationStation + " " + ActiveMissionList[i].distance.ToString("f1") + " ly " +
    //            "\n"+ ActiveMissionList[i].reward.ToString("n0") + "cr " + countdown.Days.ToString() + " days " + countdown.Hours.ToString() +" hrs " + countdown.Minutes.ToString() + " Minutes "+ countdown.Seconds.ToString() + " Secs remaining" +
    //            "\nRep = " + ActiveMissionList[i].Reputation + " Inf = " + ActiveMissionList[i].Influence;
    //   //     Debug.Log("Popup Status - " + MissionPopup + " Mission Diatance - " + ActiveMissionList[i].distance);
    //        if (ActiveMissionList[i].distance == 0 && MissionPopup == 0 && ActiveMissionList[i].DestinationSystem != "Unknown")
    //        {
    //            InSystemMissionsCount = InSystemMissionsCount + 1;
    //        }
    //    }
    //    InSystemMissions = GameObject.Find("InSystemMissions");
    //    MissionPopup = 1;
    //    InSystemMissions.GetComponent<Text>().text = "There are " + InSystemMissionsCount + " Missions in this system";
    //    SetActiveMission();
    //}

    //public void SetActiveMission()
    //{
    //    GameObject ActiveMissionDetails;
    //    GameObject ImageActive;

    //    ActiveMissionDetails = GameObject.Find("ActiveMissionDetails");
    //    ImageActive = GameObject.Find("ImageActive");

    //    foreach (var mission in ActiveMissionList)
    //    {
    //        if(mission.active == true)
    //        {
     
    //            ActiveMission = Resources.Load<Sprite>(mission.type);
    //            ImageActive.GetComponent<Image>().overrideSprite = ActiveMission;

    //            if (mission.type == "Kill")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = mission.KillCount - mission.TotalKills + "/" + mission.KillCount +
    //                    "\n" + mission.TargetType_Localised + " " + mission.TargetFaction;
    //            }
    //            if (mission.type == "Assassinate")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Target system " + mission.DestinationSystem +
    //                 "\n Kill " + mission.Target + " " + mission.TargetType_Localised;
    //            }
    //            if (mission.type == "Deliver Data")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Deliver data to the " + mission.DestinationStation + " Station" +
    //                 "\n In the " + mission.DestinationSystem + " system for the " + mission.TargetFaction;
    //            }
    //            if (mission.type == "Deliver")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Deliver " + mission.KillCount + " " + mission.TargetType_Localised +
    //                 "\n to " + mission.DestinationStation + " station for the " + mission.TargetFaction;
    //            }
    //            if (mission.type == "Disable")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Disable the " + mission.TargetType_Localised;
    //            }
    //            if (mission.type == "Hack")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Hack the " + mission.TargetType_Localised;
    //            }
    //            if (mission.type == "Mine")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Mine " + mission.KillCount + " of " + mission.TargetType_Localised +
    //                "\n and deliver to " + mission.DestinationStation + " station for the " + mission.TargetFaction;
    //            }
    //            if (mission.type == "Source")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Acquire " + mission.KillCount + " of " + mission.TargetType_Localised +
    //                "\n and deliver to " + mission.DestinationStation + " station for the " + mission.TargetFaction;
    //            }
    //            if (mission.type == "Salvage")
    //            {
    //                ActiveMissionDetails.GetComponent<Text>().text = "Salvage " + mission.KillCount + " of " + mission.TargetType_Localised +
    //                "\n and deliver to " + mission.DestinationStation + " station for the " + mission.Faction;
    //            }
    //        }
    //    }

        

    //}

}