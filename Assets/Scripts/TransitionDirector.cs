using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

namespace UHG
{
    [RequireComponent(typeof(PlayableGraph))]
    public class TransitionDirector : MonoBehaviour
    {
        [SerializeField] private PlayableGraph playableDirector;
        [SerializeField] private GameObject virtCamera;
        private bool stratedPlaying = false;

        void Start()
        {
            
        }
        public void activateVirtCamera()
        {
            virtCamera.SetActive(true);
            stratedPlaying = true;
        }
        void Update()
        {
            if (stratedPlaying && !playableDirector.IsPlaying())
                Debug.Log("Change Scene");
        }
    }
}
