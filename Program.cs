
using System.Text.Json;
using Azure.Messaging;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

Console.Clear();
Console.WriteLine("Procesador de C13 Money Transactions...");

string connectionString = "Endpoint=sb://codigoc13.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UXKjXbHo/LDLXvwGdKcEoK5LhTC9E2Xte+ASbFZICzk=";
string queueName = "C13MoneyTransactions";
await using var client = new ServiceBusClient(connectionString);
ServiceBusReceiver receiver = client.CreateReceiver(queueName);
Dictionary<string, decimal> moneyAccounts = new Dictionary<string, decimal>();

while (1==1)
{
    Console.WriteLine("Esperando por transacciones... (Ctl+C para terminar)");
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
    Console.WriteLine("Transaccion Recibida:");
    var messageBody = receivedMessage.Body.ToString();
    var transaction = JsonSerializer.Deserialize<Transaction>(messageBody);
    transaction.Name = transaction?.Name.ToUpper().Trim();

    Console.WriteLine($"Nombre: {transaction.Name}");
    Console.WriteLine($"Monto: {transaction.Amount}");

    if(moneyAccounts.ContainsKey(transaction.Name))
        moneyAccounts[transaction.Name] += transaction.Amount;
    else
        moneyAccounts.Add(transaction.Name, transaction.Amount);

    await receiver.CompleteMessageAsync(receivedMessage);

    Console.WriteLine("--------------Estados de Cuenta-----------------");
    foreach(var accounts in moneyAccounts)
    {
        Console.WriteLine($"--- Nombre:{accounts.Key} -- Saldo: {accounts.Value}");      
    }
    Console.WriteLine("--------------FIN Estados de Cuenta-----------------");
}
