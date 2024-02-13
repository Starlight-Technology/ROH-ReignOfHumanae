using System;

namespace ROH.StandardModels.File
{
    public class FileModel
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public byte[] Content { get; set; }

        public FileModel(string name, string format, string content)
        {
            Name = name;
            Format = format;
            Content = Convert.FromBase64String(content);
        }
    }
}
