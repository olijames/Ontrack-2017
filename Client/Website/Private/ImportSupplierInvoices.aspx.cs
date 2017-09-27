using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO;
//using System.Diagnostics;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Web;
using System.Web;
using System.IO;
using Electracraft.Framework.Utility;
//using System.Collections;
//using System.Data.SqlClient;

namespace Electracraft.Client.Website
{
    public partial class ImportSupplierInvoices : PageBase
    {
        protected int PreviousChar=0;
        protected bool myContinue=true;
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.URL_ContactHome);
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {

            Response.Redirect(Constants.URL_ImportSupplierInvoice);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected decimal CalculateSellPrice(decimal costprice)
        {
            decimal sellprice;
            sellprice = costprice * 110 / 100;
            if (costprice < 50) sellprice = costprice * 130 / 100;
            if (costprice < 20) sellprice = costprice * 150 / 100;
            if (costprice < 10) sellprice = costprice * 170 / 100;
            if (costprice < 5) sellprice = costprice * 200 / 100;


            return sellprice;

        }
        //---------------------------------------------------------------------------------------------
        //
        //                                   BELOW IS ADDING TO DATABASE FROM CSV FILES
        //
        //---------------------------------------------------------------------------------------------
        protected void CorysDownload(DOMaterialFilePass myFile, string[,] Materials)
        {
            //-------------------
            //CORYS SPECIFIC fill array              Jared to make sure no overload of array
            //-------------------


            //Get each char
            myFile.intChar = (myFile.F.ReadByte());
            myFile.c = (char)myFile.intChar;
            // System.Diagnostics.Debug.Write(c);

            if (myFile.CommaCount > 10)
            {   //line feed(last char of record)
                if (myFile.intChar == 10)
                {
                    myFile.CommaCount = 0;
                    myFile.ColCount = 0;
                    //new row
                    myFile.RowCount++;
                }
            }
           // System.Diagnostics.Debug.Write(DOM.c);

            //if char = comma character then... i.e. a new column
            if (myFile.intChar == 44 && PreviousChar==34)
            {

                myFile.ColCount++;
                myFile.CommaCount++;
                myContinue = true;

                //last column of the row...

            }
            else //if char != comma
            {
                if (myFile.intChar != 13)//CR
                {
                    if (myFile.intChar != 10)//LF
                    {
                        if (myFile.intChar != 34)//"
                        {
                            Materials[myFile.RowCount, myFile.ColCount] = Materials[myFile.RowCount, myFile.ColCount] + myFile.c;
                            
                            //
                            // 
                            //

                           // System.Diagnostics.Debug.Write(myFile.c + ", " + myFile.intChar + ", " +myFile.RowCount );
                        }
                    }
                }
            }


            PreviousChar = myFile.intChar;
            return;
        }


        protected void UploadButton_Click(object sender, EventArgs e)
        {
            bool booContinue=false;
            bool MaterialExistsInMaterialTable = false;
            //WORK WITH FILE
            if (FileUploadControl.HasFile)
            {

                HttpPostedFile G;
                HttpFileCollection uploadedFiles = Request.Files;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    G = uploadedFiles[i];
                    long Length = G.ContentLength;
                    string Extension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
                    booContinue = false;
                    switch (Extension)
                    {
                        case ".csv":
                            booContinue = true;
                            break;


                    }



                }
                if (booContinue)//file is uploaded
                {

                    G = uploadedFiles[0];

                    System.IO.Stream F = G.InputStream;
                    try
                    {


                        // if (FileUploadControl.PostedFile.ContentType == "image/jpeg/doc")
                        //{


                        //FileUploadControl.SaveAs(Server.MapPath("~/") + fileName);

                        StatusLabel.Text = "Upload status: File uploaded!";
                        //long Length = F.Length;
                        long Length = F.Length;
                        if (Length < 32000)
                        {
                            string[,] MaterialRecord = new string[9999, 12];
                            DOMaterialFilePass myFile = new DOMaterialFilePass();
                            myFile.F = F;
                            myFile.intChar = 0;
                            myFile.ColCount = 0;
                            myFile.CommaCount = 1;
                            myFile.RowCount = 0;
                            System.Diagnostics.Debug.WriteLine("---------------------------------------------------Adding file to array using corys format...");
                            for (int iCount = 1; iCount <= Length; iCount++)
                            {
                                CorysDownload(myFile, MaterialRecord);
                                if (myFile.ColCount == 3 && myContinue == true)
                                {
                                    //System.Diagnostics.Debug.WriteLine(MaterialRecord[myFile.RowCount, 2]);
                                    myContinue = false;
                                }

                            }
                            System.Diagnostics.Debug.WriteLine("");
                            System.Diagnostics.Debug.WriteLine("-------------------------------------------------File to array Complete!");
                            List<DOBase> AllContactMaterials = CurrentBRJob.SelectMaterialsbyContactID(CurrentSessionContext.CurrentContact.ContactID);
                            List<DOBase> AllSupplierInvoices = CurrentBRJob.SelectSupplierInvoicesInfoBySupplierID(Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"), CurrentSessionContext.CurrentContact.ContactID);
                            //System.Diagnostics.Debug.WriteLine(CurrentSessionContext.CurrentContact.ContactID.ToString());
                            bool ContinueFlag = false;
                            bool AddNewMaterialFlag = false;
                            string strExistingMaterial = "";
                            //int jCount = 0; 7/4
                            string StrMaterialID = "";
                            string StrSupplierInvoice = "";
                            // string RecentSupplierID = "";
                            string strMaterialRecord = "";
                      
                            // LOOP THROUGH ALL THE ROWS OF ARRAY:
                            for (int iCount = 0; iCount <= myFile.RowCount; iCount++)
                            {
                                if (MaterialRecord[iCount, 1] == null) //just incase there is an crlf on the last line causing a new record to be empty
                                {
                                    break;
                                }
                                AddNewMaterialFlag = true;
                                if (MaterialRecord[iCount, 0] == "HDR")   //CHECK IF THE LINE IS A HEADER, SO WE CAN ESTABLISH INFO ABOUT THE SUPPLIERINVOICE
                                {
                                    System.Diagnostics.Debug.WriteLine("----------------------");
                                   
                                    ContinueFlag = true;

                                    //SEE IF THIS SUPPLIERINVOICE ALREADY EXISTS
                                    foreach (DOSupplierInvoice SI in AllSupplierInvoices)
                                    {
                                        strMaterialRecord = MaterialRecord[iCount, 3];
                                        if (SI.SupplierReference == strMaterialRecord)
                                        {
                                            ContinueFlag = false;
                                            System.Diagnostics.Debug.WriteLine("Record already exists: " + SI.SupplierReference);
                                            break; //CANCEL THIS FOREACH LOOP

                                        }
                                    }
                                }
                                
                                if (ContinueFlag) //SUPPLIERINVOICE DOES NOT ALREADY EXIST
                                {
                                    if (MaterialRecord[iCount, 0] == "HDR")
                                    System.Diagnostics.Debug.WriteLine("Row: " + iCount + ", " + MaterialRecord[iCount, 3]);
                                   // System.Diagnostics.Debug.WriteLine("Adding: " + );

                                    if (MaterialRecord[iCount, 0] == "HDR")   //POPULATE SUPPLIERINVOICE TABLE
                                    {
                                                                        // System.Diagnostics.Debug.WriteLine("");
                                                                        //System.Diagnostics.Debug.WriteLine("--------------------------------------------------------------");
                                                                        //  System.Diagnostics.Debug.WriteLine("                       Adding record: " + MaterialRecord[iCount, 7] + "    " + MaterialRecord[iCount, 8]);
                                                                        //System.Diagnostics.Debug.WriteLine("--------------------------------------------------------------");
                                                                        // System.Diagnostics.Debug.WriteLine("");

                                        //good here. includes 0000

                                        DOSupplierInvoice SupplierInvoice = new DOSupplierInvoice();
                                        SupplierInvoice.SupplierInvoiceID = Guid.NewGuid();
                                        StrSupplierInvoice = SupplierInvoice.SupplierInvoiceID.ToString();
                                        SupplierInvoice.SupplierID = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB");// MaterialRecord[iCount, jCount]; //Corys supplierID
                                        SupplierInvoice.InvoiceDate = DateTime.Parse(MaterialRecord[iCount, 4]);
                                        SupplierInvoice.ContractorReference = MaterialRecord[iCount, 5];
                                        SupplierInvoice.TotalExGst = decimal.Parse(MaterialRecord[iCount, 8]);
                                        SupplierInvoice.SupplierReference = MaterialRecord[iCount, 3]; //changed from 7
                                        SupplierInvoice.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                                        //System.Diagnostics.Debug.WriteLine(SupplierInvoice.SupplierID);
                                        //System.Diagnostics.Debug.WriteLine(SupplierInvoice.SupplierName);
                                        SupplierInvoice.CreatedBy = CurrentSessionContext.Owner.ContactID;
                                        SupplierInvoice.CreatedDate = DateTime.Now;

                                        CurrentBRJob.SaveSupplierInvoice(SupplierInvoice);

                                        //for (int jCount = 0; jCount < 12; jCount++) //loop through columns
                                        //{
                                    }

                                    //ADDING MaterialRecord, SEE IF THEY EXIST FIRST

                                    if (MaterialRecord[iCount, 0] != "HDR") //IF IT IS A LINE ITEM
                                    {
                                        
                                           
                                                                //jCount = 0; 7/4
                                        foreach (DOMaterial mat in AllContactMaterials) //go thru all my MaterialRecord
                                        {
                                            //jCount++; 7/4
                                            //  System.Diagnostics.Debug.WriteLine("My MaterialRecord: " + mat.MaterialName);


                                            strExistingMaterial = mat.SupplierProductCode;
                                            string strNewMaterial = MaterialRecord[iCount, 1];
                                            //System.Diagnostics.Debug.WriteLine("");
                                            //System.Diagnostics.Debug.WriteLine("                     attempting to add...          ");
                                            
                                            if (strExistingMaterial == strNewMaterial)//new and old have the same supplierproductcode
                                            {
                                                //System.Diagnostics.Debug.WriteLine("");
                                                //System.Diagnostics.Debug.WriteLine("--------");

                                                //System.Diagnostics.Debug.WriteLine("--------");
                                                //System.Diagnostics.Debug.WriteLine("");

                                                decimal newCostPrice = decimal.Parse(MaterialRecord[iCount, 8] + '0');
                                                string newName = mat.MaterialName;
                                               
                                                if (mat.CostPrice == newCostPrice)//IF SUPPLIERPRODUCTCODE IS SAME AND PRICE IS THE SAME THEN DONT ADD IT
                                                {
                                                    if (newName == MaterialRecord[iCount, 2])
                                                    {
                                                        StrMaterialID = mat.MaterialID.ToString();
                                                        //MaterialExistsInMaterialTable = true;
                                                        AddNewMaterialFlag = false; //removed 5/4/16 probably wrong to be here causing only one material per supplierinvoice to be added
                                                                              //System.Diagnostics.Debug.WriteLine("");
                                                                              // System.Diagnostics.Debug.WriteLine("");
                                                                              //System.Diagnostics.Debug.WriteLine(mat.SupplierProductCode + "   This material matches existing item: " + MaterialRecord[iCount, 1] + ",  So has NOT been added to material table " + MaterialRecord[iCount, 2]);
                                                        break;
                                                    }
                                                    
                                                }
                                            }
                                        }



                                        if (AddNewMaterialFlag)//add supplier invoice material to material table if not already there
                                        {
                                            System.Diagnostics.Debug.Write("Row: " + iCount + ", ");
                                            System.Diagnostics.Debug.WriteLine("New Material added to material table:    " + MaterialRecord[iCount, 2] + ",    " + MaterialRecord[iCount, 1]);
                                            //add supplier invoice material to material table if not already there
                                            if (MaterialRecord[iCount, 1] != "")
                                            {
                                                DOMaterial MaterialItem = new DOMaterial();
                                                MaterialItem.SupplierID = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB");
                                                MaterialItem.MaterialID = Guid.NewGuid();
                                                StrMaterialID = MaterialItem.MaterialID.ToString();
                                                MaterialItem.MaterialName = MaterialRecord[iCount, 2];
                                                MaterialItem.SupplierProductCode = MaterialRecord[iCount, 1];
                                                MaterialItem.UOM = MaterialRecord[iCount, 5];
                                                MaterialItem.CostPrice = decimal.Parse(MaterialRecord[iCount, 8]);
                                                MaterialItem.RRP = decimal.Parse(MaterialRecord[iCount, 7]);
                                                MaterialItem.CreatedBy = CurrentSessionContext.Owner.ContactID;
                                                MaterialItem.CreatedDate = DateTime.Now;
                                                MaterialItem.SellPrice = CalculateSellPrice(MaterialItem.CostPrice);
                                                MaterialItem.MaterialCategoryID = Guid.Parse("ccccdddd-dddd-dddd-dddd-ddddccccdddd");
                                                //System.Diagnostics.Debug.WriteLine(MaterialItem.RRP);
                                                MaterialItem.ContactID = CurrentSessionContext.CurrentContact.ContactID;
                                                CurrentBRJob.SaveMaterial(MaterialItem);
                                                AllContactMaterials.Add(MaterialItem);
                                                // System.Diagnostics.Debug.WriteLine("                          __________________________________");
                                                //System.Diagnostics.Debug.WriteLine("                          Amount of AllcontactMaterialRecord: " + AllContactMaterialRecord.Count());
                                                //System.Diagnostics.Debug.WriteLine("                          __________________________________");
                                            }
                                            else
                                            {

                                            }
                                        }




                                        //add to table SupplierInvoiceMaterial  
                                        if (StrSupplierInvoice != "")
                                        {
                                            DOSupplierInvoiceMaterial SIMaterial = new DOSupplierInvoiceMaterial();
                                            SIMaterial.SupplierInvoiceMaterialID = Guid.NewGuid();
                                            SIMaterial.MaterialID = Guid.Parse(StrMaterialID);
                                            SIMaterial.SupplierInvoiceID = Guid.Parse(StrSupplierInvoice);
                                            SIMaterial.Qty = decimal.Parse(MaterialRecord[iCount, 6]);
                                            SIMaterial.QtyRemainingToAssign = decimal.Parse(MaterialRecord[iCount, 6]);
                                            SIMaterial.ContractorID = CurrentSessionContext.CurrentContact.ContactID;
                                            SIMaterial.Assigned = false;
                                            SIMaterial.CreatedBy = CurrentSessionContext.Owner.ContactID;
                                            SIMaterial.CreatedDate = DateTime.Now;
                                            CurrentBRJob.SaveSupplierInvoiceMaterial(SIMaterial);
                                            //increment supplierinvoice.status by 1
                                            //qty=0 means back order(from corys, maybe more)
                                            if (SIMaterial.Qty != 0) CurrentBRJob.UpdateSupplierInvoiceStatus(Guid.Parse(StrSupplierInvoice), "+");
                                            System.Diagnostics.Debug.WriteLine("Row: " + iCount + ", SIM added");


                                        }
                                        //1. NEED TO CHECK IF MATERIAL EXISTS IN MaterialRecord TABLE. IF NOT THEN ENTER NEW MATERIAL-----done


                                        //2. SELECT MATERIAL AND ADD NEW ENTRY INTO SUPPLIERINVOICEMATERIAL and CONTAINERINVOICE TABLES and qty. THIS HAPPENS
                                        //   REGARDLESS IF MATERIAL EXISTS, BUT NOT IF SUPPLIERINVOICE EXISTS




                                        // System.Diagnostics.Debug.WriteLine(AllMaterialRecord[0]);




                                    }






                                }


                            }
                            System.Diagnostics.Debug.WriteLine("Transaction complete!");

                            //F.close();
                        }

                        //Console.ReadKey();

                        //   else
                        // StatusLabel.Text = "Upload status: Only JPEG files are accepted!";

                        else
                        {
                            StatusLabel.Text = "File too large";
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message + ex.StackTrace.ToString();

                        //StatusLabel.Text=
                    }//
                    finally
                    {
                        // F.Close();
                    }

                }
            }
        }
    }
}
