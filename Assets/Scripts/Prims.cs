using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prims
{
    private Cell[,] cells;
    private int sizeX;
    private int sizeY;
    private List<int[]> neighbours = new List<int[]>();
    private bool finished = false;
    private int[] currentPoint;
    public Prims(int sX, int sY)
    {
        sizeX = sX;
        sizeY = sY;
        cells = new Cell[sY, sX];
        initCells(Random.Range(0,sX-1),Random.Range(0,sY-1));
    }

    public void initCells(int startX,int startY)//Create All Cells Needed
    {
        Cell temp;
        for (int y=0;y<sizeY;y++)
        {
            for (int x=0;x<sizeX;x++)
            {
                temp = new Cell(true, true, false);
                cells[y,x] = temp;
            }
        }
        cells[sizeY-1,sizeX-1] = new Cell(true, false, false);
        cells[startY, startX] = new Cell(true, true, true);
        currentPoint = new int[] { startY, startX };
    }
    public Cell[,] getMaze()//Return Cell State
    {
        return cells;
    }
    public bool getFinished()//Discontinued Code for a Step Through Process
    {
        return finished;
    }
    public List<int[]> findNeighboursOrVisited(int x,int y,bool visited)//Find Neighbours or Visited Nodes around a Node
    {
        List<int[]> ret = new List<int[]>();
        if (x+1<sizeX && cells[y,x+1].getVisited()==visited)
        {
            ret.Add(new int[] { y, x + 1 });
        }
        if (x-1>-1 && cells[y,x-1].getVisited()==visited)
        {
            ret.Add(new int[] { y, x - 1 });
        }
        if (y+1<sizeY && cells[y+1,x].getVisited()==visited)
        {
            ret.Add(new int[] { y + 1, x });
        }
        if (y-1>-1 && cells[y-1,x].getVisited()==visited)
        {
            ret.Add(new int[] { y - 1, x });
        }
        return ret;
    }
    public void addToNeighbours(List<int[]> adder)//Add to Neighbour List
    {
        bool alreadyIn = false;
 
        foreach (int[] num in adder)
        {
            foreach(int[] checker in neighbours)
            {
                //if ((num[0].Equals(checker[0]) && num[1].Equals(checker[1])))
                if ((num[0]==checker[0]) && (num[1]==checker[1]))
                {
                    alreadyIn = true;
                }
            }
            if (!alreadyIn)
            {
                neighbours.Add(num);
            }
            alreadyIn = false;
        }
  
    }
    public void connect(int x, int y, int x1, int y1)//Connect 2 Nodes By removing the appropriate wall
    {
        if (x<x1||y<y1)
        {
            if (x<x1)
            {
                cells[y, x].updateRight(false);
            }
            else
            {
                cells[y, x].updateBottom(false);
            }
        }
        else
        {
            if (x1<x)
            {
                cells[y1, x1].updateRight(false);
            }
            else
            {
                cells[y1, x1].updateBottom(false);
            }
        }
        cells[y, x].updateVisited(true);
    }
    public void removeNeighbour(int[] remove)//Remove neighbour specified
    {

        List<int[]> temp = new List<int[]>();
        foreach(int[] points in neighbours)
        {
            //if (!((points[0].Equals(remove[0]) && points[1].Equals(remove[1]))))
            if (!(points[0]==remove[0] && points[1]==remove[1]))
            {
                temp.Add(points);
            }
        }
        neighbours = temp;
    }
    public void currentPointUpdate()//Update the currentPoint to the next Node
    {
        if (neighbours.Count != 0)
        {
            int randNum = Random.Range(0, neighbours.Count-1);
            int[] point = neighbours[randNum];
            List<int[]> connectList = findNeighboursOrVisited(point[1], point[0], true);
            randNum = Random.Range(0, connectList.Count-1);
            connect(point[1], point[0], connectList[randNum][1], connectList[randNum][0]);
            removeNeighbour(point);
            currentPoint = point;
        }
        else
        {
            finished = true;
        }
    }
    public void tick()//Run 1 tick
    {
        addToNeighbours(findNeighboursOrVisited(currentPoint[1], currentPoint[0], false));
        currentPointUpdate();
    }
    public void loop()//Run to completion
    {
        while (finished == false)
        {
            tick();
        }
    }

    //Text Based UI Stuff Below
    public string getCell(int x, int y)
    {
        return cells[y,x].toString();
    }
    public string getRow(int y)
    {
        string ret = "";
        for (int x=0;x<sizeX;x++)
        {
            ret += getCell(x, y);
        }
        return ret;
    }
    public void draw()
    {

        Debug.Log(" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _" + "\n" + 
            "I" + getRow(0) + "\n" +
            "I" + getRow(1) + "\n" +
            "I" + getRow(2) + "\n" +
            "I" + getRow(3) + "\n" +
            "I" + getRow(4) + "\n" +
            "I" + getRow(5) + "\n" +
            "I" + getRow(6) + "\n" +
            "I" + getRow(7) + "\n" +
            "I" + getRow(8) + "\n" +
            "I" + getRow(9) + "\n" +
            "I" + getRow(10) + "\n" +
            "I" + getRow(11) + "\n" +
            "I" + getRow(12) + "\n" +
            "I" + getRow(13) + "\n" +
            "I" + getRow(14) + "\n" +
            "I" + getRow(15));
    }
}
