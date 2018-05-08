
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

// Create Software Update Configuration (scheduled deployment)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Schedule One Time update
var scheduleInfo = new ScheduleProperties
{
	Frequency = ScheduleFrequency.OneTime,
	Interval = 1,
	StartTime = DateTime.Now.AddMinutes(10),	// This is local time
};

// Update details
var updateConfiguration = new UpdateConfiguration
{
	OperatingSystem = OperatingSystemType.Windows,
	Windows = new WindowsProperties
	{
		IncludedUpdateClassifications = WindowsUpdateClasses.Critical + ',' + WindowsUpdateClasses.Security,
		ExcludedKbNumbers = new[] { "KB123", "KB123" }
	},

	Duration = TimeSpan.FromHours(3),
	AzureVirtualMachines = new[] {
	   "/subscriptions/5c028ac2-6c43-4fd8-875c-6d059beb2ef5/resourcegroups/ContosoMgmt/providers/Microsoft.Compute/virtualMachines/noSchedule"
	}
};

var sucParameters = new SoftwareUpdateConfiguration(updateConfiguration, scheduleInfo);


// Make the call to create the software update configuration
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var suc = client.SoftwareUpdateConfigurations.Create($"test-suc-{Guid.NewGuid()}", sucParameters);
if(suc == null) {
	Console.WriteLine("Creation Failed");
	return;
}
Console.WriteLine($"Created '{suc.Name}' in '{suc.ProvisioningState}' state");

// Wait for provisioning to succeed
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
DateTime now = DateTime.Now;
do {
	System.Threading.Thread.Sleep(5000);
	suc = client.SoftwareUpdateConfigurations.GetByName(suc.Name);
	Console.WriteLine(suc.ProvisioningState);
} while (suc.ProvisioningState == "Provisioning" && DateTime.Now - now < TimeSpan.FromMinutes(2));
