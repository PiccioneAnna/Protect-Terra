using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TillTileGoal : Quest.QuestGoal
{
    public override string GetDescription()
    {
        return "Till Dirt";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<TillGameEvent>(OnTill);
    }

    private void OnTill(TillGameEvent ge)
    {
        CurrentAmount++;
        Evaluate();
    }
}
