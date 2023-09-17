using HC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Conditions
{
    public static class ActionConditions
    {
        public static bool HandHolding(Human source, Human target)
        {
            List<BodyPart> sourceHands = source.TypedParts(BodyPartType.HAND);

            foreach (BodyPart part in sourceHands)
            {
                if (part.GetAdjacentObjects().Any(a => 
                    a is BodyPart && 
                    ((BodyPart)a).GetRootBody(((BodyPart)a).Body) == target))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
