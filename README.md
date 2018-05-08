# UpdateManagementAPIs
A small selection of (unofficial. unsupported) demo scripts invoking the Update Management REST APIs

This demo uses a [service principal](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal).

The easiest way to use these snippets is via [LinqPad](https://www.linqpad.net/). You will need to create an app.config with the following values defined:
```
<configuration>
  <appSettings>
    <add key="TenantId" value="" />
    <add key="ClientId" value="servicePrincipalID" />
    <add key="ClientSecret" value="servicePrincipal" />
    <add key="SubscriptionId" value="" />
  </appSettings>
</configuration>
```

The APIs themselves are documented here:

https://docs.microsoft.com/en-us/rest/api/automation/softwareupdateconfigurations

https://docs.microsoft.com/en-us/rest/api/automation/softwareupdateconfigurationruns

https://docs.microsoft.com/en-us/rest/api/automation/softwareupdateconfigurationmachineruns 
