using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorService
{
    private const string CONFIG_PATH = "LevelsConfiguration";
    private LevelsConfigurationModel config;

    public LevelGeneratorService()
    {
        config = Resources.Load<LevelsConfigurationModel>(CONFIG_PATH);
    }

    public List<LevelPartView> GenerateLevel()
    {
        var level = new List<LevelPartView>();
        var rangomLevelParts = config.GetRandomLevelParts();
        LevelExit start = LevelExit.Three;
        for (int i = 0; i < 4; i++)
        {
            var levelPart = rangomLevelParts[Random.Range(0, rangomLevelParts.Count)];
            var go = GameObject.Instantiate(levelPart);
            start = go.GenerateLevel(start);
            level.Add(go);
        }
        return level;
    }
}

public enum LevelExit
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 4,
    Four = 8,
    Five = 16
}
