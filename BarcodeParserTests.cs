using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Upharm2016.Test.마약류;
using Upharm2016.Utility.Parser;

namespace Upharm2016.Test.Helper.Parser
{
    [TestFixture]
    public class BarcodeParserTests
    {
        private string 정상순서_QR코드;
        private string 정상순서_바코드;

        private string 반대순서_QR코드;
        private string 반대순서_바코드;

        private string _스틸녹스정_1;
        private string _스틸녹스정_2;
        private string _데파스정_1;
        private string _데파스정_2;
        private string _자낙스정_1;
        private string _아네폴주사;
        private string _자낙스정_2;
        private string _자낙스정_2_Serial;
        private string _스틸녹스_1_Serial;

        [SetUp]
        public void Setup()
        {
            정상순서_QR코드 = 마약류바코드모음.정상순서_QR코드;
            정상순서_바코드 = 마약류바코드모음.정상순서_바코드;
            반대순서_QR코드 = 마약류바코드모음.반대순서_QR코드;
            반대순서_바코드 = 마약류바코드모음.반대순서_바코드;

            _스틸녹스정_1 = "01088065210064371721072510SNFV015 21A02030P3358KT0 ";
            _스틸녹스정_2 = "01088065210064371721091010SNFW001 21A02031781GWH79 ";
            _데파스정_1 = "01088064330031281720111310CE007 21171222L4G5928 ";
            _데파스정_2 = "01088064330031281720111310CE007 21171222L4Q5929 ";
            _자낙스정_1 = "010880648900664721532300874225 1718101410N42949 ";
            _아네폴주사 = "01088065780457311717050110A1234 210000000006 ";
            _자낙스정_2 = "01088064890066301717050510B1234 210000000010 ";

            _자낙스정_2_Serial = "01088064890066301717050510B1234210000000010";
            _스틸녹스_1_Serial = "01088065210064371721072510SNFV01521A02030P3358KT0";
        }


        [Test]
        public void CanBarcodeParserObjectCreat()
        {
            var obj = new BarcodeParser("");

            Assert.IsNotNull(obj);
        }
        

        // 01088065210064371721072510SNFV015 21A02030P3358KT0 
        [Test]
        public void TEST_1()
        {
            var 스틸녹스정 = _스틸녹스정_1;

            if (IsBarcode(스틸녹스정))
            {
                var GSIdx = 스틸녹스정.IndexOf(' ');
                스틸녹스정 = 스틸녹스정.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(스틸녹스정);

            Assert.AreEqual("8806521006437", result.표준코드);
            Assert.AreEqual("SNFV015", result.제조번호);
            Assert.AreEqual("A02030P3358KT0", result.일련번호);
            Assert.AreEqual("20210725", result.유효기한);
        }

        [Test]
        public void TEST_2()
        {
            var 스틸녹스정 = _스틸녹스정_2;

            if (IsBarcode(스틸녹스정))
            {
                var GSIdx = 스틸녹스정.IndexOf(' ');
                스틸녹스정 = 스틸녹스정.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(스틸녹스정);

            Assert.AreEqual(result.표준코드, "8806521006437");
            Assert.AreEqual(result.제조번호, "SNFW001");
            Assert.AreEqual(result.일련번호, "A02031781GWH79");
            Assert.AreEqual(result.유효기한, "20210910");
        }

        [Test]
        public void TEST_3()
        {
            var drug = _데파스정_1;

            if (IsBarcode(drug))
            {
                var GSIdx = drug.IndexOf(' ');
                drug = drug.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual(result.표준코드, "8806433003128");
            Assert.AreEqual(result.제조번호, "CE007");
            Assert.AreEqual(result.일련번호, "171222L4G5928");
            Assert.AreEqual(result.유효기한, "20201113");
        }

        [Test]
        public void TEST_4()
        {
            var drug = _데파스정_2;

            if (IsBarcode(drug))
            {
                var GSIdx = drug.IndexOf(' ');
                drug = drug.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual(result.표준코드, "8806433003128");
            Assert.AreEqual(result.제조번호, "CE007");
            Assert.AreEqual(result.일련번호, "171222L4Q5929");
            Assert.AreEqual(result.유효기한, "20201113");
        }


        [Test]
        public void TEST_5()
        {
            var drug = _자낙스정_1;

            if (IsBarcode(drug))
            {
                var GSIdx = drug.IndexOf(' ');
                drug = drug.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual(result.표준코드, "8806489006647");
            Assert.AreEqual(result.제조번호, "N42949");
            Assert.AreEqual(result.일련번호, "532300874225");
            Assert.AreEqual(result.유효기한, "20181014");
        }

        [Test]
        public void TEST_6()
        {
            var drug = _아네폴주사;

            if (IsBarcode(drug))
            {
                var GSIdx = drug.IndexOf(' ');
                drug = drug.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual(result.표준코드, "8806578045731");
            Assert.AreEqual(result.제조번호, "A1234");
            Assert.AreEqual(result.일련번호, "0000000006");
            Assert.AreEqual(result.유효기한, "20170501");
        }

        [Test]
        public void TEST_7()
        {
            var drug = _자낙스정_2;

            if (IsBarcode(drug))
            {
                var GSIdx = drug.IndexOf(' ');
                drug = drug.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual(result.표준코드, "8806489006630");
            Assert.AreEqual(result.제조번호, "B1234");
            Assert.AreEqual(result.일련번호, "0000000010");
            Assert.AreEqual(result.유효기한, "20170505");
        }

        [Test]
        public void TEST_8()
        {
            var drug = _자낙스정_2;

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual(result.표준코드, "8806489006630");
            Assert.AreEqual(result.제조번호, "B1234");
            Assert.AreEqual(result.일련번호, "0000000010");
            Assert.AreEqual(result.유효기한, "20170505");
        }


        [Test]
        public void TEST_1_Serial()
        {
            //_자낙스정_2_Serial = "01088064890066301717050510B1234210000000010";

            var drug = _자낙스정_2_Serial;

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual(result.표준코드, "8806489006630");
            Assert.AreEqual(result.제조번호, "B1234");
            Assert.AreEqual(result.일련번호, "0000000010");
            Assert.AreEqual(result.유효기한, "20170505");
        }

        [Test]
        public void TEST_2_Serial()
        {
            var drug = _스틸녹스_1_Serial;

            if (IsBarcode(drug))
            {
                var GSIdx = drug.IndexOf(' ');
                drug = drug.Remove(GSIdx, 1).Insert(GSIdx, ((char)29).ToString());
            }

            var result = BarcodeParser.Parse(drug);

            Assert.AreEqual("8806521006437", result.표준코드);
            Assert.AreEqual("SNFV015", result.제조번호);
            Assert.AreEqual("A02030P3358KT0", result.일련번호);
            Assert.AreEqual("20210725", result.유효기한);
        }


        private bool IsBarcode(string value)
        {
            int cnt = 0;
            const char t = ' ';
            var charArr = value.ToCharArray();

            for (int i = 0; i < charArr.Length; i++)
                if (charArr[i] == t)
                    cnt++;

            return cnt >= 2 ? true : false;
        }

    }
}
