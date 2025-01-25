using System;
using Data;
using TMPro;
using UHG;
using UnityEngine;

public class NewspaperChanger : MonoBehaviour
{
    [SerializeField] private CharacterData currentCharacter;
    [SerializeField] private bool isGood;
    
    [SerializeField] private TextMeshPro mainTitleText;
    [SerializeField] private TextMeshPro subTitleText;
    [SerializeField] private SpriteRenderer characterSprite;

    public void ChangeCurrentCharacter(CharacterData newCharacter)
    {
        currentCharacter = newCharacter;
        if (isGood)
        {
            mainTitleText.text = newCharacter.mainTitleGood;
            subTitleText.text = newCharacter.subTitleGood;
            characterSprite.sprite = currentCharacter.paperGood;
        }
        else
        {
            mainTitleText.text = newCharacter.mainTitleBad;
            subTitleText.text = newCharacter.subTitleBad;
            characterSprite.sprite = currentCharacter.paperBad;
        }
    }

    #if UNITY_EDITOR
    private void OnValidate() => ChangeCurrentCharacter(currentCharacter);
    #endif
}
