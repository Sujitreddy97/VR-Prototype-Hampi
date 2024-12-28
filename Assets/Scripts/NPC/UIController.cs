using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRHampi.NPC;

namespace VRHampi
{
    public class UIController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject talkButton; // The button that appears when in range
        [SerializeField] private GameObject dialoguePanel; // The panel for displaying dialogue
        [SerializeField] private TextMeshProUGUI dialogueText; // Text element for dialogue lines
        [SerializeField] private Button nextButton; // Button for next dialogue
        [SerializeField] private Button previousButton; // Button for previous dialogue
        [SerializeField] private Button skipButton; // Button to skip dialogue
        [SerializeField] private Button finishButton; // Button to end the dialogue sequence

        [Header("NPC Dialogue")]
        [SerializeField] private NPCDialogueSO currentDialogueSO;
        [SerializeField] private NPCController npcController;
        private string[] dialogueLines; // Array of dialogue lines to display
        private int currentLineIndex = 0; // Tracks the current dialogue line being displayed

        #region Unity Methods

        private void OnEnable()
        {
            nextButton.onClick.AddListener(NextDialogue);
            previousButton.onClick.AddListener(PreviousDialogue);
            finishButton.onClick.AddListener(EndDialogue);
        }

        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(NextDialogue);
            previousButton.onClick.RemoveListener(PreviousDialogue);
            finishButton.onClick.RemoveListener(EndDialogue);
        }

        private void Start()
        {
            ResetUI();
        }
        #endregion

        #region UI Control Methods

        public void ShowTalkButton(bool show)
        {
            talkButton.SetActive(show);
        }

        public void StartDialogue(NPCDialogueSO dialogueSO)
        {
            currentDialogueSO = dialogueSO;
            dialogueLines = dialogueSO.dialogueLines.ToArray(); // Retrieve dialogue lines from the SO
            currentLineIndex = 0;

            if (dialogueLines == null || dialogueLines.Length == 0)
            {
                Debug.LogError("Dialogue lines are not initialized or empty!");
                return;
            }

            Debug.Log($"Starting dialogue with {dialogueLines.Length} lines.");
            UpdateDialogueText();

            talkButton.SetActive(false);
            dialoguePanel.SetActive(true);

            UpdateNavigationButtons();
        }

        public void EndDialogue()
        {
            npcController.FinishInteraction();
            dialoguePanel.SetActive(false);
            ResetUI();
        }

        private void ResetUI()
        {
            talkButton.SetActive(false);
            dialoguePanel.SetActive(false);
            finishButton.gameObject.SetActive(false);
        }

        private void UpdateDialogueText()
        {
            if (dialogueLines != null && dialogueLines.Length > 0 && currentLineIndex >= 0 && currentLineIndex < dialogueLines.Length)
            {
                dialogueText.text = dialogueLines[currentLineIndex];
                Debug.Log($"Displaying dialogue line {currentLineIndex}: {dialogueLines[currentLineIndex]}");
            }
            else
            {
                Debug.LogError($"Invalid dialogue line index: {currentLineIndex}. Ensure index is within bounds.");
            }
        }

        private void UpdateNavigationButtons()
        {
            bool hasPrevious = currentLineIndex > 0;
            bool hasNext = currentLineIndex < dialogueLines.Length - 1;
            bool isFinish = currentLineIndex == dialogueLines.Length - 1;

            previousButton.gameObject.SetActive(hasPrevious);
            nextButton.gameObject.SetActive(hasNext);
            finishButton.gameObject.SetActive(isFinish);

            Debug.Log($"Navigation Buttons - Previous: {hasPrevious}, Next: {hasNext}, Finish: {isFinish}");
        }

        #endregion

        #region Button Listeners
        private void NextDialogue()
        {
            if (currentLineIndex < dialogueLines.Length - 1)
            {
                currentLineIndex++;

                Debug.Log($"Next button clicked. Current line index: {currentLineIndex}");
                UpdateDialogueText();
                UpdateNavigationButtons();
            }
            else
            {
                Debug.LogWarning("Next button clicked but no more lines available.");
            }
        }

        private void PreviousDialogue()
        {
            if (currentLineIndex > 0)
            {
                currentLineIndex--;
                Debug.Log($"Previous button clicked. Current line index: {currentLineIndex}");
                UpdateDialogueText();
                UpdateNavigationButtons();
            }
            else
            {
                Debug.LogWarning("Previous button clicked but no more lines available.");
            }
        }

        #endregion
    }
}
