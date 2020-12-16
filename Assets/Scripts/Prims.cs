using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prims
{
    private Cell[,] cells = new Cell[16,16];
    private List<int[]> neighbours = new List<int[]>();
    private bool finished = false;
    private int[] currentPoint;
    public Prims(int x,int y)
    {
        initCells(x, y);
    }

    public void initCells(int startX,int startY)//Create All Cells Needed
    {
        Cell temp;
        for (int y=0;y<16;y++)
        {
            for (int x=0;x<16;x++)
            {
                temp = new Cell(true, true, false);
                cells[y,x] = temp;
            }
        }
        cells[15,15] = new Cell(true, false, false);
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
        if (x+1<16 && cells[y,x+1].getVisited()==visited)
        {
            ret.Add(new int[] { y, x + 1 });
        }
        if (x-1>-1 && cells[y,x-1].getVisited()==visited)
        {
            ret.Add(new int[] { y, x - 1 });
        }
        if (y+1<16 && cells[y+1,x].getVisited()==visited)
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
        //Debug.Log("Neighbours = " + string.Join("",
        //     new List<int[]>(neighbours)
        //     .ConvertAll(i => "(X: "+i[1].ToString() +" Y: " +i[0].ToString()+") ")
        //     .ToArray()));
        //Debug.Log("Adding = " + string.Join("",
        //     new List<int[]>(adder)
        //     .ConvertAll(i => "(X: " + i[1].ToString() + " Y: " + i[0].ToString() + ") ")
        //     .ToArray()));
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
        //Debug.Log("Neighbours after adding = " + string.Join("",
        //     new List<int[]>(neighbours)
        //     .ConvertAll(i => "(X: " + i[1].ToString() + " Y: " + i[0].ToString() + ") ")
        //     .ToArray()));
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
        //Debug.Log("Neighbours before removing= " + string.Join("",
        //     new List<int[]>(neighbours)
        //     .ConvertAll(i => "(X: " + i[1].ToString() + " Y: " + i[0].ToString() + ") ")
        //     .ToArray()));
        //Debug.Log("removing point = (x: " + remove[1] + " y: " + remove[0] + ")");
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
        //Debug.Log("Neighbours after removing= " + string.Join("",
        //     new List<int[]>(neighbours)
        //     .ConvertAll(i => "(X: " + i[1].ToString() + " Y: " + i[0].ToString() + ") ")
        //     .ToArray()));
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
        for (int x=0;x<16;x++)
        {
            ret += getCell(x, y);
        }
        return ret;
    }
    public void draw()
    {
        //Debug.Log(" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
        //Debug.Log("|" + getRow(0));
        //Debug.Log("|" + getRow(1));
        //Debug.Log("|" + getRow(2));
        //Debug.Log("|" + getRow(3));
        //Debug.Log("|" + getRow(4));
        //Debug.Log("|" + getRow(5));
        //Debug.Log("|" + getRow(6));
        //Debug.Log("|" + getRow(7));
        //Debug.Log("|" + getRow(8));
        //Debug.Log("|" + getRow(9));
        //Debug.Log("|" + getRow(10));
        //Debug.Log("|" + getRow(11));
        //Debug.Log("|" + getRow(12));
        //Debug.Log("|" + getRow(13));
        //Debug.Log("|" + getRow(14));
        //Debug.Log("|" + getRow(15));
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
