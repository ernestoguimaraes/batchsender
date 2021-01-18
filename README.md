# Azure Messaging Services - Batch Sending 

This sample Console Application shows  how to use netcore and Azure SDK to send Batch messages to Azure Service Bus and Azure Event Hub

---
If you are looking for performance on sending messages, this code can help you. However, your Services needs to be appropriate setup to support your needs. As example, you can work with Multiple Partitions, Premium SKUs and Throuput Units to achieve desired performance.

Good Readings:

[- Automatically scale up throughput units](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-auto-inflate)

[- Best Practices for performance improvements using Service Bus Messaging](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-performance-improvements?tabs=net-standard-sdk-2)


---

### Requirements

This solution contains a [Infra](Infra) folder where two ARM template files can be used to provisioned a EventHub and a ServiceQueue to speed up your tests. You can use [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) to deploy the template:

``` powershell
 az deployment group create --resource-group YOUR RESOURCE GROUP --template-file .\Infra\template.json
```

### Solution Parts

**appSettings.json** - Config file where you should add your keys.

```json
"Messaging": {
    "NumberOfThreads": 1,
    "NumberOfMessages": 2,
    "EventHub": {
      "ConnectionString": "{ADD YOU CONNSTRING}",
      "HubName": "receivertopic"      
    },
    "ServiceBus": {
      "ConnectionString": "{ADD YOU CONNSTRING}",
      "QueueName": "sampleingress"
    }
```
