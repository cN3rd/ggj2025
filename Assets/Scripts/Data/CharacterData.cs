using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
    public class CharacterData
    {
        public Sprite characterSprite;
        public CharacterAffinity[] affinities;
        
        public DialogueData firstEncounterDialouge;
        public DialogueData successDialouge;
        public DialogueData failureDialogue;
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
