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
        
        _logger.Information("校园网自动登录定时服务已启动");
        _logger.Information("连通性检查地址：{CheckConnectivityUrl}", _ustbLoginConfiguration.CheckInternetConnectivityUrl);
        _logger.Information("连通性检查间隔：{CheckInterval}", $"{_ustbLoginConfiguration.CheckIntervalSecond} 秒");
        _logger.Information("校园网登陆地址：{USTBLoginUrl}", _ustbLoginConfiguration.LoginAddress);
        _logger.Information("登录用户名列表：{UserIDs}", _ustbLoginConfiguration.LoginInfos
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
            try
            {
                _logger.Information("正在使用 {UserID} 登录校园网", loginInfo.UserID);

                (bool? isLoginSuccess, string message, IUserLoginResponse? userLoginResponse) =
                    await _userAccountModule.Login(loginInfo.UserID, loginInfo.Password);

                // 登陆成功
                if (isLoginSuccess is true && userLoginResponse is UserLoginSuccessResponse userLoginSuccessResponse)
                {
                    List<PhysicalAddress> bindingMACList = new List<PhysicalAddress>
                    {
                        userLoginSuccessResponse.MAC1,
                        userLoginSuccessResponse.MAC2,
                        userLoginSuccessResponse.MAC3,
                        userLoginSuccessResponse.MAC4,
                        userLoginSuccessResponse.MAC5,
                        userLoginSuccessResponse.MAC6
                    }.Where(mac => !string.IsNullOrWhiteSpace(mac.ToString())).ToList();
                    
                    _logger.Information("使用 {UserID} 登录校园网成功，用户名：{UserName}，余额：{Balance}，IP，{IPAddress}，MAC地址：{MAC}",
                        loginInfo.UserID,
                        userLoginSuccessResponse.Name,
                        userLoginSuccessResponse.Balance,
                        userLoginSuccessResponse.OnlineIPAddress.ToString(),
                        userLoginSuccessResponse.OnlineMAC.ToString().Chunk(2).Select(c => $"{c[0]}{c[1]}").Aggregate((a, b) => $"{a}:{b}"));
                    _logger.Information("当前账户已绑定的MAC地址：{BindingMAC}",
                        bindingMACList.Any()
                            ? bindingMACList.Where(mac => !string.IsNullOrWhiteSpace(mac.ToString()))
                                .Select(mac => mac.ToString()
                                    .Chunk(2)
                                    .Select(c => $"{c[0]}{c[1]}")
                                    .Aggregate((a, b) => $"{a}:{b}"))
                                .Aggregate((a, b) => $"{a}, {b}")
                            : "无");

                    if (!bindingMACList.Any() || bindingMACList.All(mac => !mac.Equals(userLoginSuccessResponse.OnlineMAC)))
                    {
                        _logger.Warning("该校园网账号所绑定的MAC地址中不包含当前设备");
                    }
                    
                    return;
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
            catch (Exception e)
            {
                _logger.Error("{Message}\n{Stack}", e.Message, e.StackTrace);
            }
        }
        
        _logger.Error("配置文件所提供的校园网登录信息都未成功登录校园网，请检查日志排查问题");
    }
}