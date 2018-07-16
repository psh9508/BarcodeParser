using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Upharm2016.Utility.Parser
{
    public class BarcodeParser
    {
        private string _barcodeData;
        private const string _GS = "\u001d"; // GroupSeparater
        //private const string _GS = " "; // GroupSeparater
        //private const string _GS = "#"; // GroupSeparater
        private static byte[] _bytes;
        private const int LEN_상품코드 = 14;
        private const int LEN_유통기한 = 6;
        private const string 유통기한Separater = "17";
        private const string 제조번호Separater = "10";
        private const string 일련번호Separater = "21";

        public BarcodeParser(string strData)
        {
            _barcodeData = strData;
        }



        public static Barcode_1D Parse(string barcodeString)
        {
            _bytes = Encoding.ASCII.GetBytes(barcodeString);

            var parsedBarcode = new Barcode_1D();
            string 상품코드 = string.Empty;
            string 유통기한 = string.Empty;
            string 제조번호 = string.Empty;
            string 일련번호 = string.Empty;

            int curPos = 0;

            var 가장처음2문자 = GetBytesString(ref curPos, 2);

            if (가장처음2문자 == "01") // 처음엔 무조건 01(상품코드)
            {
                상품코드 = GetBytesString(ref curPos, LEN_상품코드);
                상품코드 = 상품코드.Substring(1, 상품코드.Length - 1);
            }

            for (int i = 0; i < 3; i++)
            {
                string 다음구분자 = Get다음구분자(barcodeString, ref curPos);

                switch (다음구분자)
                {
                    case 유통기한Separater:
                        유통기한 = "20" + GetBytesString(ref curPos, LEN_유통기한);
                        break;
                    case 제조번호Separater:
                        제조번호 = GetBytesStringUntilGS(ref curPos);
                        break;
                    case 일련번호Separater:
                        일련번호 = GetBytesStringUntilGS(ref curPos);
                        break;
                    default:
                        break;
                }
            }

            parsedBarcode.표준코드 = 상품코드;
            parsedBarcode.제조번호 = 제조번호;
            parsedBarcode.유효기한 = 유통기한;
            parsedBarcode.일련번호 = 일련번호;
            parsedBarcode.청구코드 = Get청구코드(상품코드);
            parsedBarcode.비교청구코드 = parsedBarcode.청구코드?.Substring(0, parsedBarcode.청구코드.Length - 1);
            parsedBarcode.바코드 = barcodeString.Replace("\u001d", "");

            #region 예전
            //barcodeString = barcodeString.Replace("\u001d", "");
            //barcodeString = barcodeString.Replace(" ", "");
            //barcodeString = barcodeString.Trim();

            //if (ValidateBarcodeString(barcodeString))
            //{
            //    parsedBarcode = new Barcode_1D();

            //    parsedBarcode.표준코드 = barcodeString.Substring(3, 13);
            //    parsedBarcode.유효기한 = "20" + barcodeString.Substring(18, 6);
            //    parsedBarcode.제조번호 = Get제조번호(barcodeString);
            //    parsedBarcode.일련번호 = Get일련번호(barcodeString);

            //    parsedBarcode.청구코드 = Get청구코드(parsedBarcode.표준코드);
            //    parsedBarcode.비교청구코드 = parsedBarcode.청구코드?.Substring(0, parsedBarcode.청구코드.Length - 1);
            //    parsedBarcode.바코드 = barcodeString;
            //} 
            #endregion

            return parsedBarcode;





        }

        //public static Barcode_1D Parse(string barcodeString)
        //{
        //    //barcodeString = "010880643300312817201113 10CE007 21171222L4G5928 ";
        //    var parsedBarcode = new Barcode_1D();
        //    string 상품코드 = string.Empty;
        //    string 유통기한 = string.Empty;
        //    string 제조번호 = string.Empty;
        //    string 일련번호 = string.Empty;

        //    int startPos = 0;
        //    int curPos = 0;

        //    var 가장처음2문자 = barcodeString.Substring(startPos, 2);

        //    curPos = startPos + 2;

        //    if(가장처음2문자 == "01") // 처음엔 무조건 01(상품코드)
        //    {
        //        상품코드 = barcodeString.Substring(curPos + 1, LEN_상품코드 - 1);
        //        curPos += LEN_상품코드;
        //    }

        //    for (int i = 0; i < 3; i++)
        //    {
        //        string 다음구분자 = Get다음구분자(barcodeString, ref curPos);

        //        switch (다음구분자)
        //        {
        //            case 유통기한Separater:
        //                유통기한 = "20" + barcodeString.Substring(curPos, LEN_유통기한);
        //                curPos += LEN_유통기한;
        //                break;
        //            case 제조번호Separater:
        //                제조번호 = GetValue(barcodeString, ref curPos);
        //                break;
        //            case 일련번호Separater:
        //                일련번호 = GetValue(barcodeString, ref curPos);
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    parsedBarcode.표준코드 = 상품코드;
        //    parsedBarcode.제조번호 = 제조번호;
        //    parsedBarcode.유효기한 = 유통기한;
        //    parsedBarcode.일련번호 = 일련번호;
        //    parsedBarcode.청구코드 = Get청구코드(상품코드);
        //    parsedBarcode.비교청구코드 = parsedBarcode.청구코드?.Substring(0, parsedBarcode.청구코드.Length - 1);
        //    parsedBarcode.바코드 = barcodeString;

        //    #region 예전
        //    //barcodeString = barcodeString.Replace("\u001d", "");
        //    //barcodeString = barcodeString.Replace(" ", "");
        //    //barcodeString = barcodeString.Trim();

        //    //if (ValidateBarcodeString(barcodeString))
        //    //{
        //    //    parsedBarcode = new Barcode_1D();

        //    //    parsedBarcode.표준코드 = barcodeString.Substring(3, 13);
        //    //    parsedBarcode.유효기한 = "20" + barcodeString.Substring(18, 6);
        //    //    parsedBarcode.제조번호 = Get제조번호(barcodeString);
        //    //    parsedBarcode.일련번호 = Get일련번호(barcodeString);

        //    //    parsedBarcode.청구코드 = Get청구코드(parsedBarcode.표준코드);
        //    //    parsedBarcode.비교청구코드 = parsedBarcode.청구코드?.Substring(0, parsedBarcode.청구코드.Length - 1);
        //    //    parsedBarcode.바코드 = barcodeString;
        //    //} 
        //    #endregion

        //    return parsedBarcode;
        //}

        private static string Get다음구분자(string barcodeString, ref int curPos)
        {
            var 다음구분자 = GetBytesString(ref curPos, 1);

            if (다음구분자 == _GS)
                다음구분자 = GetBytesString(ref curPos, 2);
            else
                다음구분자 += GetBytesString(ref curPos, 1);

            return 다음구분자;
        }

        private static string GetValue(string barcodeString, ref int curPos)
        {
            var buff = new StringBuilder();

            var 다음문자 = string.Empty;

            do
            {
                if (curPos >= barcodeString.Length)
                    break;

                다음문자 = barcodeString.Substring(curPos, 1);
                
                if (다음문자 != _GS)
                {
                    buff.Append(다음문자);
                    curPos += 1;
                }
            } while (다음문자 != _GS);

            return buff.ToString().Trim();
        }


        private static string Get청구코드(string 표준코드)
        {
            string retv = string.Empty;

            if(표준코드.Length >= 13)
            {
                var 업체식별코드 = 표준코드.Substring(3, 4);
                var 품목코드 = 표준코드.Substring(7, 5);

                retv = 업체식별코드 + 품목코드;
            }

            return retv;
        }

        private static string Get제조번호(string barcodeString)
        {
            var tmp = new StringBuilder();
            int separatePosition = 24;
            int 제조번호길이 = 5;

            if (barcodeString.Substring(separatePosition, 2) == "10")
            {
                tmp.Append(barcodeString.Substring(separatePosition + 2, 제조번호길이));
            }
            else
            {
                tmp.Append(barcodeString.Substring(barcodeString.Length - 제조번호길이, 제조번호길이));
            }

            return tmp.ToString();
        }

        private static string Get일련번호(string barcodeString)
        {
            var tmp = new StringBuilder();
            int separatePosition = 24;
            int 일련번호길이 = 10;

            if (barcodeString.Substring(24, 2) == "10")
            {
                tmp.Append(barcodeString.Substring(barcodeString.Length - 일련번호길이, 일련번호길이));
            }
            else if(barcodeString.Substring(separatePosition, 2) == "21")
            {
                tmp.Append(barcodeString.Substring(separatePosition + 2, 일련번호길이));
            }

            return tmp.ToString();
        }

        private static bool ValidateBarcodeString(string barcodeString)
        {
            return (barcodeString.StartsWith("01") && barcodeString.Substring(16, 2) == "17");
        }

        private static string GetBytesString(ref int curPos, int targetPos)
        {
            var buff = new List<byte>();

            for (int i = curPos; i < targetPos + curPos; i++)
                buff.Add(_bytes[i]);

            curPos = targetPos + curPos;

            return Encoding.ASCII.GetString(buff.ToArray());
        }

        private static string GetBytesStringUntilGS(ref int curPos)
        {
            var buff = new List<byte>();
            string 다음문자;

            do
            {
                if (curPos >= _bytes.Length)
                    break;

                다음문자 = ((char)_bytes[curPos]).ToString();
                
                if (다음문자 != _GS )
                {
                    buff.Add(_bytes[curPos]);
                    curPos += 1;
                }
            } while (다음문자 != _GS);

            return Encoding.ASCII.GetString(buff.ToArray()).Trim();
        }
    }


    public class Barcode_1D
    {
        public string 표준코드 {get; set;}
        public string 유효기한 {get; set;}
        public string 제조번호 {get; set;}
        public string 일련번호 {get; set;}
        public string 청구코드 {get; set;}
        public string 비교청구코드 {get; set;}
        public string 바코드 {get; set;}
    }
}
