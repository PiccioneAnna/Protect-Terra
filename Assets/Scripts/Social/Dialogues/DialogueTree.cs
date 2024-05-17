using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialogue/Tree")]
public class DialogueTree : ScriptableObject
{
    public List<DialogueContainer> dialogueContainers;

    public DialogueContainer taskContainer;

    public Actor actor;

    private int dialogueIndex = 0;

    public void Refresh()
    {
        // Temporary quest reset for now, long term would be refresh upon new save file
        foreach (DialogueContainer dialogue in dialogueContainers)
        {
            dialogue.dialogueCompletion = false;
            dialogue.quests[0].Initialized = false;
        }
    }
    
    // Theoretically goes through dialogue list and returns dialogue matching player's level
    public DialogueContainer GetCurrentDialogue()
    {
        DialogueContainer dialogue = dialogueContainers[dialogueIndex];

        if (dialogue.dialogueCompletion == true)
        {
            dialogueIndex++;
        }

        if (dialogue.dialogueCompletion == false)
        {
            if(dialogue.quests.Count > 0 && dialogue.quests[0].Initialized == false)
            {
                dialogue.quests[0].Initialize();
            }
            return dialogue;
        }

        return null;
    }
}
