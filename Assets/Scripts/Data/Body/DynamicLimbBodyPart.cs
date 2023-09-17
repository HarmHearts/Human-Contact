using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace HC.Data
{
    [Serializable]
    [ShowOdinSerializedPropertiesInInspector]
    public class DynamicLimbBodyPart : BodyPart, IBody
    {
        public Dictionary<string, BodyProcess> Processes { get => _states; }
        [OdinSerialize] private Dictionary<string, BodyProcess> _states;
        public bool Root => false;
        public IBody Parent => Body;

        [HideInInspector] public Vector2Int Anchor { get => _anchorPosition; set => _anchorPosition = value; }
        [SerializeField] private Vector2Int _anchorPosition;

        [HideInInspector] public BodyPlan CurrentPlan => _currentPlan;
        public Vector2Int WorldPosition { get => Position; set => throw new NotImplementedException(); }

        [HideInInspector] public List<DynamicLimbObject> LimbObjects { get => _instanceLimbs; set => _instanceLimbs = value; }
        [HideInInspector] private List<DynamicLimbObject> _instanceLimbs;
        [OdinSerialize] private List<DynamicLimbObject> _limbObjects;

        [SerializeField] private BodyPlan _currentPlan;

        public void ChangeState(string state, bool activate)
        {
            
        }

        public void Initialize()
        {
            LimbObjects = _limbObjects.Select(a => GameObject.Instantiate(a)).ToList();

            foreach (DynamicLimbObject dnb in LimbObjects)
            {
                dnb.Part.Initialize();
            }

            if (Processes.ContainsKey("DEFAULT"))
            {
                _currentPlan = GetNewBodyState("DEFAULT", true);
            }

            foreach (BodyProcess process in Processes.Values)
            {
                foreach (BodyPlan plan in process.Plans)
                {
                    plan.AddToBody(this);
                }
            }

            foreach (BodyPart bodyPart in _currentPlan.BodyPartGrid)
            {
                if (bodyPart == null) continue;
                World.Instance.AddToTile(bodyPart);
            }
        }

        private BodyPlan GetNewBodyState(string condition, bool activate)
        {
            Processes.ForEach(a => a.Value.Active = false);
            Processes[condition].Active = true;

            return Processes[condition].GetNext(activate);
        }

        public bool Move(Vector2Int delta)
        {
            throw new NotImplementedException();
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
                    Ghost(ghost, ((IBody)bodyPlan).CurrentPlan);
                }
            }
        }
    }
}