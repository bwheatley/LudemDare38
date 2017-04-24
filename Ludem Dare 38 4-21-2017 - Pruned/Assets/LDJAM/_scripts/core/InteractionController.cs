using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

    [Range(0.25f,2f)]
    public float Range = 1f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Fire1")) {
	        MouseClick();
	    }
	}


    void MouseClick() {
        //Debug.Log(string.Format("We clicked!"));

        int layer1 = 8;
        int layer2 = 9;
        int layermask1 = 1 << layer1;
        int layermask2 = 1 << layer2;
        //int finalmask = layermask1;
        int finalmask = layermask1 | layermask2;

        //var finalmask : int = layermask1 | layermask2; // Or, (1 << layer1) | (1 << layer2)


        var _cam = Camera.main;

        RaycastHit hit;

        //Vector3 forward = _cam.transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(_cam.transform.position, forward, Color.red, 3, false);

        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, Range, finalmask)) {
            //Debug.Log(string.Format("HIT {0}", hit.transform.name));

            if (hit.transform.name == "Turbolift") {
                hit.transform.GetComponent<Turbolift>().ClickButton();
            }
            else {
                hit.transform.GetComponent<Monitor>().ClickButton();
            }

        }

    }

}
