using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Order : MonoBehaviour {

    public float defaultTimer = 60f;
    public float timeLeft = 60f;

    [Tooltip("These are monitors that are able to Take damage")]
    public List<Monitor> activeMonitors;

    [Tooltip("Is there actively something we have to fix?")]
    public bool activeAlert = false;
    
    [Range(0,100)]
    public int alertPercent = 5;

    [Tooltip("This will determine how often the failure checks run which means a higher # means less frequent errors")]
    [Range(0,20)]
    public int LevelModifier = 20;


	void Start () {
		
	}
	
	void Update () {
	    var _randomNumber = Random.Range(0, 100);
        //Debug.Log(string.Format("Random #{0}", _randomNumber));

        //Only check every 10th frame
        if (Time.frameCount % LevelModifier == 0 && !ConversationManager.instance.Victory && !ConversationManager.instance.Death) {
	        //Debug.Log(string.Format("Random #{0}", _randomNumber));

            //Check to see if we should trip an alert!
            if (!activeAlert && _randomNumber <= alertPercent && ConversationManager.instance.TutorialCompleted) {
	            var _monitor = Random.Range(0, activeMonitors.Count);
	            activeMonitors[_monitor].BreakMonitor();

	            Debug.Log(string.Format("Alert for {0}", activeMonitors[_monitor].name));
	            //activeAlert = true;

	        }
	    }

	}
}
