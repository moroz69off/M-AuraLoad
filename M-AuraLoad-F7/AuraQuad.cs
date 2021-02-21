using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.Controls;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.OpenGLAttributes;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Effects;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.Serialization;
using SharpGL.Serialization.Wavefront;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace M_AuraLoad_F7
{
    class AuraQuad : Quadric
    {
        Vertex vertex { get; set; }

        VertexControl VertexControl { get; set; }
        public void CreateAura(OpenGL GL, Polygon polygon)
        {
            GL.Begin(RenderMode.HitTest);
            GL.Color = Color.FromArgb(1, 0, 1, 1);
            GL.DrawElementsBaseVertex(OpenGL.GL_POINTS, polygon.Vertices.Count, polygon.Faces.ToArray(), new Vertex(0, 0, 0));
        }
    }
}
