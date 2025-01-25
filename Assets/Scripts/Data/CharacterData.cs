using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UHG
{
    public enum CharacterAffinity
    {
        Neutral,
        
        Sad,
        Mysterious,
        Pure,
        Happy,
        Strong,
        Chaotic
    }
    
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Game Data/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        public Sprite characterSprite;
        public CharacterAffinity[] affinities;
        
        [Header("Dialogues")]
        public DialogueData firstEncounterDialogue;
        public DialogueData successDialogue;
        public DialogueData failureDialogue;
        
        [Header("Good paper article")]
        public string mainTitleGood;
        public string subTitleGood;
        public Sprite paperGood;
        public DialogueData paperReactionGood;

        [Header("Bad paper article")]
        public string mainTitleBad;
        public string subTitleBad;
        public Sprite paperBad;
        public DialogueData paperReactionBad;
    }

    public class MaskRequest
    {
        public MaskRequest(HashSet<CharacterAffinity> requestedAffinities) => RequestedAffinities = requestedAffinities;

        public HashSet<CharacterAffinity> RequestedAffinities { get; }
       
        public bool CheckSuccess(HashSet<CharacterAffinity> affinitiesInMask)
        {
            int points = 0;

            foreach (var affinity in affinitiesInMask)
            {
                if (RequestedAffinities.Contains(affinity)) points += 25;
                else if (affinity == CharacterAffinity.Neutral) points += 0;
                else points -= 25;
            }

            return points > 75;
        }
    }
}
