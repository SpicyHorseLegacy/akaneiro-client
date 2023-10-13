using UnityEngine;
using System.Collections;
using System; 
//using System.Data; 
//using System.Data.Odbc;
/*
public class EXCELREADER : MonoBehaviour {
    
    public static DataTable readXLS( string filetoread, string sheetName )
    {
        // Must be saved as excel 2003 workbook, not 2007, mono issue really
        string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq="+filetoread+";";
        Debug.Log(con);
        string yourQuery = "SELECT * FROM ["+ sheetName + "$]"; 
        // our odbc connector 
        OdbcConnection oCon = new OdbcConnection(con); 
        // our command object 
        OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
        // table to hold the data 
        DataTable dtYourData = new DataTable("YourData"); 
        // open the connection 
        oCon.Open(); 
        // lets use a datareader to fill that table! 
        OdbcDataReader rData = oCmd.ExecuteReader(); 
        // now lets blast that into the table by sheer man power! 
        dtYourData.Load(rData); 
        // close that reader! 
        rData.Close(); 
        // close your connection to the spreadsheet! 
        oCon.Close(); 
        // wow look at us go now! we are on a roll!!!!! 
        // lets now see if our table has the spreadsheet data in it, shall we?

		return dtYourData;
    }
    
}
*/