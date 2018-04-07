using de.playground.aspnet.core.contracts.dtos;

namespace de.playground.aspnet.core.contracts.paging
{
    /// <summary>
    /// Paging meta data.
    /// </summary>
    public class PagingMetaDataDto : IDto
    {
        /// <summary>
        /// Gets or sets total numger of items.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the offset (first item). Starts by 0.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Get or sets the item limits per call.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gest or sets the number of returned items.
        /// </summary>
        public int Returned { get; set; }
    }
}
