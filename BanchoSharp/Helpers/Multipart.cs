using System.Text;

namespace Helpers {
    public class Multipart {
        public static string GetBoundary(string contentType)
        {
            var parts = contentType.Split(';');
            var boundaryPart = parts.FirstOrDefault(p => p.Trim().StartsWith("boundary="));

            if (boundaryPart != null)
            {
                return boundaryPart.Trim().Substring("boundary=".Length);
            }

            throw new InvalidOperationException("Boundary not found in content type.");
        }

        public static Dictionary<string, string> ParseMultipartFormData(StreamReader reader, string boundary)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>();

            string line;
            while ((line = reader.ReadLine()) != null && !line.Contains(boundary))
            {
                if (line.Contains("Content-Disposition") && line.Contains("form-data"))
                {
                    var nameStart = line.IndexOf("name=", StringComparison.OrdinalIgnoreCase);
                    if (nameStart != -1)
                    {
                        var nameEnd = line.IndexOf("\"", nameStart + 6);
                        if (nameEnd != -1)
                        {
                            var name = line.Substring(nameStart + 6, nameEnd - (nameStart + 6));
                            reader.ReadLine(); // Skip the empty line after headers
                            var value = ReadUntilBoundary(reader, boundary);
                            formData[name] = value.Trim();
                        }
                    }
                }
            }

            return formData;
}



        public static string ReadUntilBoundary(StreamReader reader, string boundary)
        {
            var result = new StringBuilder();

            var line = reader.ReadLine();

            while (line != null && !line.Contains(boundary))
            {
                result.AppendLine(line);
                line = reader.ReadLine();
            }

            return result.ToString();
        }
    }
}