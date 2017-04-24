using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour {

    public AudioClip ComputerClip;
    public AudioClip AlertClip;
    public AudioSource MasterAudioSource;
    public AudioSource AlertAudioSource;
    public Color32 onColor;
    public Color32 offColor;
    public string SystemName;
    public Image SystemButton;
    public TextMeshProUGUI SystemTxt;

    [Tooltip("If a monitor has an active task it means the player needs to come here!")]
    public bool ActiveTask = false;

    [Tooltip("Play the alert every 5 seconds")]
    public float defautlAlertSound = 5f;

    public float alertTimer = 0f;



	// Use this for initialization
	void Start () {
		Debug.Log("Monitor Online");
	    GetComponent<SphereCollider>().radius = 1f;

        //Setup Screen Text
	    SystemTxt.text = SystemName;


	}
	
	// Update is called once per frame
	void Update () {

	    alertTimer += 1 * Time.deltaTime;

        //Play the alert till we fix
	    if (ActiveTask && alertTimer >= defautlAlertSound && !ConversationManager.instance.Death && !ConversationManager.instance.Victory) {
            PlayAlertSounds();
	        alertTimer = 0f;
	    }

	}

    //Break the monitor!!!
    public void BreakMonitor() {
        Debug.Log(string.Format("Break Monitor {0}", this.gameObject.name));
        SystemButton.color = offColor;
        PlayAlertSounds();
        ActiveTask = true;

        //Only if tutorials are done
        if (ConversationManager.instance.TutorialCompleted) {
            //What type of system is bahroke?
            switch (SystemName.ToLower()) {
            case "warp engines":
                ConversationManager.instance.GetConversationSequence("DamageWarpCore");
                var _sequence = ConversationManager.instance.ActiveConversationSequence;
                var _sequenceItem = _sequence.nextItemInSequence;

                if (_sequence.AlertTimer > 0) {
                    var _CM = ConversationManager.instance;
                    var _CMOrder = _CM.GetComponent<Order>();
                    if (_CM.GetComponent<Score>().level > 0) {
                        _CMOrder.timeLeft = _sequence.AlertTimer / _CM.GetComponent<Score>().level;
                        _CMOrder.timeLeft = Mathf.Clamp(_CMOrder.timeLeft, _CM.LowestTimer, _CM.HighestTimer);
                    }
                    else {
                        _CMOrder.timeLeft = _CMOrder.defaultTimer;
                    }
                }
                break;
            case "shields":
                ConversationManager.instance.GetConversationSequence("DamageShields");
                _sequence = ConversationManager.instance.ActiveConversationSequence;
                _sequenceItem = _sequence.nextItemInSequence;

                if (_sequence.AlertTimer > 0) {
                    var _CM = ConversationManager.instance;
                    var _CMOrder = _CM.GetComponent<Order>();
                    if (_CM.GetComponent<Score>().level > 0) {
                        _CMOrder.timeLeft = _sequence.AlertTimer / _CM.GetComponent<Score>().level;
                        _CMOrder.timeLeft = Mathf.Clamp(_CMOrder.timeLeft, _CM.LowestTimer, _CM.HighestTimer);
                    }
                    else {
                        _CMOrder.timeLeft = _CMOrder.defaultTimer;
                    }
                }
                break;
            case "life support":
                ConversationManager.instance.GetConversationSequence("DamageLifeSupport");
                _sequence = ConversationManager.instance.ActiveConversationSequence;
                _sequenceItem = _sequence.nextItemInSequence;

                if (_sequence.AlertTimer > 0) {
                    var _CM = ConversationManager.instance;
                    var _CMOrder = _CM.GetComponent<Order>();
                    if (_CM.GetComponent<Score>().level > 0) {
                        _CMOrder.timeLeft = _sequence.AlertTimer / _CM.GetComponent<Score>().level;
                        _CMOrder.timeLeft = Mathf.Clamp(_CMOrder.timeLeft, _CM.LowestTimer, _CM.HighestTimer);
                    }
                    else {
                        _CMOrder.timeLeft = _CMOrder.defaultTimer;
                    }
                }
                break;
            case "weapons":
                ConversationManager.instance.GetConversationSequence("DamageWeapons");
                _sequence = ConversationManager.instance.ActiveConversationSequence;
                _sequenceItem = _sequence.nextItemInSequence;

                if (_sequence.AlertTimer > 0) {
                    var _CM = ConversationManager.instance;
                    var _CMOrder = _CM.GetComponent<Order>();
                    if (_CM.GetComponent<Score>().level > 0) {
                        _CMOrder.timeLeft = _sequence.AlertTimer / _CM.GetComponent<Score>().level;
                        _CMOrder.timeLeft = Mathf.Clamp(_CMOrder.timeLeft, _CM.LowestTimer, _CM.HighestTimer);
                    }
                    else {
                        _CMOrder.timeLeft = _CMOrder.defaultTimer;
                    }
                }
                break;

            default:
                break;

            }
            ConversationManager.instance.ShowNextConversation();
        }

        //Activate the order
        ConversationManager.instance.GetComponent<Order>().activeAlert = true;


    }

    private void PlayComputerSound() {
        if (!MasterAudioSource.isPlaying) {
            MasterAudioSource.clip = ComputerClip;
            MasterAudioSource.Play();
        }
    }

    private void PlayAlertSounds() {
        if (!AlertAudioSource.isPlaying) {
            AlertAudioSource.clip = AlertClip;
            AlertAudioSource.Play();
        }
    }

    /// <summary>
    /// Did we somehow trigger a button click?
    /// </summary>
    public void ClickButton() {
        var _CM = ConversationManager.instance;
        var _CMOrder = _CM.GetComponent<Order>();

        if (ActiveTask) {
            SystemButton.color = onColor;
            ActiveTask = false;

            _CMOrder.activeAlert = false;
            if (_CM.GetComponent<Score>().level > 0) {
                _CMOrder.timeLeft = _CMOrder.defaultTimer / _CM.GetComponent<Score>().level;
                _CMOrder.timeLeft = Mathf.Clamp(_CMOrder.timeLeft, _CM.LowestTimer, _CM.HighestTimer);
            }
            else {
                _CMOrder.timeLeft = _CMOrder.defaultTimer;
            }
            var _timer = string.Format("{0:N0}", _CMOrder.defaultTimer);
            UI_Manager _uiManager = _CM.GetComponent<UI_Manager>();
            _uiManager.UI_Timer.GetComponent<TextMeshProUGUI>().text = _timer;

            _CM.GetComponent<Score>().AddScore();



            Debug.Log(string.Format("ACTIVE Click the Button for {0}", this.gameObject.name));
        }
        else {
            Debug.Log(string.Format("NOTACTIVE Click the Button for {0}", this.gameObject.name));
        }

    }
    


}
