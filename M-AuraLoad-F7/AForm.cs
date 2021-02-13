using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Effects;
using SharpGL.Serialization;
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

        public AForm()
        {
            InitializeComponent();
            LoadHuman(isMale);
        }

        private void LoadHuman(bool isMale)
        {
            if (isMale)
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "male.obj");
            else path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "female.obj");
            Scene scene = SerializationEngine.Instance.LoadScene(path);
        }

        /// <summary>
        /// Load scene control in windows form
        /// </summary>
        /// <param name="sender">Scene control</param>
        /// <param name="e">System.EventArgs</param>
        private void sceneControl_Load(object sender, EventArgs e)
        {
            object testSender = sender;
        }
        #region mouse events
        private void sceneControl_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void sceneControl_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void sceneControl_MouseUp(object sender, MouseEventArgs e)
        {

        }
        #endregion mouse events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Scene control</param>
        /// <param name="args">System.Drawing.Graphics</param>
        private void sceneControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            args.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
    }
}
