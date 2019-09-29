using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelPartView : LevelPartView
{
    [SerializeField] List<LevelLayer> levelCells;

    [SerializeField] List<LevelPartCell> constantColorCells;

    public ColorModel trueColorModel = new ColorModel(Color.red, "canTouch", true);
    public ColorModel falseColorModel = new ColorModel(Color.white, "untouchable", false);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LevelExit GenerateLevel(LevelExit levelIn)
    {
        foreach (var cell in constantColorCells)
        {
            cell.SetColor(cell.defaultColor ? trueColorModel : falseColorModel);
        }

        foreach (var layerCells in levelCells)
        {
            foreach (var cell in layerCells.cells)
            {
                cell.SetColor(falseColorModel);
            }
        }
        this.levelPartIn = levelIn;
        var previousLevelPath = levelIn;
        var currentLevelPath = LevelExit.None;
        int layer = 0;
        int cellIndex = 0;
        while (layer < levelCells.Count)
        {
            var enumerator = LevelExitsEnum();
            int pathCount = GetPathCount(previousLevelPath);
            while (enumerator.MoveNext())
            {
                if ((enumerator.Current & previousLevelPath) == enumerator.Current)
                {
                    cellIndex = LevelExitToIndex(enumerator.Current);
                    if (layer < levelCells.Count && cellIndex < levelCells[layer].cells.Count)
                    {
                        levelCells[layer].cells[cellIndex].SetColor(trueColorModel);
                    }
                    if (Random.Range(0, 3) != 0)
                    {
                        var nearElement = GetNearElement(enumerator.Current);
                        cellIndex = LevelExitToIndex(nearElement);
                        if (layer < levelCells.Count && cellIndex < levelCells[layer].cells.Count)
                        {
                            levelCells[layer].cells[cellIndex].SetColor(trueColorModel);
                        }
                        currentLevelPath |= nearElement;
                        if (pathCount < 2)
                        {
                            if (Random.Range(0, 3) != 0)
                            {
                                nearElement = GetNearElement(enumerator.Current);
                                if ((nearElement & currentLevelPath) != 0)
                                {
                                    pathCount++;
                                }
                                cellIndex = LevelExitToIndex(nearElement);
                                if (layer < levelCells.Count && cellIndex < levelCells[layer].cells.Count)
                                {
                                    levelCells[layer].cells[cellIndex].SetColor(trueColorModel);
                                }
                                currentLevelPath |= nearElement;
                            }
                        }
                    }
                    else
                    {
                        currentLevelPath |= enumerator.Current;
                    }
                }
            }
            previousLevelPath = currentLevelPath;
            currentLevelPath = LevelExit.None;
            layer++;
        }
        return previousLevelPath;
    }

    public IEnumerator<LevelExit> LevelExitsEnum()
    {
        yield return LevelExit.One;
        yield return LevelExit.Two;
        yield return LevelExit.Three;
        yield return LevelExit.Four;
        yield return LevelExit.Five;
    }

    public LevelExit GetNearElement(LevelExit levelExit)
    {
        if (LevelExit.One == (levelExit & LevelExit.One))
        {
            return LevelExit.Two;
        }
        else if (LevelExit.Five == (levelExit & LevelExit.Five))
        {
            return LevelExit.Four;
        }
        else
        {
            return (LevelExit)(Random.Range(0, 2) == 0 ? (int)levelExit << 1 : (int)levelExit >> 1);
        }
    }

    private int LevelExitToIndex(LevelExit levelExit)
    {
        if (LevelExit.One == (levelExit & LevelExit.One))
        {
            return 0;
        }
        if (LevelExit.Two == (levelExit & LevelExit.Two))
        {
            return 1;

        }
        if (LevelExit.Three == (levelExit & LevelExit.Three))
        {
            return 2;

        }
        if (LevelExit.Four == (levelExit & LevelExit.Four))
        {
            return 3;

        }
        if (LevelExit.Five == (levelExit & LevelExit.Five))
        {
            return 4;
        }
        return 0;
    }

    private int GetPathCount(LevelExit path)
    {
        int count = 0;
        int number = (int)path;
        while (number != 0)
        {
            if ((number & 1) == 1)
            {
                count++;
            }
            number = number >> 1;
        }
        return count;
    }

    
    
}

[System.Serializable]
public class LevelLayer
{
    [SerializeField]
    public List<LevelPartCell> cells;
}