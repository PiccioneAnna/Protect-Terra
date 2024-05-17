using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkGoal : Quest.QuestGoal
{
    public Actor actor;

    public override string GetDescription()
    {
        return $"Talk to {actor.name}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<TalkGameEvent>(OnTalk);
    }

    private void OnTalk(TalkGameEvent eventInfo)
    {
        if (eventInfo.actor == actor)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
