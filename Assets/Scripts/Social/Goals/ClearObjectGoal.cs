using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObjectGoal : Quest.QuestGoal
{
    public GameObject obj;
    private new string name;

    public override string GetDescription()
    {
        return $"Clear {name}";
    }

    public override void Initialize()
    {
        base.Initialize();
        name = obj.GetComponent<Resource>().name;
        EventManager.Instance.AddListener<ClearObjectGameEvent>(OnClear);
    }

    private void OnClear(ClearObjectGameEvent ge)
    {
        if (ge.resource.name == name)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
