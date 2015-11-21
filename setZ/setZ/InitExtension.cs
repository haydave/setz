using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;

namespace setZ
{
    //public class InitExtension : IExtensionApplication
    //{
    //    #region IExtensionApplication Members
    //    public void Initialize()
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }
    //    public void Terminate()
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }
    //    #endregion
    //}
 
    public class MakeZet
    {
        [CommandMethod("TEST")]
        public void TEST()
        {
            double avgElevation = 0;
            CivilDocument doc = CivilApplication.ActiveDocument;
            CogoPointCollection cogoPoints = CivilApplication.ActiveDocument.CogoPoints;
            foreach (ObjectId pointId in cogoPoints)
            {
                CogoPoint cogoPoint =
                pointId.GetObject(OpenMode.ForRead) as CogoPoint;
                avgElevation += cogoPoint.Elevation;
            }
            avgElevation /= cogoPoints.Count;
            String docInfo = String.Format("{0} \nd {1} \n", avgElevation, cogoPoints.Count);
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(docInfo);
        }
    }
}