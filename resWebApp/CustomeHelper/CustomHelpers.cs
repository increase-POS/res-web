using resWebApp.Models;
using resWebApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace resWebApp.CustomeHelper
{
    public static class CustomHelpers
    {
        public static string accuracyFormat(this HtmlHelper helper, string value)
        {
            string sdc = "0";
            if (value == null)
            {

            }
            else
            {
                decimal dc = decimal.Parse(value.ToString());

                switch (Global.basicSettings.accuracy)
                {
                    case "0":
                        sdc = string.Format("{0:F0}", dc);
                        break;
                    case "1":
                        sdc = string.Format("{0:F1}", dc);
                        break;
                    case "2":
                        sdc = string.Format("{0:F2}", dc);

                        break;
                    case "3":
                        sdc = string.Format("{0:F3}", dc);
                        break;
                    default:
                        sdc = string.Format("{0:F1}", dc);
                        break;
                }
            }
            return sdc;
        }

        public static string accuracyFormatWithCurrency(this HtmlHelper helper, string value)
        {
            string sdc = "0";
            if (value == null)
            {

            }
            else
            {
                decimal dc = decimal.Parse(value.ToString());

                switch (Global.basicSettings.accuracy)
                {
                    case "0":
                        sdc = string.Format("{0:F0}", dc);
                        break;
                    case "1":
                        sdc = string.Format("{0:F1}", dc);
                        break;
                    case "2":
                        sdc = string.Format("{0:F2}", dc);

                        break;
                    case "3":
                        sdc = string.Format("{0:F3}", dc);
                        break;
                    default:
                        sdc = string.Format("{0:F1}", dc);
                        break;
                }
            }
            return sdc+" " +Global.basicSettings.currency;
        }
        public static string processTypeConverter(this HtmlHelper helper, string processType,string cardName)
        {

            switch (processType)
            {
                case "cash": return AppResource.Cash;
                //break;
                case "doc": return AppResource.Document;
                //break;
                case "cheque": return AppResource.Cheque;
                //break;
                case "balance": return AppResource.Credit;
                //break;
                case "card": return cardName;
                //break;
                case "inv": return "-";
                case "multiple": return AppResource.MultiplePayment;

                //break;
                default: return processType;
                    //break;
            }
        }

        public static string balanceTypeConverter(this HtmlHelper helper, string balanceType,float balance)
        {
            if (balance == 0)
                return "";
            switch (balanceType)
            {
                case "0": return AppResource.Worthy;
                //break;
                case "1": return AppResource.Demands;
            }
            return "";
        }

        public static string invoiceTypeConverter(this HtmlHelper helper, string invType)
        {
            switch (invType)
            {
                case "s": return AppResource.DiningHall;
                //break;
                case "ts": return AppResource.Takeaway;
                case "ss": return AppResource.SelfService;
            }
            return "";
        }
        public static string invoiceNextStatusConverter(this HtmlHelper helper, string status)
        {
            switch (status)
            {
                case "Listed": return AppResource.Preparing;
                case "Preparing": return AppResource.Ready;
                case "Ready": return AppResource.Collected;
                case "Collected": return AppResource.InTheWay;
                case "InTheWay": return AppResource.Done;
            }
            return "";
        }
        public static string reservationStatusConverter(this HtmlHelper helper, string status)
        {
            switch (status)
            {
                case "confirm": return AppResource.Confirmed;
                 
                default: return AppResource.Waiting;
            }
     
        }
        public static string tablesNameConverter(this HtmlHelper helper, List<TableModel> tables)
        {
            string tablesName = "";
            foreach(var t in tables)
            {
                if (!tablesName.Equals(""))
                    tablesName += ", " + t.name;
                else
                    tablesName += t.name;
            }

            return tablesName;
     
        }

        public static string customerNameConverter(this HtmlHelper helper, string name)
        {
            if (name == null)
                return " - ";
            else return name;
        }
        public static string deliveryNameConverter(this HtmlHelper helper, int? shipUserId, string companyName, string shipUserName)
        {
            switch (shipUserId)
            {
                case null: return companyName;
                default: return shipUserName;
            }
        }

        public static string concatStrings(this HtmlHelper helper,  string str1, string str2)
        {
            return str1 + " - " + str2;
        }
        public static string concatTimeWithPM_AM(this HtmlHelper helper,  string str1, string str2)
        {
            var strArr = str1.Split(':');
            return strArr[0]+":"+strArr[1] + " " + str2.ToLower();
        }
        
    }
}