using UnityEngine;

namespace UHG
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "Game Data/Character Collection")]
    public class CharacterCollection : ScriptableObject
    {
        public CharacterData[] characters;
    }
}
