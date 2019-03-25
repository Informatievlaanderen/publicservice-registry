namespace PublicServiceRegistry.OrafinUpload
{
    using System;

    public class FileName
    {
        private readonly string _name;
        private readonly string _extension;

        public FileName(string name, string extension)
        {
            _name = CheckValue(name, nameof(name));
            _extension = CheckValue(extension, nameof(extension));
        }

        private static string CheckValue(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(name);

            return value;
        }

        public static implicit operator string(FileName fileName) => fileName?.ToString() ?? string.Empty;

        public override string ToString() => $"{_name}.{_extension}";
    }
}
