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
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}


public class DialogTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool spoke = false;


    public void TriggerDialogue()
    {
        if (spoke)
            return;

        DialogManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerDialogue();
            spoke = true;
        }
    }

}