namespace USTB.Login.FromConsole.Modules;

/// <summary>
///     用户账户模块
/// </summary>
public class UserAccountModule
{
    private readonly USTBLoginConfiguration _ustbLoginConfiguration;

    public UserAccountModule(USTBLoginConfiguration ustbLoginConfiguration)
    {
        _ustbLoginConfiguration = ustbLoginConfiguration;
    }
    
    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="studentNumber">学号</param>
    /// <param name="password">密码</param>
    /// <returns>
    ///     是否登陆成功、提示信息、登录响应结果
    /// </returns>
    public async Task<(bool? IsLoginSuccess, string Message, IUserLoginResponse? UserLoginResponse)> Login(
        string studentNumber, string password)
    {
        try
        {
            // 查询字符串
            List<KeyValuePair<string, string>> queryParams = new()
            {
                new KeyValuePair<string, string>("callback", "dr1004"),
                new KeyValuePair<string, string>("DDDDD", studentNumber),
                new KeyValuePair<string, string>("upass", password),
                new KeyValuePair<string, string>("0MKKey", "123456"),
                new KeyValuePair<string, string>("R1", "0"),
                new KeyValuePair<string, string>("R2", string.Empty),
                new KeyValuePair<string, string>("R3", "0"),
                new KeyValuePair<string, string>("R6", "0"),
                new KeyValuePair<string, string>("para", "00"),
                new KeyValuePair<string, string>("v6ip", string.Empty),
                new KeyValuePair<string, string>("terminal_type", "1"),
                new KeyValuePair<string, string>("lang", "zh-cn"),
                new KeyValuePair<string, string>("jsVersion", "4.1"),
                new KeyValuePair<string, string>("v", "1755"),
                new KeyValuePair<string, string>("lang", "zh")
            };

            string queryString = queryParams
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
                .Aggregate((a, b) => $"{a}&{b}");

            string url = $"http://{_ustbLoginConfiguration.LoginAddress}/drcom/login?{queryString}";

            // 发送请求
            HttpClient client = new();
            HttpRequestMessage req = new(HttpMethod.Get, url);
            HttpResponseMessage res = await client.SendAsync(req);

            // 读取响应
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string result = Encoding.GetEncoding("gb2312").GetString(await res.Content.ReadAsByteArrayAsync());

            // 匹配结果
            Match match = new Regex(@"^\s*dr1004\((?<json>.*)\);?\s*$").Match(result);
            string jsonString = match.Groups["json"].Value;

            // 正则匹配失败
            if (string.IsNullOrWhiteSpace(jsonString)) return (null, "登录请求结果正则匹配失败", null);

            // 解析结果
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new CustomDateTimeConverter());
            options.Converters.Add(new CustomIPAddressConverter());
            options.Converters.Add(new CustomPhysicalAddressConverter());
            UserLoginSuccessResponse? userLoginSuccessResponse =
                JsonSerializer.Deserialize<UserLoginSuccessResponse>(jsonString, options);

            // 登录成功
            if (userLoginSuccessResponse?.IsSucceeded is true) return (true, "登陆成功", userLoginSuccessResponse);

            // 登陆失败
            UserLoginErrorResponse? userLoginErrorResponse =
                JsonSerializer.Deserialize<UserLoginErrorResponse>(jsonString, options);
            (bool? isGettingSuccess, string message, ErrorPromptResponse? errorPromptResponse) =
                await GetErrorPrompt(userLoginErrorResponse?.MessageCode!);

            return isGettingSuccess is true
                ? (false, message, userLoginErrorResponse)
                : (false, $"获取错误提示信息异常，错误代码：{userLoginErrorResponse?.MessageCode}", userLoginErrorResponse);
        }
        catch (Exception e)
        {
            return (null, e.Message, null);
        }
    }

    private async Task<(bool? IsGettingSuccess, string Message, ErrorPromptResponse? ErrorPromptResponse)>
        GetErrorPrompt(string errorCode)
    {
        // http://202.204.48.66:801/eportal/portal/err_code/loadErrorPrompt?callback=dr1005&error_code=userid%20error1&jsVersion=4.1&v=10025&lang=zh
        try
        {
            // 查询字符串
            List<KeyValuePair<string, string>> queryParams = new()
            {
                new KeyValuePair<string, string>("callback", "dr1005"),
                new KeyValuePair<string, string>("error_code", HttpUtility.UrlEncode(errorCode)),
                new KeyValuePair<string, string>("jsVersion", "4.1"),
                new KeyValuePair<string, string>("v", "10025"),
                new KeyValuePair<string, string>("lang", "zh")
            };

            string queryString = queryParams
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
                .Aggregate((a, b) => $"{a}&{b}");

            string url = $"http://{_ustbLoginConfiguration.LoginAddress}:801/eportal/portal/err_code/loadErrorPrompt?{queryString}";

            // 发送请求
            HttpClient client = new();
            HttpRequestMessage req = new(HttpMethod.Get, url);
            HttpResponseMessage res = await client.SendAsync(req);

            // 读取响应
            string result = await res.Content.ReadAsStringAsync();

            // 匹配结果
            Match match = new Regex(@"^\s*dr1005\((?<json>.*)\);?\s*$").Match(result);
            string jsonString = match.Groups["json"].Value;

            // 正则匹配失败
            if (string.IsNullOrWhiteSpace(jsonString)) return (null, "错误代码提示获取结果正则匹配失败", null);

            // 解析结果
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new CustomDateTimeConverter());
            options.Converters.Add(new CustomIPAddressConverter());
            options.Converters.Add(new CustomPhysicalAddressConverter());
            ErrorPromptResponse? errorPromptResponse =
                JsonSerializer.Deserialize<ErrorPromptResponse>(jsonString, options);

            return (errorPromptResponse?.IsSucceeded, errorPromptResponse?.ErrorPromptChinese ?? "错误提示信息获取异常",
                errorPromptResponse);
        }
        catch (Exception e)
        {
            return (null, e.Message, null);
        }
    }
}