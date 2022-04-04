using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform moveTarget;

    private List<MyPathfinder.Node> pathToTarget = new List<MyPathfinder.Node>();

    private bool shouldCheckAgain = true;
    private float checkAgainIn = 0;

    public float curHealth;
    public bool isDead = false;

    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = MyGrid.singleton.enemyMaxHealth;
        line = GetComponent<LineRenderer>();
        moveTarget = MyGrid.singleton.helperEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0)
        {
            isDead = true;
        }

        checkAgainIn -= Time.deltaTime;
        if (checkAgainIn <= 0)
        {
            shouldCheckAgain = true;
            checkAgainIn = 1;
        }
        if (shouldCheckAgain) FindPath();

        if (!MyGrid.singleton.isPaused)
        {
            MoveAlongPath();
            if (pathToTarget != null && pathToTarget.Count >= 2)
            {
                //Damage enemy from spieks
                if (pathToTarget[0].painCaused == 5) curHealth -= 7.5f * Time.deltaTime; //5 causes about 10 damage total, 10 causes about 20 damage total

                //Damage enemy from lava
                if (pathToTarget[0].painCaused == 20) curHealth -= 5 * Time.deltaTime; //5 causes about 40 damage total

                if (pathToTarget[0].moveCost == 9999)
                {
                    //Damage wall
                    MyTerrainCube c = MyGrid.singleton.LookupCube(MyGrid.singleton.Lookup(transform.position));
                    c.curHealth -= 5 * Time.deltaTime;
                }
            }
            if (pathToTarget != null && pathToTarget.Count < 2)
            {
                //Damage Tower
                MyTerrainCube c = MyGrid.singleton.LookupCube(MyGrid.singleton.Lookup(transform.position));
                c.curHealth -= (curHealth / 2);
                isDead = true;
            }
        }
    }

    private void FindPath()
    {
        shouldCheckAgain = false;

        if (moveTarget && MyGrid.singleton)
        {
            MyPathfinder.Node start = MyGrid.singleton.Lookup(transform.position);
            MyPathfinder.Node end = MyGrid.singleton.Lookup(moveTarget.position);

            if (start == null || end == null || start == end)
            {
                pathToTarget.Clear(); //Emptying the list

                return;
            }

            pathToTarget = MyPathfinder.Solve(start, end);

            //Rendering the path on a LineRenderer
            Vector3[] positions = new Vector3[pathToTarget.Count];
            for (int i = 0; i < pathToTarget.Count; i++)
            {
                positions[i] = pathToTarget[i].position + new Vector3(0, .5f, 0);
            }
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }

    void MoveAlongPath()
    {
        if (pathToTarget == null) return;
        if (pathToTarget.Count < 2) return;

        Vector3 target = pathToTarget[1].position;
        target.y += 1;

        //To Do: Grab first item in path and move to that node
        if (pathToTarget[0].moveCost == 1) transform.position = Vector3.Lerp(transform.position, target, .01f); //Last parameter is its speed
        if (pathToTarget[0].moveCost == 9999 || pathToTarget[0].moveCost == 999) transform.position = Vector3.Lerp(transform.position, target, 0f); //Last parameter is its speed
        if (pathToTarget[0].moveCost == 10) transform.position = Vector3.Lerp(transform.position, target, .003f); //Last parameter is its speed
        if (pathToTarget[0].moveCost == 20) transform.position = Vector3.Lerp(transform.position, target, .001f); //Last parameter is its speed
        if (pathToTarget[0].moveCost == 5) transform.position = Vector3.Lerp(transform.position, target, .006f); //Last parameter is its speed


        float d = (pathToTarget[1].position - transform.position).magnitude;
        if (d < .25f)
        {
            shouldCheckAgain = true;
        }
    }
}
