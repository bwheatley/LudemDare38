using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {

    public int score;
    public int topScore;

    public int level = 0;
    public int scorePointsPer = 10;
    public float scoreMultiplier;

    public int scoreTillNextLevel = 10;


    private void Start() {
        //Also setup the UI
        var _CM = ConversationManager.instance;
        UI_Manager _uiManager = _CM.GetComponent<UI_Manager>();

        var _score = string.Format("{0:N0}", score);
        _uiManager.UI_Points.GetComponent<TextMeshProUGUI>().text = _score;
    }

    private void Update() {
        if (score > topScore) {
            topScore = score;

            //Also setup the UI
            var _CM = ConversationManager.instance;
            UI_Manager _uiManager = _CM.GetComponent<UI_Manager>();

            var _score = string.Format("{0:N0}", score);
            _uiManager.UI_Points.GetComponent<TextMeshProUGUI>().text = _score;

        }
    }
    
    //Add a scorepoint
    public void AddScore() {
        if (scoreMultiplier > 0) {
            float _scorefloat = score;
            _scorefloat += scorePointsPer * scoreMultiplier;
            score = (int)_scorefloat;
        }
        else {
            score += scorePointsPer;
        }

        //LevelUp?
        if (score >= scoreTillNextLevel) {
            LevelUP();
        }

    }

    /// <summary>
    /// You've not had a birthday, you've leveled up!
    /// </summary>
    public void LevelUP() {
        var _CM = ConversationManager.instance;
        UI_Manager _uiManager = _CM.GetComponent<UI_Manager>();

        level++;

        _uiManager.UI_LevelText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", level);

        scoreMultiplier++;

        //What's the next Score for the next level 
        scoreTillNextLevel *= (2 * level);

        _uiManager.UI_NextLevelText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:N0}", scoreTillNextLevel);

        scorePointsPer *= level;

        var _Order = _CM.GetComponent<Order>();

        _Order.LevelModifier = _Order.LevelModifier / 2;
        _Order.LevelModifier = Mathf.Clamp(_Order.LevelModifier, 1, 20);

        //If the level is high enough to win lets let them know
        if (level >= _CM.VictoryLevel) {
            Unlock();
        }

    }

    /// <summary>
    /// You've reached the level needed move up
    /// </summary>
    public void Unlock() {
        Debug.Log("Victory!!");

        var _CM = ConversationManager.instance;
        var _CMOrder = _CM.GetComponent<Order>();
        _CM.Victory = true;
        _CMOrder.activeAlert = false;
        _CMOrder.timeLeft = _CMOrder.defaultTimer;

        //Unlock the door
        _CM.ExitDoorGO.GetComponent<DoorManager>().doorLocked = false;
        //Change the image
        _CM.ExitDoor.sprite = _CM.ExitGreen;

        //Just hard code closing values
        _CM.ConversationAutoClose = 100f;
        _CM.ConversationAutoCloseActivated = false;

        //Trigger the conversation
        _CM.GetConversationSequence("freetogo");
        _CM.ShowNextConversation();
    }


}
