using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCropGoal : Quest.QuestGoal
{
    public Crop crop;

    public override string GetDescription()
    {
        return $"Harvest {crop.name}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<HarvestGameEvent>(OnHarvest);
    }

    private void OnHarvest(HarvestGameEvent ge)
    {
        if (ge.crop == crop)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
