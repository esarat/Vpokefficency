using VpokefficencyFramework;

namespace VpokefficencyGateway
{
    public class RestApiGateway : IApiGateway
    {
        public async Task<ResponseBody> GetJsonResponse(string url)
        {
            //Create a response body
            ResponseBody responseBody = new ResponseBody();
            try
            {
                //setup http client
                using (HttpClient client = new HttpClient())
                {
                    //make the call
                    HttpResponseMessage response = await client.GetAsync(url);

                    //chech resp status code
                    if (response.IsSuccessStatusCode)
                    {
                        //success - read response into payload
                        responseBody.status = "Success";
                        responseBody.PayLoad = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        //Error - pass response status code in payload
                        responseBody.status = "Error";
                        responseBody.PayLoad = response.StatusCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //Exception occurred - send ex message in payload to caller
                responseBody.status = "Exception";
                responseBody.PayLoad = ex.Message;
            }

            //send wrapped response back to caller
            return responseBody;

        }
    }
}
