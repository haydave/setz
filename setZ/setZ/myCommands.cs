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
                    if (obj.GetType().Name == "Polyline")
                    {
                        Polyline pl = (Polyline)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        pl.Elevation = 12;
                        ed.WriteMessage("polyline: {0}\n", pl.Elevation);
                    }
                    // If it is Circle change the Center Z.
                    if (obj.GetType().Name == "Circle")
                    {
                        Circle circ = (Circle)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                        circ.Center = new Point3d(circ.Center.X, circ.Center.Y, 15);
                        ed.WriteMessage("circle: {0}\n", circ.Center);
                    }
                    // If it is Point change the Position Z.
                    //if (obj.GetType().Name == "DBPoint")
                    //{
                    //    DBPoint pt = (DBPoint)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);

                    //}
                    // If it is Arc change the Center Z.
                    //if (obj.GetType().Name == "Arc")
                    //{
                    //    Arc aaaac = (Arc)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                    //}
                    ed.WriteMessage("aa: {0}\n", obj.GetType().Name);
                   
                    // If it is Ellipse, change the Center Z.
                    //if (obj.GetType().Name == "Ellipse")
                    //{
                    //    Ellipse circ = (Ellipse)tr.GetObject(obj.ObjectId, OpenMode.ForWrite);
                    //}
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
