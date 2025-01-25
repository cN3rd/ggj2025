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
        [SerializeField] private TimelineAsset newsCutscene;
        [SerializeField] private TimelineAsset bedCutscene;
        [SerializeField] private UniversalAdditionalCameraData cameraData; // its needed for VFX?
        private GameObject _targetCamera;
        private int mute_index;
        
        
        public void TransitionToTargetCamera(GameObject target) //CHANGE TO SWITCH
        {
            _targetCamera = target;
            switch (_targetCamera.name)
            {
                case "EyepeekCamera":
                    playableDirector.playableAsset = timelineInPeek;
                    break;
                case "Craft Table Camera":
                    playableDirector.playableAsset = timelineInCraft;
                    break;
                case "NewsCutsceneCamera":
                    playableDirector.playableAsset = newsCutscene;
                    break;
                case "BedCutsceneCamera":
                    playableDirector.playableAsset = bedCutscene;
                    break;
            }
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

        public void ExitCutscene()
        {
            _targetCamera.SetActive(false);
        }
    }
}
