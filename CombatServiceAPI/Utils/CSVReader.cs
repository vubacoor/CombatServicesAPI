using System.IO;
using System.Net;

namespace CombatServiceAPI.Utils
{
    public class CSVReader
    {
        public string GetCSV()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://doc-0c-94-docs.googleusercontent.com/docs/securesc/3v23lvpi13oum9gegd5hk8f7stl9a7r7/aq2n0mpb8hc2ast8eqpjqf5i2vhfemc2/1659684600000/12896236375512508393/12896236375512508393/1K5vlTAm8WaVA674447_CYixjyYwNDlkP?e=download&ax=AI9vYm4tKyrpn2YfZ2QZ2h5oE-dHAM-qV8gI9AnjQHGo1Caxgrkaj7fHKJxPDRXlKkgtw7aCfJsBxfK_QtWECGKXGr4tZS7wkx-zjbCTKlN-k7gKUbFLtlcG-TA04yzunKY8Ojl5CW9xjVSLPluXDwBPpTtoFOfXpyZ5xSCQzz7eLMceNnnJ9kja_bBYQQDBaTw6MRMD5FodTokFwi4BmbRS_wu5cIpaGlcRyyBAhyi_uAlquSZi3eT4HkXUv4K2v9U2SWPxbTga9hLEK1iUbxHVrPRgJintSp-cQgcxG8_KIIqfS-YJXv97yRDsPfxdun_D8HxOhqOlqjytcmk-stzLkSLBV6Q76G_L1t0YPCpy4NdORsYJCw17yXtU2r6pk902XrHtxtDuEbIxXBbprYUgIVanR-XUj5erNEFpRXxAnM8kvNW9tiGUVg5FXq2_eGw-mip0voXCooFaeVx3nwC66xezW5bMzyZwnZDhzd6YuywTLUvyuBEB0aOOKsnxD6wMf8I7M-BqAWFqk35ucTM1TK4jeUPZFniAYepUHLSzn_DWm6sDFnv5pf6PcLg3uVTF1JYezOWxzdvI3YjrUMX18V6_Grr1i8nFpOFLtqu-y_zCTPQxmJHf1_X74rakZGoax9yMZvWnFtzqZGtxVVTY5cLRf9tF5svi5AZYjkAVhxnF5cfya-rcfE95wCEdOSeeHeF5o0GVZBWpI12L8Yy31feSVTP6c9-dSAXVYks-h-ekSqgFd4A2RHJhX03vwESwfCVMOQAOC6Bb-Z8K2FAelKuqoxptGgYpvdSbX2S6JLWbL-o&uuid=4045f4be-7c0a-45ee-b41d-3fbffc220643&authuser=0&nonce=s1vs95veqqncc&user=12896236375512508393&hash=6ckucccib15dugohhis26gnlffhr6146");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
    }
}
