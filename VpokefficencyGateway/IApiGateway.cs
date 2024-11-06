using VpokefficencyFramework;

namespace VpokefficencyGateway
{
    public interface IApiGateway
    {
        Task<ResponseBody> GetJsonResponse(string url);

    }
}