using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public class Node
    {
        public Vector3 position;
        public float moveCost = 1;

        public float G { get; private set; }
        public float H { get; private set; }
        public float F
        {
            get
            {
                return G + H;
            }
        }

        public List<Node> neighbors = new List<Node>();

        public Node parent { get; private set; }

        public void UpdateParentAndG(Node parent, float extraG = 0)
        {
            this.parent = parent;
            if (parent != null)
            {
                G = parent.G + moveCost + extraG;
            }
            else
            {
                G = extraG;
            }
        }

        //Makes an educated guess as to how far we are from end
        public void DoHeuristic (Node end)
        {
            //Euclidean Heuristic
            Vector3 d = end.position - this.position;
            H = d.magnitude;

            //Manhattan Heuristic
            //H = d.x + d.y + d.z;
        }
    }

    public static List<Node> Solve(Node start, Node end)
    {
        if (start == null || end == null) return new List<Node>();

        List<Node> open = new List<Node>(); //All the nodes that have been discovered, but not "scanned"
        List<Node> closed = new List<Node>(); //These nodes are "scanned"

        start.UpdateParentAndG(null);
        open.Add(start);

        //Travel from start to end
        while(open.Count > 0)
        {
            //Find node in OPEN list with SMALLEST F value

            float bestF = 0;
            Node current = null;
            foreach (Node n in open)
            {
                if (n.F < bestF || current == null)
                {
                    current = n;
                    bestF = n.F;
                }
            }

            //If this node is the end, stop looping
            if (current == end)
            {
                break;
            }
            bool isDone = false;
            foreach (Node neighbor in current.neighbors)
            {
                if (!closed.Contains(neighbor)) //Node not in CLOSED
                {
                    if (!open.Contains(neighbor)) //Node not in OPEN
                    {
                        open.Add(neighbor);

                        float dis = (neighbor.position - current.position).magnitude;

                        neighbor.UpdateParentAndG(current, dis); //Set child's parent and G value
                        if (neighbor == end)
                        {
                            isDone = true;
                        }
                        neighbor.DoHeuristic(end);
                    }
                    else //If node is already in the OPEN list
                    {
                        //If this path to neighbor has lower G than previous path to neighbor
                        float dis = (neighbor.position - current.position).magnitude;

                        if (current.G + neighbor.moveCost + dis < neighbor.G)
                        {
                            //It's shorter to move to neighbor from current
                            neighbor.UpdateParentAndG(current, dis);
                        }
                    }
                }
            }
            closed.Add(current);
            open.Remove(current);
            if (isDone) break;
        }
        //Travel from end to start, building path

        List<Node> path = new List<Node>();
        /*
        Node temp = end;
        while (temp != null)
        {
            path.Add(temp);
            temp = temp.parent;
        }
        */
        for (Node temp = end; temp != null; temp = temp.parent)
        {
            path.Add(temp);
        }

        //Reverse path
        path.Reverse();

        return path;
    }
}

/*
Dijkstra Algorithm:
    Keep a list of OPEN Nodes
    Foreach node:
        Record how far node is from start
        Add neighbors to OPEN list
            If is END, return chain of Nodes
        Move to CLOSED list

Greedy Best-Search Algorithm:
    Keep a list of OPEN Nodes
    Pick one node most likely to be closer to END (Heuristic)
        Add neighbors to OPEN list
            If is END, return chain of Nodes
        Move to CLOSED list

A* Algorithm:
    Keep a list of OPEN Nodes
    Pick one node with lowest cost (cost = how far from start + how far from end)
        Add neighbors to OPEN list
            Record how far node is from start
            If is END, return chain of Nodes
        Move to CLOSED list

    F = G + H
    Heurestic
        Euclidean (line to end (Pythagorean))
        Manhattan (dx + dy (no diagonal lines))
*/