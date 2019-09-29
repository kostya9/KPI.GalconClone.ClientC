using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevelCommand : BaseCommand
{

    [Inject] public LevelGeneratorService levelGenerator { get; set; } 

    public override void Execute()
    {
        var gameView = eventData.data as GameView;
        var levelParts = levelGenerator.GenerateLevel();
        for (int i = 0; i < levelParts.Count; i++)
        {
            levelParts[i].transform.parent = gameView.transform;
            levelParts[i].transform.position = Vector3.forward * 15f * (i + 1);
        }
        gameView.SetLevel(levelParts);
    }


}
