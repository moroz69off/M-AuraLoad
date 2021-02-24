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
        AuraQuad AQuadr = new AuraQuad();
        private Material humanMaterial = new Material();
        public OpenGLControl openGLControl;
        private bool isRotate = true;

        public AForm()
        {
            InitializeComponent();
            openGLControl = new OpenGLControl();
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
            //=================================
            // Test add control

            openGLControl.BackColor = Color.FromArgb(0, 0, 0, 0);
            openGLControl.Dock = DockStyle.Fill;
            Label controlLabel = new Label();
            controlLabel.Click += new EventHandler(ConrolLabelClick);
            controlLabel.DoubleClick += new EventHandler(ConrolLabelDoubleClick);
            controlLabel.Name = "Label";
            controlLabel.Text = "Rotate " + aPolygon.Faces.Count;
            controlLabel.BackColor = Color.White;
            controlLabel.Width = 350;
            controlLabel.Height = 20;
            openGLControl.Controls.Add(controlLabel);

            sceneControl.Controls.Add(openGLControl);

            // End Test add control
            //=================================


            //sceneControl.OpenGL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);
            //sceneControl.OpenGL.Enable(OpenGL.GL_COLOR_MATERIAL);
            //sceneControl.OpenGL.Enable(OpenGL.GL_COLOR_MATERIAL_PARAMETER);
            //sceneControl.OpenGL.Enable(OpenGL.GL_COLOR_TABLE_ALPHA_SIZE_EXT);
            //sceneControl.OpenGL.Enable(OpenGL.GL_ALPHA);
            //sceneControl.OpenGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE);

            // Remove red bounding box of models
            sceneControl.Scene.RenderBoundingVolumes =
                !sceneControl.Scene.RenderBoundingVolumes;
            // Remove default axies
            sceneControl.Scene.SceneContainer.Children[0]
                .RemoveChild(sceneControl.Scene.SceneContainer.Children[0].Children[1]);
            // Remove default Grid
            sceneControl.Scene.SceneContainer.Children[0]
                .RemoveChild(sceneControl.Scene.SceneContainer.Children[0].Children[0]);

            humanMaterial.Diffuse = Color.FromArgb(255, 100, 100, 100);

            auraMaterial.Diffuse = Color.FromArgb(50, auraRed, 255, auraBlue);
            auraMaterial.Specular = Color.FromArgb(0, 0, 0, 0);

            // Lighting the scee
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
                polygon.Material = humanMaterial;
                polygon.Material.Push(sceneControl.OpenGL);
                polygon.Freeze(sceneControl.OpenGL);
                polygon.AddEffect(new OpenGLAttributesEffect());
                polygon.AddEffect(arcBallEffect);
                sceneControl.Scene.SceneContainer.AddChild(polygon);
            }
        }

        /// <summary>
        /// Load default aura to scene
        /// </summary>
        private void LoadAura()
        {
            string apath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "auraCuttb.obj");
            ObjFileFormat obj = new ObjFileFormat();
            Scene sceneAuraObject = obj.LoadData(apath);
            List<Polygon> polygons = sceneAuraObject.SceneContainer.Traverse<Polygon>().ToList();
            aPolygon = polygons[0];
            foreach (Polygon polygon in polygons)
            {
                polygon.Name = "AURA";
                BoundingVolume boundingVolume = polygon.BoundingVolume;
                var extent = new float[3];
                boundingVolume.GetBoundDimensions(out extent[0], out extent[1], out extent[2]);
                float maxExtent = extent.Max();
                float scaleFactor = maxExtent > 10 ? 10.0f / maxExtent : 1;
                polygon.Parent.RemoveChild(polygon);
                polygon.Transformation.RotateX = 90; // 
                polygon.Transformation.ScaleX = scaleFactor * 6;
                polygon.Transformation.ScaleY = scaleFactor * 6;
                polygon.Transformation.ScaleZ = scaleFactor * 6;
                polygon.Material = auraMaterial;
                polygon.Freeze(sceneControl.OpenGL);
                polygon.AddEffect(new OpenGLAttributesEffect());
                polygon.AddEffect(arcBallEffect);
                
                //sceneControl.Scene.SceneContainer.AddChild(polygon);
            }
        }

        #region mouse events

        private void ConrolLabelDoubleClick(object sender, EventArgs e)
        {
            isRotate = true;
        }

        private void ConrolLabelClick(object sender, EventArgs e)
        {
            isRotate = false;
            AQuadr.rquad = 0;
        }

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
            AQuadr.CreateAura(openGLControl.OpenGL, aPolygon);
            if (isRotate) AQuadr.rquad += 4f;
        }
    }
}
