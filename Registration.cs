using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;


namespace MarkAble2
{
    public class Registration
    {
        public enum EvaluationTypes
        {
            None,
            EvaluationPeriod,
            ClosingDate,
            ForceFail
        }


        //these should be all be overwritten by calling program to create unique reg code per application
        private static string myGUID = "{49CAE968-747F-460e-8581-5135D9AC7963}";
        private static string myKey = "49CAE968";


        private static int myEvaluationDays = 9999;
        private static string myAppName = "";
        private static DateTime myClosingDate = DateTime.Today.AddDays(myEvaluationDays);

        public static DateTime ClosingDate
        {
            get { return myClosingDate; }
            set { 
                myClosingDate = value; 
            }
        }

        public static EvaluationTypes EvaluationType = EvaluationTypes.EvaluationPeriod;

        public static StringBuilder ErrorLog = new StringBuilder();
        public static bool anyErrors = false;

        public static bool IsExhausted()
        {
            return false;
        }

        public static int DaysRemaining()
        {                
            return 999;
        }

        public static string HashIt(string regName, bool oldMethod)
        {
           reurn "XXX";
        }

        private static string HashString(string stringToHash)
        {
            return "XXX";
        }

        // Create an md5 sum string of this string
        private static string HashUnicodeString(string stringToHash)
        {
            return "XXX";
        }

        private static string HashItOldMethod(string regName)
        {
            return "XXX";
        }

        private static string HashItNewMethod(string regName)
        {
            return "XXX";
        }

        public static bool ConfirmReg(string regName, string regNum)
        {
            return true;
        }

        public static bool IsRegistered()
        {
            return true;
        }

        public static string RegisteredTo()
        {
            return "PUBLIC DOMAIN";
        }

        public static void Register(string regName)
        {
            Global.SaveSetting("RegName", regName);
            Global.SaveSetting("RegNum", HashItNewMethod(regName));
        }


        public static void SetStartDate(DateTime aDate)
        {
            string DD,MM,YY,SS;
            DD = (aDate.Day + 37).ToString();
            MM = (aDate.Month + 43).ToString();
            YY = (aDate.Year + 1398).ToString();
            DateTime tempDate = DateTime.Now;
            SS = tempDate.Second.ToString();
            SS = SS.PadRight(2,(char)55);
            Global.SaveSetting("HashValue", SS.Substring(0, 2) + DD + MM + YY);
            Global.WriteAppData(myGUID, myKey, SS.Substring(0, 2) + DD + MM + YY);
        }

        public static DateTime GetStartDate()
        {
            string tempStr1, tempStr2;
            DateTime aDate;
            tempStr1 = Global.GetSetting("HashValue");
            tempStr2 = Global.ReadAppData(myGUID, myKey);

            if (
                ((tempStr1 == "") && (tempStr2 != ""))
                ||
                ((tempStr2 == "") && (tempStr1 != ""))
                )
            {
                //user has deleted one or the other so set bad date flag
                aDate = new DateTime(1903,1,1); //1903 signifies bad date
                return aDate;
            }

            //if we get here, both entries exist, but...
            if (tempStr1 != tempStr2)
            {
                //entries don't match
                aDate = new DateTime(1903,1,1); //1903 signifies bad date
                return aDate;
            }

            if ((tempStr1 != "") && (tempStr1.Length >=8))
            {
                string DD,MM,YY;
                int dd,mm,yy;
                DD = tempStr1.Substring(2,2);
                MM =tempStr1.Substring(4,2);
                YY =tempStr1.Substring(6,4);
                try 
                {
                    dd = System.Convert.ToInt32(DD);
                    mm = System.Convert.ToInt32(MM);
                    yy = System.Convert.ToInt32(YY);
                    aDate = new DateTime(yy-1398,mm-43,dd-37);
                    return aDate;
                }
                catch
                {
                    aDate = new DateTime(1903,1,1); //1903 signifies bad date
                    return aDate;
                }
            }
            else
            {
                aDate = new DateTime(1900,1,1); //1900 signifies no date was found
                return aDate;
            }
        }

        public static string GUID
        {
            get
            {
                return myGUID;
            }
            set
            {
                myGUID = value;
            }
        }

        public static string Key
        {
            get
            {
                return myKey;
            }
            set
            {
                myKey = value;
            }
        }


        public static int EvaluationDays
        {
            get
            {
                return myEvaluationDays;
            }
            set
            {
                myEvaluationDays = value;
            }
        }

        public static string AppName
        {
            get
            {
                return myAppName;
            }
            set
            {
                myAppName = value;
            }
        }


    }
}