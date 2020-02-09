class Transaction
{
    private int ID = 0;
    private int showNumber = 0;
    private int adult = 0;
    private int child = 0;
    private int senior = 0;
    private bool refunded = false;
    private int price = 0;

    public Transaction(int ID, int adult, int child, int senior, bool refunded)
    {
        this.ID = ID;
        this.adult = adult;
        this.child = child;
        this.senior = senior;
        this.refunded = refunded;
    }

    public int GetID()
    {
        return ID;
    }
    public int GetShowNumber()
    {
        return showNumber;
    }
    public int GetAdult()
    {
        return adult;
    }
    public int GetChild()
    {
        return child;
    }
    public int GetSenior()
    {
        return senior;
    }
    public bool GetRefunded()
    {
        return refunded;
    }

    public int GetPrice()
    {
        return price;
    }

    public void SetShowNumber(int showNumber)
    {
        this.showNumber = showNumber;
    }
    public void SetPrice(int price)
    {
        this.price = price;
    }

    public void SetRefunded(bool refunded)
    {
        this.refunded = refunded;
    }

    public override string ToString()
    {
        return ID + "," + showNumber + "," + adult + "," + child + "," + senior + "," + price + "," + refunded;
    }
}