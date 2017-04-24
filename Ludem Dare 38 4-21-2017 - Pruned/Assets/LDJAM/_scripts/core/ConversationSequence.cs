using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ConversationSequence {

    public string NameOfSequence;

    [Tooltip("What's the conversation of this?")]
    [TextArea(0,10)]
    public List<string> conversation;

    [Tooltip("An image to go with the corresponding sprite")]
    public List<Sprite> conversationSprite;

    [Tooltip("Next item in the conversation to show")]
    public int nextItemInSequence;

    public float timeBetweenEachString;

    [Tooltip("If you want a piece of damage to have a non-stock timer")]
    public int AlertTimer = 0;




    public ConversationSequence() {
        
    }
    
}
