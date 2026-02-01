namespace PurgaLib.API.Features.Players.Hints;

public class PlyHint
{
    public PlyHint() : this(string.Empty){}

    public PlyHint(string message, float duration = 3, bool show = true)
    {
        Message = message;
        Duration = duration;
        Show = show;
    }

    public string Message { get; set; }
    public float Duration { get; set; }
    public bool Show { get; set; }
}