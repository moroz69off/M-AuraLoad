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
        static OpenGL gl;
        public float rquad { get; set; } = 0;

        private uint BlurTexture;


        private static uint EmptyTexture(OpenGL gl)
        {
            uint[] txtnumber = new uint[1]; // Texture ID
            // Create Storage Space For Texture Data (128x128x4)
            byte[] data = new byte[((128 * 128) * 4 * sizeof(uint))];

            gl.GenTextures(1, txtnumber);					// Create 1 Texture
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, txtnumber[0]);			// Bind The Texture
            gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, 4, 128, 128, 0,
                OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, data);			// Build Texture Using Information In data
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);

            return txtnumber[0];						// Return The Texture ID
        }

        public void CreateAura(OpenGL GL, Polygon polygon)
		{
            gl = GL;
            BlurTexture = EmptyTexture(gl);
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
