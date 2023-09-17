using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Data
{
    [Serializable]
    public class Tile
    {
        public TileType Type { get; set; }
        public List<ITileObject> Objects { get; set; }

        public Tile() 
        {
            Objects = new List<ITileObject>();
            Type = TileType.FLOOR;
        }
    }

    public enum TileType
    {
        WALL,
        FLOOR,
        NUM_TYPES
    }
}
