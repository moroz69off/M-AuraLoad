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

        public Quadric quadric { get; set; }

		public void CreateAura(OpenGL GL, Polygon polygon)
		{
            GL.LoadIdentity();
            GL.Rotate(rquad, .0f, .0f, 1.0f);

            #region draw aura quads

            for (int i = 0; i < polygon.Faces.Count; i++)
            {
                int numVertex = 0;
                
            }

            GL.Flush();

            #endregion draw aura quads
        }
	}
}
