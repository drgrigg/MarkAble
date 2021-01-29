using System;
using Microsoft.Win32;


namespace MarkAble2
{

	public class Shareware
	{
		public Shareware()
		{
		}

		private const string myGUID = "{2A579B0B-2A01-4668-9BF4-84CCE282FC85}";
		private const string myKey = "A2A579B0";

		public static bool IsExhausted;

		public static string HashIt(string regName)
		{
			regName = regName.ToUpper();
			char[] cArray = regName.ToCharArray();
			char[] GUIDArray = myGUID.ToCharArray();

			if (regName.Length < 5) 
				regName = regName + "Rumplestiltskin";

			long myTotal = 0;
			for (int n=0; n<cArray.Length; n++)
			{
				myTotal = myTotal + (long)cArray[n] * (n+1);
			}
			
			myTotal = myTotal * 997;

			for (int n=0; (n < GUIDArray.Length) && (n < cArray.Length); n++)
			{
				myTotal = myTotal + (long)((cArray[n] % 3)* GUIDArray[n]);
			}
			string part1, part2, part3, part4;
			part1 = (myTotal % 9973).ToString();
			part2 = (myTotal % 9967).ToString();
			part3 = (myTotal % 9949).ToString();
			part4 = (myTotal % 1949).ToString();
			part1 = part1.PadLeft(4,(char)48);
			part2 = part2.PadRight(4,(char)49);
			part3 = part3.PadLeft(4,(char)50);
			part4 = part4.PadRight(4,(char)51);
			return part1 + "-" + part2 + "-" + part3 + "-" + part4;
		}

		public static bool ConfirmReg(string regName, string regNum)
		{
		return (regNum == HashIt(regName));	
		}



		public static bool IsRegistered()
		{
			string regName, regNum;
			bool isReg = false;
			regName = Global.GetSetting("RegName");
			regNum = Global.GetSetting("RegNum");
			if ((regName == "") || (regNum == "")) return false;
			isReg = ConfirmReg(regName,regNum);
//			if (! isReg)
//				isReg = ConfirmESellerateReg(regName, regNum);
			return isReg;
		}

		public static string RegisteredTo()
		{
			string regName, regNum;
			regName = Global.GetSetting("RegName");
			regNum = Global.GetSetting("RegNum");
			if ((regName == "") || (regNum == "")) return "Not Registered!";
			//			if ( ConfirmReg(regName,regNum) || ConfirmESellerateReg(regName,regNum) )
			if ( ConfirmReg(regName,regNum) )
				return regName;
			return "Not Registered!";
		}

		public static void Register(string regName)
		{
			Global.SaveSetting("RegName",regName);
			Global.SaveSetting("RegNum",HashIt(regName));
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
			Global.SaveSetting("HashValue",SS.Substring(0,2) + DD + MM + YY);
			Global.WriteLocalAppData(myGUID, myKey,SS.Substring(0,2) + DD + MM + YY);
		}

		public static DateTime GetStartDate()
		{
			string tempStr1, tempStr2;
			DateTime aDate;
			tempStr1 = Global.GetSetting("HashValue");
			tempStr2 = Global.ReadAppData(myGUID,myKey);

			if (
				((tempStr1 == "") && (tempStr2 != ""))
				||
				((tempStr2 == "") && (tempStr1 != ""))
				)
			{
				//user has deleted one or the other so set it far in the past
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
					aDate = new DateTime(1901,1,1); //1901 signifies bad date
					return aDate;
				}
			}
			else
			{
				aDate = new DateTime(1900,1,1); //1900 signifies no date was found
				return aDate;
			}
		}

	}
}
