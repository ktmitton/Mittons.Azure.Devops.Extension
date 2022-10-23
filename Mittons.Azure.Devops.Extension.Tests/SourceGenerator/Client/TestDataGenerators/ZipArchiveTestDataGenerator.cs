using System.Collections;
using System.IO.Compression;
using System.Net.Mime;

namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client.TestDataGenerators;

public class ZipArchiveTestDataGenerator : IEnumerable<object[]>
{
    private static ZipArchive CreateZipArchive(Dictionary<string, string> files)
    {
        var byteArray = CreateZipArchiveByteArray(files);
        var memoryStream = new MemoryStream();
        memoryStream.Write(byteArray, 0, byteArray.Length);

        var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, false);

        return archive;
    }

    private static byte[] CreateZipArchiveByteArray(Dictionary<string, string> files)
    {
        using var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var file in files)
            {
                var entry = archive.CreateEntry(file.Key);

                using (var entryStream = entry.Open())
                using (var streamWriter = new StreamWriter(entryStream))
                {
                    streamWriter.Write(file.Value);
                }
            }
        }

        return memoryStream.ToArray();
    }

    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new FunctionDefinition<ZipArchive>(
                (ITestGitClient client) => client.ZipArchiveResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Zip,
                new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string> { { "text.txt", "Test Content" } })),
                CreateZipArchive(new Dictionary<string, string> { { "text.txt", "Test Content" } }),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<ZipArchive>(
                (ITestGitClient client) => client.ZipArchiveResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Zip,
                new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string> { { "test.csv", "Not really a csv" }, { "test.pdf", "Also not really a pdf" } })),
                CreateZipArchive(new Dictionary<string, string> { { "test.csv", "Not really a csv" }, { "test.pdf", "Also not really a pdf" } }),
                default
            )
        },
        new object[]
        {
            new FunctionDefinition<ZipArchive>(
                (ITestGitClient client) => client.ZipArchiveResponse(),
                HttpMethod.Get,
                "5.2-preview.1",
                "/get",
                string.Empty,
                MediaTypeNames.Application.Zip,
                new ByteArrayContent(CreateZipArchiveByteArray(new Dictionary<string, string>())),
                CreateZipArchive(new Dictionary<string, string>()),
                default
            )
        }
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}