namespace USTB.Login.FromConsole.Entities;

public class UserLoginErrorResponse : IUserLoginResponse
{
    /// <summary>
    /// </summary>
    public int wopt { get; set; }

    /// <summary>
    /// </summary>
    public int msg { get; set; }

    /// <summary>
    /// </summary>
    public int hidm { get; set; }

    /// <summary>
    /// </summary>
    public int hidn { get; set; }

    /// <summary>
    /// </summary>
    public IPAddress ss5 { get; set; }

    /// <summary>
    /// </summary>
    public IPAddress ss6 { get; set; }

    /// <summary>
    /// </summary>
    public int vid { get; set; }

    /// <summary>
    /// </summary>
    public PhysicalAddress ss1 { get; set; }

    /// <summary>
    /// </summary>
    public PhysicalAddress ss4 { get; set; }

    /// <summary>
    /// </summary>
    public int cvid { get; set; }

    /// <summary>
    /// </summary>
    public int pvid { get; set; }

    /// <summary>
    /// </summary>
    public int hotel { get; set; }

    /// <summary>
    /// </summary>
    public int aolno { get; set; }

    /// <summary>
    /// </summary>
    public int eport { get; set; }

    /// <summary>
    /// </summary>
    public int eclass { get; set; }

    /// <summary>
    /// </summary>
    public string ubind { get; set; }

    /// <summary>
    /// </summary>
    [JsonPropertyName("msga")]
    public string MessageCode { get; set; }

    /// <summary>
    ///     登录是否成功
    /// </summary>
    [JsonConverter(typeof(CustomBooleanConverter))]
    [JsonPropertyName("result")]
    public bool IsSucceeded { get; init; }

    /// <summary>
    ///     登录名
    /// </summary>
    [JsonPropertyName("uid")]
    public string UserID { get; set; }
}