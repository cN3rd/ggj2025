using Data;
using UnityEngine;

public class PeepholeChanger : MonoBehaviour
{
    [SerializeField] private CharacterData currentCharacter;
    [SerializeField] private SpriteRenderer characterSprite;
    
    public void ChangeCurrentCharacter(CharacterData newCharacter)
    {
        currentCharacter = newCharacter;
        characterSprite.sprite = currentCharacter.characterSprite;
    }

#if UNITY_EDITOR
    private void OnValidate() => ChangeCurrentCharacter(currentCharacter);
#endif
}
