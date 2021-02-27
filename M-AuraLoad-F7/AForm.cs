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
        private Polygon aPolygon;
        private Material humanMaterial = new Material();
        OpenGLControl openGLControl = new OpenGLControl();
        private bool isRotate = true;

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
            openGLControl.BackColor = Color.FromArgb(100, 0, 100, 100);
            openGLControl.Dock = DockStyle.Fill;
            openGLControl.OpenGLDraw += new RenderEventHandler(openGLcontrol_OpenGLDraw);
            openGLControl.Load += new EventHandler(openGLcontrol_Load);
            openGLControl.MouseDown += new MouseEventHandler(openGLcontrol_MouseDown);
            openGLControl.MouseMove += new MouseEventHandler(openGLcontrol_MouseMove);
            openGLControl.MouseUp += new MouseEventHandler(openGLcontrol_MouseUp);

            sceneControl.Controls.Add(openGLControl);

            SceneContainer container = sceneControl.Scene.SceneContainer;
            // Remove red bounding box of models
            sceneControl.Scene.RenderBoundingVolumes =
                !sceneControl.Scene.RenderBoundingVolumes;
            // Remove default axies
            container.Children[0].RemoveChild(container.Children[0].Children[1]);
            // Remove default Grid
            container.Children[0].RemoveChild(container.Children[0].Children[0]);

            SceneElement lightsFolder = sceneControl.Scene.SceneContainer.Children[1];
            Light light1 = (Light)lightsFolder.Children[0];
            Light light2 = (Light)lightsFolder.Children[1];
            Light light3 = (Light)lightsFolder.Children[2];
            light1.Position = new Vertex(-9, -9, 0);
            light2.Position = new Vertex(9, -9, 0);
            light3.Position = new Vertex(-9, 9, 0);

            sceneControl.Scene.CurrentCamera.Position = new Vertex(0, -12.345f, 0);
        }

        private void openGLcontrol_Load(object sender, EventArgs e)
        {

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
            Scene sceneData = obj.LoadData(path);
            foreach (Asset asset in sceneData.Assets) sceneControl.Scene.Assets.Add(asset);
            List<Polygon> polygons = sceneData.SceneContainer.Traverse<Polygon>().ToList();
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
                polygon.Material = humanMaterial;
                polygon.Material.Push(gl: sceneControl.OpenGL);
                polygon.Freeze(sceneControl.OpenGL);
                polygon.AddEffect(new OpenGLAttributesEffect());
                polygon.AddEffect(arcBallEffect);
                sceneControl.Scene.SceneContainer.AddChild(polygon); // add ? true
            }
        }

        /// <summary>
        /// Load default aura to scene
        /// </summary>
        private void LoadAura()
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "auraCuttb.obj");
            ObjFileFormat obj = new ObjFileFormat();
            Scene sceneData = obj.LoadData(path);
            List<Polygon> polygons = sceneData.SceneContainer.Traverse<Polygon>().ToList();
            foreach (var polygon in polygons)
            {
                polygon.Name = "AURA";
                BoundingVolume boundingVolume = polygon.BoundingVolume;
                var extent = new float[3];
                boundingVolume.GetBoundDimensions(out extent[0], out extent[1], out extent[2]);
                float maxExtent = extent.Max();
                float scaleFactor = maxExtent > 10 ? 10.0f / maxExtent : 1;
                polygon.Parent.RemoveChild(polygon);
                polygon.Transformation.RotateX = 90;
                polygon.Transformation.ScaleX = scaleFactor * 6;
                polygon.Transformation.ScaleY = scaleFactor * 6;
                polygon.Transformation.ScaleZ = scaleFactor * 6;
                polygon.Material = humanMaterial;
                polygon.Material.Push(gl: sceneControl.OpenGL);
                polygon.Freeze(sceneControl.OpenGL);
                polygon.AddEffect(new OpenGLAttributesEffect());
                polygon.AddEffect(arcBallEffect);
                aPolygon = polygon;
            }
        }

        #region mouse events

        //private void ConrolLabelDoubleClick(object sender, EventArgs e)
        //{
        //    isRotate = true;
        //}

        //private void ConrolLabelClick(object sender, EventArgs e)
        //{
        //    isRotate = false;
        //}

        private void openGLcontrol_MouseDown(object sender, MouseEventArgs e)
        {
            sceneControl_MouseDown(sender, e);
        }
        private void sceneControl_MouseDown(object sender, MouseEventArgs e)
        {
            arcBallEffect.ArcBall.SetBounds(sceneControl.Width, sceneControl.Height);
            arcBallEffect.ArcBall.MouseDown(e.X/2, e.Y/2);
        }

        private void openGLcontrol_MouseMove(object sender, MouseEventArgs e)
        {
            sceneControl_MouseMove(sender, e);
        }
        private void sceneControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) arcBallEffect.ArcBall.MouseMove(e.X/2, e.Y/2);
        }

        private void openGLcontrol_MouseUp(object sender, MouseEventArgs e)
        {
            sceneControl_MouseUp(sender, e);
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
        private void sceneControl_OpenGLDraw(object sender, RenderEventArgs args)
        {

        }

        /// <summary>
        /// Draw openGLcontrol in framerate resolution
        /// </summary>
        /// <param name="sender">OpenGLcontrol</param>
        /// <param name="args">System.Drawing.Graphics</param>
        private void openGLcontrol_OpenGLDraw(object sender, RenderEventArgs args)
        {
            
        }
    }
}
