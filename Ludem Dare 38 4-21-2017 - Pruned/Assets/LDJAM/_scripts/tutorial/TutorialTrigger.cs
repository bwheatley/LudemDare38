using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    [Tooltip("Enter the sequence name of the tutorial to show")]
    public string TutorialToShow;

    [Tooltip("We only want this to get trigger once")]
    public bool Triggered = false;

    public TutorialTrigger requiredPriorTutorial;

    [Tooltip("is this the last tutorial of the game?")]
    public bool LastTutorialConvo = false;

    public Monitor monitorToBreak;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (!Triggered) {
            //Check to see if there was a prior required tutorial
            if (requiredPriorTutorial == null || requiredPriorTutorial.Triggered) {
                Debug.Log(string.Format("{0} has entered Tutorial Trigger show {1}", other.name, TutorialToShow.ToLower()));

                //Find the sequence
                ConversationManager.instance.GetConversationSequence(TutorialToShow);
                ConversationManager.instance.ShowNextConversation();

                //Do we need to break a monitor?!?!?!
                if (monitorToBreak != null) {
                    monitorToBreak.BreakMonitor();
                }
                
                //Is this the last tutorials?
                if (LastTutorialConvo) {
                    ConversationManager.instance.TutorialCompleted = true;
                }


                Triggered = true;
            }
        }

    }

    private void OnTriggerExit(Collider other) {
        //Debug.Log(string.Format("{0} has exited Tutorial Trigger", other.name));
    }




}
