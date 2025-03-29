using UnityEngine;
using UnityEngine.UIElements;

public class MeshMorpher : MonoBehaviour
{
    // Script morphs between two meshes, using lerp
    public GameObject mesh1;
    public GameObject mesh2;
    public float morphSpeed = 1.0f;
    GameObject meshNew;
    void Start()
    {
        meshNew = Instantiate(mesh1, transform);
        meshNew.transform.localPosition = Vector3.zero;
        meshNew.transform.localScale = Vector3.one;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MorphMesh();
    }


    void MorphMesh()
    {
        Vector3[] v1 = mesh1.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] v2 = mesh2.GetComponent<MeshFilter>().mesh.vertices;

        if (v1.Length != v2.Length)
        {
            Debug.LogError("Meshes have different vertex counts.");
            return;
        }

        Vector3[] v = new Vector3[v1.Length];

        float t = Mathf.Clamp01(Time.time * morphSpeed); // Clamp to [0,1]

        for (int i = 0; i < v.Length; i++)
        {
            v[i] = Vector3.Lerp(v1[i], v2[i], t);
        }

        Mesh mesh = meshNew.GetComponent<MeshFilter>().mesh;
        mesh.vertices = v;
        mesh.RecalculateBounds();  // Optional, but good to have
        mesh.RecalculateNormals(); // Optional, if lighting is affected
    }
}
