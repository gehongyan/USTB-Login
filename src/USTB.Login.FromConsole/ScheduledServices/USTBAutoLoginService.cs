namespace USTB.Login.FromConsole.ScheduledServices;

public class USTBAutoLoginService : ScheduledService
{
    private readonly ILogger _logger;
    private readonly USTBLoginConfiguration _ustbLoginConfiguration;
    private readonly UserAccountModule _userAccountModule;
    private readonly CheckInternetModule _checkInternetModule;

    public USTBAutoLoginService(ILogger logger,
        USTBLoginConfiguration ustbLoginConfiguration,
        UserAccountModule userAccountModule,
        CheckInternetModule checkInternetModule)
        : base(TimeSpan.FromSeconds(5), 
            TimeSpan.FromSeconds(ustbLoginConfiguration.CheckIntervalSecond), 
            logger, 
            nameof(USTBAutoLoginService))
    {
        _logger = logger;
        _ustbLoginConfiguration = ustbLoginConfiguration;
        _userAccountModule = userAccountModule;
        _checkInternetModule = checkInternetModule;
        
        _logger.Information("校园网自动登录定时服务已启动\n" +
                            "连通性检查地址：{CheckConnectivityUrl}\n" +
                            "连通性检查间隔：{CheckInterval}\n" +
                            "校园网登陆地址：{USTBLoginUrl}\n" +
                            "登录用户名列表：{UserIDs}",
            _ustbLoginConfiguration.CheckInternetConnectivityUrl,
            $"{_ustbLoginConfiguration.CheckIntervalSecond} 秒",
            _ustbLoginConfiguration.LoginAddress,
            _ustbLoginConfiguration.LoginInfos
                .Select(info => info.UserID)
                .Aggregate((a, b) => $"{a}, {b}"));
    }

    protected override async Task ExecuteAsync()
    {
        // 网络连通则跳过后续操作
        if (await _checkInternetModule.CheckInternet())
        {
            _logger.Information("网络连通性检查正常");
            return;
        }

        // 网络不连通，执行登录
        foreach (LoginInfo loginInfo in _ustbLoginConfiguration.LoginInfos)
        {
            _logger.Information("正在使用 {UserID} 登录校园网", loginInfo.UserID);

            (bool? isLoginSuccess, string message, IUserLoginResponse? userLoginResponse) =
                await _userAccountModule.Login(loginInfo.UserID, loginInfo.Password);

            // 登陆成功
            if (isLoginSuccess is true && userLoginResponse is UserLoginSuccessResponse userLoginSuccessResponse)
            {
                _logger.Information("使用 {UserID} 登录校园网成功，用户名：{UserName}，余额：{Balance}",
                    loginInfo.UserID,
                    userLoginSuccessResponse.Name,
                    userLoginSuccessResponse.Balance);
                break;
            }

            // 登陆失败
            if (isLoginSuccess is false && userLoginResponse is UserLoginErrorResponse userLoginErrorResponse)
            {
                _logger.Error("使用 {UserID} 登录校园网失败，错误信息：{Message}",
                    loginInfo.UserID,
                    message);
            }

            // 其它错误
            else
            {
                _logger.Error("使用 {UserID} 登录校园网失败，错误信息：{Message}",
                    loginInfo.UserID,
                    message);
            }
        }
    }
}