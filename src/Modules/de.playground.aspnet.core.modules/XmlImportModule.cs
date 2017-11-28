using System;
using System.IO;
using System.Xml.Serialization;

using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.modules.XmlModels;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.modules
{
    public class XmlImportModule : IXmlImportModule
    {
        #region Private Fields

        private readonly ILogger logger;

        #endregion

        #region Constructor

        public XmlImportModule(ILogger<XmlImportModule> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public bool Import(string xmlData)
        {
            if (xmlData == null)
            {
                throw new ArgumentNullException(nameof(xmlData));
            }

            if (this.TryDeserialize<XmlCustomer>(xmlData, out var xmlCustomer))
            {
                return true;
            }
            else if (this.TryDeserialize<XmlCustomers>(xmlData, out var xmlCustomers))
            {
                return true;
            }

            this.logger.LogError("Can not read XML-Data.");
            return false;
        }

        #endregion

        #region Private Methods

        private bool TryDeserialize<T>(string xmlData, out T result)
        {
            if (xmlData == null)
            {
                throw new ArgumentNullException(nameof(xmlData));
            }

            using (var stringReader = new StringReader(xmlData))
            {
                try
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    result = (T) xmlSerializer.Deserialize(stringReader);

                    return true;
                }
                catch (Exception exception)
                {
                    this.logger.LogDebug(exception, "Can not read XML-Data.");
                    result =  default(T);
                    return false;
                }
            }
        }

        #endregion
    }
}
