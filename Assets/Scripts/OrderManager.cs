using System;
using System.Text;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UHG
{
    public enum OrderStatus
    {
        OrderPending,
        ResultGood,
        ResultBad,
        DeliveredGood,
        DeliveredBad,
    }

    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private CharacterCollection characterCollection;
        [SerializeField] private DialogueManager dialogueManager;
        
        private OrderStatus[] _characterOrderStatus;
        private int _currentDay = 0;
        private int _currentCharacter = 0;

        private async void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                await PlayCorrespondingDialogue();
            }
        }

        void Start()
        {
            _characterOrderStatus = new OrderStatus[characterCollection.characters.Length];
        }

        [Button("Print State")]
        public void PrintState()
        {
            StringBuilder sb = new();
            sb.AppendLine("Current Day: " + _currentDay);
            for (int i = 0; i < _characterOrderStatus.Length; i++)
            {
                sb.AppendLine($"#{i}: {characterCollection.characters[i].name} ,{_characterOrderStatus[i]}");
            }
            
            Debug.Log(sb.ToString());
        }
        
        [Button]
        private async UniTask CompleteGood()
        {
            _characterOrderStatus[_currentCharacter] = OrderStatus.DeliveredGood;
            await PlayCorrespondingDialogue();
            CycleNextCharacter();
        }

        [Button]
        private async UniTask CompleteBad()
        {
            _characterOrderStatus[_currentCharacter] = OrderStatus.ResultBad;
            await PlayCorrespondingDialogue();
            CycleNextCharacter();
        }

        [Button]
        public async UniTask PlayCorrespondingDialogue()
        {
            var character = characterCollection.characters[_currentCharacter];
            DialogueData currentDialogueData = _characterOrderStatus[_currentCharacter] switch
            {
                OrderStatus.OrderPending => character.firstEncounterDialogue,
                OrderStatus.ResultBad or OrderStatus.DeliveredBad => character.failureDialogue,
                OrderStatus.DeliveredGood or OrderStatus.ResultGood => character.successDialogue,
                _ => null
            };

            if (currentDialogueData == null)
            {
                Debug.Log("BadBadBad...");
                return;
            }

            await dialogueManager.PlayDialogue(currentDialogueData);
        }
        
        private void CycleNextCharacter()
        {
            _currentCharacter++;
            if (_currentCharacter == characterCollection.characters.Length)
            {
                Debug.Log("Ending reached");
            }
        }
    }
}
