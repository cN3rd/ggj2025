using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

namespace UHG
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueDisplayer displayer;
        
        public async UniTask PlayDialogue(DialogueData data)
        {
            if (data.dialogueSegments.Length == 0) return;
            displayer.SetSpeakerName(data.dialogueSegments[0].speaker);
            
            if (!displayer.IsVisible()) await displayer.MakeVisible();
            
            // scroll through dialogue
            foreach (var dialogueSegment in data.dialogueSegments)
            {
                var dialogueDisplayMotion = displayer.DisplayDialogueLine(dialogueSegment.speaker, dialogueSegment.text);
                await dialogueDisplayMotion.ToAwaitable();
                await UniTask.WaitForSeconds(3);
            }

            await displayer.MakeInvisible();
        }

        public async void Start()
        {
            DialogueData data = new()
            {
                dialogueSegments = new[]
                {
                    new DialogueSegment { speaker = "Mark", text = "Hello Shlomo" },
                    new DialogueSegment { speaker = "Shlomo", text = "Hello Mark" },
                    new DialogueSegment { speaker = "Tommy", text = "Hello Mark, hello Shlomo" },
                    new DialogueSegment { speaker = "Mark", text = "Fuck you all, i'm going to smoke" },
                },
                characterSprite = null
            };
            
            await PlayDialogue(data);
        }
    }

    [Serializable]
    public class DialogueData
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
