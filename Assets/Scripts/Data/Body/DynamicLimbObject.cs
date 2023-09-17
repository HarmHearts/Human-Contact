using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace HC.Data
{
    [CreateAssetMenu(fileName = "Dynamic Limb", menuName = "HC/Dynamic Limb")]
    [ShowOdinSerializedPropertiesInInspector]
    public class DynamicLimbObject : SerializedScriptableObject
    {
        [OdinSerialize] [NonSerialized] public DynamicLimbBodyPart Part;
    }
}