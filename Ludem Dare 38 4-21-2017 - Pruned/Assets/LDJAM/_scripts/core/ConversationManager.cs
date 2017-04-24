using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class ConversationManager : MonoBehaviour {

    #region FIELDS

    public static ConversationManager instance;

    public FirstPersonController _FPSController;

    //[Tooltip("A list of all the games conversations, i think?!?")]
    //[TextArea(0,10)]
    //public List<string> ConversationsList;


    [Tooltip("A list of all the games conversations, i think?!?")]
    [SerializeField]
    public List<ConversationSequence> ConversationSequences;

    public ConversationSequence ActiveConversationSequence;

    [Tooltip("Is a conversation actively underway?")]
    public bool ConversationActive;

    [Tooltip("If this is > 0 we'll have a timer to auto hit space")]
    public float ConversationAutoClose = 0;

    public bool ConversationAutoCloseActivated = false;

    [Tooltip("This will be something to tell people to hit the Jump key to continue")]
    public GameObject ProceedNotifier;
    [SerializeField]
    public AudioController _AudioController;
    public AudioClip ComputerClip;
    public AudioClip DoorLockedAlert;
    public AudioClip DoorOpenSound;
    public AudioSource VoiceAudioSource;
    public AudioSource AlertAudioSource;
    public AudioSource EffectAudioSource;


    [Tooltip("At what level does the back door open to win?")]
    [Range(1,100)]
    public int VictoryLevel;

    public GameObject ExitDoorGO;
    public bool Victory = false;
    public bool Death = false;


    [Range(0, 45)]
    public int LowestTimer = 10;
    [Range(0, 120)]
    public int HighestTimer = 10;

    public SpriteRenderer ExitDoor;
    public Sprite ExitRed;
    public Sprite ExitGreen;

    public Order OrderManager;

    public bool TutorialCompleted = false;

    #endregion

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public void OnQuit() {
        Application.Quit();
    }


    public void ShowVictoryPanel() {
        var _UI = GetComponent<UI_Manager>();
        _UI.UI_VictoryPanel.SetActive(true);
    }

    public void ShowDeathPanel() {
        var _UI = GetComponent<UI_Manager>();
        _UI.UI_DeathPanel.SetActive(true);
    }


    // Use this for initialization
    void Start () {
        //When we first startup we need to teach teh player about the game
        GetConversationSequence("WelcomeSequence");
        ShowNextConversation();
        //PlayConversation();

    }

    // Update is called once per frame
    void Update () {
        UI_Manager _uiManager = GetComponent<UI_Manager>();

        //Check for an active alert
        if (OrderManager.activeAlert) {
            OrderManager.timeLeft -= 1 * Time.deltaTime;

            //Do the counter
            var _timer = string.Format("{0:N0}", OrderManager.timeLeft);
            _uiManager.UI_Timer.GetComponent<TextMeshProUGUI>().text = _timer;

            //We're dead jim
            if (OrderManager.timeLeft <= 0) {
                GameOver();
            }

        }

        //If a conversation is active don't allow jumping
        if (ConversationActive) {
	        _FPSController.m_dontJump = true;

            //Also check if there is a timer
            if (ConversationAutoCloseActivated) {
                ConversationAutoClose -= 1 * Time.deltaTime;

            }

            //Check for button to close
	        if (Input.GetButtonDown("Jump") || (ConversationAutoClose <=0 && ConversationAutoCloseActivated ) ) {
                //We need to check if this is the last item in the sequence
	            if (CheckSequenceLength()) {
	                //Debug.Log(string.Format("We have NOMORE items in the sequence"));
	                CloseConversation();
	            }
                //We still can keep iterating
                else {
	                //Debug.Log(string.Format("We still have items in the sequence"));
	                NextItemInSequence();
	            }

	        }

	    }
	    else {
	        _FPSController.m_dontJump = false;
        }



    }

    //This should be just a random alert about something broken
    public void GenerateConversationItem() {
        
    }

    private void PlayVoiceSound() {
        if (!VoiceAudioSource.isPlaying) {
            VoiceAudioSource.clip = ComputerClip;
            VoiceAudioSource.Play();
        }
    }

    public void GameOver() {
        Debug.Log("we've died!");
        Death = true;

        var _CMOrder = GetComponent<Order>();
        _CMOrder.activeAlert = false;
        _CMOrder.timeLeft = _CMOrder.defaultTimer;
        ConversationAutoClose = 100f;
        ConversationAutoCloseActivated = false;



        ShowDeathPanel();
    }


    public void NextItemInSequence() {
        UI_Manager _uiManager = GetComponent<UI_Manager>();
        var _sequence = ActiveConversationSequence;
        _sequence.nextItemInSequence++;
        var _sequenceItem = _sequence.nextItemInSequence;

        //Debug.Log(string.Format("Sequence Before #{0}", _sequenceItem));
        //Debug.Log(string.Format("Sequence After #{0}", _sequenceItem));

        //Clear what was already there && Move to the next string in the list
        _uiManager.UI_ConversationText.GetComponent<TextMeshProUGUI>().text = _sequence.conversation[_sequenceItem];
        //Set the image
        _uiManager.UI_ConversationImage.GetComponent<Image>().sprite = _sequence.conversationSprite[_sequenceItem];
        //Play the voice
        PlayVoiceSound();


        //Show the Conversation Continue Button
        _uiManager.UI_ConversationContinueHolder.SetActive(true);


    }

    /// <summary>
    /// Check the current sequence and see if we're at the end
    /// </summary>
    /// <returns>False if we're not at the end, and can increment the counter</returns>
    public bool CheckSequenceLength() {
        var results = true;
        var _sequence = ActiveConversationSequence;
        var _sequenceItem = _sequence.nextItemInSequence;

        //We still have things to show!
        //Debug.Log(string.Format("Seq# {0} Seq Count {1}", _sequenceItem+1, _sequence.conversation.Count));
        if (_sequenceItem+1 < _sequence.conversation.Count) {
            return false;
        }

        return results;
    }

    public void GetConversationSequence(string _nameOfSequence ) {
        var _sequences = ConversationSequences;

        //Loop through all sequences and return the right one
        for (int i = 0; i < _sequences.Count; i++) {
            var _sequance = _sequences[i];
            var _sequenceName = _sequance.NameOfSequence.ToLower();

            if (_sequenceName == _nameOfSequence.ToLower()) {
                //Debug.Log(string.Format("We found it doctor!"));
                ActiveConversationSequence = _sequance;
                return;
            }
        }

        ActiveConversationSequence = null;
    }

    /// <summary>
    /// We will use this to show the next conversation
    /// </summary>
    public void ShowNextConversation() {
        UI_Manager _uiManager = GetComponent<UI_Manager>();

        //Turn on the Conversation
        _uiManager.UI_ConversationHolder.SetActive(true);

        var _sequence = ActiveConversationSequence;
        var _sequenceItem = _sequence.nextItemInSequence;

        //Clear what was already there && Move to the next string in the list
        _uiManager.UI_ConversationText.GetComponent<TextMeshProUGUI>().text = _sequence.conversation[_sequenceItem];  
        //Set the image
        _uiManager.UI_ConversationImage.GetComponent<Image>().sprite = _sequence.conversationSprite[_sequenceItem];
        //Play the voice
        PlayVoiceSound();

        //Check if there is a timer thingy
        if (_sequence.timeBetweenEachString > 0) {
            ConversationAutoCloseActivated = true;
            ConversationAutoClose = _sequence.timeBetweenEachString;
        }

        //Show the Conversation Continue Button
        _uiManager.UI_ConversationContinueHolder.SetActive(true);

        //Set the conversation as active
        ConversationActive = true;

    }

    public void CloseConversation() {
        Debug.Log("Close Conversations");
        UI_Manager _uiManager = GetComponent<UI_Manager>();

        _uiManager.UI_ConversationHolder.SetActive(false);
        _uiManager.UI_ConversationContinueHolder.SetActive(false);
        //Increment the conversation counter
        ConversationActive = false;

    }


}
