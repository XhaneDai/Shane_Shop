using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ShaneShop.Models.Util
{


    public class Operation
    {
        /// <summary>
        /// 取得交易狀態中文對應
        /// </summary>
        /// <param name="OrderStatusEN"></param>
        /// <returns></returns>
        public static string GetOrderStatusInCH(string OrderStatusEN)
        {
            string OrderStatusCH = string.Empty;
            switch (OrderStatusEN)
            {
                case "CreatePay":
                    OrderStatusCH = "訂單成立";
                    break;
                case "Approved":
                    OrderStatusCH = "付款成功";
                    break;
                case "Failed":
                    OrderStatusCH = "付款失敗";
                    break;
                default:
                    OrderStatusCH = "付款狀態異常";
                    break;
            }
            return OrderStatusCH;
        }

        /// <summary>
        /// 綠界檢查碼加密規則
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="HashKey"></param>
        /// <param name="HashIV"></param>
        /// <param name="encryptType"></param>
        /// <returns></returns>
        public static string GetCheckMacValue(Dictionary<string, string> postData, string HashKey, string HashIV, int encryptType = 0)
        {

            Dictionary<string, string> postParameterList = new Dictionary<string, string>();

            var chkList = postData.OrderBy(x => x.Key); // 排序
            StringBuilder ChkParameter = new StringBuilder(); // 依英文字母順序排序, 前後加上HashKey及HashIV

            ChkParameter.AppendFormat("HashKey={0}", HashKey);
            foreach (var item in chkList)
            {
                ChkParameter.AppendFormat("&" + item.Key + "={0}", item.Value);
            }
            ChkParameter.AppendFormat("&HashIV={0}", HashIV);

            //URL Encode
            string Chkencode = HttpUtility.UrlEncode(ChkParameter.ToString());
            //轉小寫
            string ChklowerEncoded = Chkencode.ToLower();
            //檢查碼
            string ChkMacValue = "";
            switch (encryptType)
            {
                case 0:
                    ChkMacValue = SHAEncrypt(ChklowerEncoded).ToUpper();
                    break;
                default:
                    ChkMacValue = MD5Encrypt(ChklowerEncoded).ToUpper();
                    break;
            }

            return ChkMacValue;
        }
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="str">待加密的字串</param>
        /// <returns>加密後的字串(十六進位 兩位數)</returns>
        public static string SHAEncrypt(string str)
        {
            System.Security.Cryptography.SHA256 sha256 = new System.Security.Cryptography.SHA256Managed();
            byte[] sha256Bytes = System.Text.Encoding.Default.GetBytes(str);
            byte[] cryString = sha256.ComputeHash(sha256Bytes);
            string sha256Str = string.Empty;
            for (int i = 0; i < cryString.Length; i++)
            {
                sha256Str += cryString[i].ToString("X2");
            }
            return sha256Str;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">待加密的字串</param>
        /// <returns>加密後的字串(32字)</returns>
        public static string MD5Encrypt(string str)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(str);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}