using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCropGoal : Quest.QuestGoal
{
    public Crop crop;

    public override string GetDescription()
    {
        return $"Plant {crop.name}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<SeedGameEvent>(OnSeed);
    }

    private void OnSeed(SeedGameEvent ge)
    {
        if (ge.crop == crop)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
