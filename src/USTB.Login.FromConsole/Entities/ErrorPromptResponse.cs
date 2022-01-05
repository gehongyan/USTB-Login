namespace USTB.Login.FromConsole.Entities;

public class ErrorPromptResponse
{
    /// <summary>
    ///     获取提示信息是否成功
    /// </summary>
    [JsonConverter(typeof(CustomBooleanConverter))]
    [JsonPropertyName("result")]
    public bool IsSucceeded { get; init; }

    /// <summary>
    ///     信息
    /// </summary>
    [JsonPropertyName("msg")]
    public string Message { get; init; }

    /// <summary>
    ///     错误代码
    /// </summary>
    [JsonPropertyName("error_code")]
    public string ErrorCode { get; init; }

    /// <summary>
    ///     中文提示信息
    /// </summary>
    [JsonPropertyName("error_prompt_zh")]
    public string ErrorPromptChinese { get; init; }

    /// <summary>
    ///     英文提示信息
    /// </summary>
    [JsonPropertyName("error_prompt_en")]
    public string ErrorPromptEnglish { get; init; }
}