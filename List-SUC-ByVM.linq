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

// List all Software Update Configurations by VM (failed SUCs will NOT show here)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Microsoft.Azure.Management.Automation.Models.SoftwareUpdateConfigurationListResult sucs;
//Manually created list of VM resources. This list can be generated automatically, but it is out of scope for this demo
string[] vms = new[] { 
 "/subscriptions/5c028ac2-6c43-4fd8-875c-6d059beb2ef5/resourceGroups/ContosoMgmt/providers/Microsoft.Compute/virtualMachines/FrontendServer",
 "/subscriptions/5c028ac2-6c43-4fd8-875c-6d059beb2ef5/resourceGroups/ContosoMgmt/providers/Microsoft.Compute/virtualMachines/UbuntuTest",
 "/subscriptions/5c028ac2-6c43-4fd8-875c-6d059beb2ef5/resourcegroups/ContosoMgmt/providers/Microsoft.Compute/virtualMachines/noSchedule",
 };
//Store the list of VMs which do NOT have a update configuration
List<String> unconfiguredVms = new List<String>();

foreach (string name in vms)
{
	sucs = client.SoftwareUpdateConfigurations.ListByAzureVirtualMachine(name);
	if (sucs == null || sucs.Value.Count == 0)
		{
			unconfiguredVms.Add(name);
		}
}

//sucs = client.SoftwareUpdateConfigurations.ListByAzureVirtualMachine("/subscriptions/5c028ac2-6c43-4fd8-875c-6d059beb2ef5/resourceGroups/ContosoMgmt/providers/Microsoft.Compute/virtualMachines/FrontendServer/");
Console.WriteLine(unconfiguredVms);