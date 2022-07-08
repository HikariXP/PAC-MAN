using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public static PacMan Instance;

    public float speed = 5f;
    //0:right - 1:up - 2:left - 3:down
    public int currentDir = 0;
    public int nextDir = 0;

    public Vector2Int CurrentGridPos;
    public Vector2Int GridPosRoaming;

    public enum eCanMoveDir
    { 
        right = 0,
        up,
        left,
        down
    }

    public List<eCanMoveDir> CanMoveDirs = new List<eCanMoveDir>();

    private Rigidbody2D rigid;
    private Animator anima;

    //is Next pos can reach?
    public bool canMove = true;

    private Vector2[] directions = new Vector2[]
    { Vector2.right,Vector2.up,Vector2.left,Vector2.down};

    private KeyCode[] keys = new KeyCode[]
        { KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.DownArrow,
          KeyCode.D,KeyCode.W,KeyCode.A,KeyCode.S};

    public Sprite texture;

    private void Awake()
    {
        Instance = this;

        rigid = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
    }

    private void Start()
    {
        //rigid = GetComponent<Rigidbody2D>();
        //anima = GetComponent<Animator>();

        //ReachGirdPos();
    }

    private void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKey(keys[i])) nextDir = i % 4;
        }
        //RefreshCurrentGridPos();
        
        ChangeDirAnimation();
    }

    private void FixedUpdate()
    {
        //RefreshCurrentGridPos();

        if (canMove)
        {
            Move();
        }
        else rigid.velocity = Vector2.zero;


        if (Vector2.Distance(transform.position, new Vector2(CurrentGridPos.x+0.5f,CurrentGridPos.y+0.5f)) < 0.1f)
        {
            ReachGirdPos();
        }
        
        TryChangeDir();
    }

    private void LateUpdate()
    {
        RefreshCurrentGridPos();
    }


    public void Move()
    {
        Vector2 vel =Vector2.zero;
        for (int i = 0; i < 4; i++)
        {
            if (currentDir == i)
            {
                vel = directions[i];
            }
        }
        rigid.velocity = vel * speed;
    }

    public void Dead()
    {
        canMove = false;
        anima.Play("pacman_Dead");
    }

    /// <summary>
    /// 吃豆人重生
    /// </summary>
    public void Respawn()
    {
        nextDir = 0;

        if (anima != null)
        {
            //anima.Play("pacman_" + currentDir);
            anima.Play("pacman_0");
        }
    }


    private void RefreshCurrentGridPos()
    {
        CurrentGridPos = Vector2Int.FloorToInt(gameObject.transform.position);
        GridPosRoaming = CurrentGridPos;
    }

    private void ChangeDirAnimation()
    {
        if (canMove)
        {
            anima.Play("pacman_" + currentDir);
        }
    }

    public void ReachGirdPos()
    {
        RefreshDirCanChange();
    }

    //需要添加无法索引时的报错，MapInfo.MAP[x,y]可能会获取不了，比如在边界的情况。
    public void RefreshDirCanChange()
    {
        CanMoveDirs.Clear();
        if (MapInfo.MAP[GridPosRoaming.x + 1, GridPosRoaming.y] == 0)
        {
            CanMoveDirs.Add(eCanMoveDir.right);
        }
        if (MapInfo.MAP[GridPosRoaming.x, GridPosRoaming.y+1] == 0)
        {
            CanMoveDirs.Add(eCanMoveDir.up);
        }
        if (MapInfo.MAP[GridPosRoaming.x - 1, GridPosRoaming.y] == 0)
        {
            CanMoveDirs.Add(eCanMoveDir.left);
        }
        if (MapInfo.MAP[GridPosRoaming.x, GridPosRoaming.y-1] == 0)
        {
            CanMoveDirs.Add(eCanMoveDir.down);
        }
    }

    public void TryChangeDir()
    {
        if (currentDir != nextDir)
        {
            for (int i = 0; i < CanMoveDirs.Count; i++)
            {
                if ((int)CanMoveDirs[i] == nextDir)
                {
                    currentDir = nextDir;
                    transform.position = new Vector2(CurrentGridPos.x + 0.5f, CurrentGridPos.y + 0.5f);
                }
            }
        }
    }
}
