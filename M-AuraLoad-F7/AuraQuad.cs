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
        Vertex vertex { get; set; }

        VertexControl VertexControl { get; set; }
        public void CreateAura(OpenGL GL, Polygon polygon)
        {
			GL.Begin(OpenGL.GL_QUADS);
            IntPtr quadr = GL.NewQuadric();

			

		}

		protected void calcNormal(float[,] v, float[] result)
		{
			float[] v1 = new float[3];
			float[] v2 = new float[3];  // Vector 1 (x,y,z) & Vector 2 (x,y,z)
			const int x = 0;                        // Define X Coord
			const int y = 1;                        // Define Y Coord
			const int z = 2;                        // Define Z Coord

			// Finds The Vector Between 2 Points By Subtracting
			// The x,y,z Coordinates From One Point To Another.

			// Calculate The Vector From Point 1 To Point 0
			v1[x] = v[0, x] - v[1, x];                  // Vector 1.x=Vertex[0].x-Vertex[1].x
			v1[y] = v[0, y] - v[1, y];                  // Vector 1.y=Vertex[0].y-Vertex[1].y
			v1[z] = v[0, z] - v[1, z];                  // Vector 1.z=Vertex[0].y-Vertex[1].z
														// Calculate The Vector From Point 2 To Point 1
			v2[x] = v[1, x] - v[2, x];                  // Vector 2.x=Vertex[0].x-Vertex[1].x
			v2[y] = v[1, y] - v[2, y];                  // Vector 2.y=Vertex[0].y-Vertex[1].y
			v2[z] = v[1, z] - v[2, z];                  // Vector 2.z=Vertex[0].z-Vertex[1].z
														// Compute The Cross Product To Give Us A Surface Normal
			result[x] = v1[y] * v2[z] - v1[z] * v2[y];              // Cross Product For Y - Z
			result[y] = v1[z] * v2[x] - v1[x] * v2[z];              // Cross Product For X - Z
			result[z] = v1[x] * v2[y] - v1[y] * v2[x];              // Cross Product For X - Y

            ReduceToUnit(result);                       // Normalize The Vectors
        }

		protected void ReduceToUnit(float[] vector)
		{                                   // To A Unit Normal Vector With A Length Of One.
			float length;                           // Holds Unit Length

			// Calculates The Length Of The Vector
			length = (float)Math.Sqrt((vector[0] * vector[0]) + (vector[1] * vector[1]) + (vector[2] * vector[2]));

			if (length == 0.0f)                     // Prevents Divide By 0 Error By Providing
				length = 1.0f;                      // An Acceptable Value For Vectors To Close To 0.

			vector[0] /= length;                        // Dividing Each Element By
			vector[1] /= length;                        // The Length Results In A
			vector[2] /= length;                        // Unit Normal Vector.
		}

	}
}
