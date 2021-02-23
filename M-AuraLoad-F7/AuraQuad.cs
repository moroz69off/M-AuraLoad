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
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Effects;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Primitives;
using SharpGL.Serialization;
using SharpGL.Serialization.Wavefront;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace M_AuraLoad_F7
{
    class AuraQuad : Quadric
    {
        public float rquad { get; set; } = 0;

		public void CreateAura(OpenGL GL, Polygon polygon)
		{
			#region draw quads
			GL.LoadIdentity();

			GL.Rotate(rquad, .0f, .0f, 1.0f);
			GL.Begin(OpenGL.GL_QUADS); // first 4 vertices
			//GL.Color(Color.FromArgb(100, 0, 255, 255));
            for (int i = 0; i < 4; i++)
            {
				GL.Vertex(polygon.Vertices[i].X * 5, polygon.Vertices[i].Y * 5, polygon.Vertices[i].Z * 5);
            }
            GL.End();
            GL.Flush();
            #endregion draw quads
        }
	}
}
