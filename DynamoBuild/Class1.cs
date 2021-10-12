using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Revit.GeometryConversion;
using DG = Autodesk.DesignScript.Geometry;


namespace DynamoTest
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            try
            {
                DG.Point startPoint = DG.Point.ByCoordinates(0, 0, 0);
                DG.Point endPoint = DG.Point.ByCoordinates(1000, 0, 0);
                DG.Line line = DG.Line.ByStartPointEndPoint(startPoint, endPoint);
                Line revitLine = line.ToRevitType() as Line; //将Dynamo的Geometry转化为Revit的Geometry
                using (Transaction transaction = new Transaction(document, "Create Line"))
                {
                    transaction.Start();
                    ModelCurve modelCurve = document.Create.NewModelCurve(revitLine,
                        SketchPlane.Create(document, Plane.CreateByNormalAndOrigin(XYZ.BasisZ, XYZ.Zero)));
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Result.Succeeded;


        }
    }
}

