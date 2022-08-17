using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinder : MonoBehaviour
{
    private Rigidbody2D rigid;

    public Vector2Int StartGridPos;

    public Vector2Int TargetGridPos;

    public Vector2Int CurrentGridPos;

    public Vector2Int NextGridPos;

    private Vector2Int EmergencyGridPos;

    private Vector2Int RespawnGridPos = new Vector2Int(9,11);

    public int currentDir = 0;

    public List<Vector2Int> RoundGridPos = new List<Vector2Int>();

    public List<NodeBase> nodeBases = new List<NodeBase>();

    public float speed = 5f;

    public bool canMove;

    public float NerfTime = 0f;

    public bool isNerf = false;

    public bool isDead = false;

    public CircleCollider2D ccolld2d;

    public SpriteRenderer renderer;

    public Sprite NormalSprite;

    public Sprite NerfSprite;

    public Sprite DeadSprite;

    //Ignore the rules to the target, it is not generic and should not be called from anywhere else;
    public enum Status
    { 
        normal,
        nerf,
        dead
    }

    public Status aiStatus;


    private Vector2[] directions = new Vector2[]
    { Vector2.right,Vector2.up,Vector2.left,Vector2.down};

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ccolld2d = GetComponent<CircleCollider2D>();
        CheckCurrentGridPos();
        NextGridPos = CurrentGridPos;
        ReachPathPoint();
        //StartGridPos = CurrentGridPos;
        renderer = GetComponent<SpriteRenderer>();
        NormalSprite = renderer.sprite;
    }

    private void Update()
    {
        CheckCurrentGridPos();
        if (Vector2.Distance(transform.position, new Vector2(NextGridPos.x + 0.5f, NextGridPos.y + 0.5f)) < 0.1f)
        {
            ReachPathPoint();
        }

        if (isNerf)
        {
            NerfTime -= Time.deltaTime;
            
        }
        if (NerfTime < 0f)
        {
            if (!isDead)
            {
                isNerf = false;
                aiStatus = Status.normal;
            }
        } 

        ChangeSprite();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
        else rigid.velocity = Vector2.zero;
        
    }

    private void Move()
    {
        Vector2 vel = Vector2.zero;
        for (int i = 0; i < 4; i++)
        {
            if (currentDir == i)
            {
                vel = directions[i];
            }
        }
        rigid.velocity = vel * speed;


    }
    public void CheckCurrentGridPos()
    {
        CurrentGridPos = Vector2Int.FloorToInt(transform.position);
        
    }

    public void ReachPathPoint()
    {
        if (isDead)
        {
            if (CurrentGridPos == RespawnGridPos)
                Respawn();
        }
        gameObject.transform.position = new Vector2(CurrentGridPos.x + 0.5f, CurrentGridPos.y + 0.5f);
        ResearchPath();
        ChangeDir();
    }

    public void ResearchPath()
    {
        //如果可以提前获取到位置究竟
        RoundGridPos.Clear();
        RoundGridPos.Add(new Vector2Int(CurrentGridPos.x + 1, CurrentGridPos.y));
        RoundGridPos.Add(new Vector2Int(CurrentGridPos.x - 1, CurrentGridPos.y));
        RoundGridPos.Add(new Vector2Int(CurrentGridPos.x, CurrentGridPos.y + 1));
        RoundGridPos.Add(new Vector2Int(CurrentGridPos.x, CurrentGridPos.y - 1));



        switch (currentDir)
        {
            case 0:
                RoundGridPos.Remove(CurrentGridPos+Vector2Int.left);
                EmergencyGridPos = CurrentGridPos + Vector2Int.left;
                break;            
            case 1:
                RoundGridPos.Remove(CurrentGridPos+Vector2Int.down);
                EmergencyGridPos = CurrentGridPos + Vector2Int.down;
                break;            
            case 2:
                RoundGridPos.Remove(CurrentGridPos+Vector2Int.right);
                EmergencyGridPos = CurrentGridPos + Vector2Int.right;
                break;            
            case 3:
                RoundGridPos.Remove(CurrentGridPos+Vector2Int.up);
                EmergencyGridPos = CurrentGridPos + Vector2Int.up;
                break;
        }

        nodeBases.Clear();

        foreach (Vector2Int v2 in RoundGridPos)
        {
            //can move
            if (MapInfo.MAP[v2.x, v2.y] == 0)
            {
                float G = 1;
                float H = Vector2Int.Distance(v2, TargetGridPos);
                nodeBases.Add(new NodeBase(G, H, v2));
            }
        }

        //Let it get away from the BaseArea(W:7~11,H:10-12)
        foreach (NodeBase no in nodeBases)
        {
            if (no.NodeGridPos.x >= 7 && no.NodeGridPos.x <= 11)
            {
                if (no.NodeGridPos.y >= 10 && no.NodeGridPos.y <= 12)
                {
                    if (aiStatus!=Status.dead)
                    {
                        //no.G *= 100;
                        no.H *= 100;
                    }
                }
            }
        }


        nodeBases.Sort();

        if (nodeBases.Count == 0)
        {
            NextGridPos = EmergencyGridPos;
        }
        else
        {
            NextGridPos = nodeBases[0].NodeGridPos;
        } 
    }

    public void ChangeDir()
    {
        int x = CurrentGridPos.x - NextGridPos.x;
        int y = CurrentGridPos.y - NextGridPos.y;

        if (x < 0 && y == 0) currentDir = 0;
        if (x > 0 && y == 0) currentDir = 2;

        if (x == 0 && y < 0) currentDir = 1;
        if (x == 0 && y > 0) currentDir = 3;
    }

    public void ResetPos(Vector3 vector3)
    {
        transform.position = vector3;
        CheckCurrentGridPos();
        ReachPathPoint();
        canMove = false;
        ResetStatus();
    }

    public void SetTargetGridPos(Vector2Int target)
    {
        if (!isDead)
        {
            //if alive, follow the guide;
            TargetGridPos = target;
        }
        else
        {
            // if dead, lead to the respawn point;
            TargetGridPos = RespawnGridPos;
        }
    }

    public void Nerf()
    {
        if (!isDead)
        {
            NerfTime = 10f;
            isNerf = true;
            aiStatus = Status.nerf;
        }
    }

    public void BeEatByPacman()
    {
        //Clear Nerf Status
        isNerf = false;
        NerfTime = 0f;
        //Turn to Dead Status
        //ccolld2d.isTrigger = true;
        isDead = true;
        aiStatus = Status.dead;
        SetTargetGridPos(Vector2Int.zero);
    }

    public void Respawn()
    {
        isDead = false;
        isNerf = false;
        aiStatus = Status.normal;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PAC-MAN"))
        {
            if (isNerf)
            {
                BeEatByPacman();
            }
            else
            {
                if (!isDead)
                {
                    GameManager.Instance.PlayerBeCatched();
                    collision.gameObject.GetComponent<PacMan>().Dead();
                }
            }
        }
    }

    //Call by the Player be eat or Reset Game(em?)
    public void ResetStatus()
    {
        aiStatus = Status.normal;
        isDead = false;
        isNerf = false;
        NerfTime = 0f;
    }

    public void ChangeSprite()
    {
        if (aiStatus == Status.normal)
        {
            renderer.sprite = NormalSprite;
        }
        if (aiStatus == Status.nerf)
        {
            renderer.sprite = NerfSprite;
        }
        if(aiStatus == Status.dead)
        {
            renderer.sprite = DeadSprite;
        }
    }

}

public class NodeBase:IComparable<NodeBase>
{
    public float G;
    public float H;
    public float F => G + H;

    public Vector2Int NodeGridPos;
    public NodeBase(float g, float h,Vector2Int location)
    {
        G = g;
        H = h;
        NodeGridPos = location;
    }

    public int CompareTo(NodeBase other)
    {
        //return F > other.F ? 1 : F == other.F ? 0 : -1;

        if (F > other.F)
        {
            return 1;
        }
        else if (F == other.F)
        {
            if (H > other.H)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }
}
