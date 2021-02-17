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
using SharpGL.Enumerations;

namespace M_AuraLoad_F7
{
    class DynAura : Quadric
    {
        private readonly Effect arcBallEffect = new ArcBallEffect();
        private static string dir = AppDomain.CurrentDomain.BaseDirectory;
        private static char[] delimiter = new char[] { '=' };
        private static ObjFileFormat dataAuraSphereObj = new ObjFileFormat();
        static string pathAura = Path.Combine(dir, "data", "aura.obj");
        static Scene sceneAuraObject = dataAuraSphereObj.LoadData(pathAura);

        public DynAura()
        {
            auraPolygon = (Polygon)sceneAuraObject.SceneContainer.Children[0];
        }

        public static Polygon auraPolygon { get; set; }

        private static string[] scanLines = File.ReadAllLines(Path.Combine(dir, "data", "auradata.txt"));

        public static Dictionary<string, int> ScanData { get; } = GetScanData(scanLines);

        private static Dictionary<string, int> GetScanData(string[] scanLines)
        {
            Dictionary<string, int> auraScanData = new Dictionary<string, int>(24);
            foreach (var scanLine in scanLines)
            {
                auraScanData.Add(
                    scanLine.Split(delimiter)[0].Trim(),
                    int.Parse(scanLine.Split(delimiter)[1]));
            }
            return auraScanData;
        }

        public void DrawDefaultAura(SceneControl sceneControl, Polygon auraPolygon)
        {
            var sControl = sceneControl;
            var aPolygon = auraPolygon;
        }

        Object nothing = "";
    }
}
