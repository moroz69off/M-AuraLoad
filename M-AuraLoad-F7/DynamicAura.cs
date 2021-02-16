using SharpGL.SceneGraph.Primitives;
using SharpGL.Serialization;
using SharpGL.SceneGraph;
using SharpGL.OpenGLAttributes;
using SharpGL;
using System.Collections.Generic;
using SharpGL.Serialization.Wavefront;
using System.IO;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Raytracing;
using SharpGL.SceneGraph.Helpers;
using System.Xml.Serialization;
using SharpGL.SceneGraph.Transformations;
using SharpGL.SceneGraph.Assets;
using System.Windows.Forms;
using SharpGL.SceneGraph.Effects;
using SharpGL.SceneGraph.Quadrics;

namespace M_AuraLoad_F7
{
    [Serializable()]
    public class DynamicAura : Polygon
    {
        private readonly Effect arcBallEffect = new ArcBallEffect();
        private static string dir = AppDomain.CurrentDomain.BaseDirectory;
        private static char[] delimiter = new char[] { '=' };
        private static ObjFileFormat dataAuraSphereObj = new ObjFileFormat();
        static string pathAura = Path.Combine(dir, "data", "aura.obj");
        static Scene sceneAuraObject = dataAuraSphereObj.LoadData(pathAura);

        public static Polygon auraPolygon { get; set; } = (Polygon)sceneAuraObject.SceneContainer.Children[0];

        public static List<Vertex> AuraVertices { get; set; } = auraPolygon.Vertices;

        private static string[] scanLines = File.ReadAllLines(Path.Combine(dir, "data", "auradata.txt"));
        

        public static Dictionary<string, int> ScanData { get; } = GetScanData(scanLines);

        /// <summary>
        /// Converts scanner data strings to data library
        /// </summary>
        /// <param name="scanLines"></param>
        /// <returns>Dictionary<string, int></returns>
        public static Dictionary<string, int> GetScanData(string[] scanLines)
        {
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>(24);
            foreach (var scanLine in scanLines)
            {
                keyValuePairs.Add(
                    scanLine.Split(delimiter)[0].Trim(),
                    int.Parse(scanLine.Split(delimiter)[1]));
            }
            return keyValuePairs;
        }

        public void TransformAura(Dictionary<string, int> scanData)
        {
            MessageBox.Show(scanData.GetType().ToString());
        }

        internal void LoadDefault(SceneControl sceneControl)
        {
            foreach (Asset asset in sceneAuraObject.Assets) sceneControl.Scene.Assets.Add(asset);
            List<Polygon> polygons = sceneAuraObject.SceneContainer.Traverse<Polygon>().ToList();
            foreach (Polygon polygon in polygons)
            {
                polygon.Name = "AURA";
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
                polygon.Render(sceneControl.OpenGL, RenderMode.HitTest);
            }
        }
    }
}
