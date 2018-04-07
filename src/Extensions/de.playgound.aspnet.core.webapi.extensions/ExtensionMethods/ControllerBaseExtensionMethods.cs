using System;
using System.Collections.Immutable;
using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace de.playground.aspnet.core.webapi.extensions.ExtensionMethods
{
    /// <summary>
    /// Extension method for <see cref="ControllerBase"/>.
    /// </summary>
    public static class ControllerBaseExtensionMethods
    {
        /// <summary>
        /// Creates an <see cref="OkObjectResult"/> for a result with paging.
        /// </summary>
        /// <typeparam name="TDto">The objects</typeparam>
        /// <param name="controllerBase">The <see cref="ControllerBase"/></param>
        /// <param name="routeName">The route name to create the paging links</param>
        /// <param name="items">The items from type <see cref="IImmutableList{T}"/></param>
        /// <param name="offset">The offset (first item). Starts with 0.</param>
        /// <param name="limit">The item limits per call.</param>
        /// <param name="total">The total number of items.</param>
        /// <param name="envelope">
        ///     <code>True</code> means the paging information will be part of the response body and header.
        ///     <code>False</code> means the paging information will be only part of the response header.
        /// </param>
        /// <param name="createRouteValues">Method to create the route values for the paging links</param>
        /// <returns>The <see cref="OkObjectResult"/> with paging informations.</returns>
        public static OkObjectResult OkPaging<TDto>(
            this ControllerBase controllerBase,
            string routeName,
            IImmutableList<TDto> items,
            int offset,
            int limit,
            int total,
            bool envelope,
            Func<int, int, bool, RouteValueDictionary> createRouteValues) where TDto : IDto
        {
            if (string.IsNullOrWhiteSpace(routeName))
            {
                throw new ArgumentException("Is null, empty or white space.", nameof(routeName));
            }
            
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var pagingDto = PagingDto<TDto>.Create(
                items,
                offset,
                limit,
                total,
                controllerBase.Url.Link(routeName, createRouteValues(offset, limit, envelope)),
                offset == 0 ? string.Empty : controllerBase.Url.Link(routeName, createRouteValues(Math.Max(offset - limit, 0), limit, envelope)),
                offset + limit >= total ? string.Empty : controllerBase.Url.Link(routeName, createRouteValues(offset + limit, limit, envelope)),
                controllerBase.Url.Link(routeName, createRouteValues(0, limit, envelope)),
                controllerBase.Url.Link(routeName, createRouteValues(Math.Max(total - limit, 0), limit, envelope)));

            controllerBase.Response.Headers.Add("X-Tracker-Pagination-Total", pagingDto.Packing.Total.ToString());
            controllerBase.Response.Headers.Add("X-Tracker-Pagination-Limit", pagingDto.Packing.Limit.ToString());
            controllerBase.Response.Headers.Add("X-Tracker-Pagination-Offset", pagingDto.Packing.Offset.ToString());
            controllerBase.Response.Headers.Add("X-Tracker-Pagination-Returned", pagingDto.Packing.Returned.ToString());

            if (pagingDto.Links != null)
            {
                controllerBase.Response.Headers.Add("Link", pagingDto.Links.ToLinkHeader());
            }

            return envelope
                ? controllerBase.Ok(pagingDto)
                : controllerBase.Ok(items);
        }
    }
}
