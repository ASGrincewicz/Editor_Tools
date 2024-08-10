using System;
using UnityEngine;

namespace Water
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(EdgeCollider2D))]
    [RequireComponent(typeof(WaterTriggerHandler))]
    public class InteractableWater : MonoBehaviour
    {
        [Header("Mesh Generation")] [Range(2, 500)]
        public int numOfXVertices = 70;

        public float width = 10f;
        public float height = 4f;
        public Material waterMaterial;
        private const int NUM_OF_Y_VERTICES = 2;

        [Header("Gizmo")] public Color gizmoColor = Color.white;
        private Mesh _mesh;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private Vector3[] _vertices;
        private int[] _topVerticesIndex;

        private EdgeCollider2D _edgeCollider2D;

        private void Start()
        {
            TryGetComponent(out _edgeCollider2D);
            TryGetComponent(out _meshRenderer);
            TryGetComponent(out _meshFilter);
            GenerateMesh();
        }

        private void Reset()
        {
            if (TryGetComponent(out _edgeCollider2D))
            {
                _edgeCollider2D.isTrigger = true;
            }
            
        }

        public void ResetEdgeCollider()
        {
            TryGetComponent(out _edgeCollider2D);
            Vector2[] newPoints = new Vector2[2];

            Vector2 firstPoint = new Vector2(_vertices[_topVerticesIndex[0]].x, _vertices[_topVerticesIndex[0]].y);
            newPoints[0] = firstPoint;

            Vector2 secondPoint = new Vector2(_vertices[_topVerticesIndex[^1]].x,
                _vertices[_topVerticesIndex[^1]].y);
            newPoints[1] = secondPoint;
            _edgeCollider2D.offset = Vector2.zero;
            _edgeCollider2D.points = newPoints;
        }


        public void GenerateMesh()
        {
            _mesh = new Mesh();
            
            //add vertices
            _vertices = new Vector3[numOfXVertices * NUM_OF_Y_VERTICES];
            _topVerticesIndex = new int[numOfXVertices];
            for (int y = 0; y < NUM_OF_Y_VERTICES; y++)
            {
                for (int x = 0; x < numOfXVertices; x++)
                {
                    float xPos = (x / (float)(numOfXVertices - 1)) * width - width / 2;
                    float yPos = (y / (float)(NUM_OF_Y_VERTICES - 1)) * height - height / 2;
                    _vertices[y * numOfXVertices + x] = new Vector3(xPos, yPos, 0f);

                    if (y == NUM_OF_Y_VERTICES - 1)
                    {
                        _topVerticesIndex[x] = y * numOfXVertices + x;
                    }
                }
            }
            
            //Construct Triangles
            int[] triangles = new int[(numOfXVertices - 1) * (NUM_OF_Y_VERTICES - 1) * 6];
            int index = 0;

            for (int y = 0; y < NUM_OF_Y_VERTICES - 1; y++)
            {
                for (int x = 0; x < numOfXVertices - 1; x++)
                {
                    int bottomLeft = y * numOfXVertices + x;
                    int bottomRight = bottomLeft + 1;
                    int topLeft = bottomLeft + numOfXVertices;
                    int topRight = topLeft + 1;
                    
                    //first triangle
                    triangles[index++] = bottomLeft;
                    triangles[index++] = topLeft;
                    triangles[index++] = bottomRight;
                    
                    //second triangle;
                    triangles[index++] = bottomRight;
                    triangles[index++] = topLeft;
                    triangles[index++] = topRight;
                }
            }
            
            //UVs
            Vector2[] uvs = new Vector2[_vertices.Length];
            for (int i = 0; i < _vertices.Length; i++)
            {
                uvs[i] = new Vector2((_vertices[i].x + width / 2) / width, (_vertices[i].y + height / 2) / height);
            }

            if (ReferenceEquals(_meshRenderer, null))
            {
                TryGetComponent(out _meshRenderer);
            }

            if (ReferenceEquals(_meshFilter, null))
            {
                TryGetComponent(out _meshFilter);
            }

            _meshRenderer.material = waterMaterial;

            _mesh.vertices = _vertices;
            _mesh.triangles = triangles;
            _mesh.uv = uvs;
            
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();

            _meshFilter.mesh = _mesh;
        }

        private void OnDestroy()
        {
            Destroy(_mesh);
        }
    } 
}

