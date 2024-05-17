using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject questPrefab;
    [SerializeField] private Transform questsContent;
    [SerializeField] private UnityEngine.GameObject questHolder;
    public Image QuestUI;

    public Controller player;

    public List<GameObject> questUINodes;

    public List<Quest> CurrentQuests;
    public List<Quest> CompletedQuests;

    private void Start()
    {
        questHolder.SetActive(false);
        RefreshQuestList();
    }

    private void RefreshQuestList()
    {
        foreach (var quest in CurrentQuests)
        {
            InitializeQuest(quest);
        }
    }

    private void InitializeQuest(Quest quest)
    {
        GameObject questObj = Instantiate(questPrefab, questsContent);

        // If the quest has already been initialized then break out 
        foreach (GameObject q in questUINodes)
        {
            if (q.transform.Find("Title").GetComponent<TMP_Text>().text == quest.Information.Name) 
            {
                Destroy(questObj);
                return; 
            }
        }

        questUINodes.Add(questObj);

        quest.Initialize();
        quest.QuestCompleted.AddListener(OnQuestCompleted);

        //questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;
        questObj.transform.Find("Title").GetComponent<TMP_Text>().text = quest.Information.Name;
        questHolder.GetComponent<QuestWindow>().Initialize(quest);

        questObj.GetComponent<Button>().onClick.AddListener(delegate
        {
            questHolder.GetComponent<QuestWindow>().ClearWindow();
            questHolder.GetComponent<QuestWindow>().Initialize(quest);
            questHolder.SetActive(true);
        });
    }

    public void AddQuest(Quest quest)
    {
        if (CurrentQuests.Contains(quest)) { return; }
        CurrentQuests.Add(quest);
        RefreshQuestList();
    }

    public void Build(string buildingName)
    {
        EventManager.Instance.QueueEvent(new BuildingGameEvent(buildingName));
    }

    public void Hoe()
    {
        EventManager.Instance.TriggerEvent(new HoeGameEvent());
    }

    public void Till()
    {
        EventManager.Instance.TriggerEvent(new TillGameEvent());
    }

    public void Seed(Crop crop)
    {
        EventManager.Instance.TriggerEvent(new SeedGameEvent(crop));
    }

    public void Harvest(Crop crop)
    {
        EventManager.Instance.TriggerEvent(new HarvestGameEvent(crop));
    }

    public void ClearObject(Resource resource)
    {
        EventManager.Instance.TriggerEvent(new ClearObjectGameEvent(resource));
    }

    public void KillMob(Creature mob)
    {
        EventManager.Instance.TriggerEvent(new KillMobGameEvent(mob));
    }

    public void Visit(string scene)
    {
        EventManager.Instance.TriggerEvent(new VisitGameEvent(scene));
    }

    public void CheckIfPlayerHasObject()
    {
        EventManager.Instance.TriggerEvent(new ItemGameEvent());
    }

    private void OnQuestCompleted(Quest quest)
    {
        //questsContent.GetChild(CurrentQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
        Debug.Log("QuestComplete");
        CurrentQuests.Remove(quest);
        CompletedQuests.Add(quest);
    }
}
