
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimations : MonoBehaviour {

    Animator m_AnimatorBoxes;
    Animator m_AnimatorLogo;
    public bool IsActive = false;
    public bool BoxHighlighted = false;
   

    private void Start()
    {
        GameObject MenuBoxes;
        GameObject Logo;
        Logo = GameObject.Find("Remlok");
        MenuBoxes = GameObject.Find("MenuBoxes");
        m_AnimatorBoxes = MenuBoxes.GetComponent<Animator>();
        m_AnimatorLogo = Logo.GetComponent<Animator>();

    }


    private void Update()
    {
        if(Input.GetKeyDown("n") == true)
        {
            ToggleMenu();
        }
        if (Input.GetKeyDown("m") == true)
        {
            HighlightBox("Missions");
        }
        if (Input.GetKeyDown("f") == true)
        {
   //         HighlightBox("Factions");
        }
        if (Input.GetKeyDown("s") == true)
        {
            HighlightBox("System");
        }
        if (Input.GetKeyDown("t") == true)
        {
            HighlightBox("Target");
        }

            if (Input.GetKeyDown("1") == true)
            {
                SelectMission(0, false);
            }
        if (Input.GetKeyDown("2") == true)
        {
            SelectMission(1, false);
        }

        if (Input.GetKeyDown("3") == true)
        {
            SelectMission(2, false);
        }



    }

    public void ToggleMenu () {

            if (IsActive == false)
            {
                m_AnimatorBoxes.SetBool("MenuActive", true);
                m_AnimatorLogo.SetBool("MenuActive", true);
            m_AnimatorBoxes.SetBool("BoxHighlighted", false);
            m_AnimatorLogo.SetBool("BoxHighlighted", false);
            BoxHighlighted = false;
            IsActive = true;
            }
            else
            {
                m_AnimatorBoxes.SetBool("MenuActive", false);
                m_AnimatorLogo.SetBool("MenuActive", false);
                IsActive = false;
            }
    }
    public void HighlightBox(string variable)
    {
        GameObject Mission;
        GameObject DetailText;
        GameObject Grablog;


        Grablog = GameObject.Find("GameObject");
        Mission = GameObject.Find("MissionsHeader");
        DetailText = GameObject.Find("HighlightedBox");

        if (variable != "")
        {
            Mission.GetComponent<Text>().text = variable;
            if (variable == "Missions")
            {
                DetailText.GetComponent<Text>().text = Grablog.GetComponent<GrabLog>().ActiveMissionList[0].Story;
            }
        }


        if (BoxHighlighted == false)
        {
            m_AnimatorBoxes.SetBool("BoxHighlighted", true);
            m_AnimatorLogo.SetBool("BoxHighlighted", true);
            m_AnimatorBoxes.SetBool("MenuActive", false);
            m_AnimatorLogo.SetBool("MenuActive", false);
            IsActive = false;
            BoxHighlighted = true;



        }
        else
        {
            if (variable == "")
            {
                m_AnimatorBoxes.SetBool("BoxHighlighted", false);
            m_AnimatorLogo.SetBool("BoxHighlighted", false);
            m_AnimatorBoxes.SetBool("MenuActive", true);
            m_AnimatorLogo.SetBool("MenuActive", true);

                BoxHighlighted = false;
                IsActive = true;
            }

        }
    }

    public void SelectMission(int MissionNumber, bool NoMissions)
    {

        GameObject Grablog;
        GameObject DetailText;
        DetailText = GameObject.Find("HighlightedBox");
        Grablog = GameObject.Find("GameObject");

        if (NoMissions == false)
        {
            DetailText.GetComponent<Text>().text = Grablog.GetComponent<GrabLog>().ActiveMissionList[MissionNumber].Story;
        }
        else
        {
            DetailText.GetComponent<Text>().text = "You only have " + Grablog.GetComponent<GrabLog>().ActiveMissionList.Count + " Missions!";
        }
    }
}
