using SharpGL;
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
        ScissorAttributes scissorAttributes = new ScissorAttributes();

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
            #region init cylinders
            Cylinder cylP_r  = new Cylinder(),
                     cylGI_r = new Cylinder(),
                     cylE_r  = new Cylinder(),
                     cylRP_r = new Cylinder(),
                     cylC_r  = new Cylinder(),
                     cylIG_r = new Cylinder(),
                     cylV_r  = new Cylinder(),
                     cylR_r  = new Cylinder(),
                     cylMC_r = new Cylinder(),
                     cylTR_r = new Cylinder(),
                     cylVB_r = new Cylinder(),
                     cylF_r  = new Cylinder(),
                     cylP_l  = new Cylinder(),
                     cylGI_l = new Cylinder(),
                     cylE_l  = new Cylinder(),
                     cylRP_l = new Cylinder(),
                     cylC_l  = new Cylinder(),
                     cylIG_l = new Cylinder(),
                     cylV_l  = new Cylinder(),
                     cylR_l  = new Cylinder(),
                     cylMC_l = new Cylinder(),
                     cylTR_l = new Cylinder(),
                     cylVB_l = new Cylinder(),
                     cylF_l  = new Cylinder();
            #endregion init cylinders

            List<Cylinder> cylinders = new List<Cylinder>(24);

            #region add cylinders to list
            cylinders.Add(cylP_r );
            cylinders.Add(cylGI_r);
            cylinders.Add(cylE_r );
            cylinders.Add(cylRP_r);
            cylinders.Add(cylC_r );
            cylinders.Add(cylIG_r);
            cylinders.Add(cylV_r );
            cylinders.Add(cylR_r );
            cylinders.Add(cylMC_r);
            cylinders.Add(cylTR_r);
            cylinders.Add(cylVB_r);
            cylinders.Add(cylF_r );
            cylinders.Add(cylP_l );
            cylinders.Add(cylGI_l);
            cylinders.Add(cylE_l );
            cylinders.Add(cylRP_l);
            cylinders.Add(cylC_l );
            cylinders.Add(cylIG_l);
            cylinders.Add(cylV_l );
            cylinders.Add(cylR_l );
            cylinders.Add(cylMC_l);
            cylinders.Add(cylTR_l);
            cylinders.Add(cylVB_l);
            cylinders.Add(cylF_l );
            #endregion add cylinders to list

            #region named cylindres
            cylP_r.Name  = "cylP_r";
            cylGI_r.Name = "cylGI_r";
            cylE_r.Name  = "cylE_r";
            cylRP_r.Name = "cylRP_r";
            cylC_r.Name  = "cylC_r";
            cylIG_r.Name = "cylIG_r";
            cylV_r.Name  = "cylV_r";
            cylR_r.Name  = "cylR_r";
            cylMC_r.Name = "cylMC_r";
            cylTR_r.Name = "cylTR_r";
            cylVB_r.Name = "cylVB_r";
            cylF_r.Name  = "cylF_r";
            cylP_l.Name  = "cylP_l";
            cylGI_l.Name = "cylGI_l";
            cylE_l.Name  = "cylE_l";
            cylRP_l.Name = "cylRP_l";
            cylC_l.Name  = "cylC_l";
            cylIG_l.Name = "cylIG_l";
            cylV_l.Name  = "cylV_l";
            cylR_l.Name  = "cylR_l";
            cylMC_l.Name = "cylMC_l";
            cylTR_l.Name = "cylTR_l";
            cylVB_l.Name = "cylVB_l";
            cylF_l.Name  = "cylF_l";
            #endregion named cylindres

            #region build sphere
            cylP_r.TopRadius = 1.9f;
            cylP_r.BaseRadius = 2.7f;
            cylP_r.Transformation.TranslateZ = 5f;

            cylGI_r.TopRadius = 2.7f;
            cylGI_r.BaseRadius = 3.4f;
            cylGI_r.Transformation.TranslateZ = 4f;

            cylE_r.TopRadius = 3.4f;
            cylE_r.BaseRadius = 4f;
            cylE_r.Transformation.TranslateZ = 3f;

            cylRP_r.TopRadius = 4f;
            cylRP_r.BaseRadius = 4.5f;
            cylRP_r.Transformation.TranslateZ = 2f;

            cylC_r.TopRadius = 4.5f;
            cylC_r.BaseRadius = 4.9f;
            cylC_r.Transformation.TranslateZ = 1f;

            cylIG_r.TopRadius = 4.9f;
            cylIG_r.BaseRadius = 5f;
            cylIG_r.Transformation.TranslateZ = 0f;

            cylV_r.TopRadius = 5f;
            cylV_r.BaseRadius = 4.9f;
            cylV_r.Transformation.TranslateZ = -1f;

            cylR_r.TopRadius = 4.9f;
            cylR_r.BaseRadius = 4.5f;
            cylR_r.Transformation.TranslateZ = -2f;

            cylMC_r.TopRadius = 4.5f;
            cylMC_r.BaseRadius = 4f;
            cylMC_r.Transformation.TranslateZ = -3f;

            cylTR_r.TopRadius = 4f;
            cylTR_r.BaseRadius = 3.4f;
            cylTR_r.Transformation.TranslateZ = -4f;

            cylVB_r.TopRadius = 3.4f;
            cylVB_r.BaseRadius = 2.7f;
            cylVB_r.Transformation.TranslateZ = -5f;

            cylF_r.TopRadius = 2.7f;
            cylF_r.BaseRadius = 1.9f;
            cylF_r.Transformation.TranslateZ = -6f;

            //cylP_l
            //cylGI_l
            //cylE_l
            //cylRP_l
            //cylC_l
            //cylIG_l
            //cylV_l
            //cylR_l.
            //cylMC_l
            //cylTR_l
            //cylVB_l
            //cylF_l
            #endregion build sphere

            foreach (Cylinder cylinder in cylinders)
            {
                cylinder.Material = auraMaterial;
                cylinder.AddEffect(arcBallEffect);
                cylinder.Slices = 24;
                cylinder.Stacks = 1;
                cylinder.Height = 1f;
                cylinder.QuadricDrawStyle = DrawStyle.Line;
            }

            for (int i = 0; i < 24; i++)
            {
                sceneControl.Scene.SceneContainer.AddChild(cylinders[i]);
            }
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
