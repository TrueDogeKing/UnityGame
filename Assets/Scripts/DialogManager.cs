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
    public float typingSpeed = 0.30f;

    public Animator animator;

    private DialogueLine currentLine;

    private Coroutine autoHideCoroutine;
    private const float idleTimeToHide = 10f; // Time in seconds to hide the dialog
    private Coroutine typingCoroutine;
    private Coroutine TypeSentenceCoroutine;

    private Vector2 targetIconSize = new Vector2(100f, 100f); // Width and Height in pixels

    private AudioSource audioSource; // Added audio source for managing audio

    private void Awake()
    {
        typingSpeed = typingSpeed / 450;
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        ResetAutoHideTimer();

        isDialogueActive = true;

        animator.SetTrigger("show");

        typingCoroutine = StartCoroutine(WaitForAnim());

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
            StopCoroutine(typingCoroutine);
            StopCoroutine(TypeSentenceCoroutine);
            dialogueArea.text = currentLine.line;
            isTyping = false;
            return;
        }

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Stop current audio when switching to the next line
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        NormalizeSpriteSize(characterIcon); // Ensure sprite is scaled correctly

        StopCoroutine(typingCoroutine);

        TypeSentenceCoroutine = StartCoroutine(TypeSentence(currentLine));

        // Play the audio for the new dialogue line
        if (currentLine.audioClip != null)
        {
            audioSource.clip = currentLine.audioClip;
            audioSource.Play();
        }
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        isTyping = true;
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            ResetAutoHideTimer();
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.SetTrigger("hide");

        // Stop audio when dialogue ends
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void Update()
    {
        if (!isDialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.E))
            DisplayNextDialogueLine();
    }

    private void ResetAutoHideTimer()
    {
        if (autoHideCoroutine != null)
            StopCoroutine(autoHideCoroutine);

        autoHideCoroutine = StartCoroutine(AutoHideDialog());
    }

    IEnumerator AutoHideDialog()
    {
        yield return new WaitForSeconds(idleTimeToHide);

        if (isDialogueActive)
        {
            EndDialogue();
        }
    }

    private void NormalizeSpriteSize(Image icon)
    {
        if (icon.sprite == null)
            return;

        // Set a uniform target size for all icons
        float targetSize = Mathf.Min(targetIconSize.x, targetIconSize.y);

        // Get the dimensions of the sprite
        float originalWidth = icon.sprite.bounds.size.x;
        float originalHeight = icon.sprite.bounds.size.y;

        // Calculate the scale to fit the sprite within the target size while maintaining aspect ratio
        float scale = targetSize / Mathf.Max(originalWidth, originalHeight);

        // Apply the calculated scale to the Image's RectTransform
        icon.rectTransform.sizeDelta = new Vector2(originalWidth * scale, originalHeight * scale);
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using UnityEngine;
//using TMPro;

//public class DialogManager : MonoBehaviour
//{
//    public static DialogManager Instance;

//    public Image characterIcon;
//    public TextMeshProUGUI characterName;
//    public TextMeshProUGUI dialogueArea;

//    private Queue<DialogueLine> lines;

//    public bool isDialogueActive = false;
//    private bool isTyping = false;

//    [Range(0.01f, 30.0f)]
//    [SerializeField]
//    public float typingSpeed = 0.2f;

//    public Animator animator;

//    private DialogueLine currentLine;

//    private Coroutine autoHideCoroutine;
//    private const float idleTimeToHide = 10f; // Time in seconds to hide the dialog
//    private Coroutine typingCoroutine;
//    private Coroutine TypeSentenceCoroutine;

//    private Vector2 targetIconSize = new Vector2(100f, 100f); // Width and Height in pixels


//    private void Awake()
//    {
//        typingSpeed = typingSpeed / 360;
//        if (Instance == null)
//            Instance = this;

//        lines = new Queue<DialogueLine>();
//    }

//    public void StartDialogue(Dialogue dialogue)
//    {
//        ResetAutoHideTimer();

//        isDialogueActive = true;

//        animator.SetTrigger("show");

//        typingCoroutine=StartCoroutine(WaitForAnim());

//        lines.Clear();

//        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
//        {
//            lines.Enqueue(dialogueLine);
//        }

//        DisplayNextDialogueLine();

//    }

//    IEnumerator WaitForAnim()
//    {
//        yield return new WaitForSeconds(1.5f);
//    }

//    public void DisplayNextDialogueLine()
//    {

//        if (isTyping)
//        {
//            StopCoroutine(typingCoroutine);
//            StopCoroutine(TypeSentenceCoroutine);
//            dialogueArea.text = currentLine.line; 
//            isTyping = false;
//            return;
//        }

//        if (lines.Count == 0)
//        {
//            EndDialogue();
//            return;
//        }

//        currentLine = lines.Dequeue();

//        characterIcon.sprite = currentLine.character.icon;
//        characterName.text = currentLine.character.name;

//        NormalizeSpriteSize(characterIcon); // Ensure sprite is scaled correctly

//        StopCoroutine(typingCoroutine);

//        TypeSentenceCoroutine =StartCoroutine(TypeSentence(currentLine));

//    }

//    IEnumerator TypeSentence(DialogueLine dialogueLine)
//    {
//        isTyping = true;
//        dialogueArea.text = "";
//        foreach (char letter in dialogueLine.line.ToCharArray())
//        {
//            ResetAutoHideTimer();
//            dialogueArea.text += letter;
//            yield return new WaitForSeconds(typingSpeed);
//        }
//        isTyping = false;
//    }

//    void EndDialogue()
//    {
//        isDialogueActive = false;
//        animator.SetTrigger("hide");
//    }

//    private void Update()
//    {
//        if (!isDialogueActive)
//            return;

//        if (Input.GetKeyDown(KeyCode.E))
//            DisplayNextDialogueLine();
//    }

//    private void ResetAutoHideTimer()
//    {
//        if (autoHideCoroutine != null)
//            StopCoroutine(autoHideCoroutine);

//        autoHideCoroutine = StartCoroutine(AutoHideDialog());
//    }

//    IEnumerator AutoHideDialog()
//    {
//        yield return new WaitForSeconds(idleTimeToHide);

//        if (isDialogueActive)
//        {
//            EndDialogue();
//        }
//    }

//    private void NormalizeSpriteSize(Image icon)
//    {
//        if (icon.sprite == null)
//            return;

//        // Set a uniform target size for all icons
//        float targetSize = Mathf.Min(targetIconSize.x, targetIconSize.y);

//        // Get the dimensions of the sprite
//        float originalWidth = icon.sprite.bounds.size.x;
//        float originalHeight = icon.sprite.bounds.size.y;

//        // Calculate the scale to fit the sprite within the target size while maintaining aspect ratio
//        float scale = targetSize / Mathf.Max(originalWidth, originalHeight);

//        // Apply the calculated scale to the Image's RectTransform
//        icon.rectTransform.sizeDelta = new Vector2(originalWidth * scale, originalHeight * scale);
//    }

//    public bool IsDialogueActive()
//    {
//        return isDialogueActive;
//    }

//}