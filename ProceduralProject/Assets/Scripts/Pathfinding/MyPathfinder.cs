using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPathfinder : MonoBehaviour
{
    public class Node
    {
        public Vector3 position;
        public float moveCost = 1;
        public float painCaused = 0;
        public float health = 9999;

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
        public void DoHeuristic(Node end)
        {
            //Euclidean Heuristic
            Vector3 d = end.position - this.position;
            H = d.magnitude;
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
        while (open.Count > 0)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
