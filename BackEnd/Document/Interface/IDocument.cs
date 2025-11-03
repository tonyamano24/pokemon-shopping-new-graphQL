using Microsoft.OpenApi.Models;

namespace itsc_dotnet_practice.Document.Interface
{
    public interface IDocument
    {
        OpenApiDocument GetOpenApiDocument();
    }
}
