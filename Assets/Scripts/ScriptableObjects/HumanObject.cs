using HC.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HC.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Human", menuName = "HC/Human")]
    public class HumanObject : SerializedScriptableObject
    {
        [OdinSerialize] [NonSerialized] public Human Human;
    }
}
