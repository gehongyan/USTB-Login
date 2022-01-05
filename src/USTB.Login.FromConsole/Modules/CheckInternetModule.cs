namespace USTB.Login.FromConsole.Modules;

public class CheckInternetModule
{
    private readonly USTBLoginConfiguration _ustbLoginConfiguration;

    public CheckInternetModule(USTBLoginConfiguration ustbLoginConfiguration)
    {
        _ustbLoginConfiguration = ustbLoginConfiguration;
    }
    
    /// <summary>
    ///     检查网络连通性
    /// </summary>
    /// <returns></returns>
    public async ValueTask<bool> CheckInternet()
    {
        try
        {
            HttpClient client = new();
            HttpRequestMessage req = new(HttpMethod.Get, _ustbLoginConfiguration.CheckInternetConnectivityUrl);
            HttpResponseMessage res = await client.SendAsync(req);
            return res.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}