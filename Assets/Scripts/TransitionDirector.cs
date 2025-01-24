using System.Globalization;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.Rendering.Universal;


namespace UHG
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TransitionDirector : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private CinemachineCamera eyepeekCamera;
        [SerializeField] private CinemachineCamera camera2D;

        [SerializeField] private PlayableDirector _playableDirector;
        
        public void activateEyepeekCamera()
        {
            eyepeekCamera.Priority = 100;
            gameController.disablePlayerControls();
            _playableDirector.Play();
            // StartCoroutine(TransitionInCoroutine());
        }

        // public IEnumerator TransitionInCoroutine()
        // {
        //     _playableDirector.Play();
        //     while (_playableDirector.state == PlayState.Playing)
        //     {
        //         yield return null;
        //     }
        // }
        public void changeTo2D()
        {
            Debug.Log("change to 2D");
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
            eyepeekCamera.Priority = -10;
            camera2D.Priority = 100;
        }


    }
}
