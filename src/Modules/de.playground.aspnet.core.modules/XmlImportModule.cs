using System;
using System.IO;
using System.Threading.Tasks;
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

        public async Task<bool> ImportAsync(string xmlData)
        {
            if (xmlData == null)
            {
                throw new ArgumentNullException(nameof(xmlData));
            }

            var xmlCustomer = await this.DeserializeAsync<XmlCustomer>(xmlData);
            if (xmlCustomer != default(XmlCustomer))
            {
                return true;
            }

            var xmlCustomers = await this.DeserializeAsync<XmlCustomers>(xmlData);
            if (xmlCustomers != default(XmlCustomers))
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

        private Task<T> DeserializeAsync<T>(string xmlData)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            Task.Run(() =>
            {
                if (this.TryDeserialize<T>(xmlData, out T result))
                {
                    taskCompletionSource.TrySetResult(result);
                }
                else
                {
                    taskCompletionSource.TrySetResult(default(T));
                }
            });

            return taskCompletionSource.Task;
        }

        #endregion
    }
}
