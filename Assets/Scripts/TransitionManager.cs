using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace UHG
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private TimelineAsset timelineInPeek;
        [SerializeField] private TimelineAsset timelineInCraft;
        [SerializeField] private TimelineAsset timelineOutPeek;
        [SerializeField] private TimelineAsset timelineOutCraft;
        [SerializeField] private TimelineAsset newspaperCutscene;
        [SerializeField] private UniversalAdditionalCameraData cameraData; // its needed for VFX?
        private GameObject _targetCamera;
        private int mute_index;
        
        
        public void TransitionToTargetCamera(GameObject target)
        {
            _targetCamera = target;
            if (_targetCamera.name == "EyepeekCamera")
                playableDirector.playableAsset = timelineInPeek;
            else
                playableDirector.playableAsset = timelineInCraft;
            target.SetActive(true);
            playableDirector.Play();
            // timelineIn.GetRootTrack(mute_index).muted = false;

        }

        public void ReturnToPlayer()
        {
            if (_targetCamera.name == "EyepeekCamera")
                playableDirector.playableAsset = timelineInPeek;
            else
                playableDirector.playableAsset = timelineInCraft;
            _targetCamera.SetActive(false);
            playableDirector.Play();
        }
    }
}
