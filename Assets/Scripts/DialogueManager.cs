using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.Serialization;

namespace UHG
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private DialogueDisplayer displayer;

        public async UniTask PlayDialogue(DialogueData data)
        {
            if (data.dialogueSegments.Length == 0) return;

            gameController.DisablePlayerControls();
            gameController.DisableMaskMakingControls();
            gameController.EnableVNControls();

            displayer.SetSpeakerName(data.dialogueSegments[0].speaker);
            if (!displayer.IsVisible()) await displayer.MakeVisible();

            // scroll through dialogue
            foreach (DialogueSegment dialogueSegment in data.dialogueSegments)
            {
                MotionHandle dialogueDisplayMotion =
                    displayer.DisplayDialogueLine(dialogueSegment.speaker, dialogueSegment.text);

                await dialogueDisplayMotion.ToAwaitable();
                await UniTask.WaitForSeconds(3);
            }

            await displayer.MakeInvisible();

            gameController.DisableVNControls();
            gameController.DisableMaskMakingControls();
            gameController.EnablePlayerControls();
        }
    }
}
