using SSWebDevTest.net.smdservers.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSWebDevTest.net.smdservers;
using System.Data;

namespace SSWebDevTest.Models
{
    public class UnitFunctions
    {
        public string sLocationCode = "L001";

        private static void organizeUnitPriceInfo(DataTable dt, DataTable od)
        {
            od.Columns.Add("Dimensions");
            od.Columns.Add("Standard Push Rate");
            od.Columns.Add("Climate Control");
            od.Columns.Add("Available");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                od.Rows.Add(i);
                od.Rows[i][0] = dt.Rows[i][14].ToString() + " x " + dt.Rows[i][15].ToString();
                od.Rows[i][1] = dt.Rows[i][17];
                od.Rows[i][2] = dt.Rows[i][12];
                od.Rows[i][3] = dt.Rows[i][22];

            }
            return;
        }

        public string FindUnitPushRate(string locationCode, string UnitSize, bool ClimateControlled)
        {
            CallCenterWs Call = new CallCenterWs();

            string sCorporateCode = "CAEV";

            string sUserName = "CallCenter:::SAFESTORAGE6J6F38DHN";
            string sPassword = "SAfeR0me45";

            DataSet UnitTypePriceList = Call.UnitTypePriceList(sCorporateCode, locationCode, sUserName, sPassword);
            DataTable UnitInformation = UnitTypePriceList.Tables["Table"];

            DataTable od = new DataTable();

            organizeUnitPriceInfo(UnitInformation, od);

            string pushRate = "";

            for (int i = 0; i < od.Rows.Count; i++)
            {
                if (od.Rows[i][0].ToString() == UnitSize && od.Rows[i][2].ToString() == ClimateControlled.ToString())
                {
                    double h = Convert.ToDouble(od.Rows[i][1]);
                    pushRate = string.Format("{0:0.00}", Math.Truncate(h * 10) / 10);
                    i = od.Rows.Count - 1;
                }
                else
                {
                    pushRate = "N/A";
                }                
            }

            return pushRate;
        }

        public string FindAvailableUnits(string locationCode, string UnitSize, bool ClimateControlled)
        {
            CallCenterWs Call = new CallCenterWs();

            string sCorporateCode = "CAEV";
            string sUserName = "CallCenter:::SAFESTORAGE6J6F38DHN";
            string sPassword = "SAfeR0me45";

            DataSet UnitTypePriceList = Call.UnitTypePriceList(sCorporateCode, locationCode, sUserName, sPassword);
            DataTable UnitPrices = UnitTypePriceList.Tables["Table"];
            DataTable od = new DataTable();

            organizeUnitPriceInfo(UnitPrices, od);

            string availibility = "";
            string g;
            int h = 0;
            for (int i = 0; i < od.Rows.Count; i++)
            {
                if (od.Rows[i][0].ToString() == UnitSize && od.Rows[i][2].ToString() == ClimateControlled.ToString())
                {
                    g = od.Rows[i][3].ToString();
                    h = Int32.Parse(g);
                    i = od.Rows.Count - 1;
                }
            }
            if (h == 0)
            {
                availibility = "Waitlist";
            }
            else if (h > 3)
            {
                availibility = "Reserve";
            }
            else if (h < 4 && h > 0)
            {
                availibility = "Rent Today";
            }


            return availibility;
        }

        private static void organizeUnitInfo(DataTable dt, DataTable od)
        {
            od.Columns.Add("Dimensions");
            od.Columns.Add("Unit ID");
            od.Columns.Add("Climate Control");
            od.Columns.Add("Rented");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                od.Rows.Add(i);
                od.Rows[i][0] = dt.Rows[i][6].ToString() + " x " + dt.Rows[i][7].ToString();
                od.Rows[i][1] = dt.Rows[i][4];
                od.Rows[i][2] = dt.Rows[i][8];
                od.Rows[i][3] = dt.Rows[i][10];

            }
            return;
        }

        public string FindUnitID(string locationCode, string UnitSize, bool ClimateControlled)
        {
            CallCenterWs Call = new CallCenterWs();

            string sCorporateCode = "CAEV";
            string sUserName = "CallCenter:::SAFESTORAGE6J6F38DHN";
            string sPassword = "SAfeR0me45";

            DataSet UnitInformationSet = Call.UnitsInformation(sCorporateCode, locationCode, sUserName, sPassword);
            DataTable UnitInformation = UnitInformationSet.Tables["Table"];

            DataTable od = new DataTable();
            DataTable oa = new DataTable();
            DataTable oi = new DataTable();

            organizeUnitInfo(UnitInformation, od);

            string unitID = "";
            string sAllRented = "";
            oa.Columns.Add("Unit ID");
            oa.Columns.Add("Rented");
            oi.Columns.Add("Unit ID");
            int j = 0;
            int k = 0;

            for (int i = 0; i < od.Rows.Count; i++)
            {
                if (od.Rows[i][0].ToString() == UnitSize && od.Rows[i][2].ToString() == ClimateControlled.ToString())
                {
                    oa.Rows.Add(j);
                    oa.Rows[j][0] = od.Rows[i][1];
                    oa.Rows[j][1] = od.Rows[i][3];
                    j++;
                }
            }
            for (int i = 0; i < oa.Rows.Count; i++)
            {
                if (oa.Rows[i][1].ToString() == "False")
                {
                    oi.Rows.Add(k);
                    oi.Rows[k][0] = oa.Rows[i][0];
                    k++;
                }
            }
            for (int i = 0; i < oa.Rows.Count; i++)
            {
                if (oa.Rows[i][1].ToString() == "True")
                {
                    sAllRented = "True";
                }
                else if (oa.Rows[i][1].ToString() == "False")
                {
                    sAllRented = "False";
                    i = oa.Rows.Count - 1;
                }
            }
            if (sAllRented == "False")
            {
                unitID = oi.Rows[0][0].ToString();
            }
            else if (sAllRented == "True")
            {
                unitID = oa.Rows[0][0].ToString();
            }

            return unitID;

        }

        public string Login(string sEmail, string sOnlinePassword)
        {
            CallCenterWs Call = new CallCenterWs();

            string sCorporateCode = "CAEV";
            string sUserName = "CallCenter:::SAFESTORAGE6J6F38DHN";
            string sPassword = "SAfeR0me45";

            string sTenantID = Call.TenantLogin(sCorporateCode, sLocationCode, sUserName, sPassword, sEmail, sOnlinePassword).Tables["RT"].Rows[0][0].ToString();

            return sTenantID;
        }

        public void Reservation(string sTenantID, string sLocationCode, string sUnitID, string sUnitID2, string sUnitID3, DateTime dtReservationDate, string sReservationcomment)
        {
            CallCenterWs Call = new CallCenterWs();

            string sCorporateCode = "CAEV";
            string sUserName = "CallCenter:::SAFESTORAGE6J6F38DHN";
            string sPassword = "SAfeR0me45";

            Call.ReservationNew(sCorporateCode, sLocationCode, sUserName, sPassword, sTenantID, sUnitID, sUnitID2, sUnitID3, dtReservationDate, sReservationcomment);
        }



    }
}