using SharpGL.SceneGraph.Quadrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M_AuraLoad_F7
{
    class AModel : Quadric
    {
        List<Cylinder> cylinders { get; } = new List<Cylinder>(24);

    }
}
