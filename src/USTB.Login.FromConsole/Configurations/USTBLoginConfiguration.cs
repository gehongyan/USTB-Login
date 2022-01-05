namespace USTB.Login.FromConsole.Configurations;

public class USTBLoginConfiguration
{
    /// <summary>
    ///     登录站点
    /// </summary>
    public string LoginAddress { get; init; }

    /// <summary>
    ///     检查网络连通性地址
    /// </summary>
    public string CheckInternetConnectivityUrl { get; init; }

    /// <summary>
    ///     登录使用信息
    /// </summary>
    public List<LoginInfo> LoginInfos { get; init; }

    /// <summary>
    ///     检查时间间隔秒数
    /// </summary>
    public int CheckIntervalSecond { get; set; }
}

public class LoginInfo
{
    /// <summary>
    ///     用户名
    /// </summary>
    public string UserID { get; init; }

    /// <summary>
    ///     密码
    /// </summary>
    public string Password { get; init; }
}