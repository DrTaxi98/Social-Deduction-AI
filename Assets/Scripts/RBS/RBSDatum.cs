public class RBSDatum
{
    public object value;

    public RBSDatum(object value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
