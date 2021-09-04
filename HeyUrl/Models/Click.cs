using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.Models
{
	public class Click
	{
		public Guid Id { get; set; }
		public string ShortUrl { get; set; }
		public string Browser { get; set; }
		public string Platform { get; set; }

		[DataType(DataType.Date)]
		public DateTime Clicked { get; set; }

	}
}
