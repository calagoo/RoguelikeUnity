using System;
using System.Collections.Generic;
using UnityEngine;

public struct VertexData : IEquatable<VertexData>
{
    public Vector3 position;
    public Vector3 normal;
    public Vector2 uv;

    public VertexData(Vector3 pos, Vector3 norm, Vector2 uv)
    {
        this.position = pos;
        this.normal = norm;
        this.uv = uv;
    }

    public bool Equals(VertexData other)
    {
        return position == other.position && normal == other.normal && uv == other.uv;
    }

    public override bool Equals(object obj)
    {
        return obj is VertexData other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + position.GetHashCode();
            hash = hash * 31 + normal.GetHashCode();
            hash = hash * 31 + uv.GetHashCode();
            return hash;
        }
    }

}

public static class MeshUtils
{
    public static void WeldVertices(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;
        int[] triangles = mesh.triangles;

        Dictionary<VertexData, int> vertexMap = new Dictionary<VertexData, int>();
        List<Vector3> weldedVertices = new List<Vector3>();
        List<Vector3> weldedNormals = new List<Vector3>();
        List<Vector2> weldedUVs = new List<Vector2>();
        List<int> weldedTriangles = new List<int>();

        int[] vertexRemap = new int[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            VertexData vData = new VertexData(vertices[i], normals[i], uvs[i]);

            if (!vertexMap.ContainsKey(vData))
            {
                int newIndex = weldedVertices.Count;
                vertexMap[vData] = newIndex;

                weldedVertices.Add(vData.position);
                weldedNormals.Add(vData.normal);
                weldedUVs.Add(vData.uv);
            }

            vertexRemap[i] = vertexMap[vData];
        }

        for (int i = 0; i < triangles.Length; i++)
        {
            weldedTriangles.Add(vertexRemap[triangles[i]]);
        }

        mesh.Clear();
        mesh.vertices = weldedVertices.ToArray();
        mesh.normals = weldedNormals.ToArray();
        mesh.uv = weldedUVs.ToArray();
        mesh.triangles = weldedTriangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        Debug.Log($"✅ Welded mesh: {vertices.Length} → {weldedVertices.Count} vertices");
    }
}
