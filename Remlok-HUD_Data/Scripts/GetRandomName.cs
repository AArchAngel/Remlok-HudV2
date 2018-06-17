
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GetRandomName : MonoBehaviour {

    string[] FirstNameRandom;
    string[] LastNameRandom;
    public string FirstName;
    public string LastName;

    private void Update()
    {
        if(Input.GetKeyDown("p") == true)
        {
            RolesScript();
        }
    }

    public void GetName () {

        string pathFirstName = Application.dataPath + "/Resources/firstnames.txt";
        string pathSurname = Application.dataPath + "/Resources/names.txt";

        FirstNameRandom = File.ReadAllLines(pathFirstName);
        LastNameRandom = File.ReadAllLines(pathSurname);

        FirstName = FirstNameRandom[Random.Range(0, FirstNameRandom.Length)];
        LastName = LastNameRandom[Random.Range(0, LastNameRandom.Length)];
    }

    public List<MissionStory> MissionStory = new List<MissionStory>();
    string StoryJSON;
    void MissionStoryScript()
    {
        string path = Application.dataPath + "/Resources/Story.json";

        MissionStory.Add(new MissionStory { Event = "Pirate attacks", Opening = "Commander, my name is -RNG name-, -RNG role-  -department- and I represent the -employing faction-. Thanks for accepting our proposal I asked for someone as this represents my area.", MissionGoal = "As you are aware we need you to -mission type- -target-", FactionGoal = "We have been working out of -mission giver station- to -effect on target- the -target faction-", MissionSentimentNegative = "-target faction- won't know whats going to hit them they have been working against us for too long", MissionSentimentPositive = "-target faction- have always been close allies of ours and this re affirms our commitment to helping them." });
        MissionStory.Add(new MissionStory { Event = "multiple assassination attempts against our leadership", Opening = "My name is -RNG name- and I represent the -employing Faction- it's good to have you on board. I'm not the face of the organisation you may have spoken to, we use them as covers for our real operations.", MissionGoal = "You have been made aware that we need to -mission type- -Target-", FactionGoal = "My team have been trying to -effect on target- the -target faction- for some time now and we think this -mission type- will help", MissionSentimentNegative = "They will finally pay for their crimes they headed up a campaign back in -RNG date- we have been rebuilding and waiting for the perfect opportunity", MissionSentimentPositive = "We share some common goals with -target faction- and would like to help them in -effect on target-" });
        MissionStory.Add(new MissionStory { Event = "internal ambush", Opening = "Hello, you dont need to know my name but you do need to know that I work closely with the leaders of -employing faction-.", MissionGoal = "we need you to take care of something for us -target- needs -mission type-ing", FactionGoal = "The -target faction- are in a position where our plans to -effect on target- them can finally start moving forward", MissionSentimentNegative = "It's well known there is friction between our operations we are looking to get the upper hand here.", MissionSentimentPositive = "during some more troubled times we faced in -RNG date- when we came under pressure from -rng negative event- they helped us out" });

        StoryJSON = JsonConvert.SerializeObject(MissionStory);
        File.WriteAllText(path, StoryJSON);
    }


    public List<RNGRoles> JobRoles = new List<RNGRoles>();
    string RoleJSON;
    void RolesScript()
    {
        string Rolepath = Application.dataPath + "/Resources/Roles.json";

        JobRoles.Add(new RNGRoles { JobTitle = "Head of " });
        JobRoles.Add(new RNGRoles { JobTitle = "Director of " });
        JobRoles.Add(new RNGRoles { JobTitle = "Leader of " });
        JobRoles.Add(new RNGRoles { JobTitle = "Chief Officer in " });
        JobRoles.Add(new RNGRoles { JobTitle = "Lead Adviser for " });
        JobRoles.Add(new RNGRoles { JobTitle = "Senior Manager of  " });
        JobRoles.Add(new RNGRoles { JobTitle = "Overseer for  " });

        RoleJSON = JsonConvert.SerializeObject(JobRoles);
        File.WriteAllText(Rolepath, RoleJSON);
    }
}
