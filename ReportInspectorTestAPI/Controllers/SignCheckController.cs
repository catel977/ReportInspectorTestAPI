using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportInspectorTestAPI.Controllers
{
	/// <summary>
	/// SignCheck controller class
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class SignCheckController : ControllerBase
	{
		private readonly ILogger<SignCheckController> _logger;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logger"></param>
		public SignCheckController(ILogger<SignCheckController> logger)
		{
			_logger = logger;
		}


		///<summary>
		/// Returns example ok/block response with a list of items
		///</summary>
		/// <remarks>For testing purposes, "ok" in the report text will return an "okToSign" response.  
		/// "info", "warn", "critical" will cause a "block" response with a list of items, you can pass multiple values, separating with a comma.</remarks>
		///<param name="report">Report object to analyze. </param>
		///<returns>OkToSign or BlockSign with a list of items.</returns>
		[HttpPost]
		public ICommandResponse Post(Report report)
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
