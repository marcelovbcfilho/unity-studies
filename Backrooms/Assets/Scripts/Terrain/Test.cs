using System;
using UnityEngine;

namespace Terrain
{
    public class Test : MonoBehaviour
    {
        private GameObject _mesh;
        public Vector3 _position;
        public Vector3 _positionChunk;
        public TerrainData _terrainData;
        public Bounds _bounds;

        public float _minHeight = 0;
        public float _maxHeight = 64;
        public Material material;
        public Gradient gradient;
        public float scale = 4;
        public int ocatves = 4;
        public float persistence = 0.5f;
        public float lacunarity = 2;
        public int seed = 1;

        public Vector3 position = new Vector3(0, 0, 0);
        
        private void Awake()
        {
            this._mesh = gameObject;
        }

        private void FixedUpdate()
        {
            GenerateChunk(new Vector2(position.x, position.z), new Vector3(32, 64, 32),
                this.gameObject.transform, material, gradient, scale, ocatves, persistence, lacunarity, seed);
        }

        public void GenerateChunk(Vector2 coordinates, Vector3 size, Transform parent, Material material,
            Gradient gradient,
            float scale, int octaves, float persistence, float lacunarity, int seed)
        {
            _position = new Vector3(coordinates.x * size.x, 0, coordinates.y * size.z);
            _positionChunk = new Vector3(coordinates.x, 0, coordinates.y);
            _bounds = new Bounds(_position, Vector2.one * size.x);
            Vector3 positionV3 = new Vector3(_position.x, 0, _position.z);

            _mesh = GenerateChunk(size, material, gradient, scale, octaves, persistence, lacunarity, seed);
            // _mesh.transform.position = positionV3;
            _mesh.transform.localScale = new Vector3(1, _maxHeight, 1);
            _mesh.transform.parent = parent;

            // SetVisible(false);
        }

        // public void Update(Vector3 viewerPosition, float maxViewDist)
        // {
        //     float viewerDstFromNearestEdge = Mathf.Sqrt(_bounds.SqrDistance(viewerPosition));
        //     bool visible = viewerDstFromNearestEdge <= maxViewDist;
        //     SetVisible(visible);
        // }

        private GameObject GenerateChunk(Vector3 size, Material material, Gradient gradient, float scale, int octaves,
            float persistence, float lacunarity, int seed)
        {
            // Vector2 size2 = size + new Vector2(1f, 0);
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
            var meshCollider = gameObject.GetComponent<MeshCollider>();
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            var mesh = new Mesh();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
            mesh.vertices = GenerateVertices(size, scale, octaves, persistence, lacunarity, seed);
            mesh.triangles = GenerateTriangle(size);
            mesh.colors = GenerateUvs(size, mesh.vertices, gradient);
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateTangents();
            // terrain.terrainData = new TerrainData
            // {
            //     size = new Vector3(size.x, size.y, size.z),
            //     heightmapResolution = 128
            // };
            // terrain.terrainData.SetHeights(0, 0, GenerateHeights(size2, scale));
            // terrain.materialTemplate = material;
            // terrainCollider.terrainData = terrain.terrainData;
            // _terrainData = terrain.terrainData;
            return gameObject;
        }

        private Vector3[] GenerateVertices(Vector3 size, float scale, int octaves, float persistence, float lacunarity,
            int seed)
        {
            var vertices = new Vector3[((int) size.x + 1) * ((int) size.z + 1)];
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;
            float y = 0;
            System.Random prng = new System.Random(seed);
            Vector3[] octaveOffsets = new Vector3[octaves];

            for (int i = 0; i < octaves; i++)
            {
                float xOffset = prng.Next(-100000, 100000) + _positionChunk.x;
                float zOffset = prng.Next(-100000, 100000) + _positionChunk.z;
                octaveOffsets[i] = new Vector3(xOffset, 0, zOffset);
            }


            for (int i = 0, z = 0; z <= size.z; z++)
            {
                for (int x = 0; x <= size.x; x++)
                {
                    amplitude = 1;
                    frequency = 1;
                    noiseHeight = 0;

                    for (int j = 0; j < octaves; j++)
                    {
                        y = CalculateHeight(x, z, size, scale, frequency, octaveOffsets[j]);
                        noiseHeight += y * amplitude;
                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    vertices[i] = new Vector3(x, Mathf.InverseLerp(_minHeight, _maxHeight, noiseHeight), z);
                    i++;
                }
            }

            return vertices;
        }

        private int[] GenerateTriangle(Vector3 size)
        {
            var triangles = new int[(int) size.x * (int) size.z * 6];

            int vertice = 0;
            int triangle = 0;
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    triangles[triangle + 0] = vertice + 0;
                    triangles[triangle + 1] = vertice + (int) size.x + 1;
                    triangles[triangle + 2] = vertice + 1;
                    triangles[triangle + 3] = vertice + 1;
                    triangles[triangle + 4] = vertice + (int) size.x + 1;
                    triangles[triangle + 5] = vertice + (int) size.x + 2;

                    vertice++;
                    triangle += 6;
                }

                vertice++;
            }

            return triangles;
        }

        private Color[] GenerateUvs(Vector3 size, Vector3[] vertices, Gradient gradient)
        {
            var colors = new Color[vertices.Length];

            for (int i = 0, x = 0; x <= size.x; x++)
            {
                for (int z = 0; z <= size.z; z++)
                {
                    float height = Mathf.InverseLerp(_minHeight, _maxHeight, vertices[i].y);
                    colors[i] = gradient.Evaluate(height);
                    i++;
                }
            }

            return colors;
        }

        private float CalculateHeight(float x, float z, Vector3 size, float scale, float frequency,
            Vector3 octaveOffset)
        {
            // var xCord = (x / (size.x) * scale) + ((octaveOffset.x + (_positionChunk.x * scale)) * frequency);
            // var zCord = (z / (size.z) * scale) + ((octaveOffset.z + (_positionChunk.y * scale)) * frequency);
            var xCord = (x / scale) * frequency + octaveOffset.x;
            var zCord = (z / scale) * frequency + octaveOffset.z;

            return Mathf.PerlinNoise(xCord, zCord) * 2 - 1;
        }

        public float GetHeight(int x, int z)
        {
            return _terrainData.GetHeight(x, z);
        }

        public Vector2 GetPosition()
        {
            return _positionChunk;
        }

        public void SetVisible(bool visible)
        {
            _mesh.SetActive(visible);
        }

        public bool IsVisible()
        {
            return _mesh.activeSelf;
        }
    }
}