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
    public partial class AForm : Form
    {

        private bool isMale = Properties.Settings.Default.IsMale;
        private ArcBallEffect arcBallEffect = new ArcBallEffect();
        private string path;
        private int auraBlue = 0;
        private int auraRed = 0;
        private Material auraMaterial = new Material();

        public AForm()
        {
            InitializeComponent();
            LoadHuman(isMale);
            LoadAura();
        }

        /// <summary>
        /// Load scene control in windows form
        /// </summary>
        /// <param name="sender">Scene control</param>
        /// <param name="e">System.EventArgs</param>
        private void sceneControl_Load(object sender, EventArgs e)
        {
            //sceneControl.OpenGL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);
            // Remove red bounding box of models
            sceneControl.Scene.RenderBoundingVolumes =
                !sceneControl.Scene.RenderBoundingVolumes;
            // Remove default axies
            sceneControl.Scene.SceneContainer.Children[0]
                .RemoveChild(sceneControl.Scene.SceneContainer.Children[0].Children[1]);
            // Remove default Grid
            sceneControl.Scene.SceneContainer.Children[0]
                .RemoveChild(sceneControl.Scene.SceneContainer.Children[0].Children[0]);

            auraMaterial.Diffuse = Color.FromArgb(255, auraRed, 255, auraBlue);

            SceneElement lightsFolder = sceneControl.Scene.SceneContainer.Children[1];
            Light light1 = (Light)lightsFolder.Children[0];
            Light light2 = (Light)lightsFolder.Children[1];
            Light light3 = (Light)lightsFolder.Children[2];
            light1.Position = new Vertex(-9, -9, 0);
            light2.Position = new Vertex(9, -9, 0);
            light3.Position = new Vertex(-9, 9, 0);
            sceneControl.Scene.CurrentCamera.Position = new Vertex(0, -12.345f, 0);
        }

        /// <summary>
        /// Load human model to scene
        /// </summary>
        /// <param name="isMale">from app settings</param>
        private void LoadHuman(bool isMale)
        {
            if (isMale) path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "maleMin.obj");
            else path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "femaleMin.obj");
            ObjFileFormat obj = new ObjFileFormat();
            Scene sceneHumanObject = obj.LoadData(path);
            foreach (Asset asset in sceneHumanObject.Assets) sceneControl.Scene.Assets.Add(asset);
            List<Polygon> polygons = sceneHumanObject.SceneContainer.Traverse<Polygon>().ToList();
            foreach (Polygon polygon in polygons)
            {
                polygon.Name = "HUMAN";
                BoundingVolume boundingVolume = polygon.BoundingVolume;
                var extent = new float[3];
                boundingVolume.GetBoundDimensions(out extent[0], out extent[1], out extent[2]);
                float maxExtent = extent.Max();
                float scaleFactor = maxExtent > 10 ? 10.0f / maxExtent : 1;
                polygon.Parent.RemoveChild(polygon);
                polygon.Transformation.RotateX = 180; // 
                polygon.Transformation.ScaleX = scaleFactor;
                polygon.Transformation.ScaleY = scaleFactor;
                polygon.Transformation.ScaleZ = scaleFactor;
                polygon.Freeze(sceneControl.OpenGL);
                sceneControl.Scene.SceneContainer.AddChild(polygon);
                polygon.AddEffect(new OpenGLAttributesEffect());
                polygon.AddEffect(arcBallEffect);
            }
        }

        /// <summary>
        /// Load default aura to scene
        /// </summary>
        private void LoadAura()
        {
            string apath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "aura.obj");
            ObjFileFormat obj = new ObjFileFormat();
            Scene sceneAuraObject = obj.LoadData(apath);
            List<Polygon> polygons = sceneAuraObject.SceneContainer.Traverse<Polygon>().ToList();
            var aPolygon = polygons[0];
            AuraQuad AQuadr = new AuraQuad();
            AQuadr.CreateAura(sceneControl.OpenGL, aPolygon);
        }

        #region mouse events
        private void sceneControl_MouseDown(object sender, MouseEventArgs e)
        {
            arcBallEffect.ArcBall.SetBounds(sceneControl.Width, sceneControl.Height);
            arcBallEffect.ArcBall.MouseDown(e.X/2, e.Y/2);
        }

        private void sceneControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) arcBallEffect.ArcBall.MouseMove(e.X/2, e.Y/2);
        }

        private void sceneControl_MouseUp(object sender, MouseEventArgs e)
        {
            arcBallEffect.ArcBall.MouseUp(e.X/2, e.Y/2);
        }
        #endregion mouse events

        /// <summary>
        /// Draw scene in framerate resolution
        /// </summary>
        /// <param name="sender">Scene control</param>
        /// <param name="args">System.Drawing.Graphics</param>
        private void sceneControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            if (auraRed > 255) auraRed = 255;
            if (auraRed < 0) auraRed = 0;
            if (auraBlue > 255) auraBlue = 255;
            if (auraBlue < 0) auraBlue = 0;
            auraMaterial.Diffuse = Color.FromArgb(255, auraRed, 255, auraBlue);

            OpenGL GL = sceneControl.OpenGL;


            #region draw quads
            GL.LoadIdentity();
            
            GL.Rotate(rquad, .0f, .0f, 1.0f);           // Rotate The Cube On X, Y & Z

            GL.Begin(OpenGL.GL_QUADS);                    // Start Drawing The Cube

            GL.Color(0.0f, 1.0f, 0.0f);			// Set The Color To Green
            GL.Vertex(1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Top)
            GL.Vertex(-1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Top)
            GL.Vertex(-1.0f, 1.0f, 1.0f);			// Bottom Left Of The Quad (Top)
            GL.Vertex(1.0f, 1.0f, 1.0f);			// Bottom Right Of The Quad (Top)

            GL.Color(1.0f, 0.5f, 0.0f);			// Set The Color To Orange
            GL.Vertex(1.0f, -1.0f, 1.0f);			// Top Right Of The Quad (Bottom)
            GL.Vertex(-1.0f, -1.0f, 1.0f);			// Top Left Of The Quad (Bottom)
            GL.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Bottom)
            GL.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Bottom)
            
            GL.Color(1.0f, 0.0f, 0.0f);			// Set The Color To Red
            GL.Vertex(1.0f, 1.0f, 1.0f);			// Top Right Of The Quad (Front)
            GL.Vertex(-1.0f, 1.0f, 1.0f);			// Top Left Of The Quad (Front)
            GL.Vertex(-1.0f, -1.0f, 1.0f);			// Bottom Left Of The Quad (Front)
            GL.Vertex(1.0f, -1.0f, 1.0f);			// Bottom Right Of The Quad (Front)
            
            GL.Color(1.0f, 1.0f, 0.0f);			// Set The Color To Yellow
            GL.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Back)
            GL.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Back)
            GL.Vertex(-1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Back)
            GL.Vertex(1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Back)
            
            GL.Color(0.0f, 0.0f, 1.0f);			// Set The Color To Blue
            GL.Vertex(-1.0f, 1.0f, 1.0f);			// Top Right Of The Quad (Left)
            GL.Vertex(-1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Left)
            GL.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Left)
            GL.Vertex(-1.0f, -1.0f, 1.0f);			// Bottom Right Of The Quad (Left)
            
            GL.Color(1.0f, 0.0f, 1.0f);			// Set The Color To Violet
            GL.Vertex(1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Right)
            GL.Vertex(1.0f, 1.0f, 1.0f);			// Top Left Of The Quad (Right)
            GL.Vertex(1.0f, -1.0f, 1.0f);			// Bottom Left Of The Quad (Right)
            GL.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Right)
            GL.End();                       // Done Drawing The Q
            
            GL.Flush();
            #endregion draw quads

            System.Collections.ObjectModel.ObservableCollection<SceneElement> sceneAura =
                sceneControl.Scene.SceneContainer.Children;
            var tempvar = GL.Vendor;

            //rtri += .30f;// 0.2f;						// Increase The Rotation Variable For The Triangle 
            //rquad -= .30f;// 0.15f;						// Decrease The Rotation Variable For The Quad 
        }

        float rtri = 0f;// 0.2f;						// Increase The Rotation Variable For The Triangle 
        float rquad = 0;// 0.15f;	
    }
}
