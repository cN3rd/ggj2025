using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DialogueDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterNameTMP;
    [SerializeField] private TextMeshProUGUI dialogueTextTMP;
    [SerializeField] private float textScrollSpeed = 0.05f;
        
    [SerializeField] private CanvasGroup canvasGroup;

    public void Awake() => canvasGroup.alpha = 0;

    public void SetSpeakerName(string speaker) => characterNameTMP.SetText(speaker);
        
    public MotionHandle DisplayDialogueLine(string speaker, string dialogue)
    {
        characterNameTMP.text = speaker;
        var dialogueAnimation = LMotion.String
            .Create512Bytes("", dialogue, dialogue.Length * textScrollSpeed)
            .BindToText(dialogueTextTMP);

        return dialogueAnimation;
    }

    public bool IsVisible() => Mathf.Approximately(canvasGroup.alpha, 1);

    public async UniTask MakeVisible() => await LMotion.Create(0f, 1f, 0.5f).BindToAlpha(canvasGroup).GetAwaiter();

    public async UniTask MakeInvisible() => await LMotion.Create(1f, 0f, 0.5f).BindToAlpha(canvasGroup).GetAwaiter();
}
