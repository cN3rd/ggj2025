using System.Globalization;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Unity.Cinemachine;
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
        
        
        public void activateEyepeekCamera()
        {
            eyepeekCamera.Priority = 100;
            gameController.disablePlayerControls();
            playableDirector.Play();
        }
        public void changeTo2D()
        {
            playableDirector.playableAsset = timelineIn;
            Debug.Log("change to 2D");
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
            eyepeekCamera.Priority = -10;
            camera2D.Priority = 100;
            StartCoroutine(waitAndTransitionOut());
            
        }
        public void changeTo3D()
        {
            playableDirector.playableAsset = timelineOut;
            Debug.Log("change to 3D");
            camera2D.Priority = -100;
            eyepeekCamera.Priority = -10;
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
            gameController.disablePlayerControls();
        }
        
        public IEnumerator waitAndTransitionOut() // DEBUG | CHANGE TO 2D EXIT FUNC
        {
            yield return new WaitForSeconds(2);
            changeTo3D();
            playableDirector.Play();
        }

    }
}
