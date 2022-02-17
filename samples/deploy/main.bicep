param resourceBaseName string

resource appServicePlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: '${resourceBaseName}hostingplan'
  location: resourceGroup().location
  sku: {
    name: 'F1'
    capacity: 1
  }
}

resource webApp 'Microsoft.Web/sites@2018-11-01' = {
  name: '${resourceBaseName}web'
  location: resourceGroup().location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Development'
        }
      ]
    }
  }
}
