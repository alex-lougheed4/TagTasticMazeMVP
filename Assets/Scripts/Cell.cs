using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private bool bottom;
    private bool visited;
    private bool right;
    public Cell(bool b,bool r, bool v) 
    {
        bottom = b;
        right = r;
        visited = v;
    }
    public bool getBottom()
    {
        return bottom;
    }
    public bool getRight()
    {
        return right;
    }
    public bool getVisited()
    {
        return visited;
    }
    public void updateBottom(bool b)
    {
        bottom = b;
    }
    public void updateRight(bool r)
    {
        right = r;
    }
    public void updateVisited(bool v)
    {
        visited = v;
    }
    public string toString()
    {
        string ret = "";
        if (bottom)
        {
            ret += "_";
        }
        else
        {
            ret += " ";
        }
        if (right)
        {
            ret += "I";
        }
        else
        {
            ret += " ";
        }
        return ret;
    }
    
}

