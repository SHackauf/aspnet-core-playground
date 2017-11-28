using System.Threading.Tasks;

namespace de.playground.aspnet.core.contracts.modules
{
    public interface IXmlImportModule
    {
        Task<bool> ImportAsync(string xmlFilePath);
    }
}
