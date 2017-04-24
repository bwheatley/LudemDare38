using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbolift : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickButton() {
        Debug.Log("You Win!");
        var _CM = ConversationManager.instance;
        _CM.ShowVictoryPanel();
        //_CM.GetComponent<UI_Manager>().

        //Goto Victory Screen

    }

}
