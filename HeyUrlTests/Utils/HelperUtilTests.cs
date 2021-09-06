using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeyUrl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyUrl.Utils.Tests
{
	[TestClass()]
	public class HelperUtilTests
	{
		[TestMethod()]
		public void CheckGenShortUrlTest_Equal_url()
		{
			// arrange
			//case 1
			string originalUrl = "drive.google.com/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";
			string shortUrl_exp = "CDADD"; 

			// act
			string shortUrl_gen = HelperUtil.GetShortUrl(originalUrl);
			
			// assert
			Assert.AreEqual(shortUrl_exp, shortUrl_gen); 
		}

		[TestMethod()]
		public void CheckGenShortUrlTest_Not_Equal_url()
		{
			// arrange			
			string originalUrl_1 = "drive.google.com/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";
			string originalUrl_2 = "Adrive.google.com/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";

			// act
			string shortUrl_1_gen = HelperUtil.GetShortUrl(originalUrl_1); 
			string shortUrl_2_gen = HelperUtil.GetShortUrl(originalUrl_2); 

			// assert			
			Assert.AreNotEqual(shortUrl_1_gen, shortUrl_2_gen); 
		}


		[TestMethod()]
		public void IsCheckedShortUrlRequestTest()
		{
			// arrange

			//-correct
			string url_1 = "CBEAB";
			string url_2 = "cbeab";
			string url_2_1 = "aBcdE";

			//-incorrect
			string url_3 = "CBEABA";
			string url_4 = "CBEA";
			string url_5 = "CB5AB";
			string url_6 = "CB5A?";
			string url_7 = "cbeaba";

			// act
			//true
			bool isValid_1 = HelperUtil.IsValidShortUrl(url_1);
			bool isValid_2 = HelperUtil.IsValidShortUrl(url_2);
			bool isValid_2_1 = HelperUtil.IsValidShortUrl(url_2_1);

			//false
			bool isValid_3 = HelperUtil.IsValidShortUrl(url_3);
			bool isValid_4 = HelperUtil.IsValidShortUrl(url_4);
			bool isValid_5 = HelperUtil.IsValidShortUrl(url_5);
			bool isValid_6 = HelperUtil.IsValidShortUrl(url_6);
			bool isValid_7 = HelperUtil.IsValidShortUrl(url_7);

			// assert
			Assert.IsTrue(isValid_1);
			Assert.IsTrue(isValid_2);
			Assert.IsTrue(isValid_2_1);

			Assert.IsFalse(isValid_3);
			Assert.IsFalse(isValid_4);
			Assert.IsFalse(isValid_5);
			Assert.IsFalse(isValid_6);
			Assert.IsFalse(isValid_7);
		}

		[TestMethod()]
		public void IsCheckedIncorrectUrlTest()
		{
			// arrange

			string errMes1 = "Incorrect Url format";
			string errMes2 = "Enter your original Url";

			string url_1 = "qwerty";
			string url_2 = "";
			string url_3 = "drive.google.com/ file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1"; //space
			string url_4 = "drive.googlecom/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";   //host
			string url_5 = "drive.google.com\file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";  //back slash
			string url_6 = "drive.google/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";      //host
			string url_7 = $"htt://drive.google.com/file/d/1Eq2RbIiIcAcy4AxiElSutcu2rv_Y21zZ/view?usp=sharing";  //schema
			
			// act
			Tuple<bool, string> isValid_1 = HelperUtil.IsValidUrl(url_1);
			Tuple<bool, string> isValid_2 = HelperUtil.IsValidUrl(url_2);
			Tuple<bool, string> isValid_3 = HelperUtil.IsValidUrl(url_3);
			Tuple<bool, string> isValid_4 = HelperUtil.IsValidUrl(url_4);
			Tuple<bool, string> isValid_5 = HelperUtil.IsValidUrl(url_5);
			Tuple<bool, string> isValid_6 = HelperUtil.IsValidUrl(url_6);
			Tuple<bool, string> isValid_7 = HelperUtil.IsValidUrl(url_7);

			// assert
			Assert.IsFalse(isValid_1.Item1);
			Assert.AreEqual(errMes1, isValid_1.Item2);

			Assert.IsFalse(isValid_2.Item1);
			Assert.AreEqual(errMes2, isValid_2.Item2);

			Assert.IsFalse(isValid_3.Item1);
			Assert.AreEqual(errMes1, isValid_3.Item2);

			Assert.IsFalse(isValid_4.Item1);
			Assert.AreEqual(errMes1, isValid_4.Item2);

			Assert.IsFalse(isValid_5.Item1);
			Assert.AreEqual(errMes1, isValid_5.Item2);

			Assert.IsFalse(isValid_6.Item1);
			Assert.AreEqual(errMes1, isValid_6.Item2);

			Assert.IsFalse(isValid_7.Item1);
			Assert.AreEqual(errMes1, isValid_7.Item2);
		}

		[TestMethod()]
		public void IsValidCorrectUrlTest()
		{
			// arrange
			string mes1 = "OK";

			string url_1 = "www.google.com";
			string url_2 = "drive.google.com/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";
			string url_3 = "www.rbc.ru";
			string url_4 = $"https://drive.google.com/file/d/1Eq2RbIiIcAcy4AxiElSutcu2rv_Y21zZ/view?usp=sharing";
			string url_5 = $"http://drive.google.com/file/d/1Eq2RbIiIcAcy4AxiElSutcu2rv_Y21zZ/view?usp=sharing";
			string url_6 = "drive.google.com/file/d/1FTxGaWiH3kgbP-m7eaCdeKYDexZTNFu1";
			string url_7 = $"https://www.microsoft.com/en-us/store/p/global-atm-map/9nblggh4nfpd";
			string url_8 = $"https://www.youtube.com/watch?time_continue=2&v=1Sggj3JMGi8";
			
			// act
			Tuple<bool, string> isValid_1 = HelperUtil.IsValidUrl(url_1);
			Tuple<bool, string> isValid_2 = HelperUtil.IsValidUrl(url_2);
			Tuple<bool, string> isValid_3 = HelperUtil.IsValidUrl(url_3);
			Tuple<bool, string> isValid_4 = HelperUtil.IsValidUrl(url_4);
			Tuple<bool, string> isValid_5 = HelperUtil.IsValidUrl(url_5);
			Tuple<bool, string> isValid_6 = HelperUtil.IsValidUrl(url_6);
			Tuple<bool, string> isValid_7 = HelperUtil.IsValidUrl(url_7);
			Tuple<bool, string> isValid_8 = HelperUtil.IsValidUrl(url_8);

			// assert
			Assert.IsTrue(isValid_1.Item1);
			Assert.AreEqual(mes1, isValid_1.Item2);

			Assert.IsTrue(isValid_2.Item1);
			Assert.AreEqual(mes1, isValid_2.Item2);

			Assert.IsTrue(isValid_3.Item1);
			Assert.IsTrue(isValid_4.Item1);
			Assert.IsTrue(isValid_5.Item1);
			Assert.IsTrue(isValid_6.Item1);
			Assert.IsTrue(isValid_7.Item1);
			Assert.IsTrue(isValid_8.Item1);
		}

		
	}
}