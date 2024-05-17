using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestWindow : MonoBehaviour
{
    //fields for all the elements
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform goalsContent;

    public void Initialize(Quest quest)
    {
        titleText.text = quest.Information.Name;
        descriptionText.text = quest.Information.Description;

        foreach (var goal in quest.Goals)
        {
            GameObject goalObj = Instantiate(goalPrefab, goalsContent);
            goalObj.transform.Find("Text").GetComponent<Text>().text = goal.GetDescription();

            GameObject countObj = goalObj.transform.Find("Count").gameObject;

            if (goal.Completed)
            {
                goalObj.transform.Find("Done").gameObject.SetActive(true);
                countObj.GetComponent<Text>().text = goal.RequiredAmount + "/" + goal.RequiredAmount;
            }
            else
            {
                countObj.GetComponent<Text>().text = goal.CurrentAmount + "/" + goal.RequiredAmount;
            }

        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);

        ClearWindow();
    }

    public void ClearWindow()
    {
        for (int i = 0; i < goalsContent.childCount; i++)
        {
            Destroy(goalsContent.GetChild(i).gameObject);
        }
    }
}
