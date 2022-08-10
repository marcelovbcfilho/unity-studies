using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class Controller : MonoBehaviour
    {
        public float viewDistance = 32;
        public Vector3 chunkSize = new Vector3(32, 20, 32);
        public Vector2 viewerPosition;
        public Transform viewer;
        public int chunksVisibleInViewDst;
        public Gradient chunkGradient;
        public Material chunkMaterial;
        public float scale = 4;

        private readonly Dictionary<Vector2, Chunk> _terrainChunkDictionary = new();

        private readonly Dictionary<Vector2, Chunk> _terrainChunksVisibleLastUpdate = new();

        private void Start()
        {
            chunksVisibleInViewDst = Mathf.RoundToInt(viewDistance / chunkSize.x);
        }

        private void Update()
        {
            viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
            UpdateVisibleChunks();
        }

        private void UpdateVisibleChunks()
        {
            Vector2 viewerChunk = new Vector2(
                Mathf.RoundToInt(viewerPosition.x / chunkSize.x),
                Mathf.RoundToInt(viewerPosition.y / chunkSize.x));

            // Generating a list with all chunks that are visible
            List<Vector2> viewedChunks = new List<Vector2>();
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
                {
                    viewedChunks.Add(new Vector2(viewerChunk.x + xOffset, viewerChunk.y + yOffset));
                }
            }

            // Optimizing the chunks to render 
            List<Vector2> chunksToHide = new List<Vector2>();
            foreach (var chunk in _terrainChunksVisibleLastUpdate.Values)
            {
                if (viewedChunks.Contains(chunk.GetPosition()))
                    // Remove it to avoid re-update a already loaded chunk
                    viewedChunks.Remove(chunk.GetPosition());
                else
                    // Hide it because it is no longer in viewer view distance
                    chunksToHide.Add(chunk.GetPosition());
            }
            
            // Hiding farther chunks
            foreach (var chunkToHide in chunksToHide)
            {
                _terrainChunksVisibleLastUpdate[chunkToHide].SetVisible(false);
                _terrainChunksVisibleLastUpdate.Remove(chunkToHide);
            }

            // Adding new chunks
            foreach (var viewedChunk in viewedChunks)
            {
                if (_terrainChunkDictionary.ContainsKey(viewedChunk))
                {
                    _terrainChunkDictionary[viewedChunk].Update(viewerPosition, viewDistance);
                    if (_terrainChunkDictionary[viewedChunk].IsVisible())
                    {
                        _terrainChunksVisibleLastUpdate.Add(viewedChunk,
                            _terrainChunkDictionary[viewedChunk]);
                    }
                }
                else
                {
                    _terrainChunkDictionary.Add(viewedChunk,
                        new Chunk(viewedChunk, chunkSize, transform, chunkMaterial, chunkGradient, scale));
                }
            }
        }

        public float GetChunkHeight(int x, int z)
        {
            Vector2 chunk = new Vector2((int) (x / chunkSize.x), (int) (z / chunkSize.z));
            return _terrainChunkDictionary[chunk].GetHeight(x, z);
        }
    }
}