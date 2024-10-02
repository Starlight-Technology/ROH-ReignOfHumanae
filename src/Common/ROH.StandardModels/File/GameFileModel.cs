namespace ROH.StandardModels.File
{
    public class GameFileModel
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public byte[]? Content { get; set; }
        public long Size { get; set; }
        public bool Active { get; set; }

        public GameFileModel(string name, string format, byte[]? content, long size, bool active)
        {
            Name = name;
            Format = format;
            Content = content;
            Size = size;
            Active = active;
        }
    }
}
