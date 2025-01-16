using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Added this line to fix the Image class error

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
    public AudioClip audioClip; // New field for assigning audio to each line
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogTrigger : MonoBehaviour
{
    private AudioSource source;

    public Dialogue dialogue;

    private bool spoke = false;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void TriggerDialogue()
    {
        DialogManager.Instance.StartDialogue(dialogue);
        PlayDialogueAudio(0); // Start playing audio for the first line
    }

    private void PlayDialogueAudio(int lineIndex)
    {
        if (lineIndex >= 0 && lineIndex < dialogue.dialogueLines.Count)
        {
            AudioClip clip = dialogue.dialogueLines[lineIndex].audioClip;
            if (clip != null)
            {
                source.PlayOneShot(clip, AudioListener.volume);
            }
        }
    }

    public void OnDialogueLineChanged(int lineIndex)
    {
        // This method should be called by the DialogManager when the active line changes
        PlayDialogueAudio(lineIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spoke)
            return;

        if (collision.CompareTag("Player"))
        {
            if (!DialogManager.Instance.IsDialogueActive())
            {
                TriggerDialogue();
                spoke = true;
            }
        }
    }
}


//public class DialogueCharacter
//{
//    public string name;
//    public Sprite icon;
//}

//[System.Serializable]
//public class DialogueLine
//{
//    public DialogueCharacter character;
//    [TextArea(3, 10)]
//    public string line;

//}

//[System.Serializable]
//public class Dialogue
//{
//    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
//}




//public class DialogTrigger : MonoBehaviour
//{
//    private AudioSource source;

//    public Dialogue dialogue;
//    [SerializeField]
//    private AudioClip sound;

//    private bool spoke = false;

//    void Awake()
//    {
//        source = GetComponent<AudioSource>();
//    }

//    public void TriggerDialogue()
//    {
//        DialogManager.Instance.StartDialogue(dialogue);
//        source.PlayOneShot(sound, AudioListener.volume);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (spoke)
//            return;

//        if (collision.tag == "Player")
//        {
//            if (!DialogManager.Instance.IsDialogueActive())
//            {
//                TriggerDialogue();
//                spoke = true;
//            }
//        }
//    }

//}