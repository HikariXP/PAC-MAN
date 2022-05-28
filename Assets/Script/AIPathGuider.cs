using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base on special rule set up target GridPos for Enmey;
public class AIPathGuider : MonoBehaviour
{
    public static AIPathGuider Instance;

    public AIPathFinder Blinky;
    public AIPathFinder Pinky;
    public AIPathFinder Inky;
    public AIPathFinder Clyde;

    public PacMan player;

    public Vector2Int[] PinkyTargetOffset = new Vector2Int[] { new Vector2Int(3, 0), new Vector2Int(-3, 3), new Vector2Int(-3, 0), new Vector2Int(0, -3) };

    public float RefreshCoolDown = 0.5f;
    public float TimePass = 0f;

    public bool EscapeMode = false;

    public Vector2Int EscapeGridPos;

    public Vector2Int[] EscapeGridPoss = new Vector2Int[] { new Vector2Int(17, 20), new Vector2Int(17, 1), new Vector2Int(1, 1), new Vector2Int(1, 20) };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.GameWinEvent += OnGameWin;
    }

    public void OnGameWin()
    {
        Blinky.canMove = false;
        Pinky.canMove = false;
        Inky.canMove = false;
        Clyde.canMove = false;

        player.canMove = false;
    }

    public void Update()
    {
        TimePass += Time.deltaTime;
        if (TimePass > RefreshCoolDown)
        {
            TimePass = 0f;
            SetTarget();
        }
    }




    public void SetTarget()
    {
        SetTarget_Blinky();
        SetTarget_Pinky();
        SetTarget_Inky();
        SetTarget_Clyde();
        SetEscapeGridPos();
    }



    public void SetTarget_Blinky()
    {
        if (Blinky.aiStatus == AIPathFinder.Status.normal)
        {
            Blinky.SetTargetGridPos(player.CurrentGridPos);
        }
        else if(Blinky.aiStatus == AIPathFinder.Status.nerf)
        {
            Blinky.SetTargetGridPos(EscapeGridPos);
        }
    }    
    public void SetTarget_Pinky()
    {
        if (Pinky.aiStatus == AIPathFinder.Status.normal)
        {
            Vector2Int tempTarget = Vector2Int.zero;
            for (int i = 0; i < 4; i++)
            {
                if (player.currentDir == i)
                {
                    tempTarget = player.CurrentGridPos + PinkyTargetOffset[i];
                }
            }

            tempTarget.x = tempTarget.x < 0 ? 0 : tempTarget.x > 18 ? 18 : tempTarget.x;
            tempTarget.y = tempTarget.y < 0 ? 0 : tempTarget.y > 21 ? 21 : tempTarget.y;

            if (MapInfo.MAP[tempTarget.x, tempTarget.y] == 0)
            {
                Pinky.SetTargetGridPos(tempTarget);
            }
        }
        else if (Pinky.aiStatus == AIPathFinder.Status.nerf)
        {
            Pinky.SetTargetGridPos(EscapeGridPos);
        }
    }    
    public void SetTarget_Inky()
    {
        if (Inky.aiStatus == AIPathFinder.Status.normal)
        {
            Vector2Int tempTarget = Vector2Int.zero;
            tempTarget = player.CurrentGridPos - Blinky.CurrentGridPos;

            tempTarget.x = tempTarget.x < 0 ? 0 : tempTarget.x > 18 ? 18 : tempTarget.x;
            tempTarget.y = tempTarget.y < 0 ? 0 : tempTarget.y > 21 ? 21 : tempTarget.y;


            if (MapInfo.MAP[tempTarget.x, tempTarget.y] == 0)
            {
                Inky.SetTargetGridPos(tempTarget);
            }
        }
        else if (Inky.aiStatus == AIPathFinder.Status.nerf)
        {
            Inky.SetTargetGridPos(EscapeGridPos);
        }
    }    
    public void SetTarget_Clyde()
    {
        if (Clyde.aiStatus == AIPathFinder.Status.normal)
        {
            if (Vector2Int.Distance(player.CurrentGridPos, Clyde.CurrentGridPos) > 5f)
            {
                Clyde.SetTargetGridPos(player.CurrentGridPos);
            }
            else
            {
                Clyde.SetTargetGridPos(Vector2Int.one);
            }
        }
        else if (Clyde.aiStatus == AIPathFinder.Status.nerf)
        {
            Clyde.SetTargetGridPos(EscapeGridPos);
        }
    }

    public void SetEscapeGridPos()
    {
        if (player.CurrentGridPos.x > 9)
        {
            if (player.CurrentGridPos.y > 11)
            {
                EscapeGridPos = EscapeGridPoss[0];
            }
            else
            {
                EscapeGridPos = EscapeGridPoss[1];
            }
        }
        else
        {
            if (player.CurrentGridPos.y > 11)
            {
                EscapeGridPos = EscapeGridPoss[3];
            }
            else
            {
                EscapeGridPos = EscapeGridPoss[2];
            }
        }
    }

    public void OnNerf()
    {
        Blinky.Nerf();
        Pinky.Nerf();
        Inky.Nerf();
        Clyde.Nerf();
    }

    


}
