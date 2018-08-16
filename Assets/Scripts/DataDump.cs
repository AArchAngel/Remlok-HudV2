using System;
using UnityEngine;
[System.Serializable]
public class DataDump
{
    public DateTime timestamp { get; set; }
    public string @event { get; set; }
    public string Commander { get; set; }
    public string LocalisedName { get; set; }
    public string TargetType_Localised { get; set; }
    public string Target_Localised { get; set; }
    public string TargetFaction { get; set; }
    public string Faction { get; set; }
    public int KillCount { get; set; }
    public string DestinationSystem { get; set; }
    public int Reward { get; set; }
    public string MissionID { get; set; }
    public string name { get; set; }
    public string Target { get; set; }
    public DateTime Expiry { get; set; }
    public string VictimFaction { get; set; }
    public string[] StarPos { get; set; }
    public JournalArrays[] Active { get; set; }
    public FactionArrays[] Factions { get; set; }
    public int Count { get; set; }
    public string Commodity_Localised { get; set; }
    public string DestinationStation { get; set; }
    public int PassengerCount { get; set; }
    public bool PassengerVIPs { get; set; }
    public bool PassengerWanted { get; set; }
    public string PassengerType { get; set; }
    public string Influence { get; set; }
    public string Reputation { get; set; }
    public int part { get; set; }
    public string NewDestinationStation { get; set; }
    public string NewDestinationSystem { get; set; }
    public string PilotName_Localised { get; set; }
    public string Ship_Localised { get; set; }
    public string PilotRank { get; set; }
    public int Bounty { get; set; }
    public string LegalStatus { get; set; }
    public bool TargetLocked { get; set; }
    public string StarSystem { get; set; }
    public string SystemFaction { get; set; }
    public string SystemEconomy_Localised { get; set; }
    public string SystemSecurity_Localised { get; set; }
    public string SystemGovernment_Localised { get; set; }
    public string Population { get; set; }
}
[System.Serializable]
public class JournalArrays
{ 
    public string MissionID { get; set; }
    public string Name { get; set; }
    public int Expires { get; set; }
}

public class FactionArrays
{
    public string Name { get; set; }
    public string FactionState { get; set; }
    public string Government { get; set; }
    public float Influence { get; set; }
    public string Allegiance { get; set; }
    public StateArrays[] Pendingstates { get; set; }
    public StateArrays[] Recoveringstates { get; set; }
}

public class StateArrays
{
    public string State { get; set; }
    public string Trend { get; set; }
}

public class MissionAdd
{
    public DateTime AcceptedTime;
    public string MissionID;
    public bool MissionActive = true;
    public int reward;
    public string LocalisedName;
    public string TargetType_Localised;
    public string TargetFaction;
    public string Faction;
    public string Reputation;
    public string Influence;
    public int KillCount;
    public string KillCountstring;
    public string Target;
    public string DestinationSystem;
    public DateTime Expiry;
    public string Countdown;
    public float distance;
    public Vector3 Location;
    public string x;
    public string y;
    public string z;
    public int TotalKills = 0;
    public string type;
    public bool active = false;
    public int Count;
    public string Commodity;
    public string DestinationStation;
    public int PassengerCount;
    public bool PassengerVIPs;
    public bool PassengerWanted;
    public string PassengerType;
    public string Story;
    public string Sentiment;
    public string TargetEffect;
    public string Department;
    public string FactionContact;
    public string ContactRole;

}
public class MissionEnd
{
    public string MissionID;
}

public class PlayerInfo
{
    public string Location;
    public string X;
    public string Y;
    public string Z;
    public Vector3 PlayerLocation;
    public string PlayerLocation1;
    public string Name;
    public string Credits;
}

public class Systems
{
    public string name;
    public string x;
    public string y;
    public string z;
}

public class KillList
{
    public string Faction;
    public DateTime KillTime;
}


public class MissionStory
{
    public string Opening;
    public string MissionGoal;
    public string FactionGoal;
    public string MissionSentimentPositive;
    public string MissionSentimentNegative;
    public string Event;

}

public class FactionVariables
{
    public string FactionName;
    public string FactionContact;
    public string ContactRole;
    public string ContactDepartment;
    public string State;
    public string Allegience;
    public string PlayerImpact;
}

public class FactionSystems
{
    public string FactionName;
    public string SystemName;
    public float Influence;
}

public class RNGNames
{
    public string FirstName;
    public string Surname;
}

public class RNGRoles
{
    public string JobTitle;
}

public class Commodities
{
    public int id;
    public string name;
}

public class Markets
{
    public string Station_id;
    public string Commodity_id;
    public string Supply;
    public string buy_price;
    public string sell_price;
}

