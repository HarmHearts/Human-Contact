using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HC.Data
{
    public interface ITilePart : ITileObject
    {
        public Vector2Int RelativePosition { get; set; }
    }
}
