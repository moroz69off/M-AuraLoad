using SharpGL;
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
            DynAura DAura = new DynAura();
            Dictionary<string, int> scanData = DynAura.ScanData;
            Polygon aura = DynAura.auraPolygon;
            DAura.QuadricDrawStyle = DrawStyle.Line;
            DAura.DrawDefaultAura(sceneControl, aura);
            
            //// From AynamicAura class
            //DynA = new DynamicAura();
            //List<Vertex> aVerts = DynamicAura.AuraVertices;
            //DynA.LoadDefault((SceneControl)Controls[0]);
            //DynA.TransformAura(DynamicAura.ScanData);

            //Sphere sphereAura = new Sphere();
            //sphereAura.QuadricDrawStyle = DrawStyle.Line;
            //sphereAura.Transformation.ScaleX = 4f;
            //sphereAura.Transformation.ScaleY = 4f;
            //sphereAura.Transformation.ScaleZ = 6f;
            //sphereAura.AddEffect(arcBallEffect);
            //int stacks = sphereAura.Stacks;
            //sphereAura.Material = auraMaterial;
            //sceneControl.Scene.SceneContainer.AddChild(sphereAura);
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
        }
    }
}
