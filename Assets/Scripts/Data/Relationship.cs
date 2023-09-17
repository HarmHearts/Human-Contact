using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Data
{
    public class Relationship
    {
        public Human Target;
        public RelationshipType RelationType;
        public Action<string, bool> Action;
    }

    public enum RelationshipType
    {
        FRIENDS,
        LOVERS,
        ENEMIES,
        SHY,
        RELAXED
    }
}
