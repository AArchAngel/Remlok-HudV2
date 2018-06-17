using UnityEngine;
using System.Diagnostics;

public class GameRunningCheck : MonoBehaviour
{

  //  public bool Running;

    private void Start()
    {
        InvokeRepeating("GameCheck", 0, 1);
    }

    public void GameCheck()
    {


        if (Process.GetProcessesByName("EliteDangerous64").Length > 0)
        {
            gameObject.GetComponent<JournalWatcher>().RunningCheck = true;
        }
        else
        {
            gameObject.GetComponent<JournalWatcher>().RunningCheck = false;
        }
    }

}