using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    [Tooltip("This is the UI holder for the points display")]
    public GameObject UI_Points;
    [Tooltip("This is the UI holder for the conversation with mr rot")]
    public GameObject UI_ConversationHolder;

    [Tooltip("The actual textmeshpro of the text")]
    public GameObject UI_ConversationText;

    [Tooltip("The image that goes along with the text")]
    public GameObject UI_ConversationImage;

    [Tooltip("This holds the Continue Button")]
    public GameObject UI_ConversationContinueHolder;

    [Tooltip("Object that holds time left counter")]
    public GameObject UI_Timer;

    public GameObject UI_LevelText;
    public GameObject UI_NextLevelText;

    public GameObject UI_VictoryPanel;
    public GameObject UI_DeathPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
