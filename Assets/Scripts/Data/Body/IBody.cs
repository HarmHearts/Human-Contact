using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HC.Data
{
    public interface IBody
    {
        public Dictionary<string, BodyProcess> Processes { get; }
        public BodyPlan CurrentPlan { get; }
        public bool Root { get; }
        public Vector2Int Anchor { get; set; }
        public IBody Parent { get; }
        public Vector2Int WorldPosition { get; set; }
        public void Initialize();
        public bool Move(Vector2Int delta);
        public void Ghost(bool ghost);
    }
}
