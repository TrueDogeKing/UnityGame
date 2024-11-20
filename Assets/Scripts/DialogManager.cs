using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    private bool isTyping = false;

    [Range(0.01f, 30.0f)]
    [SerializeField]
    public float typingSpeed = 0.2f;

    public Animator animator;

    private DialogueLine currentLine;

    private void Awake()
    {
        typingSpeed = typingSpeed / 360;
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        animator.SetTrigger("show");

        StartCoroutine(WaitForAnim());

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(1.5f);
    }

    public void DisplayNextDialogueLine()
    {

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueArea.text = currentLine.line; 
            isTyping = false;
            return;
        }

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        isTyping = true;
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.SetTrigger("hide");
    }

    private void Update()
    {
        if (!isDialogueActive)
            return;

        if(Input.GetKeyDown(KeyCode.E))
            DisplayNextDialogueLine();
    }
}