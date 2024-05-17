using Crafting;
using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward
{
    public Item item;
    public int itemCount;
}

[Serializable]
public class DialogueLine
{
    public DialogueType dialogueType;
    public int playerChoice;
    public string lineA;
    public string lineB;
    public string lineC;
}

[Serializable]
public enum DialogueType
{
    NPCTalking,
    NPCResponding,
    PlayerTalking
}

/// <summary>
/// Dialogue container consists of NPC Dialogue, rewards, and quest to activate
/// </summary>
[CreateAssetMenu(menuName = "Data/Dialogue/Container")]
public class DialogueContainer : ScriptableObject
{
    public List<DialogueLine> linesBQC; // Before Quest Completion
    public List<DialogueLine> linesAQC; // After Quest Completion
    public List<Reward> rewardsBQC; // Before Quest Completion Rewards
    public List<Reward> rewardsAQC; // After Quest Completion Rewards
    public List<Item> addedShopItems;
    public List<CraftRecipe> addedRecipes; //can be split into recipes gained before quest completion and after
    public List<Reward> removedPlayerItems;
    public List<Quest> quests;
    public bool dialogueCompletion;
}
