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
            SceneContainer container = sceneControl.Scene.SceneContainer;
            // Remove red bounding box of models
            sceneControl.Scene.RenderBoundingVolumes =
                !sceneControl.Scene.RenderBoundingVolumes;
            // Remove default axies
            //container.Children[0].RemoveChild(container.Children[0].Children[1]);
            // Remove default Grid
            container.Children[0].RemoveChild(container.Children[0].Children[0]);

            humanMaterial.Diffuse = Color.FromArgb(255, 100, 100, 100);

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
            //foreach (Asset asset in sceneHumanObject.Assets) sceneControl.Scene.Assets.Add(asset);
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
                polygon.Material.Push(gl: sceneControl.OpenGL);
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

        }

        #region mouse events

        private void ConrolLabelDoubleClick(object sender, EventArgs e)
        {
            isRotate = true;
        }

        private void ConrolLabelClick(object sender, EventArgs e)
        {
            isRotate = false;
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
            //OpenGL GL = sceneControl.OpenGL;
            //GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            
            if (auraRed > 255) auraRed = 255;
            if (auraRed < 0) auraRed = 0;
            if (auraBlue > 255) auraBlue = 255;
            if (auraBlue < 0) auraBlue = 0;
            auraMaterial.Diffuse = Color.FromArgb(255, auraRed, 255, auraBlue);
        }
    }
}
//Sphere sphereAura = new Sphere();
//sphereAura.QuadricDrawStyle = DrawStyle.Line;
//sphereAura.Transformation.ScaleX = 4f;
//sphereAura.Transformation.ScaleY = 4f;
//sphereAura.Transformation.ScaleZ = 6f;
//sphereAura.AddEffect(arcBallEffect);
//int stacks = sphereAura.Stacks;
//sphereAura.Material = auraMaterial;
//sceneControl.Scene.SceneContainer.AddChild(sphereAura);
//Cylinder cylP_r = new Cylinder(),
//         cylGI_r = new Cylinder(),
//         cylE_r = new Cylinder(),
//         cylRP_r = new Cylinder(),
//         cylC_r = new Cylinder(),
//         cylIG_r = new Cylinder(),
//         cylV_r = new Cylinder(),
//         cylR_r = new Cylinder(),
//         cylMC_r = new Cylinder(),
//         cylTR_r = new Cylinder(),
//         cylVB_r = new Cylinder(),
//         cylF_r = new Cylinder(),
//         cylP_l = new Cylinder(),
//         cylGI_l = new Cylinder(),
//         cylE_l = new Cylinder(),
//         cylRP_l = new Cylinder(),
//         cylC_l = new Cylinder(),
//         cylIG_l = new Cylinder(),
//         cylV_l = new Cylinder(),
//         cylR_l = new Cylinder(),
//         cylMC_l = new Cylinder(),
//         cylTR_l = new Cylinder(),
//         cylVB_l = new Cylinder(),
//         cylF_l = new Cylinder();

//List<Cylinder> cylinders = new List<Cylinder>(24);

//cylinders.Add(cylP_r);
//cylinders.Add(cylGI_r);
//cylinders.Add(cylE_r);
//cylinders.Add(cylRP_r);
//cylinders.Add(cylC_r);
//cylinders.Add(cylIG_r);
//cylinders.Add(cylV_r);
//cylinders.Add(cylR_r);
//cylinders.Add(cylMC_r);
//cylinders.Add(cylTR_r);
//cylinders.Add(cylVB_r);
//cylinders.Add(cylF_r);
//cylinders.Add(cylP_l);
//cylinders.Add(cylGI_l);
//cylinders.Add(cylE_l);
//cylinders.Add(cylRP_l);
//cylinders.Add(cylC_l);
//cylinders.Add(cylIG_l);
//cylinders.Add(cylV_l);
//cylinders.Add(cylR_l);
//cylinders.Add(cylMC_l);
//cylinders.Add(cylTR_l);
//cylinders.Add(cylVB_l);
//cylinders.Add(cylF_l);

//cylP_r.Name = "cylP_r";
//cylGI_r.Name = "cylGI_r";
//cylE_r.Name = "cylE_r";
//cylRP_r.Name = "cylRP_r";
//cylC_r.Name = "cylC_r";
//cylIG_r.Name = "cylIG_r";
//cylV_r.Name = "cylV_r";
//cylR_r.Name = "cylR_r";
//cylMC_r.Name = "cylMC_r";
//cylTR_r.Name = "cylTR_r";
//cylVB_r.Name = "cylVB_r";
//cylF_r.Name = "cylF_r";
//cylP_l.Name = "cylP_l";
//cylGI_l.Name = "cylGI_l";
//cylE_l.Name = "cylE_l";
//cylRP_l.Name = "cylRP_l";
//cylC_l.Name = "cylC_l";
//cylIG_l.Name = "cylIG_l";
//cylV_l.Name = "cylV_l";
//cylR_l.Name = "cylR_l";
//cylMC_l.Name = "cylMC_l";
//cylTR_l.Name = "cylTR_l";
//cylVB_l.Name = "cylVB_l";
//cylF_l.Name = "cylF_l";

//foreach (Cylinder cylinder in cylinders)
//{
//    cylinder.Material = auraMaterial;
//    cylinder.AddEffect(arcBallEffect);
//    //cylinder.TopRadius  = 1f;
//    //cylinder.BaseRadius = 1f;
//    cylinder.Slices = 24;
//    cylinder.Stacks = 1;
//    cylinder.Height = .1;
//    cylinder.QuadricDrawStyle = DrawStyle.Line;
//    cylinder.Transformation.ScaleX = 4f;
//    cylinder.Transformation.ScaleY = 4f;
//    cylinder.Transformation.ScaleZ = 4f;
//}

//for (int i = 0; i < 24; i++)
//{
//    sceneControl.Scene.SceneContainer.AddChild(cylinders[i]);
//}

//for (int i = 12; i >= 1; i--)
//{
//    int r = Math.Abs(i - 11);
//    cylinders[i].TopRadius = r / 10f;
//    cylinders[i + 11].TopRadius = r / 10f;
//    cylinders[i].Transformation.TranslateZ = i / 2.5f;
//    cylinders[i + 11].Transformation.TranslateZ = -i / 2.5f;
//}

