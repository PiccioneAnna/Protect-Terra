using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMobGoal : Quest.QuestGoal
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
        name = obj.GetComponent<Creature>().name;
        EventManager.Instance.AddListener<KillMobGameEvent>(OnKill);
    }

    private void OnKill(KillMobGameEvent ge)
    {
        if (ge.mob.name == name)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
