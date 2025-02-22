﻿// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace MadEyeMatt.AspNetCore.Authentication.ApiKey.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using Xunit;

	public class ApiKeyUtilsTests
	{
		[Fact]
		public static void BuildClaimsPrincipal_adds_single_identity_with_claims()
		{
			string schemeName = "Test";
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, "abc@xyz.com"),
				new Claim(ClaimTypes.Role, "admin")
			};
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(null, schemeName, null, claims);
			Assert.NotNull(claimsPrincipal);
			Assert.Single(claimsPrincipal.Identities);
			Assert.NotNull(claimsPrincipal.Identity);
			Assert.Equal(schemeName, claimsPrincipal.Identity.AuthenticationType);
			Assert.NotEmpty(claimsPrincipal.Claims);
			Assert.Equal(claims.Count, claimsPrincipal.Claims.Count());
		}

		[Fact]
		public static void BuildClaimsPrincipal_adds_single_identity_without_claims()
		{
			string schemeName = "Test";
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(null, schemeName, null);
			Assert.NotNull(claimsPrincipal);
			Assert.Single(claimsPrincipal.Identities);
			Assert.NotNull(claimsPrincipal.Identity);
			Assert.Equal(schemeName, claimsPrincipal.Identity.AuthenticationType);
			Assert.Empty(claimsPrincipal.Claims);
		}

		[Fact]
		public static void BuildClaimsPrincipal_null_claims_no_exception()
		{
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(null, "Test", null);
			Assert.NotNull(claimsPrincipal);
		}

		[Fact]
		public static void BuildClaimsPrincipal_null_claimsIssuer_no_exception()
		{
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(null, "Test", null);
			Assert.NotNull(claimsPrincipal);
		}

		[Fact]
		public static void BuildClaimsPrincipal_null_ownerName_no_exception()
		{
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(null, "Test", "Test");
			Assert.NotNull(claimsPrincipal);
		}

		[Fact]
		public static void BuildClaimsPrincipal_null_schemeName_throws_ArgumentNullException()
		{
			ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => ApiKeyUtils.BuildClaimsPrincipal(null, null, null));
			Assert.Contains("schemeName", exception.Message);
		}

		[Fact]
		public static void BuildClaimsPrincipal_ownerName_adds_Name_and_NameIdentifier_claims()
		{
			string ownerName = "Test";
			string schemeName = "Test";
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(ownerName, schemeName, null);
			Assert.NotNull(claimsPrincipal);
			Assert.NotEmpty(claimsPrincipal.Claims);
			Assert.Equal(2, claimsPrincipal.Claims.Count());
			Assert.Contains(claimsPrincipal.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == ownerName);
			Assert.Contains(claimsPrincipal.Claims, c => c.Type == ClaimTypes.Name && c.Value == ownerName);
		}

		[Fact]
		public static void BuildClaimsPrincipal_ownerName_adds_Name_and_NameIdentifier_claims_if_not_already_exists()
		{
			string ownerName = "Test";
			string schemeName = "Test";
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "Admin"),
				new Claim(ClaimTypes.Role, "admin")
			};
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(ownerName, schemeName, null, claims);
			Assert.NotNull(claimsPrincipal);
			Assert.NotEmpty(claimsPrincipal.Claims);
			Assert.NotEqual(claims.Count, claimsPrincipal.Claims.Count());
			Assert.Equal(claims.Count + 1, claimsPrincipal.Claims.Count());
			Assert.Contains(claimsPrincipal.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == ownerName);
			Assert.Contains(claimsPrincipal.Claims, c => c.Type == ClaimTypes.Name && c.Value != ownerName);
			Assert.Contains(claimsPrincipal.Claims, c => c.Type == ClaimTypes.Name && c.Value == "Admin");
		}

		[Fact]
		public static void BuildClaimsPrincipal_ownerName_adds_Name_and_NameIdentifier_claims2()
		{
			string ownerName = "Test";
			string schemeName = "Test";
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, "abc@xyz.com"),
				new Claim(ClaimTypes.Role, "admin")
			};
			ClaimsPrincipal claimsPrincipal = ApiKeyUtils.BuildClaimsPrincipal(ownerName, schemeName, null, claims);
			Assert.NotNull(claimsPrincipal);
			Assert.NotEmpty(claimsPrincipal.Claims);
			Assert.NotEqual(claims.Count, claimsPrincipal.Claims.Count());
			Assert.Equal(claims.Count + 2, claimsPrincipal.Claims.Count());
			Assert.Contains(claimsPrincipal.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == ownerName);
			Assert.Contains(claimsPrincipal.Claims, c => c.Type == ClaimTypes.Name && c.Value == ownerName);
		}
	}
}
