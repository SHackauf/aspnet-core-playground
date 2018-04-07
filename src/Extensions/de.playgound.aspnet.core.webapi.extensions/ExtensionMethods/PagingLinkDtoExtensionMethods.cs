using System.Collections.Generic;
using de.playground.aspnet.core.contracts.paging;

namespace de.playground.aspnet.core.webapi.extensions.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="PagingLinkDto"/>.
    /// </summary>
    public static class PagingLinkDtoExtensionMethods
    {
        /// <summary>
        /// Creates the link header format.
        /// </summary>
        /// <param name="pagingLinkDto">The <see cref="PagingLinkDto"/></param>
        /// <returns>String with link header format.</returns>
        public static string ToLinkHeader(this PagingLinkDto pagingLinkDto)
        {
            var links = new List<string>();

            if (!string.IsNullOrWhiteSpace(pagingLinkDto.Self))
            {
                links.Add($"<{pagingLinkDto.Self}>; rel=\"self\"");
            }

            if (!string.IsNullOrWhiteSpace(pagingLinkDto.Prev))
            {
                links.Add($"<{pagingLinkDto.Prev}>; rel=\"prev\"");
            }

            if (!string.IsNullOrWhiteSpace(pagingLinkDto.Next))
            {
                links.Add($"<{pagingLinkDto.Next}>; rel=\"next\"");
            }

            if (!string.IsNullOrWhiteSpace(pagingLinkDto.First))
            {
                links.Add($"<{pagingLinkDto.First}>; rel=\"first\"");
            }

            if (!string.IsNullOrWhiteSpace(pagingLinkDto.Last))
            {
                links.Add($"<{pagingLinkDto.Last}>; rel=\"last\"");
            }

            return string.Join(",", links);
        }
    }
}
