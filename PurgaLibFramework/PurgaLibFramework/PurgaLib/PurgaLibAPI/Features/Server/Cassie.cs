namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server
{
    public static class Cassie
    {
        public static void Message(string message, bool isNoisy = true, bool isSubtitles = true)
        {
            LabApi.Features.Wrappers.Cassie.Clear();
            
            LabApi.Features.Wrappers.Cassie.Message(message, isNoisy: isNoisy, isSubtitles: isSubtitles, customSubtitles: message);
        }
        
        public static void Clear()
        {
            LabApi.Features.Wrappers.Cassie.Clear();
        }
    }
}