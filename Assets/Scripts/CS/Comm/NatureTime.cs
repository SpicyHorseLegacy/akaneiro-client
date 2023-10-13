using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


    public class NatureTime
    {

        public enum NatureTimeState
        {
            eNatureTimeSecond = 1,
            eNatureTimeMinute = eNatureTimeSecond * 60,
            eNatureTimeHour = eNatureTimeMinute * 60,
            eNatureTimeDay = eNatureTimeHour * 24,
            eNatureTimeMonth = eNatureTimeDay * 30,
            eNatureTimeYear = eNatureTimeMonth * 12,
        };

        enum NatureTimeStartYear
        {
            eStartYear = 2000
        };

        public NatureTime() { _iTotalSecond = 0; }
        public NatureTime(int Y, int Mon, int D, int H, int M, int S)
        {
            _iTotalSecond = Y * (int)NatureTimeState.eNatureTimeYear
                + Mon * (int)NatureTimeState.eNatureTimeMonth
                + D * (int)NatureTimeState.eNatureTimeDay
                + H * (int)NatureTimeState.eNatureTimeHour
                + M * (int)NatureTimeState.eNatureTimeMinute
                + S * (int)NatureTimeState.eNatureTimeSecond;
        }
        public NatureTime(NatureTime r) { _iTotalSecond = r._iTotalSecond; }
        public NatureTime(int S) { _iTotalSecond = S; }

        // 2009-10-28 12:32:45
        //NatureTime(String str);


        public static NatureTime operator + (NatureTime op1, NatureTime op2)
        {
            NatureTime returnVal = new NatureTime();
            returnVal._iTotalSecond = op1._iTotalSecond + op2._iTotalSecond;
            return returnVal;
        }

        public static NatureTime operator - (NatureTime op1, NatureTime op2)
        {
            NatureTime returnVal = new NatureTime();
            returnVal._iTotalSecond = op1._iTotalSecond - op2._iTotalSecond;
            return returnVal;
        }


        public static NatureTime operator + (NatureTime op1, int second)
        {
            NatureTime returnVal = new NatureTime();
            returnVal._iTotalSecond = op1._iTotalSecond + second;
            return returnVal;
        }

        public static NatureTime operator - (NatureTime op1, int second)
        {
            NatureTime returnVal = new NatureTime();
            returnVal._iTotalSecond = op1._iTotalSecond - second;
            return returnVal;
        }

        public static bool operator == (NatureTime op1, NatureTime op2)
        {
            return Object.Equals(op1._iTotalSecond, op2._iTotalSecond);
        }

        public static bool operator != (NatureTime op1, NatureTime op2)
        {
            return !Object.Equals(op1._iTotalSecond, op2._iTotalSecond);
        }

        public override bool Equals(object o)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(NatureTime op1, int second)
        {
            return Object.Equals(op1._iTotalSecond, second);
        }

        public static bool operator !=(NatureTime op1, int second)
        {
            return !Object.Equals(op1._iTotalSecond, second);
        }

        public int GetTotalSecond() { return _iTotalSecond; }

        public int GetYear()
        {
            return _iTotalSecond / (int)NatureTimeState.eNatureTimeYear + (int)NatureTimeStartYear.eStartYear;
        }

        public int GetMonth()
        {
            return (_iTotalSecond % (int)NatureTimeState.eNatureTimeYear) / (int)NatureTimeState.eNatureTimeMonth;
        }

        public int GetDay()
        {
            return ((_iTotalSecond % (int)NatureTimeState.eNatureTimeYear) % (int)NatureTimeState.eNatureTimeMonth) / (int)NatureTimeState.eNatureTimeDay;
        }

        public int GetHour()
        {
            return (((_iTotalSecond % (int)NatureTimeState.eNatureTimeYear) % (int)NatureTimeState.eNatureTimeMonth) % (int)NatureTimeState.eNatureTimeDay) / (int)NatureTimeState.eNatureTimeHour;
        }

        public int GetMinute()
        {
            return ((((_iTotalSecond % (int)NatureTimeState.eNatureTimeYear) % (int)NatureTimeState.eNatureTimeMonth) % (int)NatureTimeState.eNatureTimeDay) % (int)NatureTimeState.eNatureTimeHour) / (int)NatureTimeState.eNatureTimeMinute;
        }

        public int GetSecond()
        {
            return _iTotalSecond % (int)NatureTimeState.eNatureTimeMinute;
        }

        public override string ToString()
        {
            string str = "";

            str += GetYear() + "-" + GetMonth() + "-" + GetDay() + " " + GetHour() + ":" + GetMinute() + ":" + GetSecond();
            return str;
        }

        public static NatureTime Now()
        {
            DateTime MyDateTime = DateTime.Now;
            NatureTime returnVal = new NatureTime(MyDateTime.Year - (int)NatureTimeStartYear.eStartYear, MyDateTime.Month, MyDateTime.Day, MyDateTime
                .Hour, MyDateTime.Minute, MyDateTime.Second);
            return returnVal;
        }

        protected int _iTotalSecond;


    }
