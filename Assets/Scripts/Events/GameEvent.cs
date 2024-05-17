using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string EventDescription;
}

public class BuildingGameEvent : GameEvent
{
    public string BuildingName;

    public BuildingGameEvent(string name)
    {
        BuildingName = name;
    }
}

public class TalkGameEvent : GameEvent
{
    public Actor actor;
    public TalkGameEvent(Actor a)
    {
        actor = a;
    }
}

public class HoeGameEvent : GameEvent
{
    public HoeGameEvent() { }
}

public class TillGameEvent : GameEvent
{
    public TillGameEvent() { }
}

public class SeedGameEvent : GameEvent
{
    public Crop crop;
    public SeedGameEvent(Crop c)
    {
        crop = c;
    }
}

public class HarvestGameEvent : GameEvent
{
    public Crop crop;
    public HarvestGameEvent(Crop c)
    {
        crop = c;
    }
}

public class ClearObjectGameEvent : GameEvent
{
    public Resource resource;
    public ClearObjectGameEvent(Resource r) 
    {
        resource = r;
    }
}

public class KillMobGameEvent : GameEvent
{
    public Creature mob;
    public KillMobGameEvent(Creature m)
    {
        mob = m;
    }
}

public class VisitGameEvent : GameEvent
{
    public string scene;
    public VisitGameEvent(string s)
    {
        scene = s;
    }
}

public class ItemGameEvent : GameEvent
{
    public ItemGameEvent() { }
}
