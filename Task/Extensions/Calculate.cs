namespace Tasks.Extensions;

public static class Calculate
{
    public static double TotalPrice(double vat, int amount, double price)
    {
        var calculatedPrice = ((amount * price) * (1 + vat));

        return calculatedPrice;
    }
}