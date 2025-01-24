using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

namespace UHG
{
    [RequireComponent(typeof(PlayableGraph))]
    public class TransitionDirector : MonoBehaviour
    {
        [SerializeField] private Playable playableDirector;
        [SerializeField] private GameObject virtCamera;
        private bool stratedPlaying = false;

        // void Start()
        // {
        //     playableDirector = gameObject.GetComponent<Playable>();
        // }
        public void activateVirtCamera()
        {
            virtCamera.SetActive(true);
            stratedPlaying = true;
        }
        void Update()
        {
            // if (stratedPlaying && !playableDirector.IsPlaying())
            //     Debug.Log("Change Scene");
        }
    }
}
