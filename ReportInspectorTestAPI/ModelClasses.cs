using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportInspectorTestAPI
{
	public interface ICommandResponse
	{
		string OnSignResponse { get; }
		IList<RequestForInformation> Items { get; set; }
	}

	public class OkToSignResponse : ICommandResponse
	{
		public string OnSignResponse { get; } = "okToSign";

		public IList<RequestForInformation> Items { get; set; } = null;
	}

	public class BlockSignResponse : ICommandResponse
	{
		public string OnSignResponse { get; } = "blockSign";

		public IList<RequestForInformation> Items { get; set; } = new List<RequestForInformation>();
	}

	public class RequestForInformation
	{
		public string Id { get; set; }
		public string Display { get; set; }
		public BlockSeverity Severity { get; set; }
	}

	public enum BlockSeverity
	{
		Info,
		Warn,
		Critical
	}

	public class Report
	{
		public string Text { get; set; }
		public string ReasonForExam { get; set; }
		public PatientInfo Patient { get; set; }
	}

	public class PatientInfo
	{
		public string Sex { get; set; }
		public DateTime dob { get; set; }
	}
}
