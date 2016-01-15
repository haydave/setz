// (C) Copyright 2015 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Interop;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.AutoCAD.Interop.Common;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Collections;
using System.Linq;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(setZ.MyCommands))]

namespace setZ
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Modal Command with localized name
        [CommandMethod("MyGroup", "MyCommand", "MyCommandLocal", CommandFlags.Modal)]
        public void MyCommand() // This method can have any name
        {

            using (Transaction tr = Application.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                // Get the current document and database
                Document acDoc = Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = acDoc.Database;
                double[] textInfo = new double[3];
                List<double[]> listInfo = new List<double[]>();
                List<Autodesk.AutoCAD.DatabaseServices.Entity> targets = new List<Autodesk.AutoCAD.DatabaseServices.Entity>();
                double txtValue;
               // double i=0;
                PromptSelectionResult selection = ed.SelectAll();
                if (selection.Status == PromptStatus.OK)
                {
                    foreach (ObjectId id in selection.Value.GetObjectIds())
                    {
                        Autodesk.AutoCAD.DatabaseServices.Entity ent = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(id, OpenMode.ForRead);
                        LayerTableRecord layer = (LayerTableRecord)tr.GetObject(ent.LayerId, OpenMode.ForRead);
                        if (!layer.IsFrozen)
                        {
                            //i = i + 1;
                            Autodesk.AutoCAD.DatabaseServices.Entity obj = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(id, OpenMode.ForRead);
                            // Collect DBText and MTexts in listInfo, others in targets 
                            if (obj.GetType().Name == "DBText")
                            {
                                DBText txt = (DBText)tr.GetObject(obj.ObjectId, OpenMode.ForRead);
                                if (double.TryParse(txt.TextString, out txtValue))
                                {
                                    textInfo[0] = txtValue;
                                    textInfo[1] = txt.Position.X;
                                    textInfo[2] = txt.Position.Y;
                                }
                            }
                            else if (obj.GetType().Name == "MText")
                            {
                                MText txt = (MText)tr.GetObject(obj.ObjectId, OpenMode.ForRead);
                                if (double.TryParse(txt.Contents, out txtValue))
                                {
                                    textInfo[0] = txtValue;
                                    textInfo[1] = txt.Location.X;
                                    textInfo[2] = txt.Location.Y;
                                }
                            }
                            else
                            {
                                targets.Add(obj);
                            }
                            listInfo.Add(textInfo.ToArray());
                        }
                    }
                }
                // ed.WriteMessage("\n{0}", i);
                // Loop through each element (DBText and MText should be skipped)
                foreach (Autodesk.AutoCAD.DatabaseServices.Entity obj in targets)
                {
                    if (obj.GetType().Name == "Ellipse")
                    {
                        Ellipse el = (Ellipse)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        el.Center = new Point3d(el.Center.X, el.Center.Y, GetNearText(ref listInfo, el.Center.X, el.Center.Y));
                    }
                    else if (obj.GetType().Name == "Circle")
                    {
                        Circle el = (Circle)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        el.Center = new Point3d(el.Center.X, el.Center.Y, GetNearText(ref listInfo, el.Center.X, el.Center.Y));
                    }
                    else if (obj.GetType().Name == "Arc")
                    {
                        Arc el = (Arc)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        el.Center = new Point3d(el.Center.X, el.Center.Y, GetNearText(ref listInfo, el.Center.X, el.Center.Y));
                    }
                    else if (obj.GetType().Name == "DBPoint")
                    {
                        DBPoint el = (DBPoint)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        el.Position = new Point3d(el.Position.X, el.Position.Y, GetNearText(ref listInfo, el.Position.X, el.Position.Y));
                    }
                    else if (obj.GetType().Name == "Polyline")
                    {
                        // Do nothing
                        //Polyline pl = (Polyline)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        //pl.Elevation = 12;
                    }
                }
                tr.Commit();
           }
        }

        private double GetNearText(ref List<double[]> listInfo, double elX, double elY)
        {
            int minI = 0;
            double minDistance = 999999999;
            double distance = 0;
            double txtValue = 0;
            for (int i = 0; i < listInfo.Count; ++i)
            {
                distance = Math.Sqrt((listInfo[i][1] - elX) * (listInfo[i][1] - elX) + (listInfo[i][2] - elY) * (listInfo[i][2] - elY));
                if (minDistance > distance)
                {
                    txtValue = listInfo[i][0];
                    minDistance = distance;
                    minI = i;
                }
            }
            listInfo.RemoveAt(minI);
            return txtValue;
        }
    }
}
