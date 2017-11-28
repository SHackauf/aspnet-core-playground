using System;
using System.Collections.Generic;
using System.Text;

namespace de.playground.aspnet.core.contracts.modules
{
    public interface IXmlImportModule
    {
        bool Import(string xmlFilePath);
    }
}
