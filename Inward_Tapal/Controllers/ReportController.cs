using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Inward_Tapal.Models;

namespace Inward_Tapal.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, string searchBy, string search, string searching, string searching1)
        {
            DataTable dtData = new DataTable();
            string fileName = Guid.NewGuid().ToString();
           String reportHeader = "This is a header.";
            try
            {
                if (searchBy == "Date")
                {
                    DateTime searchfrom = Convert.ToDateTime(searching);
                    DateTime searchto = Convert.ToDateTime(searching1);


                    {

                        string conString = ConfigurationManager.ConnectionStrings["TapalsEntitiesContect"].ConnectionString;
                        //string sql = "select * from Inward where " + searchBy + "='" + search.ToString() + "'";
                        string sql = "select * from Inward where Date_in between'" + searchfrom + "'and '" + searchto + "'";
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter(command))
                                {
                                    connection.Open();
                                    da.Fill(dtData); connection.Close();
                                }
                            }
                        }

                    }
                }
                else {
                    string conString = ConfigurationManager.ConnectionStrings["TapalsEntitiesContect"].ConnectionString;
                    string sql = "select * from Inward where " + searchBy + "='" + search.ToString() + "'";
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(command))
                            {
                                connection.Open();
                                da.Fill(dtData); connection.Close();
                            }
                        }
                    }
                }
        
                var MyWorkBook = new XLWorkbook();
                    var MyWorkSheet = MyWorkBook.Worksheets.Add("Sheet 1");
                    int TotalColumns = dtData.Columns.Count;

                    //-->headline
                    //first row is intentionaly left blank.
                    var headLine = MyWorkSheet.Range(MyWorkSheet.Cell(2, 1).Address, MyWorkSheet.Cell(2, TotalColumns).Address);
                    headLine.Style.Font.Bold = true;
                    headLine.Style.Font.FontSize = 15;
                    headLine.Style.Font.FontColor = XLColor.White;
                    headLine.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headLine.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    headLine.Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.25);
                    headLine.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    headLine.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    headLine.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    headLine.Style.Border.RightBorder = XLBorderStyleValues.Medium;

                    headLine.Merge();
                  //  headLine.Value = reportHeader;
                    //<-- headline

                    //--> column settings
                    for (int i = 1; i < dtData.Columns.Count + 1; i++)
                    {
                        String combinedHeaderText = dtData.Columns[i - 1].ColumnName.ToString();
                        string separatedColumnHeader = "";
                        foreach (char letter in combinedHeaderText)
                        {
                            if (Char.IsUpper(letter) && separatedColumnHeader.Length > 0)
                                separatedColumnHeader += " " + letter;
                            else
                                separatedColumnHeader += letter;
                        }
                        MyWorkSheet.Cell(4, i).Value = separatedColumnHeader;
                        MyWorkSheet.Cell(4, i).Style.Alignment.WrapText = true;
                    }

                    var columnRange = MyWorkSheet.Range(MyWorkSheet.Cell(4, 1).Address, MyWorkSheet.Cell(4, TotalColumns).Address);
                    columnRange.Style.Font.Bold = true;
                    columnRange.Style.Font.FontSize = 10;
                    columnRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columnRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    columnRange.Style.Fill.BackgroundColor = XLColor.FromArgb(171, 195, 223);
                    columnRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    columnRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    columnRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    columnRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //<-- column settings

                    //--> row data & settings
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        DataRow row = dtData.Rows[i];
                        for (int j = 0; j < dtData.Columns.Count; j++)
                        {
                            MyWorkSheet.Cell(i + 5, j + 1).Value = row[j].ToString();
                        }
                    }

                    var dataRowRange = MyWorkSheet.Range(MyWorkSheet.Cell(5, 1).Address, MyWorkSheet.Cell(dtData.Rows.Count + 4, TotalColumns).Address);
                    dataRowRange.Style.Font.Bold = false;
                    dataRowRange.Style.Font.FontSize = 10;
                    dataRowRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    dataRowRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    dataRowRange.Style.Fill.BackgroundColor = XLColor.FromArgb(219, 229, 241);
                    dataRowRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    dataRowRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    dataRowRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    dataRowRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //<-- row data & settings

                    // Prepare the response
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=\"" + reportHeader + ".xlsx\"");

                    // Flush the workbook to the Response.OutputStream
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        MyWorkBook.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        memoryStream.Close();
                    }

                    Response.End();
                    return View();
                }
            
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}