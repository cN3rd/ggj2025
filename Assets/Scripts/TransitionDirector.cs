using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

namespace UHG
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TransitionDirector : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private CinemachineCamera eyepeekCamera;
        [SerializeField] private CinemachineCamera camera2D;
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private TimelineAsset timelineIn;
        [SerializeField] private TimelineAsset timelineOut;
        [SerializeField] private UniversalAdditionalCameraData cameraData;

        public void ActivateEyepeekCamera()
        {
            eyepeekCamera.Priority = 100;
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
            eyepeekCamera.Priority = -10;
        }

        private IEnumerator WaitAndTransitionOut() // DEBUG | CHANGE TO 2D EXIT FUNC
        {
            yield return new WaitForSeconds(2);
            yield return DeactivateEyepeekCamera();
        }

        private IEnumerator DeactivateEyepeekCamera()
        {
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
