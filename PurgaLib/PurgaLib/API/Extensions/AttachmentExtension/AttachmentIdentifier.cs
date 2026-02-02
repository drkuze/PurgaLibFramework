namespace PurgaLib.API.Extensions.AttachmentExtension
{
    public class AttachmentIdentifier
    {
        public string Name { get; init; }

        public int Id { get; init; }

        public AttachmentIdentifier(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString() => $"{Name} (ID: {Id})";
    }
}