using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AutoMapper;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.contracts.utils.logger;
using de.playground.aspnet.core.modules.XmlModels;

using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.modules
{
    public class XmlImportModule : IXmlImportModule
    {
        #region Private Fields

        private readonly ICustomerDataAccess customerDataAccess;
        private readonly IProductDataAccess productDataAccess;

        private readonly IMapper mapper;
        private readonly ILogger logger;

        #endregion

        #region Constructor

        public XmlImportModule(ILogger<XmlImportModule> logger, IMapper mapper, ICustomerDataAccess customerDataAccess, IProductDataAccess productDataAccess)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.customerDataAccess = customerDataAccess ?? throw new ArgumentNullException(nameof(customerDataAccess));
            this.productDataAccess = productDataAccess ?? throw new ArgumentNullException(nameof(productDataAccess));
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
                var importedCustomerDto = await this.ImportCustomerAsync(xmlCustomer);
                if (importedCustomerDto != null)
                {
                    this.logger.LogInformation(LoggingEvents.ImportItem, $"{nameof(this.ImportAsync)}: successful [Id: {importedCustomerDto.Id}]");
                    return true;
                }

                return false;
            }

            var xmlCustomers = await this.DeserializeAsync<XmlCustomers>(xmlData);
            if (xmlCustomers != default(XmlCustomers))
            {
                foreach (var internalXmlCustomer in xmlCustomers.Customers)
                {
                    var importedCustomerDto = await this.ImportCustomerAsync(internalXmlCustomer);
                    if (importedCustomerDto != null)
                    {
                        this.logger.LogInformation(LoggingEvents.ImportItem, $"{nameof(this.ImportAsync)}: successful [Id: {importedCustomerDto.Id}]");
                    }
                }

                return true;
            }

            this.logger.LogError("Can not read XML-Data.");
            return false;
        }

        #endregion

        #region Private Methods

        private async Task<CustomerPoco> ImportCustomerAsync(XmlCustomer xmlCustomer)
        {
            if (xmlCustomer == null)
            {
                throw new ArgumentNullException(nameof(xmlCustomer));
            }

            var customerPoco = this.mapper.Map<CustomerPoco>(xmlCustomer);

            var savedCustomerPoco = await this.customerDataAccess.InsertCustomerAsync(customerPoco);
            if (savedCustomerPoco != null)
            {
                foreach (var xmlProduct in xmlCustomer.Products)
                {
                    var productPoco = this.mapper.Map<ProductPoco>(xmlProduct, option => option.Items["CustomerId"] = savedCustomerPoco.Id);
                    await this.productDataAccess.InsertProductAsync(productPoco);
                }
            }

            return savedCustomerPoco;
        }

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
