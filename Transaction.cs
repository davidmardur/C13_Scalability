public class Transaction
{   public string Name{get;set;}
    public decimal Amount{get;set;}
    public Transaction(string name, decimal amount)
    {
        this.Name = name;
        this.Amount = amount;
    }
}
