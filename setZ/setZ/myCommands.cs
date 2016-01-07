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
                CivilDocument civildoc = Autodesk.Civil.ApplicationServices.CivilApplication.ActiveDocument;
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ObjectId cirId = ObjectId.Null;
                Database db = HostApplicationServices.WorkingDatabase;
                // Get the current document and database
                Document acDoc = Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = acDoc.Database;
                BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                foreach (ObjectId id in btr)
                {
                    Autodesk.AutoCAD.DatabaseServices.Entity obj = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(id, OpenMode.ForRead);
                    // If it is Polyline change the elevation.
                    //if (obj.GetType().Name == "Polyline")
                    //{
                    //    Polyline pl = (Polyline)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                    //    pl.Elevation = 12;
                    //    ed.WriteMessage("polyline: {0}\n", pl.Id);
                    //}
                    ////// If it is Circle change the Center Z.
                    //if (obj.GetType().Name == "Circle")
                    //{
                    //    Circle el = (Circle)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                    //    double minDistance = 999999999999999;
                    //    double distance;
                    //    double txtValue = 0;
                    //    // Test MText elements
                    //    foreach (ObjectId ids in btr)
                    //    {
                    //        Autodesk.AutoCAD.DatabaseServices.Entity objs = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(ids, OpenMode.ForRead);
                    //        if (objs.GetType().Name == "MText")
                    //        {
                    //            MText txt = (MText)tr.GetObject(objs.ObjectId, OpenMode.ForRead);
                    //            distance = Math.Sqrt((txt.Location.X - el.Center.X) * (txt.Location.X - el.Center.X) + (txt.Location.Y - el.Center.Y) * (txt.Location.Y - el.Center.Y));
                    //            if (minDistance > distance)
                    //            {
                    //                try
                    //                {
                    //                    txtValue = Convert.ToDouble(txt.Contents);
                    //                    Console.WriteLine("sadasdasd '{0}' to {1}.", txtValue, distance);
                    //                    minDistance = distance;
                    //                    Console.WriteLine("Converted '{0}' to {1}.", txt.Contents, txtValue);
                    //                }
                    //                catch (FormatException)
                    //                {
                    //                    Console.WriteLine("Unable to convert '{0}' to a Double.", txt.Contents);
                    //                }
                    //                catch (OverflowException)
                    //                {
                    //                    Console.WriteLine("'{0}' is outside the range of a Double.", txt.Contents);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    el.Center = new Point3d(el.Center.X, el.Center.Y, txtValue);
                    //}
                    //// If it is Point change the Position Z.
                    //if (obj.GetType().Name == "DBPoint")
                    //{
                    //    DBPoint pt = (DBPoint)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                    //    double minDistance = 999999999999999;
                    //    double distance;
                    //    double txtValue = 0;
                    //    // Test MText elements
                    //    foreach (ObjectId ids in btr)
                    //    {
                    //        Autodesk.AutoCAD.DatabaseServices.Entity objs = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(ids, OpenMode.ForRead);
                    //        if (objs.GetType().Name == "MText")
                    //        {
                    //            MText txt = (MText)tr.GetObject(objs.ObjectId, OpenMode.ForRead);
                    //            distance = Math.Sqrt((txt.Location.X - pt.Position.X) * (txt.Location.X - pt.Position.X) + (txt.Location.Y - pt.Position.Y) * (txt.Location.Y - pt.Position.Y));
                    //            if (minDistance > distance)
                    //            {
                    //                try
                    //                {
                    //                    txtValue = Convert.ToDouble(txt.Contents);
                    //                    Console.WriteLine("sadasdasd '{0}' to {1}.", txtValue, distance);
                    //                    minDistance = distance;
                    //                    Console.WriteLine("Converted '{0}' to {1}.", txt.Contents, txtValue);
                    //                }
                    //                catch (FormatException)
                    //                {
                    //                    Console.WriteLine("Unable to convert '{0}' to a Double.", txt.Contents);
                    //                }
                    //                catch (OverflowException)
                    //                {
                    //                    Console.WriteLine("'{0}' is outside the range of a Double.", txt.Contents);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    pt.Position = new Point3d(pt.Position.X, pt.Position.Y, txtValue);
                    //}
                    //// If it is Ellipse, change the Center Z.
                    //if (obj.GetType().Name == "Ellipse")
                    //{
                    //    Ellipse el = (Ellipse)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                    //    double minDistance = 999999999999999;
                    //    double distance;
                    //    double txtValue = 0;
                    //    // Test MText elements
                    //    foreach (ObjectId ids in btr)
                    //    {
                    //        Autodesk.AutoCAD.DatabaseServices.Entity objs = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(ids, OpenMode.ForRead);
                    //        if (objs.GetType().Name == "MText")
                    //        {
                    //            MText txt = (MText)tr.GetObject(objs.ObjectId, OpenMode.ForRead);
                    //            distance = Math.Sqrt((txt.Location.X - el.Center.X) * (txt.Location.X - el.Center.X) + (txt.Location.Y - el.Center.Y) * (txt.Location.Y - el.Center.Y));
                    //            if (minDistance > distance)
                    //            {
                    //                try
                    //                {
                    //                    txtValue = Convert.ToDouble(txt.Contents);
                    //                    Console.WriteLine("sadasdasd '{0}' to {1}.", txtValue, distance);
                    //                    minDistance = distance;
                    //                    Console.WriteLine("Converted '{0}' to {1}.", txt.Contents, txtValue);
                    //                }
                    //                catch (FormatException)
                    //                {
                    //                    Console.WriteLine("Unable to convert '{0}' to a Double.", txt.Contents);
                    //                }
                    //                catch (OverflowException)
                    //                {
                    //                    Console.WriteLine("'{0}' is outside the range of a Double.", txt.Contents);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    el.Center = new Point3d(el.Center.X, el.Center.Y, txtValue);
                    //}
                    //// If it is Arc, change the Center Z.
                    if (obj.GetType().Name == "Arc")
                    {
                        Arc el = (Arc)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        double minDistance = 999999999999999;
                        double distance;
                        double txtValue = 0;
                        // Test MText elements
                        foreach (ObjectId ids in btr)
                        {
                            Autodesk.AutoCAD.DatabaseServices.Entity objs = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(ids, OpenMode.ForRead);
                            if (objs.GetType().Name == "DBText")
                            {
                                // MText .Location                                    
                                DBText txt = (DBText)tr.GetObject(objs.ObjectId, OpenMode.ForRead);
                                distance = Math.Sqrt((txt.Position.X - el.Center.X) * (txt.Position.X - el.Center.X) + (txt.Position.Y - el.Center.Y) * (txt.Position.Y - el.Center.Y));
                                if (minDistance > distance)
                                {
                                    try
                                    {
                                        txtValue = Convert.ToDouble(txt.TextString);
                                        minDistance = distance;
                                        ed.WriteMessage("Converted '{0}' to {1}.", txt.TextString, txtValue);
                                    }
                                    catch (FormatException)
                                    {
                                        ed.WriteMessage("Unable to convert '{0}' to a Double.", txt.TextString);
                                    }
                                    catch (OverflowException)
                                    {
                                        ed.WriteMessage("'{0}' is outside the range of a Double.", txt.TextString);
                                    }
                                }
                            }
                        }
                        el.Center = new Point3d(el.Center.X, el.Center.Y, txtValue);
                       // ed.WriteMessage("txtValue: {0}\n", txtValue);
                    }
                }
                tr.Commit();
            }
        }

        // Modal Command with pickfirst selection
        [CommandMethod("MyGroup", "MyPickFirst", "MyPickFirstLocal", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void MyPickFirst() // This method can have any name
        {
            PromptSelectionResult result = Application.DocumentManager.MdiActiveDocument.Editor.GetSelection();
            if (result.Status == PromptStatus.OK)
            {
                // There are selected entities
                // Put your command using pickfirst set code here
            }
            else
            {
                // There are no selected entities
                // Put your command code here
            }
        }

        // Application Session Command with localized name
        [CommandMethod("MyGroup", "MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        public void MySessionCmd() // This method can have any name
        {
            // Put your command code here
        }

        // LispFunction is similar to CommandMethod but it creates a lisp 
        // callable function. Many return types are supported not just string
        // or integer.
        [LispFunction("MyLispFunction", "MyLispFunctionLocal")]
        public int MyLispFunction(ResultBuffer args) // This method can have any name
        {
            // Put your command code here

            // Return a value to the AutoCAD Lisp Interpreter
            return 1;
        }

    }

}
