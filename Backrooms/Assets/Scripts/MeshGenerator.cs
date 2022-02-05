using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = this.mesh;
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        this.vertices = new Vector3[(this.xSize + 1) * (this.zSize + 1)];

        for (int i = 0, x = 0; x <= this.xSize; x++)
        {
            for (int z = 0; z <= this.zSize; z++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                this.vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        this.triangles = new int[this.xSize * this.zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int x = 0; x < this.xSize; x++)
        {
            for (int z = 0; z < this.zSize; z++)
            {
                this.triangles[tris + 0] = vert + 0;
                this.triangles[tris + 1] = vert + this.xSize + 1;
                this.triangles[tris + 2] = vert + 1;
                this.triangles[tris + 3] = vert + 1;
                this.triangles[tris + 4] = vert + this.xSize + 1;
                this.triangles[tris + 5] = vert + this.xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // this.vertices = new Vector3[] {
        //     new Vector3(0, 0, 0),
        //     new Vector3(0, 0, 1),
        //     new Vector3(1, 0, 0),
        //     new Vector3(1, 0, 1)
        // };

        // triangles = new int[] {
        //     0, 1, 2,
        //     1, 3, 2
        // }
    }

    private void UpdateMesh()
    {
        this.mesh.Clear();

        this.mesh.vertices = this.vertices;
        this.mesh.triangles = this.triangles;

        this.mesh.RecalculateNormals();
        this.mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = this.mesh;
    }

    private void OnDrawGizmos()
    {
        if (this.vertices == null)
            return;

        for (int i = 0; i < this.vertices.Length; i++)
        {
            Gizmos.DrawSphere(this.vertices[i], 0.1f);
        }
    }
}
