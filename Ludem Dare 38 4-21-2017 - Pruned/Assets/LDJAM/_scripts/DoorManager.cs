using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour {

    public GameObject Door;
    public bool doorLocked = false;

	// Use this for initialization
	void Start () {
		GetComponent<SphereCollider>().center = new Vector3(0,3,3);
	    GetComponent<SphereCollider>().radius = 3f;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        Debug.Log(string.Format("Object Entered Door {0}", other.gameObject.name));

        if (!doorLocked) {
            //If the door is locked buzz, if not open
            var _animator = Door.GetComponent<Animator>();
            var _showing = _animator.GetBool("Open");
            _animator.SetBool("Open", true);
            PlayOpenSounds();
        }
        else {
            //Play Failed noise
            PlayAlertSounds();

        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log(string.Format("Object Exited Door {0}", other.gameObject.name));
        var _animator = Door.GetComponent<Animator>();
        var _showing = _animator.GetBool("Open");
        _animator.SetBool("Open", false);

    }

    private void PlayOpenSounds() {
        var _CM = ConversationManager.instance;
        var AlertAudioSource = _CM.EffectAudioSource.GetComponent<AudioSource>();
        var AlertClip = _CM.DoorOpenSound;

        if (!AlertAudioSource.isPlaying) {
            AlertAudioSource.clip = AlertClip;
            AlertAudioSource.Play();
        }
    }


    private void PlayAlertSounds() {
        var _CM = ConversationManager.instance;
        var AlertAudioSource = _CM.EffectAudioSource.GetComponent<AudioSource>();
        var AlertClip = _CM.DoorLockedAlert;

        if (!AlertAudioSource.isPlaying) {
            AlertAudioSource.clip = AlertClip;
            AlertAudioSource.Play();
        }
    }




}
