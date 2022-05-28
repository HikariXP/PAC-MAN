using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instance;

    public Tilemap CollisionMap;

    private TileBase[] collisionTileBase;

    public Tile CollisionTile;

    public GameObject DotPerfab;
    public GameObject PowerDotPerfab;

    public Transform DOTS;

    public Vector2 Vector2Offset = new Vector2(0.5f,0.5f);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetCollisionTiles();
        SetDots();
    }

    private void SetCollisionTiles()
    {
        collisionTileBase = GetCollisionTiles();
        CollisionMap.SetTilesBlock(MapInfo.GetBoundsInt(), collisionTileBase);
        Debug.Log("CollisionTiles has set");
    }
    public void SetDots()
    {
        //Clear Dots in scene;
        foreach (Transform go in DOTS)
        {
            Destroy(go.gameObject);
        }

        //Set Dot;
        for (int y = 0; y < MapInfo.H; y++)
        {
            for (int x = 0; x < MapInfo.W; x++)
            {
                if (MapInfo.DOTMAP[x, y] == 0)
                {
                    var tempDot =  Instantiate(DotPerfab, DOTS);
                    tempDot.transform.position = new Vector2(x+0.5f, y+0.5f);
                }
                if (MapInfo.DOTMAP[x, y] == 2)
                {
                    //var tempDot = Instantiate(DotPerfab, DOTS);
                    //PowerDot Has Bug
                    var tempDot = Instantiate(PowerDotPerfab, DOTS);
                    tempDot.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                }
            }
        }
        Debug.Log("Dots Set");
    }

    public TileBase[] GetCollisionTiles()
    {
        TileBase[] mapTiles = new TileBase[MapInfo.W * MapInfo.H];
        for (int y = 0; y < MapInfo.H; y++)
        {
            for (int x = 0; x < MapInfo.W; x++)
            { 
                if (MapInfo.MAP[x, y] == 1)
                {
                    mapTiles[y * MapInfo.W + x] = CollisionTile;
                }
            }
        }
        return mapTiles;
    }


}
