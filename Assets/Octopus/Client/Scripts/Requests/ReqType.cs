using System.ComponentModel;

public enum ReqType
{
    [Description("Init Request")]
    create,
    [Description("Start Request")]
    receive,
    [Description("Update Request")]
    token,
    [Description("Track Request")]
    track
}