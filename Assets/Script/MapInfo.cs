using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public TextAsset MapText;
    public TextAsset DotText;

    static public int[,] MAP;
    static public int[,] DOTMAP;

    static public int W;
    static public int H;

    private void Start()
    {
        MAP = LoadMapText(MapText);
        DOTMAP = LoadMapText(DotText);
    }

    public int[,] LoadMapText(TextAsset textAsset)
    {
        // Read in the map data as ana array of lines
        string[] lines = textAsset.text.Split('\n');
        H = lines.Length;
        string[] tileNums = lines[0].Trim().Split(' ');// A space between ' '   
        W = tileNums.Length;

        // Place the map data into a 2D Array for very fast access
        int[,] temp = new int[W, H];            // Generate a 2D array of the right size
        for (int j = 0; j < H; j++)
        {
            // Iterate over every tileNum string
            tileNums = lines[j].Trim().Split(' ');
            for (int i = 0; i < W; i++)
            {
                temp[i, j] = int.Parse(tileNums[i]);
            }
        }

        Debug.Log(textAsset.name+ " size: " + W + " wide by " + H + " high");
        return temp;
    }

    static public BoundsInt GetBoundsInt()
    {
        BoundsInt bounds = new BoundsInt(0, 0, 0, W, H, 1);
        return bounds;
    }

    
    public static int GET_MAP_AT_VECTOR2(Vector2 pos)
    {
        Vector2Int posInt = Vector2Int.FloorToInt(pos);
        return MAP[posInt.x, posInt.y];
    }
}
