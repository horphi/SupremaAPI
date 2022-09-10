using System;
using RestSharp;
using System.Security.Cryptography;
using System.Text;


namespace Suprema
{
    class Program
    {

        //API Private Key UAT (environment)
        private static string privateKey =
            "PLEASE_REPLACE_ME_WITH_UAT_PRIVATE_KEY";

        //API EndPoint UAT (environment)
        private static string apiEndpointUrl =
            "PLEASE_REPLACE_ME_WITH_UAT_URL";

        //Prefix for the string to be hashed for Check Code
        private static string prefix = "brazilRestAPI";


        static void Main(string[] args)
        {
            // Example usage for calling the API
            // Get Email Verification Code
            // {{url}}/BrazilRestAPI/email_verification
            GetEmailVerificationCode();


            // Example usage for calling the API
            // Create account
            // {{url}}/BrazilRestAPI/create_account
            CreateAccount();  


        }

        //Validated
        static void GetEmailVerificationCode()
        {
            //Parameters that required for this endpoint 

            //This endpoint only takes 1 parameter which is email
            var email = "horphi7@gmail.com";

            //This Parameter is needed for each of every single call to the API
            var checkcode = "";


            //CheckCode calculation process


            //Splicing "brazilRestAPI" with all the parameter and finally with the privatekey with "," as the seperator.
            //so the meta in this situation would be:
            //"brazilRestAPI,email,privatekey"

            //Real case scenerio
            var sToHash = "brazilRestAPI,horphi7@gmail.com,32fe1350667444de96f3c3f7a1bfb59a";

            //sending the spliced string to hash
            //the output: "944e5d8464bc2b341224b5ced3ed0f66"
            checkcode = GetMd5Hash(sToHash);
            Console.WriteLine(checkcode);


            //calling the API
            var client = new RestClient(apiEndpointUrl);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Accept", "application/json");
            //Json body only 2 parameter required in this case which is 
            // email and checkcode 
            var body = new
            {
                email = email,
                checkcode = checkcode,
            };
            request.AddJsonBody(body);
            var response = client.Execute(request).Content;

            Console.WriteLine(response);
        }


        static void CreateAccount()
        {
            //Parameters that required for this endpoint 

            //This endpoint will takes 9 parameters
            var email = "tony@caliantech";
            var username = "tony";
            var password = "123456";
            var nickname = "tony";
            var force_bind_email = "1"; //true => 1, false => 0
            var app_version = "h2poker";
            var birth_date = "";
            var validation_code = "7463";
            //This Parameter is needed for each of every single call to the API
            var checkcode = "";


            //CheckCode calculation process

            //Splicing "brazilRestAPI" with all the parameter and finally with the privatekey with "," as the seperator.
            //so the meta in this situation would be:
            //"brazilRestAPI,email,username,password,nickname,force_bind_email,app_version,birth_date,validation_code,privatekey"

            //Real case scenerio
            var sToHash = $"{prefix},{username},{password},{nickname},{force_bind_email},{app_version},{birth_date},{email},{validation_code},{privateKey}";

            //sending the spliced string to hash
            //the output: "f1fe88449176fd8b293c38c3e8b2fa4e"
            checkcode = GetMd5Hash(sToHash);
            Console.WriteLine(checkcode);


            //calling the API
            var client = new RestClient(apiEndpointUrl);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Accept", "application/json");
            //Json body with all 9 parameters required in this case which is 
            // email username password nickname force_bind_email app_version birth_date validation_code and checkcode 
            var body = new
            {
                username = username,
                password = password,
                nickname = nickname,
                force_bind_email = force_bind_email,
                app_version = app_version,
                birth_date = birth_date,
                email = email,
                validation_code = validation_code,
                checkcode = checkcode,
            };
            request.AddJsonBody(body);
            var response = client.Execute(request).Content;

            Console.WriteLine(response);
        }



        /// <summary>
        /// This Method MD5 Hash the parameter string to produce CheckCode
        /// and attached into each of every request to API
        /// </summary>
        /// <param name="param">String to be hashed</param>
        /// <returns></returns>
        static string GetMd5Hash(string param)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            using (var md5Hash = MD5.Create())
            {

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(param));

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
            }
            return sBuilder.ToString();
        }
    }
}
