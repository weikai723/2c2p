using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    using Jose;   
    using System.Net;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Http.Headers;


    internal class Program
    {
        static void Main(string[] args)
        {
            var paymentRequest = "<MaintenanceRequest>" +
                "<Version>2.3</Version>" +
                "<timeStamp>160623142900</timeStamp>" +
                "<merchantID>" + "702702000002233" + "</merchantID>" +
                "<storeCardUniqueID></storeCardUniqueID>" +
                "<pan>4111111111111111</pan>" +
                "<cardholderName>Test</cardholderName>" +
                "<cardholderEmail>wei_kai723@hotmail.com</cardholderEmail>" +
                "<panExpiry>1230</panExpiry>" +                
                "<action>A</action>" +
                "</MaintenanceRequest>";
            string receiverPublicCertPath = "C:/Users/wei_k/source/repos/ConsoleApp1/ConsoleApp1/bin/Debug/sandbox-jwt-2c2p.demo.2.1(public).cer"; //2c2p public cert key

            X509Certificate2 receiverPublicCert = (X509Certificate2)null;

            receiverPublicCert = new X509Certificate2(receiverPublicCertPath);
            var receiverPublicKey = receiverPublicCert.GetRSAPublicKey();

            string senderPrivateKeyPath = "C:/Users/wei_k/source/repos/ConsoleApp1/ConsoleApp1/bin/Debug/privatekey.pfx"; //merchant generated private key
            string senderPrivateKeyPassword = "iM5tr9cs"; //private key password

    
            X509Certificate2 senderPrivateCert = (X509Certificate2)null;

            senderPrivateCert = new X509Certificate2(senderPrivateKeyPath, senderPrivateKeyPassword);
            var senderPrivateKey = senderPrivateCert.GetRSAPrivateKey();


            var jweRequest = Jose.JWT.Encode(paymentRequest, receiverPublicKey, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM, null);

            var jwsRequest = Jose.JWT.Encode(jweRequest, senderPrivateKey, JwsAlgorithm.PS256, null);

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/*+json");
            client.Headers.Add("Accept", "text/plain");
            string responseData = client.UploadString("https://demo2.2c2p.com/2C2PFrontend/PaymentAction/2.0/action", "POST", jwsRequest);

        }
    }
}
