namespace LibrairieClasse;

public class InvalidPlayException : Exception
{
    public InvalidPlayException(string message): base(message)
    {
    }
}