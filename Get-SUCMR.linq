<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <NuGetReference Version="3.0.1-preview" Prerelease="true">Microsoft.Azure.Management.Automation</NuGetReference>
  <NuGetReference>Microsoft.Azure.Management.ResourceManager.Fluent</NuGetReference>
  <Namespace>Microsoft.Azure.Management.Automation</Namespace>
  <Namespace>Microsoft.Azure.Management.Automation.Models</Namespace>
  <Namespace>Microsoft.Azure.Management.ResourceManager.Fluent</Namespace>
  <Namespace>Microsoft.Azure.Management.ResourceManager.Fluent.Authentication</Namespace>
  <Namespace>System.Configuration</Namespace>
  <AppConfig>
    <Path Relative="..\linqpad.config">C:\Users\zachal\OneDrive - Microsoft\test scripts\updateMgmt\linqpad.config</Path>
  </AppConfig>
</Query>

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Nugets:
//   Microsoft.Azure.Management.Automation
//   Microsoft.Azure.Management.ResourceManager.Fluent
// Using:
//  System.Configuration
//  Microsoft.Azure.Management.Automation
//  Microsoft.Azure.Management.Automation.Models
//  Microsoft.Azure.Management.ResourceManager.Fluent
//  Microsoft.Azure.Management.ResourceManager.Fluent.Authentication
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// Use service principal to create Azure Credentials object
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
string tenantId = ConfigurationManager.AppSettings["TenantId"];
var servicePrincipal = new ServicePrincipalLoginInformation
{
	ClientId = ConfigurationManager.AppSettings["ClientId"],
	ClientSecret = ConfigurationManager.AppSettings["ClientSecret"],
};

var credentials = new AzureCredentials(servicePrincipal, tenantId, AzureEnvironment.AzureGlobalCloud);


// Create automation client, and set the resource group and account name context to be used
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var client = new AutomationClient(credentials)
{
	SubscriptionId = ConfigurationManager.AppSettings["SubscriptionId"],
	ResourceGroupName = "DefaultResourceGroup-WCUS",
	AutomationAccountName = "Automate-5c028ac2-6c43-4fd8-875c-6d059beb2ef5-WCUS"
};

// Get Machine Runs for specific update run
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//var runName = client.SoftwareUpdateConfigurationRuns.List().Value.First().Name;
//var runs = client.SoftwareUpdateConfigurationMachineRuns.ListByCorrelationId(Guid.Parse(runName));
//Console.WriteLine(runs);

// Get Machine Runs with specific state
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//runs = client.SoftwareUpdateConfigurationMachineRuns.ListByStatus("Succeeded");
//Console.WriteLine(runs);

// Get Machine Runs with specific state
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var runs = client.SoftwareUpdateConfigurationMachineRuns.ListByTargetComputer("/subscriptions/5c028ac2-6c43-4fd8-875c-6d059beb2ef5/resourceGroups/ContosoMgmt/providers/Microsoft.Compute/virtualMachines/UbuntuTest");
Console.WriteLine(runs);

////
runs = client.SoftwareUpdateConfigurationMachineRuns.ListByCorrelationId(Guid.Parse("e3a0b5b2-02de-4e2b-a223-dedea2cff435"));
Console.WriteLine(runs);