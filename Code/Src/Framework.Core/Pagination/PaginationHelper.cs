using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Pagination
{
    public static class PaginationHelper
    {
        public static List<T> ApplyPagination<T>(this List<T> models, PaginationModel paginationModel)
        {
            var pageIndex = paginationModel.PageIndex <= 0 ? 0 : paginationModel.PageIndex - 1;

            var pageSize = paginationModel.PageSize == 0 ? int.MaxValue : paginationModel.PageSize;

            paginationModel.TotalCount = models.Count;

            var pageCount = (int)Math.Ceiling((decimal)models.Count / pageSize);
            if (pageIndex > pageCount)
                pageIndex = 1;

            paginationModel.TotalPageCount = pageCount == 0 ? 1 : pageCount;

            var result = models
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();
            return result;
        } 
    }
}
