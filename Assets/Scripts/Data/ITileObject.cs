using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HC.Data
{
    public interface ITileObject
    {
        public IBody Body { get; set; }
        public bool CheckCollision(ITileObject other);
        public Vector2Int Position { get; set; }
        public Collidability Collidability { get; }
    }

    public enum Collidability
    {
        NONE,
        WALL,
        BODY,
        OBJECT,
        GHOST,
        NUM_TYPES
    }
}
