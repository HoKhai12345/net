using TransportApi.Interface;
namespace TransportApi.Common
{
	public class PagedResult<T> : IPagedResult<T>
	{
		public List<T> Items { get; set; } // Danh sách các bản ghi của trang hiện tại
		public long TotalCount { get; set; }  // Tổng số bản ghi
		public int PageSize { get; set; }      // Kích thước trang (limit)
		public int PageNumber { get; set; }    // Số trang hiện tại (page)
	}
}