using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DialogueManager", menuName = "Game Data/Dialogue Manager")]
    public class DialogueDataAsset : ScriptableObject
    {
        public DialogueData data;
    }

    [Serializable]
    public class DialogueData
    {
        public DialogueSegment[] dialogueSegments;
    }
    
    [Serializable]
    public class DialogueSegment
    {
        public string speaker;
        public string text;
    }
}
