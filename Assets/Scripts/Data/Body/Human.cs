using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor;
using UnityEngine;

namespace HC.Data
{
    [Serializable]
    [ShowOdinSerializedPropertiesInInspector]
    public class Human : IBody
    {
        public List<Relationship> Relationships;

        [HideInInspector] public BodyPlan CurrentPlan => _currentPlan;
        [OdinSerialize] private BodyPlan _currentPlan;

        [HideInInspector] public Dictionary<string, BodyProcess> Processes { get => _bodyStates; }
        [HideInInspector] private Dictionary<string, BodyProcess> _bodyStates;

        [HideInInspector] public Vector2Int Anchor { get => _anchorPosition; set => _anchorPosition = value; }
        [OdinSerialize] private Vector2Int _anchorPosition;

        [HideInInspector] public Vector2Int WorldPosition { get => _worldPosition; set => _worldPosition = value; }
        [OdinSerialize] private Vector2Int _worldPosition;
        
        public Action OnPlanChange;

        public bool Root => true;
        public IBody Parent => null;

        public void Initialize()
        {
            CurrentPlan.AddToBody(this);

            foreach (BodyPart bodyPart in _currentPlan.BodyPartGrid)
            {
                if (bodyPart == null) continue;
                World.Instance.AddToTile(bodyPart);
                if (bodyPart is IBody)
                {
                    ((IBody)bodyPart).Initialize();
                }
            }
        }

        private bool ApplyBodyPlan(BodyPlan plan)
        {
            // TODO: Collision checking on changing bodyplan


            OnPlanChange?.Invoke();

            return true;
        }

        public List<BodyPart> TypedParts(BodyPartType type)
        {
            return TypedParts(type, this);
        }

        public List<BodyPart> TypedParts(BodyPartType type, IBody body)
        {
            List<BodyPart> parts = new List<BodyPart>();

            foreach (BodyPart bodyPart in _currentPlan.BodyPartGrid)
            {
                if (bodyPart == null) continue;
                if (bodyPart.Type == type) parts.Add(bodyPart);
                if (bodyPart is DynamicLimbBodyPart)
                {
                    parts.Concat(TypedParts(type, (DynamicLimbBodyPart)bodyPart));
                }
            }

            return parts;
        }

        public bool Move(Vector2Int delta)
        {
            Ghost(true);

            foreach (BodyPart bodyPart in _currentPlan.BodyPartGrid)
            {
                if (bodyPart == null) continue;
                if (!bodyPart.CheckCollisionOnTile(bodyPart.Position + delta))
                {
                    Ghost(false);
                    return false;
                }
                if (bodyPart is DynamicLimbBodyPart)
                {
                    foreach (BodyPart innerPart in ((DynamicLimbBodyPart)bodyPart).CurrentPlan.BodyPartGrid)
                    {
                        if (innerPart == null) continue;
                        if (!innerPart.CheckCollisionOnTile(innerPart.Position + delta))
                        {
                            Ghost(false);
                            return false;
                        }
                    }
                }
            }

            World.Instance.MoveHuman(this, delta);

            Ghost(false);

            return true;
        }

        public bool RotateFull(bool left)
        {
            Ghost(true);

            Ghost(false);

            return true;
        }

        public void Ghost(bool ghost)
        {
            Ghost(ghost, CurrentPlan);
        }

        private void Ghost(bool ghost, BodyPlan bodyPlan)
        {
            foreach (BodyPart bodyPart in bodyPlan.BodyPartGrid)
            {
                if (bodyPart == null) continue;

                bodyPart.SetGhost(ghost);

                if (bodyPart is IBody)
                {
                    Ghost(ghost, ((IBody)bodyPart).CurrentPlan);
                }
            }
        }
    }
}