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
        private void Start()
        {
            ResetUI();
            AddButtonListeners();
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
            }
        }

        private void UpdateNavigationButtons()
        {
            previousButton.gameObject.SetActive(currentLineIndex > 0);
            nextButton.gameObject.SetActive(currentLineIndex < dialogueLines.Length - 1);
            finishButton.gameObject.SetActive(currentLineIndex == dialogueLines.Length - 1);
        }

        #endregion

        #region Button Listeners

        private void AddButtonListeners()
        {
            nextButton.onClick.AddListener(NextDialogue);
            previousButton.onClick.AddListener(PreviousDialogue);
            skipButton.onClick.AddListener(SkipDialogue);
            finishButton.onClick.AddListener(EndDialogue);
        }

        private void NextDialogue()
        {
            if (currentLineIndex < dialogueLines.Length - 1)
            {
                currentLineIndex++;
                UpdateDialogueText();
                UpdateNavigationButtons();
            }
        }

        private void PreviousDialogue()
        {
            if (currentLineIndex > 0)
            {
                currentLineIndex--;
                UpdateDialogueText();
                UpdateNavigationButtons();
            }
        }

        private void SkipDialogue()
        {
            EndDialogue();
        }

        #endregion
    }
}
