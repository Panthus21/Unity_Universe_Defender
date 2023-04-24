using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class GridGenerator : MonoBehaviour
{
    //Grid Var
    public int gridWidth = 30;
    public int gridHeight = 15;
    public GameObject preTile;

    //Start end Point Variabels
    public Vector3 startPoint;
    public Vector3 endPoint;
    public List<GameObject> pathTiles = new List<GameObject>();
    void Start()
    {        
        Random.InitState(System.DateTime.Now.Millisecond);
        //Random Start and Endpoint 
        startPoint = new Vector3(29, 0,Random.Range(0, 15));
        endPoint = new Vector3(0, 0, Random.Range(0, 15));
        //Creating The Grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {   
                GameObject Tile = Instantiate(preTile,this.transform);
                Tile.transform.position = new Vector3(x, 0, z);
                Tile.transform.parent = transform;
                Tile.name = preTile.name + "_" + x + "_0_" + z;
            }
        }
        //Creating the Path Betwenn start- and endpoint
        Vector3 up = new Vector3(0,0,1);
        Vector3 down = new Vector3(0,0,-1);
        Vector3 left = new Vector3(-1,0,0);
        Vector3 curPos = startPoint;
        Vector3 lastMove = new Vector3(0,0,0);
        for (int i = 0; i < 1000; i++)        
        {
            Debug.Log(curPos);
            if (curPos == endPoint)
            {
                for (int a = 0; a < pathTiles.Count; a++)
                {
                    pathTiles[a].SetActive(false);                   
                }
                break;
            }

            //Set all Moves that are Valid
            bool bLeft = false;
            bool bUp = false;
            bool bDown = false;
            if (curPos.x != 1) //before the last row we can move in every direction only not backwards
            {
                bLeft = true;
                if (curPos.z != 0 && lastMove != up)
                {
                    bDown = true;
                }
                if (curPos.z !=14 && lastMove != down)
                {
                    bUp = true;
                }
            }
            else // Prefents to have no path at the last row
            {
                if (endPoint.z == curPos.z)
                {
                    bLeft = true;
                }
                else if (endPoint.z > curPos.z)
                {
                    bUp = true;
                }
                else
                {
                    bDown = true;
                }
            }
            

            //Calculation the Path
            if (curPos == startPoint) //First Move can only go forward
            {
                Tuple<Vector3,Vector3> result = MoveLeft(curPos, left);
                curPos = result.Item1;
                lastMove = result.Item2;
            }
            else if (bDown && bUp &&bLeft) // has 3 ways to up,down and left 
            {                
                int random = Random.Range(1, 4);
                if (random == 1)
                {
                    Tuple<Vector3, Vector3> result = MoveDown(curPos, down);
                    curPos = result.Item1;
                    lastMove = result.Item2;
                }
                else if (random == 2)
                {
                    Tuple<Vector3, Vector3> result = MoveUp(curPos, up);
                    curPos = result.Item1;
                    lastMove = result.Item2;
                }
                else
                {
                    if (lastMove == left || curPos.x ==1)
                    {
                        Tuple<Vector3, Vector3> result = MoveLeft(curPos, left);
                        curPos = result.Item1;
                        lastMove = result.Item2;
                    }
                    else 
                    {
                        Tuple<Vector3, Vector3> result = MoveDoubleLeft(curPos, left);
                        curPos = result.Item1;
                        lastMove = result.Item2;
                    }
                    
                }
            }
            else if (bDown && bLeft) // has 2 ways to go down and left 
            {
                int random = Random.Range(1, 3);
                if (random == 1)
                {
                    Tuple<Vector3, Vector3> result = MoveDown(curPos, down);
                    curPos = result.Item1;
                    lastMove = result.Item2;
                }                
                else
                {
                    if (lastMove == left || curPos.x == 1)
                    {
                        Tuple<Vector3, Vector3> result = MoveLeft(curPos, left);
                        curPos = result.Item1;
                        lastMove = result.Item2;
                    }
                    else
                    {
                        Tuple<Vector3, Vector3> result = MoveDoubleLeft(curPos, left);
                        curPos = result.Item1;
                        lastMove = result.Item2;
                    }
                }
            }
            else if (bUp && bLeft) // has 2 ways to go up and left 
            {
                int random = Random.Range(1, 3);
                Debug.Log(random);
                if (random == 1)
                {
                    Tuple<Vector3, Vector3> result = MoveUp(curPos, up);
                    curPos = result.Item1;
                    lastMove = result.Item2;
                }
                else
                {
                    if (lastMove == left || curPos.x == 1)
                    {
                        Tuple<Vector3, Vector3> result = MoveLeft(curPos, left);
                        curPos = result.Item1;
                        lastMove = result.Item2;
                    }
                    else
                    {
                        Tuple<Vector3, Vector3> result = MoveDoubleLeft(curPos, left);
                        curPos = result.Item1;
                        lastMove = result.Item2;
                    }
                }
            }
            else if(bLeft) // has 1 way to go left 
            {
                if (lastMove == left || curPos.x == 1)
                {
                    Tuple<Vector3, Vector3> result = MoveLeft(curPos, left);
                    curPos = result.Item1;
                    lastMove = result.Item2;
                }
                else
                {
                    Tuple<Vector3, Vector3> result = MoveDoubleLeft(curPos, left);
                    curPos = result.Item1;
                    lastMove = result.Item2;
                }
            }
            else if (bDown) // has 1 way to go down 
            {
                Tuple<Vector3, Vector3> result = MoveDown(curPos, down);
                curPos = result.Item1;
                lastMove = result.Item2;
            }
            else if (bUp) // has 1 way to go up 
            {
                Tuple<Vector3, Vector3> result = MoveUp(curPos, up);
                curPos = result.Item1;
                lastMove = result.Item2;
            }
           // Debug.Log("Down"+bDown);
            //Debug.Log("Up"+bUp);
            //Debug.Log("Left"+bLeft);            
        }
    }
    //Function Path move Left 
    public Tuple<Vector3,Vector3> MoveLeft(Vector3 curPos, Vector3 dir)
    {
        curPos += dir;
        pathTiles.Add(GameObject.Find(preTile.name +"_"+ curPos.x + "_" + curPos.y + "_" + curPos.z));

        return Tuple.Create(curPos,dir);
    }
    //Moves 2 steps
    public Tuple<Vector3, Vector3> MoveDoubleLeft(Vector3 curPos, Vector3 dir)
    {
        curPos += dir;
        pathTiles.Add(GameObject.Find(preTile.name + "_" + curPos.x + "_" + curPos.y + "_" + curPos.z));
        curPos += dir;
        pathTiles.Add(GameObject.Find(preTile.name + "_" + curPos.x + "_" + curPos.y + "_" + curPos.z));
        return Tuple.Create(curPos, dir);
    }
    //Function Path move Up
    public Tuple<Vector3,Vector3> MoveUp(Vector3 curPos, Vector3 dir)
    {
        curPos += dir;
        pathTiles.Add(GameObject.Find(preTile.name+"_" + curPos.x + "_" + curPos.y + "_" + curPos.z));
        return Tuple.Create(curPos,dir);
    }
    //Function Path mvoe Down
    public Tuple<Vector3, Vector3> MoveDown(Vector3 curPos, Vector3 dir)
    {
        curPos += dir;
        pathTiles.Add(GameObject.Find(preTile.name + "_" + curPos.x + "_" + curPos.y + "_" + curPos.z));
        return Tuple.Create(curPos, dir);
    }
}


