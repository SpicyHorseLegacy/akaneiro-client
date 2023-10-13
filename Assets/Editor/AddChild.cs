using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

public class AddChild : ScriptableObject
{
    [MenuItem ("GameObject/Add Child ^n")]
    static void MenuAddChild()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

        foreach(Transform transform in transforms)
        {
            GameObject newChild = new GameObject("_Child");
            newChild.transform.parent = transform;
			newChild.transform.localPosition = Vector3.zero;
			Selection.activeGameObject = newChild;
        }
    }

    [MenuItem ("GameObject/Show Selection Info")]
    static void MenuShowSelectionCount()
    {
        int gCount = 0;
        int mCount = 0;
        int tCount = 0;
        Hashtable hTable = new Hashtable();

        foreach( GameObject go in  Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel) )
        {
            gCount++;

            // Count mesh
            Component[] mf = go.GetComponentsInChildren(typeof(MeshFilter));
            if (mf != null && mf.Length > 0)
            {
                for (int j = 0; j < mf.Length; ++j)
                {
                    mCount++;
                    int inc = ((MeshFilter)mf[j]).sharedMesh.triangles.Length / 3;
                    tCount += inc;

                    hTable.SumObject(mf[j].gameObject.name, inc);
                }
            }

            // Count Skined mesh
            Component[] smr = go.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
            if (smr != null && smr.Length > 0)
            {
                for (int j = 0; j < smr.Length; ++j)
                {
                    mCount++;
                    int inc = ((SkinnedMeshRenderer)smr[j]).sharedMesh.triangles.Length / 3;
                    tCount += inc;

                    hTable.SumObject(smr[j].gameObject.name, inc);
                }
            }

            // Count Particle Emitter
            Component[] pae = go.GetComponentsInChildren(typeof(ParticleEmitter));
            if (pae != null && pae.Length > 0)
            {
                for (int j = 0; j < pae.Length; ++j)
                {
                    mCount++;
                    int inc = ((ParticleEmitter)pae[j]).particleCount * 2;
                    tCount += inc;

                    hTable.SumObject(pae[j].gameObject.name, inc);
                }
            }

            // Count Particle System
            Component[] pas = go.GetComponentsInChildren(typeof(ParticleSystem));
            if (pas != null && pas.Length > 0)
            {
                for (int j = 0; j < pas.Length; ++j)
                {
                    mCount++;
                    int inc = ((ParticleSystem)pas[j]).particleCount * 2;
                    tCount += inc;

                    hTable.SumObject(pas[j].gameObject.name, inc);
                }
            }
        }

        StringBuilder s = new StringBuilder();
        s.AppendFormat("Selection: {0}   Objs: {3}   Meshes: {1}   Tris: {2} \t#NO\t{2}\n", gCount, mCount, tCount, hTable.Count);

        foreach (DictionaryEntry de in hTable)
        {
             s.AppendFormat("{0}\t {1} \t {2}\n", de.Key, ((Vector2)de.Value).x,((Vector2)de.Value).y);
        }

        Debug.LogWarning(s.ToString());
    }


    [MenuItem("GameObject/Show Selection Info", true)]
    public static bool ValidateMenuShowSelectionCount()
    {
        return Selection.activeGameObject;
    }
}

// Extension for hash table
public static class HashtableExtension
{
    public static void SumObject(this Hashtable target, string name, int n)
    {

        Vector2 sum = new Vector2(0f, 0f);

        if (target.ContainsKey(name))
        {
            sum = (Vector2)target[name];
        }

        sum.x += 1;
        sum.y += n;

        target[name] = sum ;
    }
}