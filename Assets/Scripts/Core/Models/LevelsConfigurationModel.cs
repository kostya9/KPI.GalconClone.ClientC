using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfiguration", menuName = "Levels Configuration", order = 51)]
public class LevelsConfigurationModel : ScriptableObject
{

    [SerializeField] List<LevelPartView> levelParts;
    [SerializeField] List<RandomLevelPartView> randomLevelParts;

    public List<LevelPartView> GetLevelParts()
    {
        return levelParts;
    }

    public List<RandomLevelPartView> GetRandomLevelParts()
    {
        return randomLevelParts;
    }
}
