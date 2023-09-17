using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HC.Data
{
    public class World : SerializedMonoBehaviour
    {
        public static World Instance { get => _instance; }
        private static World _instance;

        [OdinSerialize] public Tile[,] Tiles;
        public GameObject TileSquare;

        public void Awake()
        {
            Destroy(Instance);

            _instance = this;

            Tiles = new Tile[100, 100];

            for (int j = 0; j  < Tiles.GetLength(1); j++)
            {
                for (int i = 0; i < Tiles.GetLength(0); i++)
                {
                    Tiles[i, j] = new Tile();
                    Instantiate(TileSquare, new Vector2(i * 0.5f, j * -0.5f) + (Vector2)transform.position, Quaternion.identity, transform);
                }
            }
        }

        public Tile GetTile(Vector2Int position)
        {
            if (position.x < Tiles.GetLength(0) && position.y < Tiles.GetLength(1)) {
                return Tiles[position.x, position.y];
            } else
            {
                throw new Exception("Checked a tile out of bounds!");
            }
        }

        public void AddToTile(ITileObject tileObject)
        {
            GetTile(tileObject.Position).Objects.Add(tileObject);
        }

        public void RemoveFromTile(ITileObject tileObject)
        {
            GetTile(tileObject.Position).Objects.Remove(tileObject);
        }

        public void MoveTileObject(ITileObject tileObject, Vector2Int destination)
        {
            RemoveFromTile(tileObject);
            tileObject.Position = destination;
            AddToTile(tileObject);
        }

        public void MoveHuman(Human human, Vector2Int delta)
        {
            foreach (BodyPart bodyPart in human.CurrentPlan.BodyPartGrid)
            {
                if (bodyPart == null) continue;
                RemoveFromTile(bodyPart);
            }

            human.WorldPosition += delta;

            foreach (BodyPart bodyPart in human.CurrentPlan.BodyPartGrid)
            {
                if (bodyPart == null) continue;
                AddToTile(bodyPart);
            }
        }
    }
}
