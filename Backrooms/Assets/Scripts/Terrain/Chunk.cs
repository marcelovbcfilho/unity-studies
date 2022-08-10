using UnityEngine;

namespace Terrain
{
    public class Chunk
    {
        private readonly GameObject _mesh;
        private readonly Vector2 _position;
        private readonly Vector2 _positionChunk;
        private TerrainData _terrainData;
        private Bounds _bounds;

        private float _minHeight = 0;
        private float _maxHeight = 16;

        public Chunk(Vector2 coordinates, Vector3 size, Transform parent, Material material, Gradient gradient,
            float scale)
        {
            _position = coordinates * size.x;
            _positionChunk = coordinates;
            _bounds = new Bounds(_position, Vector2.one * size.x);
            
            Vector3 positionV3 = new Vector3(_position.x, 0, _position.y);
            _mesh = GenerateChunk(size, material, gradient, scale);
            _mesh.transform.position = positionV3;
            _mesh.transform.localScale = new Vector3(1, _maxHeight, 1);
            _mesh.transform.parent = parent;

            SetVisible(false);
        }

        public void Update(Vector3 viewerPosition, float maxViewDist)
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(_bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDist;
            SetVisible(visible);
        }

        private GameObject GenerateChunk(Vector3 size, Material material, Gradient gradient, float scale)
        {
            GameObject gameObject = new GameObject("Chunk_" + _positionChunk.x + "_" + _positionChunk.y)
            {
                tag = "LevelPart"
            };
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            var meshCollider = gameObject.AddComponent<MeshCollider>();
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            var mesh = new Mesh();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
            mesh.vertices = GenerateVertices(size, scale);
            mesh.triangles = GenerateTriangle(size);
            mesh.colors = GenerateUvs(size, mesh.vertices, gradient);
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateTangents();
            return gameObject;
        }

        private Vector3[] GenerateVertices(Vector3 size, float scale)
        {
            var vertices = new Vector3[((int) size.x + 1) * ((int) size.z + 1)];
            float y;

            for (int i = 0, z = 0; z <= size.z; z++)
            {
                for (int x = 0; x <= size.x; x++)
                {
                    y = CalculateHeight(x, z, size, scale);
                    vertices[i] = new Vector3(x, y, z);
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
                    float y = Mathf.InverseLerp(-1, 1, vertices[i].y);
                    colors[i] = gradient.Evaluate(y);
                    i++;
                }
            }

            return colors;
        }

        private float CalculateHeight(int x, int z, Vector3 size, float scale)
        {
            var xCord = ((x / (size.x)) + (20 + _positionChunk.x ))  * scale;
            var zCord = ((z / (size.z)) + (20 + _positionChunk.y )) *  scale;

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