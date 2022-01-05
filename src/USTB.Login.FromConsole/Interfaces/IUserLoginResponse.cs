namespace USTB.Login.FromConsole.Interfaces;

public interface IUserLoginResponse
{
    /// <summary>
    ///     登录是否成功
    /// </summary>
    public bool IsSucceeded { get; init; }

    /// <summary>
    ///     登录名
    /// </summary>
    public string UserID { get; set; }
}