using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoeDirtGoal : Quest.QuestGoal
{
    public override string GetDescription()
    {
        return "Hoe Dirt";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<HoeGameEvent>(OnHoe);
    }

    private void OnHoe(HoeGameEvent ge)
    {
        CurrentAmount++;
        Evaluate();
    }
}
