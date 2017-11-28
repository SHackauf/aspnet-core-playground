using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
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

            using (var stringReader = new StringReader(xmlData))
            {
                try
                {
                    var xmlSerializer = new XmlSerializer(typeof(Customer));
                    var customer = xmlSerializer.Deserialize(stringReader) as Customer;

                    return true;
                }
                catch (Exception exception)
                {
                    this.logger.LogError(exception, "Can not read XML-Data.");
                    return false;
                }
            }
        }

        #endregion
    }
}
