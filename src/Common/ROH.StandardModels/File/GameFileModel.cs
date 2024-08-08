namespace ROH.StandardModels.File
{
    public class GameFileModel
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public byte[]? Content { get; set; }

        public GameFileModel(string name, string format, byte[]? content)
        {
            Name = name;
            Format = format;
            Content = content;
        }
    }
}
