using System.Collections.Immutable;
using de.playground.aspnet.core.contracts.dtos;

namespace de.playground.aspnet.core.contracts.paging
{
    /// <summary>
    /// Paging <see cref="IDto"/>
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public class PagingDto<TDto> : IDto where TDto : IDto
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the packing informations.
        /// </summary>
        public PagingMetaDataDto Packing { get; set; }

        /// <summary>
        /// Gest or sets the packing links.
        /// </summary>
        public PagingLinkDto Links { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IImmutableList<TDto> Items { get; set; }

        #endregion

        #region Static Methos

        /// <summary>
        /// Creates a <see cref="PagingDto{TDto}"/> without <see cref="PagingLinkDto"/>.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="offset">The offset (first item position)</param>
        /// <param name="limit">The limit (max number of items)</param>
        /// <param name="total">The total number of items.</param>
        /// <returns>The <see cref="PagingDto{TDto}"/></returns>
        public static PagingDto<TDto> Create(IImmutableList<TDto> items, int offset, int limit, int total)
        {
            return new PagingDto<TDto>
            {
                Packing = new PagingMetaDataDto
                {
                    Offset = offset,
                    Limit = limit,
                    Total = total,
                    Returned = items.Count
                },
                Items = items
            };
        }

        /// <summary>
        /// Creates a <see cref="PagingDto{TDto}"/> with <see cref="PagingLinkDto"/>.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="offset">The offset (first item position)</param>
        /// <param name="limit">The limit (max number of items)</param>
        /// <param name="total">The total number of items</param>
        /// <param name="selfLink">The self link</param>
        /// <param name="prevLink">The pre</param>
        /// <param name="nextLink"></param>
        /// <param name="firstLink"></param>
        /// <param name="lastLink"></param>
        /// <returns></returns>
        public static PagingDto<TDto> Create(
            IImmutableList<TDto> items, 
            int offset, 
            int limit, 
            int total, 
            string selfLink, 
            string prevLink, 
            string nextLink, 
            string firstLink, 
            string lastLink)
        {
            return new PagingDto<TDto>
            {
                Packing = new PagingMetaDataDto
                {
                    Offset = offset,
                    Limit = limit,
                    Total = total,
                    Returned = items.Count
                },
                Links = new PagingLinkDto
                {
                    Self = selfLink,
                    Prev = prevLink,
                    Next = nextLink,
                    First = firstLink,
                    Last = lastLink
                },
                Items = items
            };
        }

        #endregion
    }
}
