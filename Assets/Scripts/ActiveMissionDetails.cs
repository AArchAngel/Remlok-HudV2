
using UnityEngine;

public class ActiveMissionDetails : MonoBehaviour {

    private bool MissionsActive;

	void ActivateMissionDetails () {
        GetComponent<Animation>().Play("MissionDetails");
	}
    void HideMissionDetails()
    {
        GetComponent<Animation>().Play("HideMissions");
        Debug.Log("Hidden!");
    }

    public void MissionShow () {

        if (MissionsActive == false)
        {
            ActivateMissionDetails();
            MissionsActive = true;
        }
        else
        {
            HideMissionDetails();
            MissionsActive = false;
        }

    }
}

