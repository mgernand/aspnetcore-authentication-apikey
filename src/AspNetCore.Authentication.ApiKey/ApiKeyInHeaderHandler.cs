﻿// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace MadEyeMatt.AspNetCore.Authentication.ApiKey
{
	using System.Linq;
	using System.Text.Encodings.Web;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using Microsoft.Extensions.Primitives;

	public class ApiKeyInHeaderHandler : ApiKeyHandlerBase
	{
		public ApiKeyInHeaderHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder)
			: base(options, logger, encoder)
		{
		}

		/// <inheritdoc />
		protected override Task<string> ParseApiKeyAsync()
		{
			if(this.Request.Headers.TryGetValue(this.Options.KeyName, out StringValues value))
			{
				return Task.FromResult(value.FirstOrDefault());
			}

			// No ApiKey found
			return Task.FromResult(string.Empty);
		}
	}
}
