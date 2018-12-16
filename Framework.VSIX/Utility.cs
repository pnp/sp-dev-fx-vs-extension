using Framework.VSIX.Resources;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Framework.VSIX
{
	public static class Utility
	{
		#region Application Insights

		public static string AppInsightsKey = "0475a9f1-215a-41a9-860e-c9c9a337868c";

		#endregion

		#region Set Command line

		public static bool SetItemCommand(string Framework, string ComponentName,
																			string ComponentDescription, string ComponentType,
																			string ExtensionType, out string command)
		{
			command = String.Empty;
			bool result = true;

			if (String.IsNullOrEmpty(ComponentName))
				result = false;
			if (String.IsNullOrEmpty(ComponentDescription))
				result = false;
			if (String.IsNullOrEmpty(ComponentType))
				result = false;
			if (ComponentType == "webpart" && String.IsNullOrEmpty(Framework))
				result = false;
			if (ComponentType == "extension" && String.IsNullOrEmpty(ExtensionType))
				result = false;
			if (ComponentType == "extension" && ExtensionType == "FieldCustomizer" && String.IsNullOrEmpty(Framework))
				result = false;

			command = SetCommand(null, Framework, ComponentName, ComponentDescription,
													 ComponentType, ExtensionType, null, false, false, false, null, false);
			return result;
		}

		public static bool SetProjectCommand(string SolutionName, string Framework, string ComponentName,
																				 string ComponentDescription, string ComponentType, string ExtensionType,
																				 string Environment, bool SkipFeatureDeployment, bool SkipInstall, bool PlusBeta, string PackageManager, bool DomainIsolated,
																				 out string command)
		{
			command = String.Empty;
			bool result = true;

			if (String.IsNullOrEmpty(SolutionName))
				result = false;
			if (String.IsNullOrEmpty(Framework))
				result = false;
			if (String.IsNullOrEmpty(ComponentName))
				result = false;
			if (String.IsNullOrEmpty(ComponentDescription))
				result = false;
			if (String.IsNullOrEmpty(ComponentType))
				result = false;
			if (ComponentType == "extension" && String.IsNullOrEmpty(ExtensionType))
				result = false;

			command = SetCommand(SolutionName, Framework, ComponentName, ComponentDescription,
													 ComponentType, ExtensionType, Environment, SkipFeatureDeployment, SkipInstall, PlusBeta, PackageManager, DomainIsolated);
			return result;
		}

			private static string SetCommand(string SolutionName, string Framework, string ComponentName,
																			 string ComponentDescription, string ComponentType, string ExtensionType,
																			 string Environment, bool SkipFeatureDeployment, bool SkipInstall, bool PlusBeta, string PackageManager, bool DomainIsolated)
			{
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append($"yo @microsoft/sharepoint");

			if (!String.IsNullOrEmpty(SolutionName))
				commandBuilder.Append($" --solution-name \"{SolutionName}\"");

			if (!String.IsNullOrEmpty(ComponentName))
				commandBuilder.Append($" --component-name \"{ComponentName}\"");

			if (!String.IsNullOrEmpty(ComponentDescription))
				commandBuilder.Append($" --component-description \"{ComponentDescription}\"");

			if (!String.IsNullOrEmpty(ComponentType))
				commandBuilder.Append($" --component-type \"{ComponentType}\"");

			if (!String.IsNullOrEmpty(Framework))
				commandBuilder.Append($" --framework \"{Framework}\"");

			if (SkipInstall)
				commandBuilder.Append(" --skip-install");

            if (PlusBeta)
                commandBuilder.Append(" --plusbeta");

            if (!String.IsNullOrEmpty(Environment))
                commandBuilder.Append($" --environment \"{Environment}\"");

            if (!String.IsNullOrEmpty(ExtensionType))
            	commandBuilder.Append($" --extension-type \"{ExtensionType}\"");

            if (!String.IsNullOrEmpty(PackageManager))
            {
                commandBuilder.Append($" --package-manager \"{PackageManager}\"");
            }

            if (SkipFeatureDeployment)
                commandBuilder.Append(" --skip-feature-deployment");

            if (DomainIsolated)
                commandBuilder.Append(" --is-domain-isolated");

            commandBuilder.Append(" --skip-cache");

            return commandBuilder.ToString();
		}
		#endregion

	}
}
