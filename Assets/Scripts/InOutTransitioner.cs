using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace UHG
{
    public class InOutTransitioner : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private CinemachineCamera targetCamera;
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private TimelineAsset timelineIn;
        [SerializeField] private TimelineAsset timelineOut;
        [SerializeField] private UniversalAdditionalCameraData cameraData;

        public void ActivateTargetCamera()
        {
            targetCamera.Priority = 100;
            gameController.DisablePlayerControls();
            playableDirector.Play();
        }

        public void TransitionInEnd() => TransitionIn();

        private void TransitionIn()
        {
            // enter dialog mode
            playableDirector.playableAsset = timelineIn;
            Debug.Log("Transition In");
            StartCoroutine(WaitAndTransitionOut());
        }

        private void TransitionOut()
        {
            playableDirector.playableAsset = timelineOut;
            Debug.Log("Transition Out");
            targetCamera.Priority = -10;
        }

        private IEnumerator WaitAndTransitionOut() // DEBUG | CHANGE TO 2D EXIT FUNC
        {
            yield return new WaitForSeconds(2);
            yield return DeactivateTargetCamera();
        }

        public IEnumerator DeactivateTargetCamera()
        {
            Debug.Log("Deactivate Target Camera");
            TransitionOut();
            playableDirector.Play();
            while (playableDirector.state == PlayState.Playing)
            {
                yield return null;
            }

            playableDirector.playableAsset = timelineIn;
        }
    }
}
