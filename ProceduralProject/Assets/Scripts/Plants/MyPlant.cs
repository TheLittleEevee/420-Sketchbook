using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MyPlant : MonoBehaviour
{
    [Range(2, 11)]
    public int iterations = 5;

    [Range(5, 30)]
    public float spreadDegrees = 10;

    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    private void OnValidate()
    {
        Build();
    }

    void Build()
    {
        //Making Storage for Instances
        List<CombineInstance> instances = new List<CombineInstance>();

        //Spawn the Instances
        Grow(instances, Vector3.zero, Quaternion.identity, new Vector3(.25f, 1, .25f), iterations);

        //Combining the Instances Together
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(instances.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter) meshFilter.mesh = mesh;
    }

    void Grow(List<CombineInstance> instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0)
    {
        if (num < 0) num = 0;
        if (num >= max) return; //Stop recursion

        //Make a cube mesh and add it to the list
        CombineInstance inst = new CombineInstance();
        inst.mesh = MeshTools.MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, scale);
        instances.Add(inst);

        int maxBranchPicker = (max - num);
        int branchPicker = (int)Random.Range(0, maxBranchPicker);
        print("Max: " + max + " Num: " + (num) + "    maxBP " + (maxBranchPicker) + " bP: " + branchPicker + " Final: " + (maxBranchPicker - branchPicker));

        //Add to num, calc %
        float percentAtEnd = ++num / (float)max;

        Vector3 endPoint = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        if ((pos - endPoint).sqrMagnitude < .1f) return; //Too small, stop recursion

        //Do recursion
        { //Temp scope
            Quaternion randRot = rot * Quaternion.Euler(spreadDegrees, Random.Range(-90, 90f), 0);
            Quaternion upRot = Quaternion.RotateTowards(rot, Quaternion.identity, 45);

            Quaternion newRot = Quaternion.Lerp(randRot, upRot, percentAtEnd);

            Grow(instances, endPoint, newRot, scale * .9f, max, num);
        }

        if (1 == (maxBranchPicker - branchPicker) || 2 == (maxBranchPicker - branchPicker))
        {
            float degrees = Random.Range(-90, 90);
            if (degrees > 0 && degrees < 45) degrees = 45;
            if (degrees < 0 && degrees > -45) degrees = -45;
            Quaternion newRot = rot * Quaternion.LookRotation(endPoint - pos) * Quaternion.Euler(0, 0, degrees);
            Grow(instances, endPoint, newRot, scale * .9f, max, num);
        }
        if (3 == (maxBranchPicker - branchPicker))
        {
            float degrees = Random.Range(0, 90);
            Quaternion newRot = rot * Quaternion.LookRotation(endPoint - pos) * Quaternion.Euler(0, 0, degrees);
            Grow(instances, endPoint, newRot, scale * .9f, max, num);

            degrees = Random.Range(-90, 0);
            newRot = rot * Quaternion.LookRotation(endPoint - pos) * Quaternion.Euler(0, 0, degrees);
            Grow(instances, endPoint, newRot, scale * .9f, max, num);
        }
        if (4 == (maxBranchPicker - branchPicker))
        {
            float degrees = Random.Range(30, 90);
            Quaternion newRot = rot * Quaternion.LookRotation(endPoint - pos) * Quaternion.Euler(0, 0, degrees);
            Grow(instances, endPoint, newRot, scale * .9f, max, num);

            degrees += Random.Range(-90, -30);
            newRot = rot * Quaternion.LookRotation(endPoint - pos) * Quaternion.Euler(0, 0, degrees);
            Grow(instances, endPoint, newRot, scale * .9f, max, num);

            degrees = Random.Range(-30, 30);
            newRot = rot * Quaternion.LookRotation(endPoint - pos) * Quaternion.Euler(0, 0, degrees);
            Grow(instances, endPoint, newRot, scale * .9f, max, num);
        }
    }
}
