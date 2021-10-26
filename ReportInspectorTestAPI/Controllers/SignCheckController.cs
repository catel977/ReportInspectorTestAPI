using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportInspectorTestAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class SignCheckController : ControllerBase
	{
		private readonly ILogger<SignCheckController> _logger;

		public SignCheckController(ILogger<SignCheckController> logger)
		{
			_logger = logger;
		}


		[HttpPost]
		public ICommandResponse CheckReport(Report report)
		{
			var text = report.Text.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim().ToLowerInvariant()).ToList();

			if (text.Contains("ok")) return new OkToSignResponse();

			var response = new BlockSignResponse();
			if (text.Contains("warn"))
			{
				response.Items.Add(new RequestForInformation
				{
					Id = "1",
					Severity = BlockSeverity.Warn,
					Display = "Warning Text Here"
				});
			}
			if (text.Contains("critical"))
			{
				response.Items.Add(new RequestForInformation
				{
					Id = "2",
					Severity = BlockSeverity.Critical,
					Display = "Critical Text Here"
				});
			}
			if (text.Contains("info"))
			{
				response.Items.Add(new RequestForInformation
				{
					Id = "3",
					Severity = BlockSeverity.Info,
					Display = "Informational Text Here"
				});
			}

			return response;
		}
	}
}
