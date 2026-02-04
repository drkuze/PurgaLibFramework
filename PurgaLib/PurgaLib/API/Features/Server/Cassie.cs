using System.Text.RegularExpressions;
using Cassie;

namespace PurgaLib.API.Features.Server;

public static class Cassie
{
    private static string CleanMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return "";

        message = Regex.Replace(message, @"pitch_\d+(\.[\d]+)?", "", RegexOptions.IgnoreCase);
        message = Regex.Replace(message, @"jam_[w\d_]+", "", RegexOptions.IgnoreCase);
        message = Regex.Replace(message, @"\.g\d+", "", RegexOptions.IgnoreCase);
        message = Regex.Replace(message, @"\s{2,\}", "");

        return message.Trim();
    }

    public static void Message(string message, bool isHeld = false, bool isNoisy = true, bool isSubtitles = false)
    {
        var m = CleanMessage(message);
        new CassieAnnouncement(new CassieTtsPayload(m, isSubtitles, isHeld), 0f, isNoisy ? 1 : 0).AddToQueue();
    }

    public static void Clear() => CassieAnnouncementDispatcher.ClearAll();
}