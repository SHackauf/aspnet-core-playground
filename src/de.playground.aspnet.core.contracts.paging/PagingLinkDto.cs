using de.playground.aspnet.core.contracts.dtos;

namespace de.playground.aspnet.core.contracts.paging
{
    /// <summary>
    /// Paging Links
    /// </summary>
    public class PagingLinkDto : IDto
    {
        /// <summary>
        /// Gets or sets previously href from paging.
        /// </summary>
        public string Prev { get; set; }

        /// <summary>
        /// Gets or sets next href from paging.
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// Gets or sets actual href from paging.
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// Gets the first href from paging.
        /// </summary>
        public string First { get; set; }

        /// <summary>
        /// Gets or sets last href from paging.
        /// </summary>
        public string Last { get; set; }
    }
}
