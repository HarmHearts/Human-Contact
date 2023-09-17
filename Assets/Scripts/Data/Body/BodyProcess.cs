using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HC.Data
{
    [Serializable]
    public class BodyProcess
    {
        public List<BodyPlan> Plans;
        public int Index = 0;
        public bool Active = false;
        public bool Complete = false;
        public Vector2Int ReactiveVector;
        public PlanChangeState ChangeState = PlanChangeState.DEFAULT;

        public BodyPlan GetNext(bool activate)
        {
            int next = activate ?
                Index == Plans.Count - 1
                    ? Plans.Count - 1 : Index + 1 :
                Index == 0 ?
                    0 : Index - 1;

            Index = next;
            
            if (Index == Plans.Count - 1)
            {
                Complete = true;
            } else
            {
                Complete = false;
            }

            return Plans[next];
        }

        public bool TryChangeState(IBody body, World world)
        {
            BodyPlan current = body.CurrentPlan;
            bool push = false;
            BodyPlan next = Plans[Index];

            body.Ghost(true);

            foreach (BodyPart bodyPart in next.BodyPartGrid)
            {
                if (bodyPart == null) continue;
                if (!bodyPart.CheckCollisionOnTile(bodyPart.Position))
                {
                    body.Ghost(false);
                    push = true;
                    break;
                }
            }

            if (push)
            {
                if (!body.Move(ReactiveVector)) {
                    ChangeState = PlanChangeState.STRAIN;
                    return false;
                }
            }

            if (current != null)
            {
                foreach (BodyPart bodyPart in current.BodyPartGrid)
                {
                    if (bodyPart == null) continue;
                    World.Instance.RemoveFromTile(bodyPart);
                }
            }

            current = next;

            if (current != null)
            {
                foreach (BodyPart bodyPart in current.BodyPartGrid)
                {
                    if (bodyPart == null) continue;
                    World.Instance.AddToTile(bodyPart);
                }
            }

            return true;
        }

        public IEnumerator ActivateState(IBody body, bool activate)
        {
            ChangeState = PlanChangeState.CHANGING;

            Vector2Int reflexive = new Vector2Int();

            Index = activate ? 0 : Plans.Count - 1;

            for (int i = 0; i < Plans.Count; i++)
            {
                if (TryChangeState(body, World.Instance))
                {
                    Index += activate ? 1 : -1;
                    yield return new WaitForSeconds(0.2f);
                } else
                {
                    yield break;
                }
            }

            ChangeState = PlanChangeState.DEFAULT;
        }
    }

    public enum PlanChangeState
    {
        DEFAULT,
        CHANGING,
        STRAIN
    }
}
