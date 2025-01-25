using System;
using UnityEngine;

namespace UHG
{
    [CreateAssetMenu(fileName = "DialogueManager", menuName = "Game Data/Dialogue Manager")]
    public class DialogueData : ScriptableObject
    {
        public DialogueSegment[] dialogueSegments;
        public Sprite characterSprite;
    }
    
    [Serializable]
    public class DialogueSegment
    {
        public string speaker;
        public string text;
    }
}
