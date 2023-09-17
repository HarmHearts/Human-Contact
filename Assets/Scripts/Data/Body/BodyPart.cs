using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HC.Data
{
    [Serializable]
    public abstract class BodyPart : ITilePart
    {
        public IBody Body { get; set; }
        [OdinSerialize] public BodyPartType Type { get; set; }
        [OdinSerialize] public Sprite Sprite { get; set; }
        public Vector2Int Position { get => _relativePosition + Body.WorldPosition; set => throw new NotImplementedException(); }
        public Vector2Int RelativePosition { get => _relativePosition; set => _relativePosition = value; }
        public Collidability Collidability { get => _collidability; }

        private Collidability _collidability = Collidability.BODY;

        protected Vector2Int _relativePosition;

        /// <summary>
        /// Checks collision between this and a different tile object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CheckCollision(ITileObject other)
        {
            if (other is BodyPart)
            {
                if (((BodyPart)other).GetRootBody(((BodyPart)other).Body) == GetRootBody(Body) 
                    && other.Collidability == Collidability.GHOST)
                {
                    return true;
                }

                return false;
            }
            else if (other.Collidability == Collidability.NONE)
            {
                return true;
            }

            //TODO: Object collidability movement

            return false;
        }

        public List<ITileObject> GetAdjacentObjects()
        {
            List<ITileObject> bodies = new List<ITileObject>();

            bodies.Concat(World.Instance.GetTile(Position + Vector2Int.up).Objects);
            bodies.Concat(World.Instance.GetTile(Position + Vector2Int.down).Objects);
            bodies.Concat(World.Instance.GetTile(Position + Vector2Int.left).Objects);
            bodies.Concat(World.Instance.GetTile(Position + Vector2Int.right).Objects);

            bodies = bodies.Distinct().ToList();

            return bodies;
        }

        public IBody GetRootBody(IBody body)
        {
            if (body.Parent == null)
            {
                return body;
            } else
            {
                return GetRootBody(body.Parent);
            }
        }

        public bool CheckCollisionOnTile(Vector2Int place)
        {
            List<ITileObject> objects = World.Instance.GetTile(place).Objects;
            foreach (ITileObject obj in objects)
            {
                if (!CheckCollision(obj))
                {
                    return false;
                }
            }

            return true;
        }


        public void SetGhost(bool ghost)
        {
            if (ghost)
            {
                _collidability = Collidability.GHOST;
            }
            else
            {
                _collidability = Collidability.BODY;
            }
        }
    }

    public enum BodyPartType
    {
        HEAD,
        BODY,
        ARM,
        HAND,
        LEG,
        FOOT,
        SPECIAL,
        NUM_TYPES
    }

}
