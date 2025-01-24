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

        public void ChangeTo2D() => SwitchTo2DRenderer();

        private void SwitchTo2DRenderer()
        {
            playableDirector.playableAsset = timelineIn;
            Debug.Log("change to 2D");
            cameraData.SetRenderer(1);
            eyepeekCamera.Priority = -10;
            camera2D.Priority = 100;
            StartCoroutine(WaitAndTransitionOut());
        }

        private void SwitchTo3DRenderer()
        {
            playableDirector.playableAsset = timelineOut;
            Debug.Log("change to 3D");
            camera2D.Priority = -100;
            eyepeekCamera.Priority = -10;
            cameraData.SetRenderer(0);
            gameController.DisablePlayerControls();
        }

        private IEnumerator WaitAndTransitionOut() // DEBUG | CHANGE TO 2D EXIT FUNC
        {
            yield return new WaitForSeconds(2);

            yield return DeactivateEyepeekCamera();
        }

        private IEnumerator DeactivateEyepeekCamera()
        {
            SwitchTo3DRenderer();
            playableDirector.Play();
            while (playableDirector.state == PlayState.Playing)
            {
                yield return null;
            }

            playableDirector.playableAsset = timelineIn;
        }
    }
}
