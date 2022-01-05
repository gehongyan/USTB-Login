namespace USTB.Login.FromConsole.Entities;

public class UserLoginSuccessResponse : IUserLoginResponse
{
    /// <summary>
    /// </summary>
    public int aolno { get; init; }

    /// <summary>
    /// </summary>
    public int m46 { get; init; }

    /// <summary>
    /// </summary>
    public IPAddress v46ip { get; init; }

    /// <summary>
    /// </summary>
    public string myv6ip { get; init; }

    /// <summary>
    /// </summary>
    public int sms { get; init; }

    /// <summary>
    ///     当前余额
    /// </summary>
    [JsonConverter(typeof(CustomBalanceConverter))]
    [JsonPropertyName("ufee")]
    public double Balance { get; init; }

    /// <summary>
    ///     姓名
    /// </summary>
    [JsonPropertyName("NID")]
    public string Name { get; init; }

    /// <summary>
    /// </summary>
    public int olno { get; init; }

    /// <summary>
    /// </summary>
    public string udate { get; init; }

    /// <summary>
    ///     在线设备MAC地址
    /// </summary>
    [JsonPropertyName("olmac")]
    public PhysicalAddress OnlineMAC { get; init; }

    /// <summary>
    /// </summary>
    public int ollm { get; init; }

    /// <summary>
    /// </summary>
    public string olm1 { get; init; }

    /// <summary>
    /// </summary>
    public string olm2 { get; init; }

    /// <summary>
    /// </summary>
    public int olm3 { get; init; }

    /// <summary>
    /// </summary>
    public int olmm { get; init; }

    /// <summary>
    /// </summary>
    public int olm5 { get; init; }

    /// <summary>
    /// </summary>
    public int gid { get; init; }

    /// <summary>
    /// </summary>
    public int ispid { get; init; }

    /// <summary>
    /// </summary>
    public IPAddress opip { get; init; }

    /// <summary>
    /// </summary>
    [JsonPropertyName("olip")]
    public IPAddress OnlineIPAddress { get; init; }

    /// <summary>
    /// </summary>
    public int oaf { get; init; }

    /// <summary>
    /// </summary>
    public int oat { get; init; }

    /// <summary>
    ///     绑定MAC1
    /// </summary>
    public PhysicalAddress MAC1 { get; init; }

    /// <summary>
    ///     绑定MAC5
    /// </summary>
    public PhysicalAddress MAC2 { get; init; }

    /// <summary>
    ///     绑定MAC3
    /// </summary>
    public PhysicalAddress MAC3 { get; init; }

    /// <summary>
    ///     绑定MAC4
    /// </summary>
    public PhysicalAddress MAC4 { get; init; }

    /// <summary>
    ///     绑定MAC5
    /// </summary>
    public PhysicalAddress MAC5 { get; init; }

    /// <summary>
    ///     绑定MAC6
    /// </summary>
    public PhysicalAddress MAC6 { get; init; }

    /// <summary>
    /// </summary>
    public string ac0 { get; init; }

    /// <summary>
    /// </summary>
    public long oltime { get; init; }

    /// <summary>
    /// </summary>
    public long olflow { get; init; }

    /// <summary>
    /// </summary>
    public IPAddress lip { get; init; }

    /// <summary>
    /// </summary>
    public DateTime stime { get; init; }

    /// <summary>
    /// </summary>
    public DateTime etime { get; init; }

    /// <summary>
    /// </summary>
    public int sv { get; init; }

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