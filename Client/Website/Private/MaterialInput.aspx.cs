using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;

namespace Electracraft.Client.Website
{

    public partial class MaterialInput : PageBase
    {

        PageBase pb = new PageBase();
        static void UpdateTable()
        {
           
            try
            {



                //---------------------------------------------
                //REQUIRES CHECK - JARED
                //want to check if connection string should be taken from webconfig
                //-------------------------------------------------------

                ////////string connectionString =
                ////////   @"server=.\sqlexpress;database=ontrack3;user=sa;password=leteljain1;";
                ////////using (System.Data.SqlClient.SqlConnection conn =
                ////////    new System.Data.SqlClient.SqlConnection(connectionString))
                ////////{
                ////////    DOTask task = new DOTask();
                ////////    task.TaskID = Guid.Parse("ACEDEE2F-2C55-48A8-B103-000136883218");
                ////////    PageBase pb = new PageBase();
                ////////    pb.CurrentBRJob.SaveTask(task);

                //conn.Open();
                ////Page.ClientScript.RegisterStartupScript(Page.GetType(), "MessageBox", "<script language='javascript'>alert('" + Message + "');</script>");
                //using (System.Data.SqlClient.SqlCommand cmd =
                //    new System.Data.SqlClient.SqlCommand("UPDATE task SET tasktype=1 WHERE TaskId='ACEDEE2F-2C55-48A8-B103-000136883218'", conn))
                //{
                //  //  cmd.Parameters.AddWithValue("@Id", "ACEDEE2F-2C55-48A8-B103-000136883218");
                //    //cmd.Parameters.AddWithValue("@TaskType", 0);

                //    int rows = cmd.ExecuteNonQuery();

                //    //rows number of record got updated


            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                //Log exception
                //Display Error message
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write("Hello");
            //   MaterialInput.UpdateTable();

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {

            if (FileUploadControl.HasFile)
            {
                try
                {
                    //MaterialInput.UpdateTable();
                    int intChar;
                    // if (FileUploadControl.PostedFile.ContentType == "image/jpeg/doc")
                    //{

                    string filename = System.IO.Path.GetFileName(FileUploadControl.FileName);
                    FileUploadControl.SaveAs(Server.MapPath("~/") + filename);
                    StatusLabel.Text = "Upload status: File uploaded!";
                    //filename = "D:\Projects\Electracraft App\trunk\test.txt";
                    System.IO.FileStream F = new System.IO.FileStream(@"D:\Projects\Electracraft App\trunk\test.csv", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                    //for (int iCount = 1; iCount <= 20; iCount++)
                    //{
                    //    F.WriteByte((byte)iCount);
                    //    F.Position = 0;
                    //}
                    long Length = F.Length;
                    char c;
                    string[,] Materials = new string[999, 12];
                    int ColCount = 0;
                    int RowCount = 0;
                    int CommaCount = 1;
                    for (int iCount = 1; iCount <= Length; iCount++)
                    //-------------------
                    //CORYS SPECIFIC
                    //-------------------
                    {

                        //Get each char
                        intChar = (F.ReadByte());
                        c = (char)intChar;
                        System.Diagnostics.Debug.Write(c);

                        if (CommaCount > 10)
                        {   //line feed(last char of record)
                            if (intChar == 10)
                            {
                                CommaCount = 0;
                                ColCount = 0;
                                //new row
                                RowCount++;
                            }
                        }

                        //if char = comma character then... i.e. a new column
                        if (intChar == 44)
                        {

                            ColCount++;
                            CommaCount++;

                            //last column of the row...

                        }
                        else //if char != comma
                        {
                            if (intChar != 13)//CR
                            {
                                if (intChar != 10)//LF
                                {
                                    if (iCount != 34)//"
                                    {
                                        Materials[RowCount, ColCount] = Materials[RowCount, ColCount] + c;
                                        //
                                        // call method and where its from
                                        //


                                    }
                                }
                            }
                        }




                    }
                    //Loop through all cols and rows...
                    for (int iCount = 0; iCount < RowCount; iCount++)
                    {
                        for (int jCount = 0; jCount < 12; jCount++)
                        {
                            // System.Diagnostics.Debug.Write( Materials[iCount, jCount]);

                            /** Mandeep's code       
                           **/
                            DOSupplier supplier = new DOSupplier();
                            supplier.SupplierID = Guid.NewGuid();
                            supplier.Code = Materials[iCount, jCount];
                            CurrentBRJob.SaveSupplier(supplier);
                            
                        }

                    }

                    F.Close();
                }

                //Console.ReadKey();

                //   else
                // StatusLabel.Text = "Upload status: Only JPEG files are accepted!";


                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }//
            }
        }//
    }
}//